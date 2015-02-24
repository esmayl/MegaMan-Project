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
    internal float bulletSpeed =1;
    public float range = 1;

    //Pathfinding variables

    internal Vector3[] wayPoints = new Vector3[2];
    Vector3 target;
    Vector3 velocity;
    Rigidbody controller;
    int currentWaypoint = 0;
    bool endReached;

    Path path;
    

    public virtual void Start()
    {
        controller = rigidbody;
        wayPoints[0] = transform.position;
        wayPoints[1] = transform.position + transform.forward * walkDistance;
    }


	public virtual void Update () {

        if (hp <= 0)
        {
            Destroy(gameObject);
        }

        //Pathfinding 
        

	}

    public void PathCalculationsComplete(Path p)
    {
        if (!p.error)
        {
            endReached = false;
            path = p;
            currentWaypoint = 0;
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
        if (currentWaypoint < wayPoints.Length)
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
            else
            {
                walkDirection = Vector3.zero;
            }
        }
        velocity.y =0;
        controller.velocity = velocity;
        transform.LookAt(target);
    }


    public void MoveToPlayer(Transform target) 
    {
            walkDirection = target.transform.position - transform.position;
            velocity = walkDirection.normalized * speed;
            velocity.y = 0;
            controller.velocity = velocity;
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


        Vector3 flattendPos = player.transform.position;
        flattendPos.y = transform.position.y;
        transform.LookAt(flattendPos);

        GameObject tempObj = Instantiate(damageDealer, transform.position + Direction,Quaternion.identity) as GameObject;
        tempObj.transform.LookAt(transform.position + Direction);
        tempObj.rigidbody.velocity = transform.forward * bulletSpeed;
    }
    public IEnumerator DetectPlayer(float Radius) 
    {
        while (true)
        {

            //==============================UPDATE========================================//
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


            Collider[] hits;
            hits = Physics.OverlapSphere(transform.position, Radius);
            if (hits.Length > 0)

            {
                Debug.Log("Looping");
                foreach (Collider h in hits)
                {
                    if (h.gameObject.layer == LayerMask.NameToLayer("Player"))
                    {

                        player = h.gameObject;
                        currentState = EnemyStates.Attack;
                        
                        
                        
                        
                        //using sphere cast to fake damage on collision
                        if (Mathf.Abs(Vector3.Distance(player.transform.position, transform.position)) < 1.5f)
                        {
                            player.gameObject.GetComponent<PlayerMovement>().TakeDamage(meleeDamage);
                        }
                        
                        Debug.Log("" + h);
                    }
                }
            }
            if (player)
            {
                if (Vector3.Distance(player.transform.position, transform.position) > range)
                {
                    player = null;
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }

    /// <summary>
    /// only use in air AKA flying enemy
    /// </summary>
    public virtual void MoveUp() { }
}
