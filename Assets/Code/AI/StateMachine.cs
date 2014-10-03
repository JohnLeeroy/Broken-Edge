using UnityEngine;
using System.Collections;

public abstract class StateMachine<T>{
	
	T owner;
	public State<T> currentState;
	public State<T> previousState;
	public State<T> globalState;
	
	public StateMachine(T _owner)
	{
		owner = _owner;
	}
	
	public virtual void ChangeState(State<T> newState)
	{
		if(newState == null)
		{
			Debug.LogError("StateMachine::ChangeState newState == null");
			return;	
		}
		
		previousState = currentState;
		currentState.Exit(owner);
		currentState = newState;
		currentState.Enter(owner);
	}
	
	public virtual void ChangeState(StateHub<T> hub)
	{
		if(hub == null)
		{
			Debug.LogError("StateMachine::ChangeState hub == null");
			return;	
		}
		hub.Enter(owner);
	}
	
	public void RevertToPreviousState()
	{
		ChangeState(previousState);	
	}
	
}
