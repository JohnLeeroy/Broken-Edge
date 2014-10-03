using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class ItemManager : MonoBehaviour {
	
	public static ItemManager self;
	List<Item> lItems;
	
	void Awake()
	{
		if(self != null)
		{
			Destroy(gameObject);
			return;
		}
		self = this;
		
		lItems = new List<Item>();
		lItems.Add(new Armor(1, "Iron Cap", Item.Types.ARMOR, 2, Armor.Slot.HEAD));
		lItems.Add(new Armor(2, "Iron Plate", Item.Types.ARMOR, 2, Armor.Slot.CHEST));
		//lItems.Add(new Armor(3, "Iron Gauntlet", Item.Types.ARMOR, 2, Armor.Slot.ARMS));
		//lItems.Add(new Armor(4, "Iron Boots", Item.Types.ARMOR, 2, Armor.Slot.LEGS));
		
		LoadWeaponData();
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public Item GetItem(int id)
	{
		//OPTIMIZE
		foreach(Item item in lItems)
		{
			if(item.id == id)
				return item;	
		}
		Debug.Log("Cant find item");
		return null;
	}
	
	void LoadWeaponData()
	{
		string weaponData;
		StreamReader reader = new StreamReader(	Application.dataPath + "/Data/Weapons.json");
			weaponData = reader.ReadToEnd();
		reader.Close();
		
		IList weaponJSON = (IList) MiniJSON.Json.Deserialize(weaponData);
		print("Weapon.json: Weapon Count " + weaponJSON.Count);
		foreach (IDictionary _weapon in weaponJSON) {
			int id = Convert.ToInt32(_weapon["id"]);
			string name = (string)_weapon["name"];
			
			int range = Convert.ToInt32(_weapon["range"]);
			int hands = Convert.ToInt32(_weapon["hands"]);
			int clipSize = Convert.ToInt32(_weapon["clip"]);
			
			Weapon.Slot slot = Weapon.Slot.PRIMARY;
			if(Convert.ToString(_weapon["slot"]) == "PRIMARY")
				slot = Weapon.Slot.PRIMARY;
			else if(Convert.ToString(_weapon["slot"]) == "SECONDARY")
				slot = Weapon.Slot.SECONDARY;
			
			int DDS_dmg = Convert.ToInt32(_weapon["DDS_dmg"]);
			int DDS_numDice = Convert.ToInt32(_weapon["DDS_numDice"]);
			int DDS_Dice = Convert.ToInt32(_weapon["DDS_Dice"]);
			DamageDice single = new DamageDice(DDS_dmg, DDS_numDice, DDS_Dice);
			DamageDice multiple = null;
			if(_weapon["DDM_dmg"] != null)
			{
				int DDM_dmg = Convert.ToInt32(_weapon["DDM_dmg"]);
				int DDM_numDice = Convert.ToInt32(_weapon["DDM_numDice"]);
				int DDM_Dice = Convert.ToInt32(_weapon["DDM_Dice"]);
			 	multiple = new DamageDice(DDM_dmg, DDM_numDice, DDM_Dice);
			}
			
			lItems.Add(new Weapon(id, name, Item.Types.WEAPON, single, multiple, hands, slot, range, clipSize));
			print("Loaded Weapon: " + name);
		} 	
	}
}

/*
 * 			data = new Dictionary<string, string>();
			data.Add("id", weapon.id.ToString());
			data.Add("name", weapon.name);
			data.Add("type", weapon.type.ToString());
			data.Add("hands", weapon.hands.ToString());
			data.Add("slot", weapon.slot.ToString());
			data.Add("range", weapon.range.ToString());
			data.Add("clip", weapon.clipSize.ToString());
			data.Add("DDS_dmg", weapon.singleShot.baseDamage.ToString());
			data.Add("DDS_numDice", weapon.singleShot.numDices.ToString());
			data.Add("DDS_Dice", weapon.singleShot.dice.ToString());
			if(weapon.multiShot != null)
			{
				data.Add("DDM_dmg", weapon.multiShot.baseDamage.ToString());
				data.Add("DDM_numDice", weapon.multiShot.numDices.ToString());
				data.Add("DDM_Dice", weapon.multiShot.dice.ToString());
			}
 */
