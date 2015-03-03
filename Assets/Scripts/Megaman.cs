using UnityEngine;
using System.Collections;

public class Megaman : PlayerMovement {

    public KeyCombo combo1;
    public KeyCombo combo2;
    public KeyCombo combo3;
    public KeyCombo combo4;

    public override void Start()
    {
        base.Start();
        if (transform.name.Contains("(Clone)"))
        {
            transform.name.Remove(transform.name.IndexOf("("));
        }
    }

    public override void Update()
    {
        base.Update();



        if (Input.GetButtonDown("Fire1"))
        {
            activePower = powers[0];
            if (powerHolder.name != activePower.name) { Destroy(powerHolder); powerHolder = Instantiate(activePower.gameObject, transform.position, Quaternion.identity) as GameObject; }
            activePower.Attack(transform);
        }

        if (combo2.Check())
        {
            activePower = powers[1];
            if (UseMP(activePower.mpCost))
            {
                if (powerHolder.name != activePower.name) { Destroy(powerHolder); powerHolder = Instantiate(activePower.gameObject, transform.position, Quaternion.identity) as GameObject; }

                activePower.Attack(transform);
            }
        }
        if (combo3.Check())
        {
            activePower = powers[2];
            if (UseMP(activePower.mpCost))
            {
                if (powerHolder.name != activePower.name) { Destroy(powerHolder); powerHolder = Instantiate(activePower.gameObject, transform.position, Quaternion.identity) as GameObject; }

                activePower.Attack(transform);
            }
        }
    }

    public override void FixedUpdate() 
    {
        base.FixedUpdate();
	}
}
