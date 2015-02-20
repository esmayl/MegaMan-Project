using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public ParticleSystem particle;
    internal float baseDamage=1f;
    internal float weaponDamage = 1f;
    internal Transform attackHolder;


    void Start()
    {
        StartCoroutine("DeathTimer");
    }

    public virtual void OnCollisionEnter(Collision coll)
    {

        if (coll.gameObject.tag == "Enemy")
        {
            coll.gameObject.SendMessage("TakeDamage", baseDamage * weaponDamage);
            Destroy(gameObject);
        }
        if(coll.gameObject.tag == "Ground")
        {
            Destroy(gameObject);
        }
    }

    public virtual IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

    public void AddChargeDmg(float chargeDmg)
    {
        weaponDamage += chargeDmg*10;
    }

}
