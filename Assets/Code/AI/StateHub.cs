using UnityEngine;
using System.Collections;

public abstract class StateHub<T>
{
	public abstract void Enter(T obj);
}
