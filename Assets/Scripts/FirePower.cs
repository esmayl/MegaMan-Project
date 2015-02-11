using UnityEngine;
using System.Collections;

public class FirePower : Power {

    public GameObject fireObject;

    public override void Attack(Transform player)
    {
        Transform gun;
        gun = player.GetComponent<PlayerMovement>().gun.transform;

        Vector3 tempPos = gun.transform.position + (gun.transform.forward + gun.transform.up);
        tempPos.x = player.position.x;
        instance = Instantiate(fireObject, tempPos, Quaternion.identity) as GameObject;
        instance.transform.rotation = Quaternion.AngleAxis(-45, transform.right);

        instance.rigidbody.AddForce(instance.transform.forward*speed);
        instance.GetComponent<FireBomb>().attackHolder = player.GetComponent<PlayerMovement>().powerHolder.transform;
        instance.transform.parent = player.GetComponent<PlayerMovement>().powerHolder.transform;
    }
}
