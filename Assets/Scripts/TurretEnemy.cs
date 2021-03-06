using UnityEngine;
using System.Collections;

public class TurretEnemy : EnemyBase {

    public GameObject barrel;

    public override void Start()
    {
        base.Start();
        currentState = EnemyStates.Attack;

        StartCoroutine("DetectPlayer", range);
    }

    public override void Update()
    {
        base.Update();
    }

    public override IEnumerator DetectPlayer(float Radius)
    {
        while (true)
        {


        //==============================UPDATE========================================//
            Collider[] hits;
            hits = Physics.OverlapSphere(transform.position, Radius);
            if (hits.Length > 0)
            {
                foreach (Collider h in hits)
                {
                    if (h.gameObject.layer == LayerMask.NameToLayer("Player"))
                    {
                        player = h.gameObject;
                        Vector3 playerPos = player.transform.position;
                        currentState = EnemyStates.Attack;
                        if (!animation.isPlaying) { animation.Play(); yield return new WaitForSeconds(2f); animation.enabled = false; }
                    }
                }
            }
            if (player)
            {
                if (Vector3.Distance(player.transform.position, transform.position) > Radius)
                {
                    player = null;
                }
            }

            switch (currentState)
            {
                case EnemyStates.Idle:
                    velocity = Vector3.zero;
                    StartCoroutine("Idle");
                    break;
                case EnemyStates.Attack:
                    Attack(transform.forward);
                    break;
                default:
                    break;
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    public override void Attack(Vector3 Direction)
    {
        if (!player)
        {
            currentState = EnemyStates.Idle;
            return;
        }
            barrel.transform.LookAt(player.transform.position+transform.up/2);

            //using transform.up to make sure the bullet instances above the ground
            GameObject tempObj = Instantiate(damageDealer, transform.position + barrel.transform.forward*1.5f + transform.up*1.5f , Quaternion.identity) as GameObject;
            tempObj.transform.LookAt(player.transform.position + transform.up / 2);
            tempObj.rigidbody.velocity = barrel.transform.forward;
            tempObj = null;
    }

    public override void Patrol()
    {
        //cause a turret can't patrol :D
    }
}
