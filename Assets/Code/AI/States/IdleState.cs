using UnityEngine;
using System.Collections;

public class IdleState : State<SmartUnit> {
	
	static IdleState self;
	
	public static IdleState Instance
	{
     get 
      {
         if (self == null)
         {
            self = new IdleState();
         }
         return self;
      }	
	}
	
	public override void Enter (SmartUnit _unit)
	{
		Debug.Log(_unit.name + " Entered Idle State.");
	}
	
	public override void Execute (SmartUnit _unit)
	{
		Debug.Log(_unit.name + " Executing Idle State.");
		
		//Check for any members of opposing team within range.
		//if within range, ChangeState(Combat) and Execute()
		if(_unit.DetectEnemy())
		{
			float distance = _unit.TargetDistance();
			float weaponRange = _unit.primaryWeapon.range * _unit.primaryWeapon.range;
			if(distance < weaponRange)
			{
				_unit.StateMachine.ChangeState(CombatState.Instance);
				_unit.StateMachine.currentState.Execute(_unit);
			}
			else
			{
				_unit.StateMachine.ChangeState(MoveState.Instance);
				_unit.StateMachine.currentState.Execute(_unit);
			}
		}else
		{
			//Couldnt find anyone. Finished turn
			_unit.currentTurn.NoMoreActions();
		}
	}
	
	
	public override void Exit(SmartUnit _unit)
	{
		Debug.Log(_unit.name + " Exiting Idle State.");
	}
}
