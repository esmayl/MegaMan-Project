using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Item : MonoBehaviour {

    public string itemName;
    public int gainAmount;
    public ItemType itemType;
    public bool pickedUp;
    bool startAnimation;
    public GameObject mesh;
    public Material meshMaterial;
    Vector3 startPos;

	void Start () {
        renderer.material = meshMaterial;
        startPos = transform.position;
        startAnimation = true;

        pickedUp = false;
	}

    public void Update()
    {
        if (startAnimation)
        {
            transform.Translate(transform.up * Time.deltaTime);

            if (transform.position.y > startPos.y + 1) { startAnimation = false; }
        }
    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player" && !pickedUp)
        {
            col.GetComponent<PlayerMovement>().UseItem(this);
            pickedUp = true;
            transform.collider.enabled = false;
            StartCoroutine("PickedUp");
        }
    }

    public IEnumerator PickedUp()
    {
        while (renderer.material.color.a > 0.01f)
        {
            renderer.material.color -= new Color(0, 0, 0, 0.05f);
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
}
