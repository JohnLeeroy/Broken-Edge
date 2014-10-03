using UnityEngine;
using System.Collections;

public class Armor :  Item {
	
	public enum Slot{HEAD, CHEST, ACCESSORY_1, COUNT};
	public int defense;
	
	public Slot slot;
	public Armor(int ID, string Name, Types ItemType, int Defense, Slot ItemSlot) :base(ID, Name, ItemType)
	{
		defense = Defense;
		slot = ItemSlot;
	}

	public Armor(int ID, string Name, int Defense) : base(ID, Name, Types.ARMOR){}

	//Default Object 
	private static Armor noArmor;
	public static Armor None
	{
		get{
			if(noArmor == null)		
				noArmor = new Armor(-1, "None", 0);

			return noArmor;
		}
	}
}
