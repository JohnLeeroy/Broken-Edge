using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

	public delegate void OnStart(); 
	OnStart kill;
	void PrintOne()
	{
		Debug.Log("Hey you");	
	}
	void PrintTwo()
	{
		Debug.Log("Listen");	
	}
	
	void PrintThree()
	{
		Debug.Log("Zolda");	
	}
	
	// Use this for initialization
	void Start () {
		OnStart start = delegate {
			Debug.Log("Hey Dog");
		};
		
		kill += PrintOne;
		kill += PrintTwo;
		kill += PrintThree;
		
		
		start();
		kill();
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
