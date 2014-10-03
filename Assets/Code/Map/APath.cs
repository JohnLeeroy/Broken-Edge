using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class APath  {
	
	public int baseDistance = 10;
	public enum Mark{MOVE, GREY_MOVE, ATTACK};
	Mark mode;
	
	Map map;
	
	public GameObject prefabTile;
	
	Tile currentTile;
	
	List<Tile> openList;
	
	int range;
	int iter = 0;
	float totalTime = 0;
	public void Init(Map map)
	{
		openList = new List<Tile>();
		this.map = map;	
		//map.selectedUnit = GameObject.Find("Unit").GetComponent<Unit>();
	}
	
	void DepthFirstSearch(Tile currentTile)
	{
		if(!IsTileInList(currentTile))
			openList.Add(currentTile);
		
		iter++;
		totalTime += Time.deltaTime;
		//Debug.Log("==================Start of DFS " + currentTile.mapIndex);
		List<Tile> adjacentTiles;
		adjacentTiles = map.FindAllAdjacentTiles(currentTile);
		CalculateAdjacentCosts(adjacentTiles, currentTile);
		
		//Debug.Log("Adjacent Count " + adjacentTiles.Count + " " + currentTile.mapIndex);
		foreach(Tile targetTile in adjacentTiles)
		{
			//Debug.Log(targetTile.mapIndex + " Cost and Range " + targetTile.pathCost + " " + range); 
			if(targetTile.pathCost <= range)
			{
				//Debug.Log(targetTile.mapIndex + " Got In ");
				//Debug.Log("Is it explored? " + targetTile.bExplored);
				//Debug.Log("Parent Tile " + targetTile.parent.mapIndex);
				if(!targetTile.bExplored && targetTile.parent.mapIndex == currentTile.mapIndex)
				{
					PaintTile(targetTile);
					currentTile.bExplored = true;
					DepthFirstSearch(targetTile);
				}
			}
		}
		//Debug.Log("Ended");
	}
	
	void PaintTile(Tile tile)
	{
		switch(mode)
		{
		case Mark.MOVE:
			tile.PaintMovementTile();
			break;
		case Mark.ATTACK:
			tile.PaintAttack();
			break;
		case Mark.GREY_MOVE:
			tile.PaintGreyTile();
			break;
		}
	}
	public List<Tile> GetMovementMap()
	{
		return openList;	
	}
		
	public void PaintMovementRange()
	{
		ClearOpenList();
		range = map.selectedUnit.moveRange;	
		currentTile = map.selectedUnit.GetTile();
		Debug.Log("CurrentTile " + currentTile.mapIndex);
		DepthFirstSearch(currentTile);
	}
	
	public void PaintAttackRange()
	{
		ClearOpenList();
		Unit selectedUnit = map.selectedUnit;
		Debug.Log(selectedUnit.name);
		if(selectedUnit.primaryWeapon.rangeType == Weapon.RangeTypes.NORMAL)
		{
			range = selectedUnit.primaryWeapon.range;
			currentTile = selectedUnit.GetTile();
			mode = Mark.ATTACK;
			DepthFirstSearch(currentTile);
		}
	}
	
	void CalculateAdjacentCosts(List<Tile> adjacentTiles, Tile currentTile)
	{
		int xDiff = 0, yDiff = 0;
		bool bDiagonal;
		
		foreach(Tile tile in adjacentTiles)
		{
			xDiff = currentTile.xIndex - tile.xIndex;
			yDiff = currentTile.yIndex - tile.yIndex;
			bDiagonal = (xDiff != 0 && yDiff != 0) ? true : false;
			
			CalculatePathCost(currentTile, tile, bDiagonal);
		}
	}
	
	bool IsTileInList(Tile target)
	{
		if(openList.Count > 0)
		{
			foreach(Tile tile in openList)
			{
				if(tile.mapIndex == target.mapIndex)
				{
					return true;
				}
			}
		}
		return false;	
	}
	
	
	void CalculatePathCost(Tile currentTile, Tile targetTile, bool bDiagonal)
	{
		float pathCost = currentTile.pathCost;
		
		switch(mode)
		{
		case Mark.MOVE:
			if(bDiagonal)
				pathCost += targetTile.moveCost * 1.4f;
			else
				pathCost += targetTile.moveCost;	
			break;
		case Mark.GREY_MOVE:
			if(bDiagonal)
				pathCost += targetTile.moveCost * 1.4f;
			else
				pathCost += targetTile.moveCost;	
			break;
		case Mark.ATTACK:
			pathCost += (bDiagonal ? baseDistance * 1.4f : baseDistance);
			break;
		}

		
		//Debug.Log("Path Cost " + pathCost);
		if(targetTile.pathCost == 0 || targetTile.pathCost > pathCost)
		{
			targetTile.parent = currentTile;
			targetTile.pathCost = pathCost;
			//Debug.Log("Cheaper");
			//Debug.Log("Added " + targetTile.mapIndex + " " + currentTile.mapIndex);
		}

	}
	
	public void PaintPath(Tile target)
	{
		Tile start = map.selectedUnit.GetTile();
		while(target != start)
		{
			target.PaintPath();
			target = target.parent;
		}
	}
	
	public void SetMarkType(Mark _mark)
	{
		mode = _mark;	
	}
	
	
	public void ClearOpenList()
	{
		foreach(Tile tile in openList)
		{
			tile.ResetTile();
		}
		openList.Clear();
	}
	
	
	
	
	
	
}
