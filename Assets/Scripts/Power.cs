using UnityEngine;
using System.Collections;

public class Power : MonoBehaviour {

    public Texture2D armorTexture;
    public Texture2D attackTexture;
    public GameObject gun;
    public ParticleSystem shotParticle;
    public int speed;
    internal float value;
    internal GameObject instance;
    internal GameObject powerHolder;    

    float strength = 1;
    float mpCost = 10;

	// Use this for initialization
	public virtual void Start () {
        value = 0.1f;
	
	}

    public virtual void Attack(Transform player)
    {

    }
}
