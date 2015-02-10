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
    public virtual void LookAtPlayer(Vector3 PlayerPos) { }
    public virtual void Attack(Vector3 Direction) { }
    public virtual void DetectPlayer(float Radius) 
    {
        //Insert overlapSphereCast 
    }

    /// <summary>
    /// only use in air AKA flying enemy
    /// </summary>
    public virtual void MoveUp() { }
}
