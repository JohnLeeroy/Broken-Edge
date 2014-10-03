using UnityEngine;
using System.Collections;

public class CombatHub : StateHub<SmartUnit> {
	
	static CombatHub instance;
	
	public static CombatHub Instance
	{
     get 
      {
         if (instance == null)
         {
            instance = new CombatHub();
         }
         return instance;
      }	
	}
	
	public override void Enter (SmartUnit _unit)
	{
		//Do some calculations to figure out which state the unit will enter next
		/*  Possible Actions
		 *		FireSingle    
		 * 		FireMulti
		 * 		Reload
		 * 		CloseIn
		 * 		Retreat
		 */
		
		State<SmartUnit> nextState;
		
		Debug.Log(_unit.charName + " is entering combat hub");
		
		//Is enemy in range?
		
		//Is Unit in cover?
		/*if(false)
			nextState = DefaultState.Instance; 	
		else if(true)
			nextState = IdleState.Instance;	
		
		_unit.StateMachine.ChangeState(nextState);
		*/
	}
}
