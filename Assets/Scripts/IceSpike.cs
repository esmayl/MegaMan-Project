using UnityEngine;
using System.Collections;

public class IceSpike : Bullet {

    bool hit = false;

    void Start()
    {
        lifeTime =2;
        weaponDamage = 10;    
    }
    public override void Update()
    {
    }

    public override void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "Ground" && !hit)
        {
            //coll.contacts[0].point-transform.forward
            attackHolder.SendMessage("DoAttack2", coll.contacts[0].point );
            Destroy(gameObject);
            hit = true;
            return;
        }
        if (coll.gameObject.tag == "Enemy" && !hit)
        {
            attackHolder.SendMessage("DoAttack2", coll.contacts[0].point - transform.forward);
            Destroy(gameObject);
            hit = true;
            return;
        }

        Destroy(gameObject);

    }

    public void OnTriggerEnter(Collider coll)
    {

        if (coll.tag == "Enemy")
        {
            coll.gameObject.SendMessage("TakeDamage", baseDamage * weaponDamage);
            StartCoroutine("DeathTimer");
        }
        if (coll.tag == "Ground")
        {
            StartCoroutine("DeathTimer");
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
