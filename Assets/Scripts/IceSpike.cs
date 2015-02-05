using UnityEngine;
using System.Collections;

public class IceSpike : Bullet {

    public Light tipLight;

    float duration;
    float currentTime;

    void Start()
    {
        weaponDamage = 3;    
        tipLight = transform.FindChild("TipLight").light;
        particle = transform.FindChild("ParticleStream").particleSystem;
        StartCoroutine("DeathTimer");
    }

    public void Update()
    {
        if (!transform.collider.isTrigger)
        {
            currentTime += Time.deltaTime;
            duration = Time.time;
            float angle = duration / currentTime;
            if (angle >= 15) { angle = 15; }
            if (angle < 0) { angle = 0; }

            transform.Rotate(transform.right, angle / 10, Space.Self);
        }
    }
    public void TurnOffLight() 
    {
        tipLight.enabled = false;
    }

    public override void OnCollisionEnter(Collision coll)
    {
        base.OnCollisionEnter(coll);
        if (coll.gameObject.tag == "Enemy")
        {
            coll.gameObject.SendMessage("TakeDamage", baseDamage * weaponDamage);
            Destroy(gameObject);
        }
        if (coll.gameObject.tag == "Ground")
        {
            attackHolder.SendMessage("DoAttack",transform.position);
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
