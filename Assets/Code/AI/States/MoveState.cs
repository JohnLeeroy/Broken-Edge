using UnityEngine;
using System.Collections;

public class MoveState : State<SmartUnit> {
	
	static MoveState self;
	
	public static MoveState Instance
	{
     get 
      {
         if (self == null)
         {
            self = new MoveState();
         }
         return self;
      }	
	}
	
	public override void Enter (SmartUnit _unit)
	{
		Debug.Log(_unit.name + " Entering Move State.");
	}
	
	public override void Execute (SmartUnit _unit)
	{
		Debug.Log(_unit.name + " Executing Move State.");
		
		
	
	}
	
	
	public override void Exit(SmartUnit _unit)
	{
		Debug.Log(_unit.name + " Exiting Combat State.");
	}
}
