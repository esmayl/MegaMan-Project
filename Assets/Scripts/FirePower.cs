using UnityEngine;
using System.Collections;

public class FirePower : Power {

    public GameObject fireObject;

    public override void Start()
    {
        base.Start();
    }

    public override void Attack(Transform player)
    {

        Transform gun;
        gun = player.GetComponent<PlayerMovement>().gun.transform;

        Vector3 tempPos = gun.position+gun.transform.forward;
        tempPos.x = player.position.x;

        if (tempPos.y < player.position.y )
        {
            Debug.Log(tempPos);
            return;
        }
        else
        {
            instance = Instantiate(fireObject) as GameObject;
            instance.transform.position = tempPos;

            instance.transform.LookAt(gun.transform.position + (gun.transform.forward * 1.2f));

            instance.rigidbody.AddForce(instance.transform.forward * speed);

            instance.GetComponent<FireBomb>().attackHolder = player.GetComponent<PlayerMovement>().powerHolder.transform;
            instance.transform.parent = player.GetComponent<PlayerMovement>().powerHolder.transform;
        }
    }
}
