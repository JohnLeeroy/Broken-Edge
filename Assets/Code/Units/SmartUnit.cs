using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SmartUnit: Unit {
	
	UnitStateMachine sm;
	Unit target;
	
	public bool bTurnFinished = false;
	
	public override void Initialize (string _Name, int _Team)
	{
		base.Initialize(_Name, _Team);
		sm = new UnitStateMachine(this);
		sm.ChangeState(IdleState.Instance);
	}
	
	public UnitStateMachine StateMachine
	{
		get{
			return sm;	
		}
	}
	
	public List<Tile> GetPossibleMoveTiles()
	{
		Selector.Instance.selectedUnit = this;
		Map.Instance.ShowMovement(this);
		return Map.Instance.GetMovementMap();
	}
	
	public List<Unit> GetEnemiesInRange()
	{
		
		return null;
	}
	
	public bool DetectEnemy()
	{
		List<Unit> opposition = TurnManager.instance.GetOpposingTeam(team);
		int sightRange = sight * sight;
		if(opposition.Count > 0)
		{
			int count = opposition.Count;
			float distance = 0;
			Vector3 enemyPosition;
			Vector3 unitPosition = transform.position;
			for(int i = 0; i < count; i++)
			{
				enemyPosition = opposition[i].transform.position;
				distance =  Vector3.Dot(enemyPosition - unitPosition, enemyPosition - unitPosition) ;//* Vector3.Dot(enemyPosition, unitPosition);
				if(distance < sightRange)
					return true;
			}
		}
		else
			Debug.LogWarning("Warning SmartUnit.DetectEnemy() : No units left");
		
		return false;
	}
	
	public void SelectClosestTarget()
	{
		List<Unit> opposition = TurnManager.instance.GetOpposingTeam(team);
		int sightRange = sight * sight;
		if(opposition.Count > 0)
		{
			int count = opposition.Count;
			int closestIndex = -1;
			float closestdistance = 10000000;
			float distance = 0;
			Vector3 enemyPosition;
			Vector3 unitPosition = transform.position;
			
			for(int i = 0; i < count; i++)
			{
				enemyPosition = opposition[i].transform.position;
				distance =  Vector3.Dot(enemyPosition - unitPosition, enemyPosition - unitPosition) ;//* Vector3.Dot(enemyPosition, unitPosition);
				if(distance < closestdistance)
				{
					closestIndex = i;
					closestdistance = distance;
				}
			}
			
			if(closestIndex != -1)
			{
				Debug.Log("Closest Target: " + opposition[closestIndex].charName + opposition[closestIndex].unitID);
				target = opposition[closestIndex];	
			}
			else
				Debug.LogWarning("SmartUnit.SelectClosestTarget : No target fount");
		}
		else
			Debug.LogWarning("SmartUnit.SelectClosestTarget : No target fount");
	}
	
	public float TargetDistance()
	{
		if(target == null)
			SelectClosestTarget();	
		return Vector3.Dot( transform.position - target.transform.position,  transform.position - target.transform.position);
	}
	public void SelectTarget()
	{
		//Some heurisitc here
		//Currently selects random target
		List<Unit> opposition = TurnManager.instance.GetOpposingTeam(team);
		if(opposition.Count == 0)
		{
			Debug.LogWarning("SmartUnit.SelectTarget() : No Target Found");
			return;
		}
		target = opposition[Random.Range(0, opposition.Count)];
	}
	
	
	//Check range
	//Check LOS
	public override void ShootSingle(Unit _target)
	{
		Debug.Log("Shooting Single shot");
		//currentTurn.Attack(target);
		//GameObject.Find("Game").GetComponent<CombatResolver>().SingleShot(this, target);
		_target = target; //HACK
		SelectTarget();
		currentTurn.Attack( SingleShot.Instance, _target);
	}
	
	public override void ShootMulti(Unit _target)
	{
		Debug.Log("Shooting Multi shot");
		//currentTurn.Attack(target);
		//GameObject.Find("Game").GetComponent<CombatResolver>().MultiShot(this, target);
		_target = target; //HACK
		SelectTarget();
		currentTurn.Attack(MultiShot.Instance ,_target);
	}
	
	public override void StartTurn ()
	{
		base.StartTurn ();
		bTurnFinished = false;
	}
	
	protected override void EndTurn ()
	{
		base.EndTurn ();
		bTurnFinished = true;
	}
	
}


/*
My initial inheritance structure didnt play well my state machine. 
BAD: Unit -> EnemyUnit -> SmartEnemy
GOOD: Unit -> SmartUnit -> SmartEnemy
 * */