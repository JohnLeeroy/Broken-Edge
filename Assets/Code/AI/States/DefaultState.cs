using UnityEngine;
using System.Collections;

public class DefaultState : State<SmartUnit> {

static DefaultState self;
	
	public static DefaultState Instance
	{
     get 
      {
         if (self == null)
         {
            self = new DefaultState();
         }
         return self;
      }	
	}
	
	public override void Enter (SmartUnit _unit) {}

	public override void Execute (SmartUnit _unit) {} 
	
	public override void Exit(SmartUnit _unit) {} 
}
