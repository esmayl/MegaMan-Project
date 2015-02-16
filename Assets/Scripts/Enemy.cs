using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Pathfinding;

[RequireComponent(typeof(Rigidbody),typeof(Seeker))]
public class Enemy : MonoBehaviour {

    public GameObject player;
    public float hp = 100;
    public int walkDistance = 5;
    public float speed = 3;
    public float meleeDamage = 4f;

    //Pathfinding variables

    Seeker seeker;
    Rigidbody controller;
    bool endReached;
    Vector3 walkDirection;

    Path path;
    
    int currentWaypoint = 0;

    public virtual void Start()
    {
        seeker = GetComponent<Seeker>();
        controller = rigidbody;

        seeker.StartPath(transform.position,transform.position+(transform.forward * walkDistance),PathCalculationsComplete);
    }


	public virtual void Update () {

        if (hp <= 0)
        {
            Destroy(gameObject);
        }
        if (path == null){return;}

        //Pathfinding 
        if (endReached)
        {
            controller.velocity = Vector3.zero;
            //add random number generator to select next action

        }


        if (currentWaypoint >= path.vectorPath.Count)
        {
            if (endReached) { return; }
            endReached = true;
        }
        if (currentWaypoint < path.vectorPath.Count)
        {
            walkDirection = path.vectorPath[currentWaypoint] - transform.position;
            walkDirection.Normalize();
            walkDirection.x = 0;
        }

        //Actually moves the enemy
        if (!endReached)
        {
            controller.velocity = walkDirection * speed;
            if (Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]) < 1)
            {
                currentWaypoint++;
            }
        }

	}

    public void PathCalculationsComplete(Path p)
    {
        if (!p.error)
        {
            endReached = false;
            Debug.Log(p.vectorPath.Count);
            path = p;
            currentWaypoint = 0;
        }
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
    }

    public virtual void Jump() { }
    public virtual void MoveForward() { }
    public void MoveTo(Transform target) 
    {
        transform.LookAt(new Vector3(transform.position.x, transform.position.y, target.position.z));
        rigidbody.velocity = transform.forward * speed/2.5f;
    }
    public virtual void LookAtPlayer(Vector3 PlayerPos) { }
    public virtual void Attack(Vector3 Direction) { }
    public IEnumerator DetectPlayer(float Radius) 
    {
        while (true)
        {
            if (endReached)
            {
                transform.LookAt(new Vector3(transform.position.x, transform.position.y, transform.position.z) - transform.forward * walkDistance);
                seeker.StartPath(transform.position, transform.position + (transform.forward * walkDistance), PathCalculationsComplete);
            }
            else { seeker.StartPath(transform.position, transform.position + (transform.forward * walkDistance), PathCalculationsComplete); }



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
                        if (Mathf.Abs(Vector3.Distance(player.transform.position, transform.position)) < 1.5f)
                        {
                            player.gameObject.GetComponent<PlayerMovement>().TakeDamage(meleeDamage);
                        }
                        
                        Debug.Log("" + h);
                    }
                }
            }
            else
            {

                player = null;
            }
            yield return new WaitForSeconds(1f);
        }
    }

    /// <summary>
    /// only use in air AKA flying enemy
    /// </summary>
    public virtual void MoveUp() { }
}
