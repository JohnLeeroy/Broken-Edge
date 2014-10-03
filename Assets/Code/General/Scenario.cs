using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Scenario : MonoBehaviour {


	/* Responsiblities
	 * Spawn Map
	 * 		spawn props
	 * List of known units
	 * 		spawn units
	 * Data
	 * 		Scenario description
	 * 		
	 */
	
	public string description = "Default Description";
	public string mapName = "DefaultMap.json";
	public string unitDataPath;
	
	void Start()
	{
		LoadDefault();
	}
	
	void LoadScenario() {}
	
	void LoadDefault()
	{
		//Spawn Map
		Map map = GameObject.Find("Game").GetComponent<Map>();
		map.Initialize(mapName);
		
		//Spawn units
		Unit unit1 = UnitFactory.Instance.ConstructDefaultUnit(0);
		Unit unit2 = UnitFactory.Instance.ConstructDefaultUnit(0);
		
		unit1.SetTile(map.map[111]);
		unit1.GetTile().Occupy(unit1);
		
		unit2.SetTile(map.map[112]);
		unit2.GetTile().Occupy(unit2);	
		
		TurnManager.instance.Initialize(2);
		TurnManager.instance.AddUnit(unit1, unit1.team);
		TurnManager.instance.AddUnit(unit2, unit2.team);
		
		
		//SmartEnemy enemy1 = (SmartEnemy)UnitFactory.Instance.ConstructDefaultEnemy(1);
		//SmartEnemy enemy2 = (SmartEnemy)UnitFactory.Instance.ConstructDefaultEnemy(1);
		
		List<int> spawns = map.GetSpawnIndicies();
		foreach(int index in spawns)
		{
			SmartEnemy enemy1 = (SmartEnemy)UnitFactory.Instance.ConstructDefaultEnemy(1);
			enemy1.SetTile(map.map[index]);
			enemy1.GetTile().Occupy(enemy1);		
			TurnManager.instance.AddUnit(enemy1, enemy1.team);
		}
		//enemy1.SetTile(map.map[215]);
		//enemy1.GetTile().Occupy(enemy1);
		//enemy2.SetTile(map.map[216]);
		//enemy2.GetTile().Occupy(enemy2);
		
		//TurnManager.instance.AddUnit(enemy1, enemy1.team);
		//TurnManager.instance.AddUnit(enemy2, enemy2.team);
		
		TurnManager.instance.NextTurn();
	}
	
	
	
}
