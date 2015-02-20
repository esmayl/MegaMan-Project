using UnityEngine;
using System.Collections;

public class IceSpike : Bullet {

    float duration;
    float currentTime;

    void Start()
    {
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
            attackHolder.SendMessage("DoAttack",transform.position);
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "Enemy")
        {
            coll.gameObject.SendMessage("TakeDamage", baseDamage * weaponDamage);
        }
    }

    public void ShapeIce(Vector3 scale)
    {
        transform.localScale = scale;
    }
}
