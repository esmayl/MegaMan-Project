using UnityEngine;
using System.Collections;

public class IceSpike : Bullet {

    float duration;
    float currentTime;

    void Start()
    {
        lifeTime =2;
        weaponDamage = 3;    
    }

    public void Update()
    {
        if (!transform.collider.isTrigger)
        {
            //currentTime += Time.deltaTime;
            //duration = Time.time;
            //float angle = duration / currentTime;
            //if (angle >= 15) { angle = 15; }
            //if (angle < 0) { angle = 0;}
            //
            //transform.Rotate(transform.right, angle / 10);


        }
    }

    public override void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "Ground")
        {
            attackHolder.SendMessage("DoAttack",coll.contacts[0].point-transform.forward);
            Destroy(gameObject);
        }
        if (coll.gameObject.tag == "Enemy")
        {
            attackHolder.SendMessage("DoAttack", coll.transform.position - transform.forward);
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter(Collider coll)
    {
        StartCoroutine("DeathTimer");

        if (coll.tag == "Enemy")
        {
            coll.gameObject.SendMessage("TakeDamage", baseDamage * weaponDamage);
        }
    }


    public override IEnumerator DeathTimer()
    {
        MeshRenderer[] meshes = new MeshRenderer[transform.GetComponentsInChildren<MeshRenderer>().Length]; 
        meshes = transform.GetComponentsInChildren<MeshRenderer>() as MeshRenderer[];

        if (meshes.Length > 0)
        {
            foreach (MeshRenderer r in meshes)
            {
                while (r.material.color.a > 0)
                {
                    r.material.color -= new Color(0, 0, 0, Time.deltaTime);
                    yield return new WaitForEndOfFrame();
                }
            }
        }
        base.DeathTimer();
        yield return null;
    }

    public void ShapeIce(Vector3 scale)
    {
        transform.localScale = scale;
    }
}
