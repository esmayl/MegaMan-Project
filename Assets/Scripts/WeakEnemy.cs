using UnityEngine;
using System.Collections;

public class WeakEnemy : Enemy {

    public float range = 1;

    public override void Start()
    {
        base.Start();

        range = transform.GetChild(0).GetComponent<MeshFilter>().mesh.bounds.extents.y*range;
        StartCoroutine("DetectPlayer", range);
    }
	
	public override void Update () {
        base.Update();
	}

    public void OnDrawGizmosSelected ()
    {
        Gizmos.color = new Color(255,255,255,0.5f);

        Gizmos.DrawSphere(transform.position, range);

        Gizmos.DrawLine(transform.position,transform.position+transform.forward*walkDistance);
        Gizmos.DrawLine(transform.position + transform.up * 0.5f, transform.position + transform.up * 0.5f + transform.forward * walkDistance);
        Gizmos.DrawLine(transform.position + transform.right * 0.5f, transform.position + transform.right * 0.5f + transform.forward * walkDistance);
        Gizmos.DrawLine(transform.position + -transform.up * 0.5f, transform.position + -transform.up * 0.5f + transform.forward * walkDistance);

    }
}
