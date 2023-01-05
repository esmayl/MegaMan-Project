using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Pathfinding;

public enum EnemyStates { Idle,Patrol,Attack,AttackAtPlayer}
public enum EnemyTypes { Melee,Ranged,FlyingMelee,FlyingRanged}

[RequireComponent(typeof(Rigidbody))]
public class EnemyBase : MonoBehaviour {

    public EnemyTypes enemyType = EnemyTypes.Ranged;
    public GameObject damageDealer;
    public EnemyStates currentState = EnemyStates.Patrol;
    public GameObject player;
    public float hp = 100;
    public float speed = 3;
    public int meleeDamage = 5;
    public float rangeToStop = 0.5f;
    internal Vector3 walkDirection;
    public float range = 15;
    public bool canGoForward = false;                                                                                                    

    //Pathfinding variables
    internal Vector3 velocity;
    internal RaycastHit hit;
    Vector3 target;
    Rigidbody controller;
    float height;

    

    public virtual void Start()
    {


        if(Physics.Raycast(new Ray(transform.position,-transform.up),out hit,10f))
        {
            if (hit.transform.tag == "Ground")
            {
                transform.position = hit.point+transform.up/4;
                height = transform.position.y;
            }
        }
        controller = GetComponent<Rigidbody>();
        StartCoroutine("DetectPlayer", range);

    }


	public virtual void Update () {

        
        if (transform.position.y > height)
        {
            transform.position = new Vector3(transform.position.x, height, transform.position.z);
        }

        
        if (canGoForward)
        {
            controller.velocity = velocity;
        }

        if (hp < 0)
        {
            ItemDatabase.DropItem(transform.position, transform.name);
            Destroy(gameObject);
            Debug.LogError("");
        }
	}

    public void TakeDamage(float damage)
    {
        if (hp < 0)
        {
            ItemDatabase.DropItem(transform.position, transform.name);
            Destroy(gameObject);
        }

        hp -= damage;
        if (hp <= 0)
        {
            ItemDatabase.DropItem(transform.position, transform.name);
            Destroy(gameObject);
        }
    }

    public virtual void Jump() { }


    //doesnt move in y direction, detects path on its own by raycasting forward and at 1.5 times forward in the down direction
    public virtual void Patrol()
    {
        velocity.y =0;

        RaycastHit hit;
        if (Physics.Raycast(new Ray(transform.position + transform.forward * 1.5f,-transform.up),out hit,10))
        {
            if (hit.transform.tag == "Ground")
            {
                if (hit.distance < 0.3f)
                {
                    if (!Physics.Linecast(transform.position, transform.position + transform.forward))
                    {
                        canGoForward = true;
                        target = transform.position + transform.forward;
                        walkDirection = target - transform.position;
                        velocity = controller.velocity;
                        if (walkDirection.magnitude > rangeToStop)
                        {
                            velocity = walkDirection.normalized * speed;
                        }
                    }
                    else
                    {
                        canGoForward = false;
                        transform.LookAt(transform.position - transform.forward);
                    }

                }
                if (hit.distance > 0.41f)
                {
                    canGoForward = false;
                    transform.LookAt(transform.position-transform.forward);
                }
                else { return; }
            }
        }
        else
        {
            canGoForward = false;
        }

    }

    public void MoveToPlayer(Transform target) 
    {
        RaycastHit hit;
        if (Physics.Raycast(new Ray(transform.position + transform.forward * 1.5f, -transform.up), out hit, 10))
        {
            if (hit.transform.tag == "Ground")
            {
                if (hit.distance < 0.4f)
                {
                    if (!Physics.Linecast(transform.position, transform.position + transform.forward))
                    {
                        canGoForward = true;
                        walkDirection = target.position - transform.position;
                        walkDirection.y = 0;
                        velocity = controller.velocity;

                        if (walkDirection.magnitude > rangeToStop)
                        {
                            velocity = walkDirection.normalized * speed;
                        }
                        velocity.y = 0;
                        controller.velocity = velocity;
                    }
                    else
                    {
                        canGoForward = false;
                        transform.LookAt(transform.position - transform.forward);
                    }

                }
                if (hit.distance > 0.41f)
                {
                    canGoForward = false;
                    transform.LookAt(transform.position - transform.forward);
                }
                else { return; }
            }
        }
        else
        {
            canGoForward = false;
        }
    }

    public IEnumerator Idle()
    {
        yield return new WaitForSeconds(0.5f);
        currentState = EnemyStates.Patrol;
    }

    public virtual void Attack(Vector3 Direction) 
    {
        if (!player)
        {
            currentState = EnemyStates.Idle;
            return;
        }

        velocity = Vector3.zero;
        if (Mathf.Abs(player.transform.position.y - transform.position.y) < 1f)
        {
            Vector3 flattendPos = player.transform.position;
            flattendPos.y = transform.position.y;
            transform.LookAt(flattendPos);
            
            //using transform.up to make sure the bullet instances above the ground
            GameObject tempObj = Instantiate(damageDealer, transform.position + Direction+(transform.up/8), Quaternion.identity) as GameObject;
            tempObj.transform.LookAt(flattendPos + (transform.up / 8));
        }
    }

    public virtual IEnumerator DetectPlayer(float Radius) 
    {
        while (true)
        {

            //==============================UPDATE========================================//

            Collider[] hits;
            hits = Physics.OverlapSphere(transform.position, range);
            if (hits.Length > 0)
            {
                foreach (Collider h in hits)
                {
                    if (h.gameObject.layer == LayerMask.NameToLayer("Player"))
                    {

                        if(Physics.Raycast(new Ray(transform.position+transform.up/4,h.transform.position-transform.position),out hit))
                        {
                            if (hit.transform.tag == "Player")
                            {
                                player = h.gameObject;
                                Vector3 playerPos = player.transform.position;
                                playerPos.y = transform.position.y;
                                transform.LookAt(playerPos);
                                currentState = EnemyStates.AttackAtPlayer;

                                if (player)
                                {
                                    //using sphere cast to fake damage on collision
                                    if (Mathf.Abs(Vector3.Distance(player.transform.position, transform.position)) < rangeToStop)
                                    {
                                        player.gameObject.GetComponent<PlayerMovement>().TakeDamage(meleeDamage);
                                    }
                                }
                                hits = null;
                            }
                            if (hit.transform.tag == "Ground")
                            {
                                currentState = EnemyStates.Patrol;
                            }
                        }
                        
                        
                        
                    }
                }
            }
            if (player)
            {
                if (Vector3.Distance(player.transform.position, transform.position) > range)
                {
                    player = null;
                    currentState = EnemyStates.Patrol;
                }
            }

            switch (currentState)
            {
                case EnemyStates.Patrol:
                    Patrol();
                    break;
                case EnemyStates.Idle:
                    velocity = Vector3.zero;
                    StartCoroutine("Idle");
                    break;
                case EnemyStates.Attack:
                    Attack(transform.forward);
                    break;
                case EnemyStates.AttackAtPlayer:
                    MoveToPlayer(player.transform);
                    //AttackPlayer();
                    break;

                default:
                    Patrol();
                    break;
            }


            yield return new WaitForSeconds(0.5f);
        }
    }


    /// <summary>
    /// only use in air AKA flying enemy
    /// </summary>
    public virtual void MoveUp() { }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(255, 255, 255, 0.5f);

        Gizmos.DrawSphere(transform.position, range);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + transform.forward, -transform.up);
    }
}
