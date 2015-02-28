using UnityEngine;
using System.Collections;

public class Megaman : PlayerMovement {

    public KeyCombo combo1;
    public KeyCombo combo2;
    public KeyCombo combo3;

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        if (combo1.Check())
        {
            activePower = powers[0];
            activePower.Attack(transform);
        }
        if (combo2.Check())
        {
            activePower = powers[1];
            if (UseMP(activePower.mpCost))
            {
                activePower.Attack(transform);
            }
        }
        if (combo3.Check())
        {
            activePower = powers[2];
            if (UseMP(activePower.mpCost))
            {
                activePower.Attack(transform);
            }
        }
        base.Update();
    }

    public override void FixedUpdate() 
    {
        base.FixedUpdate();
	}
}
