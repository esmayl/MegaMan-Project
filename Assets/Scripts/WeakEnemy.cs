using UnityEngine;
using System.Collections;

public class WeakEnemy : Enemy {

    public Item itemDrop;


    public override void Start()
    {
        base.Start();

        StartCoroutine("DetectPlayer", range);
    }
	
	public override void Update () {


        base.Update();
	}

    public void OnDrawGizmosSelected ()
    {
        Gizmos.color = new Color(255,255,255,0.5f);

        Gizmos.DrawSphere(transform.position, range);

        Gizmos.color = Color.green;

        Gizmos.DrawLine(transform.position,transform.position+transform.forward*walkDistance);
        Gizmos.DrawLine(transform.position + transform.up * 0.5f, transform.position + transform.up * 0.5f + transform.forward * walkDistance);
        Gizmos.DrawLine(transform.position + transform.right * 0.5f, transform.position + transform.right * 0.5f + transform.forward * walkDistance);
        Gizmos.DrawLine(transform.position + -transform.up * 0.5f, transform.position + -transform.up * 0.5f + transform.forward * walkDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + transform.forward*1.1f, -transform.up);
        foreach (Vector3 v in wayPoints)
        {
            Gizmos.DrawSphere(v, 0.5f);
        }

    }
}
