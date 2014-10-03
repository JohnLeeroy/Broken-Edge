using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class WeaponBuilder : MonoBehaviour {

		
	class WeaponData
	{
		public IDictionary data;
		
		public WeaponData(Weapon weapon)
		{
			data = new Dictionary<string, string>();
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
		}
		
	}
	
	List<Weapon> weapons;
	void Awake()
	{
		weapons = new List<Weapon>();
		
		DamageDice AutoPistol_S = new DamageDice(2, 1, 6);
		DamageDice AutoPistol_M = new DamageDice(2, 3, 6);
		Weapon AutoPistol = new Weapon(10, "AutoPistol", Item.Types.WEAPON, AutoPistol_S, AutoPistol_M, 1, Weapon.Slot.PRIMARY, 60, 6);
		
		DamageDice AutoRifle_S = new DamageDice(2, 1, 6);
		DamageDice AutoRifle_M = new DamageDice(2, 3, 6);
		Weapon AutoRifle = new Weapon(11, "AutoRifle", Item.Types.WEAPON, AutoRifle_S, AutoRifle_M, 2, Weapon.Slot.PRIMARY, 200, 12);
		
		DamageDice Shotgun_S = new DamageDice(0, 4, 6);
		Weapon Shotgun = new Weapon(12, "Shotgun", Item.Types.WEAPON, Shotgun_S, null, 2, Weapon.Slot.PRIMARY, 40, 6);
		
		DamageDice HuntingRifle_S = new DamageDice(3, 1, 10);
		Weapon HuntingRifle = new Weapon(13, "HuntingRifle", Item.Types.WEAPON, HuntingRifle_S, null, 2, Weapon.Slot.PRIMARY, 200, 6);	
		
		DamageDice SniperRifle_S = new DamageDice(5, 1, 10);
		Weapon SniperRifle = new Weapon(14, "SniperRifle", Item.Types.WEAPON, SniperRifle_S, null, 2, Weapon.Slot.PRIMARY, 400, 5);
		
		WeaponData AutoPistol_Data = new WeaponData(AutoPistol);
		WeaponData AutoRifle_Data = new WeaponData(AutoRifle);
		WeaponData Shotgun_Data = new WeaponData(AutoRifle);
		WeaponData HuntingRifle_Data = new WeaponData(HuntingRifle);
		WeaponData SniperRifle_Data = new WeaponData(SniperRifle);
		
		List<IDictionary> weaponList = new List<IDictionary>();
		weaponList.Add(AutoPistol_Data.data);
		weaponList.Add(AutoRifle_Data.data);
		weaponList.Add(Shotgun_Data.data);
		weaponList.Add(HuntingRifle_Data.data);
		weaponList.Add(SniperRifle_Data.data);
		
		StreamWriter writer = new StreamWriter(Application.dataPath + "/Data/Weapons.json");
		string serial = MiniJSON.Json.Serialize(weaponList);
		writer.WriteLine(serial);
		writer.Flush();
		writer.Close();
		print(serial);
	}
	
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
