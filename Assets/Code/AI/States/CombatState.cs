using UnityEngine;
using System.Collections;

public class CombatState : State<SmartUnit> {
	
	static CombatState self;
	
	public static CombatState Instance
	{
     get 
      {
         if (self == null)
         {
            self = new CombatState();
         }
         return self;
      }	
	}
	
	public override void Enter (SmartUnit _unit)
	{
		Debug.Log(_unit.name + " Entering Combat State.");
	}
	
	public override void Execute (SmartUnit _unit)
	{
		Debug.Log(_unit.name + " Executing Combat State.");
		bool bTookAction = true;
		//loop until no turns left or no action was found appropriate
		while(_unit.currentTurn.actionsLeft > 0 && bTookAction)
		{
			//TODO check ammo
			bTookAction = false;
			if(Random.Range(0,10) > 5)
			{
				_unit.ShootSingle(null);
				bTookAction = true;
			}
			else
			{
				_unit.ShootMulti(null);
				bTookAction = true;	
			}
		}
		//May need to change if enter different state
		_unit.currentTurn.NoMoreActions();
	}
	
	
	public override void Exit(SmartUnit _unit)
	{
		Debug.Log(_unit.name + " Exiting Combat State.");
	}
}
