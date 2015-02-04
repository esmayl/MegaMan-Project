using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public ParticleSystem particle;
    public Transform player;
    internal float baseDamage=1f;
    internal float weaponDamage = 1f;
    internal Transform attackHolder;


    void Start()
    {
        particle = transform.GetChild(0).GetComponent<ParticleSystem>();
        particle.enableEmission = false;
        StartCoroutine("DeathTimer");
    }

    public virtual void OnCollisionEnter(Collision coll)
    {
        
        if (coll.gameObject.tag == "Enemy")
        {
            particle.enableEmission = true;
            particle.Emit(100);
            coll.gameObject.SendMessage("TakeDamage",baseDamage * weaponDamage);
            Destroy(this.gameObject,0.2f);
            particle.enableEmission = false;
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
