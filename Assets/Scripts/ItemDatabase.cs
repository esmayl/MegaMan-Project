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
        loot.itemName = "Health Potion";
        loot.gainAmount = 20;
        loot.itemType = ItemType.hp;

        Item loot2 = new Item();
        loot.itemName = "Score";
        loot.gainAmount = 20;
        loot.itemType = ItemType.score;

        Item loot3 = new Item();
        loot.itemName = "Mana Potion";
        loot.gainAmount = 20;
        loot.itemType = ItemType.mp;

        ItemDatabase.AddToList("BaseEnemy10", loot);
        ItemDatabase.AddToList("BaseEnemy30", loot2);
        ItemDatabase.AddToList("BaseEnemy20", loot3);
        ItemDatabase.AddToList("Turret", loot2);
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
