using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum ItemType
{
    hp,mp,score
}


public class ItemDatabase : MonoBehaviour {

    public static Dictionary<string, Item> lootTable = new Dictionary<string,Item>();

    [SerializeField]
    public Dictionary<string, Item> publicLootTable = new Dictionary<string, Item>(3);

	// Use this for initialization
	void Start () {

        Item loot = new Item();
        loot.itemName = "Potion";
        loot.gainAmount = 20;
        loot.itemType = ItemType.hp;

        ItemDatabase.AddToList("BaseEnemy10", loot);
	

	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public static void AddToList(string enemyName, Item item)
    {
        if (lootTable.ContainsKey(enemyName))
        {
            Debug.Log(enemyName);
            lootTable[enemyName] = item;
            return;
        }
        lootTable.Add(enemyName, item);
    }
    public static void DropItem(Vector3 deathPosition, string name)
    {
        GameObject loot = GameObject.CreatePrimitive(PrimitiveType.Cube);
        loot.transform.position = deathPosition;
        loot.name = lootTable[name].itemName;

        loot.AddComponent<Item>().gainAmount = lootTable[name].gainAmount;
        loot.GetComponent<Item>().itemName = lootTable[name].itemName;
        loot.GetComponent<Item>().itemType = lootTable[name].itemType;

        loot.collider.isTrigger = true;
        
    }
}
