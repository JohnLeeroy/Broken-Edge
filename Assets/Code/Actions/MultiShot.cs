using UnityEngine;
using System.Collections;

public class MultiShot : CombatAction {
	
	static MultiShot instance;
	
	public static MultiShot Instance
	{
     get 
      {
         if (instance == null)
         {
            instance = new MultiShot();
         }
         return instance;
      }	
	}
	
	private MultiShot() : base(2) 
	{
	}
	
	
	public override void Execute(Unit _unit, Unit _target)
	{
		Debug.Log("Resolving Combat");
		_unit.selectedWeapon.ShootMultiShot();
		if(_unit.RollToHit())
		{
			int damage = 0;
			damage = _unit.primaryWeapon.GetMultiShotDamage();
			
			int targetArmor = _target.GetTotalArmor();
			
			_target.health -= (damage - targetArmor);
			
			if(_target.health <= 0)
			{
				_target.Died();	
			}
			
			if(damage > 0)
				_target.StartDamageEffect(damage);
			
			Debug.Log("Damage Roll: " + damage);
			Debug.Log("Target Armor: " + targetArmor);
			Debug.Log("Total Damage: " + (damage - targetArmor));
			Debug.Log("Target's Remaining Health: " + _target.health);
		}
		else
		{
			Debug.Log("Unit Missed!");
			_unit.StartMissEffect();
		}
	}
}
