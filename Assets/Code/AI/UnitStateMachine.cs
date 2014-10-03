using UnityEngine;
using System.Collections;

public class UnitStateMachine : StateMachine<SmartUnit> {

	public UnitStateMachine(SmartUnit _owner) : base(_owner)
	{
		 currentState = DefaultState.Instance;
	}
}
