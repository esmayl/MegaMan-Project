using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

    public float hp = 100;

	public virtual void Update () {

        if (hp <= 0)
        {
            Destroy(gameObject);
        }

	}

    public void TakeDamage(float damage)
    {
        hp -= damage;
    }

    public virtual void Jump() { }
    public virtual void MoveForward() { }
    public virtual void MoveTo(Transform target) { }
    public virtual void LookAtPlayer(Vector3 PlayerPos) { }
    public virtual void Attack(Vector3 Direction) { }
    public IEnumerator DetectPlayer(float Radius) 
    {
        while (true)
        {
            Collider[] hits;
            hits = Physics.OverlapSphere(transform.position, Radius);
            if (hits.Length > 0)
            {
                Debug.Log("Looping");
                foreach (Collider h in hits)
                {
                    if (h.gameObject.layer == LayerMask.NameToLayer("Player"))
                    {
                        Debug.Log("" + h);
                    }
                }
            }

            yield return new WaitForSeconds(1f);
        }
    }

    /// <summary>
    /// only use in air AKA flying enemy
    /// </summary>
    public virtual void MoveUp() { }
}
