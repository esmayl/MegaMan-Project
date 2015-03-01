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

    public void OnDrawGizmosSelected ()
    {
        Gizmos.color = new Color(255,255,255,0.5f);

        Gizmos.DrawSphere(transform.position, range);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + transform.forward , -transform.up);
    }
}
