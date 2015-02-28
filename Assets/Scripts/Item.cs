﻿using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Item : MonoBehaviour {

    public string itemName;
    public int gainAmount;
    public ItemType itemType;
    public bool pickedUp;
    bool startAnimation;
    Vector3 startPos;

	// Use this for initialization
	void Start () {
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
        }
    }
}
