using UnityEngine;
using System.Collections;

public class TurnActionPanel : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void onEndTurnPressed()
	{
		NotificationCenter.PostNotification (this, "EndTurn_Pressed");
	}
}
