using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

    public GameObject player;
    public float hp = 100;
    public float speed = 3;
    public float meleeDamage = 4f;

	public virtual void Update () {

        if (hp <= 0)
        {
            Destroy(gameObject);
        }

        if (player != null)
        {
            MoveTo(player.transform);
        }

	}

    public void TakeDamage(float damage)
    {
        hp -= damage;
    }

    public virtual void Jump() { }
    public virtual void MoveForward() { }
    public void MoveTo(Transform target) 
    {
        transform.LookAt(new Vector3(transform.position.x, transform.position.y, target.position.z));
        rigidbody.velocity = transform.forward * speed/2.5f;
    }
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
                        player = h.gameObject;
                        if (Mathf.Abs(Vector3.Distance(player.transform.position, transform.position)) < 1.5f)
                        {
                            player.gameObject.GetComponent<PlayerMovement>().TakeDamage(meleeDamage);
                        }
                        MoveTo(player.transform);
                        
                        Debug.Log("" + h);
                    }
                }
            }
            else
            {
                player = null;
            }
            yield return new WaitForSeconds(1f);
        }
    }

    /// <summary>
    /// only use in air AKA flying enemy
    /// </summary>
    public virtual void MoveUp() { }
}
