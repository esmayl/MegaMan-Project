using UnityEngine;
using System.Collections;

public class FireBomb : Bullet {

    GameObject explosion;
    float fireCounter;

	// Use this for initialization
	void Start () {
	    StartCoroutine("DoExplosion");
	}
	
    public IEnumerator DoExplosion()
    {
        yield return new WaitForSeconds(0.5f);
        explosion = Instantiate(particle, transform.position, Quaternion.identity) as GameObject;
        Destroy(gameObject);
    }
}
