using UnityEngine;
using System.Collections;

public class IcePower : Power {

    public GameObject iceObj;
    Vector3 iceDirection;
    float iceSpeed = 200;
    float iceStartDistance = 1.3f;
    float iceSpacing = 0.25f;
    float iceAmount = 10f;
    float iceDistance = 1f;
    float snowTextureAmount;
    RaycastHit hit;
    Vector3 icePos;
    bool attacking = false;

	public override void Start () 
    {
        base.Start();

        powerHolder = transform.gameObject;
        powerHolder.name = "IcePower";


	}
	
    public override void Attack(Transform player)
    {
        if (player.GetComponent<PlayerMovement>())
        {
            gun = player.GetComponent<PlayerMovement>().gun;
            iceDirection = gun.transform.forward;

        }
        else
        {
            return;
        }

        if (!instance)
        {

            instance = Instantiate(iceObj, gun.transform.position - iceDirection / 1.2f, Quaternion.identity) as GameObject;
            instance.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            instance.transform.LookAt(gun.transform.position + (iceDirection * 1.1f));
            instance.rigidbody.useGravity = true;
            instance.rigidbody.AddForce(instance.transform.forward * iceSpeed);
            instance.GetComponent<IceSpike>().attackHolder = player.GetComponent<PlayerMovement>().powerHolder.transform;
            instance.transform.parent = player.GetComponent<PlayerMovement>().powerHolder.transform;


        }
        else
        {
            Destroy(instance);

            instance = Instantiate(iceObj, gun.transform.position - iceDirection / 1.2f, Quaternion.identity) as GameObject;
            instance.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            instance.transform.LookAt(gun.transform.position + (iceDirection * 1.1f));
            instance.rigidbody.useGravity = true;
            instance.rigidbody.AddForce(instance.transform.forward * iceSpeed);
            instance.GetComponent<IceSpike>().attackHolder = player.GetComponent<PlayerMovement>().powerHolder.transform;
            instance.transform.parent = player.GetComponent<PlayerMovement>().powerHolder.transform;
        }
    }


    //bugged, goes through walls
    public IEnumerator DoAttack(Vector3 pos)
    {
        if (attacking) { yield return null; }
        if (powerHolder.transform.childCount > 0)
        {
            foreach (Transform t in powerHolder.transform)
            {
                if (t.collider.isTrigger)
                {
                    Destroy(t.gameObject);
                }
            }
        }
        attacking = true;
        iceDirection = gun.transform.forward;

        icePos = pos + (iceStartDistance / 2) * iceDirection;
        float counter = 0;
        if (Physics.Raycast(new Ray(icePos + transform.up , new Vector3(0, -1, 0)), out hit))
        {                      
            if (hit.transform.tag == "Ground")
            {
                icePos.y = hit.point.y;
                Debug.Log(icePos.y);
            }

        }
        for (int i = 0; i < iceAmount; i++)
        {
            if (i % 2 == 1)
            {

                float scale = counter * 0.1f;
                if (scale >= 0.3f)
                {
                    scale = 0.3f;
                }
  
                Vector3 location = (icePos + (iceStartDistance / 2 + counter) * iceDirection) + iceSpacing * iceDirection;
                location.y = hit.point.y;
                            
                InstantiateIce(location, new Vector3(scale, scale, scale), -10f, -40f);
                
            }

            else if (i % 2 == 0)
            {
                
                float scale = counter * 0.1f;

                if (scale >= 0.3f)
                {
                    scale = 0.3f;
                }

 
                Vector3 location = (icePos + (iceStartDistance / 2 + counter) * iceDirection) + iceSpacing * iceDirection;
                location.y = hit.point.y;
                            
                InstantiateIce(location, new Vector3(scale, scale, scale), 10f, 40f);

            }
            counter++;
            yield return new WaitForFixedUpdate();
        }
        attacking = false;
    }

    void InstantiateIce(Vector3 position, Vector3 scale, float min, float max)
    {
        GameObject obj = Instantiate(iceObj) as GameObject;

        ShapeIce(obj,scale);
            obj.transform.position = position;
            obj.transform.LookAt(obj.transform.position+obj.transform.up);
            obj.transform.Rotate(transform.forward, Random.Range(min, max));
            obj.transform.Rotate(transform.right, Random.Range(0, 45));
        obj.transform.parent = powerHolder.transform;
        obj.transform.collider.isTrigger = true;
        Destroy(obj.rigidbody);       
    }

    public void ShapeIce(GameObject ga ,Vector3 scale)
    {
        ga.SendMessage("ShapeIce", scale);
        
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(icePos + transform.up * 1.5f, -transform.up);
    }
}
