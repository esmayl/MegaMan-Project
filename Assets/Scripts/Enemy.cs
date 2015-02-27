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
    public int walkDistance = 5;
    public float speed = 3;
    public int meleeDamage = 4;
    public float rangeToStop = 0.5f;
    internal Vector3 walkDirection;
    internal float bulletSpeed =20;
    public float range = 1;
    public bool canGoForward = false                                                                                                     ;

    //Pathfinding variables

    internal Vector3[] wayPoints = new Vector3[2];
    Vector3 target;
    Vector3 velocity;
    Rigidbody controller;
    int currentWaypoint = 0;
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
        wayPoints[0] = transform.position + transform.forward * walkDistance;
        wayPoints[1] = transform.position;
    }


	public virtual void Update () {

        if (hp <= 0)
        {
            Destroy(gameObject);
        }
        if (transform.position.y > height)
        {
            transform.position = new Vector3(transform.position.x, height, transform.position.z);
        }
	}

    public void TakeDamage(float damage)
    {
        hp -= damage;
    }

    public virtual void Jump() { }


    //doesnt move in y direction
    public virtual void Patrol()
    {
        if (currentWaypoint < wayPoints.Length-1)
        {
            target = wayPoints[currentWaypoint];
            walkDirection = target - transform.position;
            velocity = controller.velocity;

            if (walkDirection.magnitude > rangeToStop)
            {
                velocity = walkDirection.normalized * speed;
            }
            else
            {
                currentWaypoint++;
            }
        }
        else
        {
            if (currentState == EnemyStates.Patrol)
            {
                currentWaypoint = 0;
            }
        }
        velocity.y =0;

        RaycastHit hit;
        if (Physics.Raycast(new Ray(transform.position + transform.forward * 1.1f,-transform.up),out hit,3f))
        {
            if (hit.transform.tag == "Ground")
            {
                if (hit.distance < 0.4f)
                {
                    canGoForward = true;
                }
                else { return; }
            }
        }
        else
        {
            canGoForward = false;
        }

        if (canGoForward)
        {
            controller.velocity = velocity;
        }
        transform.LookAt(transform.position+velocity);
    }

    public void MoveToPlayer(Transform target) 
    {
            walkDirection = target.transform.position - transform.position;
            velocity = walkDirection.normalized * speed;
            velocity.y = 0;


            if (canGoForward)
            {
                controller.velocity = velocity;
            }
            transform.LookAt(target);
            if (Vector3.Distance(transform.position,target.position)<rangeToStop)
            {
                currentState = EnemyStates.Attack;
            }

        //transform.LookAt(new Vector3(transform.position.x, transform.position.y, target.position.z));
        //rigidbody.velocity = transform.forward * speed/2.5f;
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

                                //using sphere cast to fake damage on collision
                                if (Mathf.Abs(Vector3.Distance(player.transform.position, transform.position)) < 1.5f)
                                {
                                    player.gameObject.GetComponent<PlayerMovement>().TakeDamage(meleeDamage);
                                }
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
