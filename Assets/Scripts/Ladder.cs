using UnityEngine;
using System.Collections;

public class Ladder : MonoBehaviour {


    public void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            if (Input.GetAxis("Vertical") > 0.1f || Input.GetAxis("Vertical") < -0.1f)
            {
                col.GetComponent<PlayerMovement>().climbing = true;
                col.GetComponent<PlayerMovement>().ladder = transform.gameObject;
            }

        }
    }

    public void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player")
        {
            if (Input.GetAxis("Vertical") > 0.1f || Input.GetAxis("Vertical") < -0.1f)
            {
                col.transform.position = new Vector3(col.transform.position.x, col.transform.position.y, transform.position.z);
                col.GetComponent<PlayerMovement>().ladder = transform.gameObject;
                col.GetComponent<PlayerMovement>().climbing = true;
            }
            
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
