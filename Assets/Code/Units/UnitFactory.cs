using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class UnitFactory : MonoBehaviour {
	
	static UnitFactory instance;
	
	public static UnitFactory Instance
	{
		get
		{	

         return instance;
		}
	}
	/*
		public static IdleState Instance
	{
     get 
      {
         if (self == null)
         {
            self = new IdleState();
         }
         return self;
      }	
	}
	*/
	void Awake()
	{
	     if (instance == null)
         {
            instance = this;
         }	
		else
			Destroy(gameObject);
	}
	public GameObject prefabUnit, prefabEnemy;
	
	List<Unit> lUnits;
	
	public Unit ConstructDefaultUnit(int Team)
	{
		GameObject heavy = (GameObject)GameObject.Instantiate(prefabUnit);
		heavy.AddComponent<Unit>();
		heavy.GetComponent<Unit>().Initialize("Heavy", Team);
		heavy.name = "Default Soldier";
		return heavy.GetComponent<Unit>();
	}
	public Unit ConstructDefaultEnemy(int Team)
	{
		
		GameObject enemy = (GameObject)GameObject.Instantiate(prefabEnemy);
		enemy.AddComponent<SmartEnemy>();
		enemy.GetComponent<SmartEnemy>().Initialize("Enemy", Team);
		enemy.name = "Bad Guy";
		return enemy.GetComponent<SmartEnemy>();
	}
	
	// Use this for initialization
	void Start () 
	{
		/*
		lUnits = new List<Unit>();	
		
		Unit defaultUnit = ConstructDefaultUnit(1);
		lUnits.Add(defaultUnit);
		
		Map map = GameObject.Find("Game").GetComponent<Map>();
		defaultUnit.SetTile(map.map[111]);
		defaultUnit.GetTile().Occupy(defaultUnit);
		
		Unit enemyUnit = ConstructDefaultEnemy(2);
		enemyUnit.SetTile(map.map[115]);
		
		
		//TurnManager turnManager = GameObject.Find("Game").GetComponent<TurnManager>();
		//turnManager.AddTurn(defaultUnit.currentTurn);
		//turnManager.NextTurn();
		*/
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
