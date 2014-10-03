using UnityEngine;
using System.Collections;

public abstract class State<T> {
	public abstract void Enter(T obj); 
	public abstract void Execute(T obj); 
	public abstract void Exit(T obj); 
}
