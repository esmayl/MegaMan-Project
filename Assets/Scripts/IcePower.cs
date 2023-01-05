using UnityEngine;
using System.Collections;

public class IcePower : Power {

    public GameObject iceObj;
    public GameObject iceExplosion;
    Vector3 iceDirection;
    float iceSpeed = 200;
    float iceStartDistance = 1.3f;
    float iceSpacing = 0.25f;
    float iceAmount = 10f;
    float iceDistance = 1f;
    float snowTextureAmount;
    RaycastHit hit;
    Vector3 icePos;
    bool instancing = false;

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
            instance.GetComponent<Rigidbody>().useGravity = true;
            instance.GetComponent<Rigidbody>().AddForce(instance.transform.forward * iceSpeed);
            instance.GetComponent<IceSpike>().attackHolder = player.GetComponent<PlayerMovement>().powerHolder.transform;
            instance.transform.parent = player.GetComponent<PlayerMovement>().powerHolder.transform;


        }
        else
        {
            Destroy(instance);

            instance = Instantiate(iceObj, gun.transform.position - iceDirection / 1.2f, Quaternion.identity) as GameObject;
            instance.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            instance.transform.LookAt(gun.transform.position + (iceDirection * 1.1f));
            instance.GetComponent<Rigidbody>().useGravity = true;
            instance.GetComponent<Rigidbody>().AddForce(instance.transform.forward * iceSpeed);
            instance.GetComponent<IceSpike>().attackHolder = player.GetComponent<PlayerMovement>().powerHolder.transform;
            instance.transform.parent = player.GetComponent<PlayerMovement>().powerHolder.transform;
        }
    }


    //bugged, goes through walls
    public IEnumerator DoAttack(Vector3 pos)
    {

        if (powerHolder.transform.childCount > 0)
        {
            foreach (Transform t in powerHolder.transform)
            {
                if (t.GetComponent<Collider>().isTrigger)
                {
                    Destroy(t.gameObject);
                }
            }
        }

        iceDirection = gun.transform.forward;

        icePos = pos + (iceStartDistance / 2) * iceDirection;
        float counter = 0;
        if (Physics.Raycast(new Ray(icePos + transform.up , new Vector3(0, -1, 0)), out hit,LayerMask.NameToLayer("Ground")))
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
                pos.y = hit.point.y;
                            
                InstantiateIce(iceDirection,pos, new Vector3(scale, scale, scale), -5f, -10f);
                
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

                            
                InstantiateIce(iceDirection,pos, new Vector3(scale, scale, scale), 5f, 10f);

            }
            counter++;
            yield return new WaitForFixedUpdate();
        }
    }

    public IEnumerator DoAttack2(Vector3 center)
    {
        GameObject test = Instantiate(iceExplosion) as GameObject;
        test.transform.position = center;
        yield return new WaitForSeconds(0.27f);
        Destroy(test);
        yield return null;
    }

    void InstantiateIce(Vector3 direction , Vector3 position, Vector3 scale, float min, float max)
    {
        GameObject obj = Instantiate(iceObj) as GameObject;

        ShapeIce(obj,scale);
            obj.transform.position = position;
            obj.transform.Rotate(transform.forward, Random.Range(min, max));
            if (direction.z >0.1f)
            {
                obj.transform.Rotate(transform.right, Random.Range(0, -90));
            }
            else
            {
                obj.transform.Rotate(transform.right, Random.Range(-180, -90));
            }
        obj.transform.parent = powerHolder.transform;
        obj.transform.GetComponent<Collider>().isTrigger = true;
        Destroy(obj.GetComponent<Rigidbody>());       
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
