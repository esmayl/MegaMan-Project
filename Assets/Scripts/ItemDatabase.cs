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
    Item loot = new Item();
    Item loot2 = new Item();
    Item loot3 = new Item();

	// Use this for initialization
	void Start () {

        loot.itemName = "Health Potion";
        loot.gainAmount = 20;
        loot.itemType = ItemType.hp;
        loot.meshMaterial = new Material(Shader.Find("Transparent/Diffuse"));

        loot2.itemName = "Score";
        loot2.gainAmount = 20;
        loot2.itemType = ItemType.score;
        loot2.meshMaterial = new Material(Shader.Find("Transparent/Diffuse"));

        loot3.itemName = "Mana Potion";
        loot3.gainAmount = 50;
        loot3.itemType = ItemType.mp;
        loot3.meshMaterial = new Material(Shader.Find("Transparent/Diffuse"));

        ItemDatabase.AddToList("MeleeEnemy", loot);
        ItemDatabase.AddToList("Turret", loot2);
        ItemDatabase.AddToList("RangedEnemy", loot3);
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
        loot.GetComponent<Item>().meshMaterial = lootTable[name].meshMaterial;
        loot.GetComponent<Collider>().isTrigger = true;
        
    }
}
