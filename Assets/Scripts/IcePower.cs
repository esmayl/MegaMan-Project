using UnityEngine;
using System.Collections;

public class IcePower : Power {

    public GameObject iceObj;
    float iceSpeed = 200;
    GameObject iceParent;
    float iceStartDistance = 1.3f;
    float iceSpacing = 0.5f;
    float iceAmount = 10f;
    float iceDistance = 1f;
    float snowTextureAmount;
    RaycastHit hit;

	// Use this for initialization
	public override void Start () 
    {
        base.Start();

        iceParent = transform.gameObject;
        iceParent.name = "IcePower";
	}
	
    public override void Attack(Transform player)
    {
        Transform gun;
        gun = player.FindChild("Gun");

        instance = Instantiate(iceObj, gun.transform.position+gun.transform.forward, Quaternion.identity) as GameObject;
        instance.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        instance.transform.rotation = Quaternion.AngleAxis(90, transform.right);

        instance.rigidbody.useGravity = true;
        instance.rigidbody.AddForce(player.transform.forward * iceSpeed);
        instance.GetComponent<IceSpike>().attackHolder = player.GetComponent<PlayerMovement>().powerHolder.transform;
        instance.transform.parent = player.GetComponent<PlayerMovement>().powerHolder.transform;

    }

    public IEnumerator DoAttack(Vector3 pos)
    {
        //Time.timeScale = 0.5f;
        foreach (Transform t in iceParent.transform)
        {
            Destroy(t.gameObject);
        }
        iceParent.transform.position = pos;

        Vector3 tempPos = pos + (iceStartDistance / 2) * transform.forward;

        if (Physics.Raycast(new Ray(tempPos, new Vector3(0, -1, 0)), out hit, LayerMask.NameToLayer("Projectile")))
        {
            if (hit.transform.tag == "Ground")
            {
                tempPos.y = hit.point.y;
            }
        }

        float counter = 0;

        for (int i = 0; i < iceAmount; i++)
        {

            if (i % 2 == 1)
            {

                tempPos = pos + (iceStartDistance / 2 + counter) * transform.forward;

                float scale = counter * 0.1f;
                if (scale >= 0.3f)
                {
                    scale = 0.3f;
                }

                Vector3 location = tempPos + iceSpacing * transform.forward;
                location.y = tempPos.y;
                InstantiateIce(location, new Vector3(scale, scale, scale), -10f, -40f);
            }

            else if (i % 2 == 0)
            {
                tempPos = pos + (iceStartDistance / 2 + counter) * transform.forward;
                
                float scale = counter * 0.1f;

                if (scale >= 0.3f)
                {
                    scale = 0.3f;
                }

                Vector3 location = tempPos + iceSpacing * transform.forward;
                location.y = tempPos.y;
                InstantiateIce(location, new Vector3(scale, scale, scale), 10f, 40f);
            }
            counter++;
            yield return new WaitForEndOfFrame();
        }

        iceParent.transform.rotation = transform.rotation;

        Time.timeScale = 1f;
    }

    void InstantiateIce(Vector3 position, Vector3 scale, float min, float max)
    {
        GameObject obj = Instantiate(iceObj) as GameObject;

        ShapeIce(obj,scale);
            obj.transform.position = position - new Vector3(0,obj.collider.bounds.extents.y*2,0);
            obj.transform.Rotate(transform.forward, Random.Range(min, max));
            obj.transform.Rotate(transform.right, Random.Range(0, 45));
        obj.transform.parent = iceParent.transform;
        obj.transform.SendMessage("TurnOffLight");
        obj.transform.collider.isTrigger = true;
        obj.rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        obj.GetComponent<IceSpike>().player = transform;

        value = 0.1f;
    }

    public void ShapeIce(GameObject ga ,Vector3 scale)
    {
        ga.SendMessage("ShapeIce", scale);
        
    }
}
