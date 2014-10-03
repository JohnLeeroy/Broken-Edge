using UnityEngine;
using System.Collections;

public abstract class Action {
	
	public int cost;
	
	public Action(int _cost)
	{
		cost = _cost;
	}
	
	public virtual void Execute() {}

}
