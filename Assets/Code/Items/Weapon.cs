using UnityEngine;
using System.Collections;

public class DamageDice
{
	public int baseDamage;
	public int numDices;
	public int dice;
	
	public DamageDice(int BaseDamage, int NumDices, int Dice)
	{
		baseDamage = BaseDamage;
		numDices = NumDices;
		dice = Dice;
	}
	
	public int RollDamage()
	{
		int totalDamage = baseDamage;
		for(int i = 0; i < numDices; i++)
			totalDamage += Random.Range(1, dice);
				
		return totalDamage;
	}
}

public class Weapon : Item {
	
	public enum Slot{PRIMARY, SECONDARY};
	public enum RangeTypes{NORMAL, SPECIAL};
	public RangeTypes rangeType;
	public const int singleShotCost = 1;
	public const int multiShotCost = 2;
	
	public int clipSize = 30;
	public int ammo = 30;
	public int range;
	public int damage;
	public int diceCount;
	public DamageDice singleShot;
	public DamageDice multiShot;
	
	public Slot slot;
	
	public int[] rof;
	
	public int hands; //hands required
	
	public Weapon(int ID, string Name, Types ItemType, DamageDice single, DamageDice multiple, int Hands, Slot ItemSlot, int Range, int ClipSize) :base(ID, Name, ItemType)
	{
		hands = Hands;
		rangeType = RangeTypes.NORMAL;
		range = Range;
		clipSize = ClipSize;
		singleShot = single;
		multiShot = multiple;
	}
	
	public int GetSingleShotDamage()
	{
		return singleShot.RollDamage();
	}
	
	public int GetMultiShotDamage()
	{
		return multiShot.RollDamage();
	}
	
	public void ShootSingleShot()
	{
		ammo -= singleShotCost;	
	}
	
	public void ShootMultiShot()
	{
		ammo -= multiShotCost;	
	}
	
	public int GetReloadActions()
	{
		//TODO
		return 1;	
	}
	
	public void Reload()
	{
		ammo = clipSize;	
	}
}
