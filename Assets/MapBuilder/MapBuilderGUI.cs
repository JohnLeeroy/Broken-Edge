using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapBuilderGUI : MonoBehaviour {
	List<Rect> lButtons;
	
	public Rect guiRect, guiBox;
	
	Rect genInfoRect, genInfoBox;
	void Awake()
	{
		guiRect = new Rect(Screen.width * .8f, Screen.height * .1f, Screen.width * .2f, Screen.height * .8f); 
		guiBox = new Rect(0,0, Screen.width * .2f, Screen.height*.8f);
		
		genInfoRect = new Rect(Screen.width * .9f, Screen.height * .1f, Screen.width * .1f, Screen.height * .2f);
		genInfoBox = new Rect(0,0, Screen.width * .1f, Screen.height * .8f);
		
	}
	
	// Use this for initialization
	void Start () {
		lButtons = new List<Rect>();
		int yOffset = 50;
		for(int i = 0; i < 6; i++)
		{
			lButtons.Add(new Rect(10,20 + i * yOffset, 90, 40));
			lButtons.Add(new Rect(110,20 + i * yOffset, 90, 40));		
		}
		//lButtons.Add(new Rect(10,20, 70, 40));
		//lButtons.Add(new Rect(10,70, 70, 40));
		//lButtons.Add(new Rect(90,20, 70, 40));
		//lButtons.Add(new Rect(90,70, 70, 40));
		
		
		lButtons.Add(new Rect(10,520, 150, 40));
		lButtons.Add(new Rect(10,570, 150, 40));
		

		//lButtons.Add(new Rect(90,120, 70, 40));
		//lButtons.Add(new Rect(90,170, 70, 40));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI()
	{
		DisplayActionBox();
	}
	
	void DisplayActionBox()
	{
		GUI.BeginGroup(guiRect);
	
		GUI.Box(guiBox, "Actions" );
		if(GUI.Button(lButtons[0], "+ Height"))
		{
			NotificationCenter.PostNotification(this, "IncreaseHeight");
		}
		if(GUI.Button(lButtons[1], "+ Cover"))
		{
			NotificationCenter.PostNotification(this, "IncreaseCover");
		}			
		if(GUI.Button(lButtons[2], "- Height"))
		{
			NotificationCenter.PostNotification(this, "DecreaseHeight");
		}
		if(GUI.Button(lButtons[3], "- Cover"))
		{
			NotificationCenter.PostNotification(this, "DecreaseCover");
		}		
		
		if(GUI.Button(lButtons[4], "Toggle Spawn"))
		{
			NotificationCenter.PostNotification(this, "ToggleSpawn");
		}
		if(GUI.Button(lButtons[5], "Show Spawn"))
		{
			NotificationCenter.PostNotification(this, "ShowSpawn");
		}				
				
		

		if(GUI.Button(lButtons[lButtons.Count - 1], "LoadMap"))
		{
			NotificationCenter.PostNotification(this, "LoadMap");
		}
		if(GUI.Button(lButtons[lButtons.Count - 2], "SaveMap"))
		{
			NotificationCenter.PostNotification(this, "SaveMap");
		}
		GUI.EndGroup();
	}
	
}
