using UnityEngine;
using System.Collections;

public class Buster : Power {

    public GameObject bullet;

	public override void Start () {
        base.Start();
        transform.name = "BusterPower";
	}

    public override void Attack(Transform player)
    {
        if (value >= 2) { value = 2f; }

        value = 0.5f;

        Transform gun;
        gun = player.GetComponent<PlayerMovement>().gun.transform;

        instance = Instantiate(bullet, gun.transform.position + gun.transform.forward, Quaternion.identity) as GameObject;
        instance.transform.localScale = new Vector3(value, value, value);

        instance.rigidbody.useGravity = false;
        instance.rigidbody.velocity = player.transform.forward * speed;
        instance.SendMessage("AddChargeDmg", value);
        instance.transform.parent = player.GetComponent<PlayerMovement>().powerHolder.transform;
        instance.GetComponent<Bullet>().attackHolder = player.GetComponent<PlayerMovement>().powerHolder.transform;
    }
}
