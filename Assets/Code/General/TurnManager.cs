using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurnManager : MonoBehaviour {
	// 0 is ALWAYS player's team
	
	public static TurnManager instance;
	
	List<Turn> history;
	List<List<Unit>> turnQueue;
	public int currentTeam = 0; 
	
	int teamCount = 2;
	public static int turnCounter = 0;
	
	Coroutine checkTurn;
	
	void Awake()
	{
		history = new List<Turn>();
		turnQueue = new List<List<Unit>>();
		instance = this;
		
		NotificationCenter.AddObserver(this, "EndTurn_Pressed");
		NotificationCenter.AddObserver(this, "SingleShot");
		NotificationCenter.AddObserver(this, "MultiShot");
		NotificationCenter.AddObserver(this, "Reload");
	}
	
	public void Initialize(int _teamCount) {
		teamCount = _teamCount;
		for(int i = 0; i < teamCount; i++)
			turnQueue.Add(new List<Unit>());	
	}
	
	public void AddToHistory(Turn turn)
	{
		history.Add(turn);
	}
	public bool IsUnitInTeam(Unit _unit, int _team)
	{
		int id = _unit.unitID;
		foreach(Unit unit in turnQueue[_team])
		{
			if(unit.unitID == id)
				return true;
		}
		return false;
	}
	public void AddUnit(Unit _unit, int _team)
	{
		if(!IsUnitInTeam(_unit, _team))
			turnQueue[_team].Add(_unit);
		else
			Debug.LogWarning("Unit already exists in team.");
	}
	
	public void AddTurn(Turn turn)
	{
		history.Add(turn);	
	}
	
	public void AddToTurnQueue(Unit unit)
	{
		//turnQueue.Add(unit);	
	}
	
	//Checks if team is AI
	bool IsTeamSmart()
	{
		if(turnQueue[currentTeam].Count > 0)
			return (turnQueue[currentTeam][0] is SmartUnit);	
		
		return false;
	}
	bool CheckIfFinishedTurn()
	{
		bool isEveryoneDone = true;
		int teamSize = turnQueue[currentTeam].Count;
		List<Unit> team = turnQueue[currentTeam];
		SmartUnit currentUnit;
		
		for(int i = 0; i < teamSize; i++)
		{
			currentUnit = team[i] as SmartUnit;
			if(currentUnit.GetActionsLeft() > 0) 
				isEveryoneDone = false;
		}	
		return isEveryoneDone;
	}
	IEnumerator CR_CheckTurnFinished()
	{
		yield  return new WaitForSeconds(2.0f);
		int turnIndex = turnCounter;
		bool bAllTurnsUsed = true;
		/*
		while(turnIndex == turnCounter)
		{
			Debug.Log("Coroutine CR_CheckTurnFinished");
			if(CheckIfFinishedTurn())
			{
				break;
			}
			yield  return new WaitForSeconds(2.0f);
		}
		*/
		Debug.Log("AI Team " + currentTeam + " is finished");
		EndTurn();
	}
	
	public void NextTurn()
	{
		print("Current Team " + currentTeam);
		foreach(Unit unit in turnQueue[currentTeam])
		{
			unit.StartTurn();
			unit.PaintBlue();
		}	
		
		//If AI Team, start coroutine to check if they are done
		if(IsTeamSmart())
			checkTurn = StartCoroutine("CR_CheckTurnFinished");
	}
	
	void EndTurn_Pressed()
	{
		EndTurn();	
	}
	
	public void EndTurn()
	{
		Debug.Log("ENDING TEAM " + currentTeam + " TURN ------------------------------------------");
		if(++currentTeam >= turnQueue.Count)
		{
			currentTeam = 0;
		}
		turnCounter++;
		NotificationCenter.PostNotification(this, "EndTurn");
		StopCoroutine("CR_CheckTurnFinished");
		NextTurn();
	}
	
	public List<Unit> GetOpposingTeam(int curTeam)
	{
		List<Unit> opponents = new List<Unit>();
		
		for(int i = 0; i < turnQueue.Count; i++)
		{
			if(curTeam == i)
				continue;
			
			for(int j = 0; j < turnQueue[i].Count; j++)
			{
				opponents.Add(turnQueue[i][j]);	
			}
		}
		return opponents;
	}
	
	void SingleShot()
	{
		Selector.Instance.selectedUnit.ShootSingle(Selector.Instance.selectedTarget);
	}
	
	void MultiShot()
	{
		Selector.Instance.selectedUnit.ShootMulti(Selector.Instance.selectedTarget);
	}
	
	void Reload()
	{
		Selector.Instance.selectedUnit.currentTurn.ReloadWeapon(Selector.Instance.selectedUnit);
		
	}
}
