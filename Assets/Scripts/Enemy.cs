using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Pathfinding;

public enum EnemyStates { Idle,Patrol,Attack,AttackAtPlayer}
public enum EnemyTypes { Melee,Ranged,FlyingMelee,FlyingRanged}

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour {

    public EnemyTypes enemyType = EnemyTypes.Ranged;
    public GameObject damageDealer;
    public EnemyStates currentState = EnemyStates.Patrol;
    public GameObject player;
    public float hp = 100;
    public float speed = 3;
    public int meleeDamage = 4;
    public float rangeToStop = 0.5f;
    internal Vector3 walkDirection;
    internal float bulletSpeed =20;
    public float range = 1;
    public bool canGoForward = false                                                                                                     ;

    //Pathfinding variables
    Vector3 target;
    Vector3 velocity;
    Rigidbody controller;
    float height;

    RaycastHit hit;
    

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
        controller = rigidbody;
    }


	public virtual void Update () {

        if (hp <= 0)
        {
            ItemDatabase.DropItem(transform.position, "BaseEnemy10");
            Destroy(gameObject,0.1f);
        }
        if (transform.position.y > height)
        {
            transform.position = new Vector3(transform.position.x, height, transform.position.z);
        }

        if (player)
        {
            //using sphere cast to fake damage on collision
            if (Mathf.Abs(Vector3.Distance(player.transform.position, transform.position)) < 1.5f)
            {
                player.gameObject.GetComponent<PlayerMovement>().TakeDamage(Mathf.FloorToInt(meleeDamage*Time.deltaTime));
            }
        }
	}

    public void TakeDamage(float damage)
    {
        hp -= damage;
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
                if (hit.distance < 0.4f)
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

    }

    public IEnumerator Idle()
    {
        yield return new WaitForSeconds(0.5f);
        currentState = EnemyStates.Patrol;
    }
    public virtual void LookAtPlayer(Vector3 PlayerPos) { }

    public virtual void Attack(Vector3 Direction) 
    {
        if (!player)
        {
            currentState = EnemyStates.Idle;
            return;
        }

        if (Mathf.Abs(player.transform.position.y - transform.position.y) < 1f)
        {
            Vector3 flattendPos = player.transform.position;
            flattendPos.y = transform.position.y;
            transform.LookAt(flattendPos);
            
            //using transform.up to make sure the bullet instances above the ground
            GameObject tempObj = Instantiate(damageDealer, transform.position + Direction+(transform.up/8), Quaternion.identity) as GameObject;
            tempObj.transform.LookAt(transform.position + Direction + (transform.up / 8));
            tempObj.rigidbody.velocity = transform.forward * bulletSpeed * 1.5f;
        }
    }

    public IEnumerator DetectPlayer(float Radius) 
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
                                currentState = EnemyStates.Attack;

                                
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
}
