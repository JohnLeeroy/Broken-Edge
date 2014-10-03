using UnityEngine;
using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;

public class Map : MonoBehaviour {
	
	public static Map Instance;
	APath pathFinder;
	public string MapName = "Streets_1";
	public GameObject prefabTile;
	public List<Tile> map;
	List<Tile> movement;
	
	List<int> spawnTiles;
	
	List<Tile> attack;
	
	public int width = 20;
	public int height = 20;
	float offset = .1f;
	
	public Unit selectedUnit;
	
	public TextAsset currentMap;
	
	// Use this for initialization
	void Awake () 
	{
		Instance = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void Initialize(string mapName)
	{
		//NotificationCenter.AddObserver(this, "UpdateMovementMap");
		
		Transform world = GameObject.Find("World").transform;

		LoadMap(mapName);
		
		pathFinder = new APath();
		pathFinder.Init(this);
	}
	
	void OffsetMap()
	{
		for (int j = 0; j < height; j++)
		{
			for( int i = 0; i < width; i++)
			{
				map[i + j * height].transform.position = new Vector3(i + (i * offset), 0, j + (j * offset));
			}
		}
		
	}

	public List<Tile> FindAllAdjacentTiles(Tile targetTile)
	{
		int x = targetTile.xIndex;
		int y = targetTile.yIndex;
		
		//print(x.ToString() + " " +y.ToString());
		bool isLeft, isTop, isRight, isBottom, isLTop, isRTop, isLBottom, isRBottom;
		
	 	isLeft = (x == 0) ? false : true;
	 	isTop = (y == (height - 1)) ? false : true;
	 	isRight = (x == (width - 1)) ? false : true;
	 	isBottom = (y == 0) ? false : true;
		
		isLTop = (isLeft && isTop) ? true : false;
		isRTop = (isRight && isTop) ? true : false;		
		
		isLBottom = (isLeft && isBottom) ? true : false;
		isRBottom = (isRight && isBottom) ? true : false;		
		
		List<Tile> adjacentTiles = new List<Tile>();
		//Orthogonal
		if(isLeft)
			adjacentTiles.Add(map[targetTile.mapIndex - 1]);
		
		if(isTop)
			adjacentTiles.Add(map[targetTile.mapIndex + width]);		
		
		if(isRight)
			adjacentTiles.Add(map[targetTile.mapIndex + 1]);
		
		//print("map Index" + targetTile.mapIndex);
		if(isBottom)
			adjacentTiles.Add(map[targetTile.mapIndex - width]);
		//Diagonal
		if(isLTop)
			adjacentTiles.Add(map[targetTile.mapIndex + width - 1]);
		
		if(isRTop)
			adjacentTiles.Add(map[targetTile.mapIndex + (width + 1)]);
		
		if(isLBottom)
			adjacentTiles.Add(map[targetTile.mapIndex - width - 1]);
		
		//print(   targetTile.mapIndex.ToString() + " " +  height);
		if(isRBottom)
			adjacentTiles.Add(map[targetTile.mapIndex - width + 1]);	
		
		return adjacentTiles;
	}
	
	void UpdateMovementMap()
	{
		print("Update Movement Map");
		//ShowMovement();	
	}
	
	public void ShowMovement(Unit _unit)
	{
		//selectedUnit = Selector.Instance.selectedUnit;
		selectedUnit = _unit;
		if(_unit == null)
		{
			Debug.LogWarning("Can't show movement. No Selected unit");
			return;	
		}
		pathFinder.PaintMovementRange();
		movement = pathFinder.GetMovementMap();
		//check straight, else depth first A*
	}
	
	public void CleanMovementMap()
	{
		foreach(Tile tile in movement)
		{
			tile.ResetTile();	
		}
		movement.Clear();
	}
	
	public void ShowAttackRange(Unit SelectedUnit)
	{
		selectedUnit = SelectedUnit;
		pathFinder.PaintAttackRange();
	}
	
	public bool IsTileInList(Tile target)
	{
		if(movement.Count > 0)
		{
			foreach(Tile tile in movement)
			{
				if(tile.mapIndex == target.mapIndex)
				{
					return true;
				}
			}
		}
		return false;	
	}
	
	void DestroyMap()
	{
		foreach(Tile tile in map)
		{
			GameObject.Destroy(tile.gameObject);	
		}
		map.Clear();
	}
	
	void LoadMap(string mapName)
	{
		DestroyMap();
		string mapData;
		string spawnData;
		
		string[] data = currentMap.text.Split(new Char[]{'\n'});
		width = Convert.ToInt32(data[0]);
		height = Convert.ToInt32(data[1]);
		mapData = data[2];
		spawnData = data[3];
		/*
		StreamReader reader = new StreamReader(	Application.dataPath + "/Maps/" + mapName);
		width = Convert.ToInt32(reader.ReadLine());
		height = Convert.ToInt32(reader.ReadLine());
		mapData = reader.ReadLine();
		spawnData = reader.ReadLine();
		reader.Close();
		*/
		GameObject world = GameObject.Find("World");
		
		IList mapJSON = (IList) MiniJSON.Json.Deserialize(mapData);
		print("Tile Count " + mapJSON.Count);
		foreach (IDictionary _tile in mapJSON) {

			int x,y,z;
			Tile newTile = ((GameObject)GameObject.Instantiate(prefabTile)).GetComponent<Tile>();
			x = Convert.ToInt32(_tile["x"]);
			y = Convert.ToInt32(_tile["y"]);
			z = Convert.ToInt32(_tile["z"]);
			newTile.transform.position = new Vector3(x,y,z);
			newTile.transform.parent = world.transform;
			
			
			newTile.xIndex = Convert.ToInt32(_tile["xIndex"]);
			newTile.yIndex = Convert.ToInt32(_tile["yIndex"]);
			newTile.mapIndex = Convert.ToInt32(_tile["mapIndex"]);
			newTile.moveCost = Convert.ToInt32(_tile["moveCost"]);
			newTile.height = Convert.ToInt32(_tile["height"]);
			newTile.cover = new int[]{0,0,0,0};

			newTile.cover[0] = Convert.ToInt32(_tile["leftCover"]);
			newTile.cover[1] = Convert.ToInt32(_tile["topCover"]);
			newTile.cover[2] = Convert.ToInt32(_tile["righttCover"]);
			newTile.cover[3] = Convert.ToInt32(_tile["bottomtCover"]);
			
			//if(newTile.cover[1] > 0)
			//{
			//	newTile.moveCost += 1000;	
			//}
			if(newTile.height > 0)
			{
				newTile.moveCost += 1000;	
			}
			if(y >= 1f) 
			{
				newTile.renderer.material.color = Color.grey;
			}
			map.Add(newTile);
		} 	
		
		if(spawnData.Length == 0)
			return;
		
		if(spawnTiles == null)
			spawnTiles = new List<int>();
		
		spawnTiles.Clear();
		IList spawnJSON = (IList) MiniJSON.Json.Deserialize(spawnData);
		foreach (object spawnIndex in spawnJSON) 
		{
			spawnTiles.Add(Convert.ToInt32(spawnIndex));
		}
		
		print("Spawn Count " + spawnTiles.Count);
		
	}
	
	public List<int> GetSpawnIndicies()
	{
		return spawnTiles;
	}
	
	public List<Tile> GetMovementMap()
	{
		return movement;	
	}
	
	public void SetTileMark(APath.Mark _mark)
	{
		pathFinder.SetMarkType(_mark);	
	}
}
