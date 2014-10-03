using UnityEngine;
		
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
public class MapBuilder : MonoBehaviour {
	
	public string MapName = "Map";
	APath pathFinder;
	MapSelector selector;
	public GameObject prefabTile;
	public List<MapTile> map;
	List<MapTile> movement;
	List<MapTile> attack;
	
	
	List<MapTile> spawnTiles; 
	public int width = 20;
	public int height = 20;
	float offset = .1f;
	
	bool bShowSpawn = false;
	
	void Awake()
	{
		Transform world = GameObject.Find("World").transform;
		map = new List<MapTile>();

		for (int j = 0; j < height; j++)
		{
			for( int i = 0; i < width; i++)
			{
					GameObject newTile = (GameObject)Instantiate(prefabTile);
					newTile.name = "Tile_" + i.ToString() + "_" + j.ToString();
					newTile.transform.position = new Vector3(i, 0, j);
					newTile.transform.parent = world;
					
					newTile.GetComponent<MapTile>().Init( map.Count,i, j);
					map.Add(newTile.GetComponent<MapTile>());
				if((i & 1) == 0 && (j&1) == 0) 
				{
					newTile.GetComponent<MapTile>().SetColor();	
				}
			}
		}
		
		selector = Camera.mainCamera.GetComponent<MapSelector>();
		
		spawnTiles = new List<MapTile>();
		//DEBUG
		/*
		Unit defaultUnit = GameObject.Find("Unit").GetComponent<Unit>();
		defaultUnit.SetTile(map[225]);
		defaultUnit.GetTile().Occupy(defaultUnit);
		
		Unit defaultEnemy = GameObject.Find("Enemy").GetComponent<Unit>();
		defaultEnemy.SetTile(map[250]);
		defaultEnemy.GetTile().Occupy(defaultEnemy);
		*/
		//OffsetMap();
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
	// Use this for initialization
	void Start () 
	{
		NotificationCenter.AddObserver(this, "IncreaseHeight");
		NotificationCenter.AddObserver(this, "DecreaseHeight");
		NotificationCenter.AddObserver(this, "IncreaseCover");
		NotificationCenter.AddObserver(this, "DecreaseCover");
		NotificationCenter.AddObserver(this, "SaveMap");
		NotificationCenter.AddObserver(this, "LoadMap");
		NotificationCenter.AddObserver(this, "ToggleSpawn");
		NotificationCenter.AddObserver(this, "ShowSpawn");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
	public List<MapTile> FindAllAdjacentTiles(MapTile targetTile)
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
		
		List<MapTile> adjacentTiles = new List<MapTile>();
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
			
	int GetDirection(MapTile A, MapTile B)
	{
		int xDiff = A.xIndex - B.xIndex;	
		int yDiff = A.yIndex - B.yIndex;
		
		
		if(xDiff == -1 && yDiff == 0)
			return 0;
		else if(xDiff == 0 && yDiff == 1)
			return 1;
		else if(xDiff == 1 && yDiff == 0)
			return 2;
		else if(xDiff == 0 && yDiff == -1)
			return 3;
		
		return -1;
	}
	public bool IsTileInList(MapTile target)
	{
		if(movement.Count > 0)
		{
			foreach(MapTile tile in movement)
			{
				if(tile.mapIndex == target.mapIndex)
				{
					return true;
				}
			}
		}
		return false;	
	}
	
	void IncreaseCover()
	{
		print("increase cover");
		List<MapTile> selected = selector.lSelected;
		
		foreach(MapTile tile in selected)
		{
			//tile.co();
			tile.IncreaseCover();
		}
		if(selected.Count > 0)
			print("increase cover to " + selected[0].cover[0]);
	}
	
	void DecreaseCover()
	{
		List<MapTile> selected = selector.lSelected;
		
		foreach(MapTile tile in selected)
		{
			tile.DecreaseCover();	
		}
		if(selected.Count > 0)
			print("decrease cover to " + selected[0].cover);
	}
	
	public void IncreaseHeight()
	{
		print("Receiving increase height");
		List<MapTile> selected = selector.lSelected;
		
		foreach(MapTile tile in selected)
		{
			tile.IncreaseHeight();	
		}
	}
	
	public void DecreaseHeight()
	{
		print("Receiving decrease height");
		List<MapTile> selected = selector.lSelected;
		
		foreach(MapTile tile in selected)
		{
			tile.DecreaseHeight();	
		}
	}
	
	bool IsTileInSpawnList(MapTile _tile)
	{
		foreach(MapTile tile in spawnTiles)
		{
			if(tile.Equals(_tile))
				return true;
		}
		return false;
	}
	void ToggleSpawn()
	{
		Debug.Log("Toggling spawn");
		List<MapTile> selected = selector.lSelected;
		foreach(MapTile tile in selected)
		{
			if(!IsTileInSpawnList(tile))
			{
				tile.SetSpawn();
				spawnTiles.Add(tile);
			}
		}
	}
	void ShowSpawn()
	{
		bShowSpawn = !bShowSpawn;
		if(bShowSpawn)
			foreach(MapTile tile in spawnTiles)
				tile.SetSpawn();
		else
			foreach(MapTile tile in spawnTiles)
				tile.ResetPaint();
	}
	
	class TileData
	{
		IDictionary data;
		
		public TileData(MapTile tile)
		{
			data = new Dictionary<string, int>();
			data.Add("x", tile.transform.position.x);
			data.Add("y", tile.transform.position.y);
			data.Add("z", tile.transform.position.z);
		}
		
	}
	
	void SaveMap()
	{	
		List<Dictionary<string,int>> tileList = new List<Dictionary<string, int>>();
		foreach(MapTile tile in map)
		{
			Dictionary<string,int> dTile = new Dictionary<string, int>();
			dTile["x"] = (int)tile.transform.position.x;
			dTile["y"] = (int)tile.transform.position.y;
			dTile["z"] = (int)tile.transform.position.z;
			dTile["xIndex"] = tile.xIndex;
			dTile["yIndex"] = tile.yIndex;
			dTile["mapIndex"] = tile.mapIndex;
			dTile["moveCost"] = (int)tile.moveCost;
			dTile["height"] = tile.height;
			dTile["leftCover"] = tile.cover[0];
			dTile["topCover"] = tile.cover[1];
			dTile["rightCover"] = tile.cover[2];
			dTile["bottomCover"] = tile.cover[3];
			tileList.Add(dTile);
		}
		
		List<int> spawnList = new List<int>();
		foreach(MapTile tile in spawnTiles)
		{
			spawnList.Add(tile.mapIndex);	
		}
		
		StreamWriter writer = new StreamWriter(Application.dataPath + "/Maps/" + MapName + ".json");
		string serial = MiniJSON.Json.Serialize(tileList);
		string spawns = MiniJSON.Json.Serialize(spawnList);
		print(serial);
		writer.WriteLine(width);
		writer.WriteLine(height);
		writer.WriteLine(serial);
		writer.WriteLine(spawns);
		writer.Flush();
		writer.Close();
		

	}
		
	void DestroyMap()
	{
		foreach(MapTile tile in map)
		{
			GameObject.Destroy(tile.gameObject);	
		}
		map.Clear();
	}
	void LoadMap()
	{
		DestroyMap();
		
		string mapData;
		string spawnData;
		StreamReader reader = new StreamReader(	Application.dataPath + "/Maps/" + MapName + ".json");
		width = Convert.ToInt32(reader.ReadLine());
		height = Convert.ToInt32(reader.ReadLine());
		mapData = reader.ReadLine();
		spawnData = reader.ReadLine();
		reader.Close();
		
		GameObject world = GameObject.Find("World");
		
		IList mapJSON = (IList) MiniJSON.Json.Deserialize(mapData);
		print("Tile Count " + mapJSON.Count);
		foreach (IDictionary _tile in mapJSON) {

			int x,y,z;
			MapTile newTile = ((GameObject)GameObject.Instantiate(prefabTile)).GetComponent<MapTile>();
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
			map.Add(newTile);
		} 
		
		spawnTiles.Clear();
		IList spawnJSON = (IList) MiniJSON.Json.Deserialize(spawnData);
		print("Tile Count " + spawnJSON.Count);		
		foreach (object spawnIndex in spawnJSON) 
		{
			spawnTiles.Add(map[Convert.ToInt32(spawnIndex)]);
		}
		
		
		UpdateCover();
	}
	
	//Goes through each tile and updates cover values
	void UpdateCover()
	{
		for(int i = 0; i < map.Count; i++)
		{
			List<MapTile> adjacent = FindAllAdjacentTiles(map[i]);
			
			foreach(MapTile tile in adjacent)
			{
				if(map[i].height > tile.height)
				{
					int direction = GetDirection(map[i], tile);
					print(direction);
					if(direction != -1)
						tile.cover[direction] += 1;
				}
			}
		}
	}
}
