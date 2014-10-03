using UnityEngine;
using System.Collections;

public class Item {
	public enum Types{WEAPON, ARMOR, QUEST, CONSUMABLE};
	public int id;
	public string name;
	public Types type;
	
	public Item(int ID, string Name, Types ItemType)
	{
		id = ID;
		name = Name;
		type = ItemType;
	}
}
