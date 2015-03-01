using UnityEngine;
using System.Collections;

public class RangedEnemy : Enemy {


    public override void Start()
    {
        base.Start();

        StartCoroutine("DetectPlayer", range);
    }
	
	public override void Update () {


        base.Update();
	}

    public override IEnumerator DetectPlayer(float Radius)
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

                        if (Physics.Raycast(new Ray(transform.position + transform.up / 4, h.transform.position - transform.position), out hit))
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

    public void OnDrawGizmosSelected ()
    {
        Gizmos.color = new Color(255,255,255,0.5f);

        Gizmos.DrawSphere(transform.position, range);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + transform.forward , -transform.up);
    }
}
