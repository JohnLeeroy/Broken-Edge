using UnityEngine;
using System.Collections;

public abstract class CombatAction : Action 
{

	protected CombatAction(int _cost) : base(_cost) { }
	public abstract void Execute(Unit _unit, Unit _target);
}
