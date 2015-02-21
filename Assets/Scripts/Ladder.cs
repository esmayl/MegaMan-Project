using UnityEngine;
using System.Collections;

public class Ladder : MonoBehaviour {


    public void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player")
        {
            col.GetComponent<PlayerMovement>().climbing = true;
            col.GetComponent<PlayerMovement>().canJump = false;
        }
    }

    public void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            col.GetComponent<PlayerMovement>().climbing = false;
        }
    }
}
