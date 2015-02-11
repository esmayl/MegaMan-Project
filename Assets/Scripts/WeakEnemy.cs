using UnityEngine;
using System.Collections;

public class WeakEnemy : Enemy {

    public float range = 1;

    void Start()
    {
        range = transform.GetChild(0).GetComponent<MeshFilter>().mesh.bounds.extents.y*range;
        StartCoroutine("DetectPlayer", range);
    }
	
	public override void Update () {
        base.Update();
	}

    public void OnDrawGizmosSelected ()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawSphere(transform.position, range);
    }
}
