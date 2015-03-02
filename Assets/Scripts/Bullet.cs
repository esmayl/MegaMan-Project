using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public ParticleSystem particle;
    internal float baseDamage=1f;
    public float weaponDamage = 1f;
    internal Transform attackHolder;
    internal float lifeTime = 5;
    public int bulletSpeed = 5;

    void Start()
    {
        StartCoroutine("DeathTimer");
    }

    public virtual void FixedUpdate()
    {
        rigidbody.velocity = transform.forward * bulletSpeed;
    }
    public virtual void OnCollisionEnter(Collision coll)
    {

        if (coll.gameObject.tag == "Enemy")
        {
            coll.gameObject.SendMessage("TakeDamage", baseDamage * weaponDamage);
            Destroy(gameObject);
        }
        if (gameObject.layer == LayerMask.NameToLayer("EnemyProjectiles"))
        {
            if (coll.gameObject.tag == "Player")
            {
                coll.gameObject.SendMessage("TakeDamage", baseDamage * weaponDamage);
                Destroy(gameObject);
            }
        }
        if(coll.gameObject.tag == "Ground")
        {
            Destroy(gameObject);
        }
    }

    public virtual IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    public void AddChargeDmg(float chargeDmg)
    {
        weaponDamage += chargeDmg*10;
    }

}
