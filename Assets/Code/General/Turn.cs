using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Turn {
	
	public Unit unit;
	List<Action> actions;
	public int turnIndex;
	public int actionsLeft = 0;
	public Turn(Unit target)
	{
		actions = new List<Action>();		
		unit = target;	
		actionsLeft = unit.totalActions;
		turnIndex = TurnManager.turnCounter;
	}
	
	public void UseAction()
	{
		
	}
		
	bool IsActionsLeft()
	{
		if(actionsLeft > 0)
			return true;
		else
		{
			Debug.LogWarning("No Actions Left");
			return false;
		}
	}
	bool IsUnitsTurn()
	{
		return (TurnManager.instance.currentTeam == unit.team);
	}
	
	public bool CanTakeAction()
	{
		if(IsActionsLeft() && IsUnitsTurn())
			return true;
		else 
			return false;
	}
	public void Move(Tile targetTile)
	{
		if(!CanTakeAction())
			return;
		
		if(targetTile.Equals(unit.GetTile()))
		{
			Debug.LogWarning("Turn.Move() : Can't move on same tile!");
			return;	
		}
		if(!Map.Instance.IsTileInList(targetTile))
		{
			Debug.LogWarning("Turn.Move() : tile isnt in movement list");
			return;
		}
		
		Debug.Log("Moving unit");
		actionsLeft -=1;
		//actions.Add(new MoveAction());
	
		unit.GetTile().bOccupied = false;
		unit.SetTile(targetTile);
		
		unit.MoveToTile(unit.GetTile());
		
		TurnManager.instance.AddTurn(this);
		
		if(CanTakeAction())
			Map.Instance.SetTileMark(APath.Mark.MOVE);	
		else
			Map.Instance.SetTileMark(APath.Mark.GREY_MOVE);	
		
		Map.Instance.ShowMovement(unit);
		Debug.Log("Moving Unit. " + actionsLeft + " actions left");
		
	}
	
	/* Player selects Attack GUI
	  Highlight attack range
	  Player selects tile 
	 	 if(tile contains enemy and is in range)
	 	 	Change GUI to COMBAT_CONFIRM
	 	 else
	 	 	Nothing
	  if(Menu == COMBAT_CONFIRM)
	      if(Attack_Pressed)
	      	   Execute Attack
	      	   Log action
	*/
	public void Attack(CombatAction action, Unit target)
	{
		Debug.Log("Actions Left: " + actionsLeft + " | Cost" + action.cost);
		if(actionsLeft >= action.cost && IsUnitsTurn())
		{
			Debug.Log("Attacking " + target.charName);
			actionsLeft -= action.cost;
			action.Execute(unit, target);
			Debug.Log("Attacked target. " + actionsLeft + " actions left");	
		}
	}
	
	public void ReloadWeapon(Unit unit)
	{
		if(actionsLeft >= unit.selectedWeapon.GetReloadActions() && IsUnitsTurn())
		{
			actionsLeft -= unit.selectedWeapon.GetReloadActions();
			Reload.Instance.Execute(unit);
		}
	}
			
	 
	public void StartTurn()
	{
		if(unit.GetType().ToString() == "SmartEnemy")
		{
			Debug.Log("Doing AI Things");
			((SmartUnit)unit).StateMachine.currentState.Execute((SmartUnit)unit);
		}
	}
	
	//When the AI doesnt have anything else to do
	public void NoMoreActions()
	{
		actionsLeft = 0;
	}
}
