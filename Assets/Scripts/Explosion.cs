using UnityEngine;
using System.Collections;

public class Explosion : Bullet
 {
    void Start()
    {
        lifeTime = 2;
        weaponDamage = 10;
        StartCoroutine("DeathTimer");
    }

    public void OnParticleCollision(GameObject other)
    {
        if (other.layer == LayerMask.NameToLayer("Enemy"))
        {
            Debug.LogError("object dmg - 10 = " +other.name);
            other.SendMessage("TakeDamage",baseDamage *weaponDamage);
        }
    }
}
