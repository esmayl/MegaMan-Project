using UnityEngine;
using System.Collections;

public class Explosion : Bullet
 {
    public float life = 5;
    void Start()
    {
        weaponDamage = 10;
        StartCoroutine("DeathTimer");
    }

    public override IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(life);
        Destroy(transform.parent.gameObject);
    }

    public void OnParticleCollision(GameObject other)
    {
        if (other.layer == LayerMask.NameToLayer("Enemy"))
        {
            other.SendMessage("TakeDamage",(baseDamage *weaponDamage));
        }
    }
}
