using UnityEngine;
using System.Collections;


public class Button
{
	public string text;
	public Rect rect;
	public string command;
}

public class GameUI : MonoBehaviour {
	
	public GUIText gtName;
	public GUIText gtHealth;
	public GUIText gtPrimaryWeapon;
	public GUIText gtSecondaryWeapon;
	
	string unitName;
	string health;
	string weaponAmmo;
	string actionsLeft;
	
	
	Rect optionGroup;
	Rect optionBox;
	Button[] optionBtns;
	
	Rect actionGroup;
	Rect actionBox;
	Button[] actionBtns;
	
	void Awake()
	{
	}
	
	void Start () {
		NotificationCenter.AddObserver(this, "UnitSelected");

		
		InitOptions();
		InitActions();
	}
	
	void InitOptions()
	{
		optionGroup = new Rect(0,Screen.height*.3f, Screen.width * .08f, Screen.height*.4f);
		optionBox = new Rect(0,0, optionGroup.width, optionGroup.height);
		
		int optionBtnCount = 5;
		optionBtns = new Button[optionBtnCount];
		
		float xOffset = optionBox.width * .1f;
		float yOffset = optionBox.height * .05f;
		float btnWidth = optionBox.width - xOffset * 2;
		float btnHeight = (optionBox.height - yOffset * (optionBtnCount + 1))/optionBtnCount;
		
		for(int i = 0; i < optionBtnCount; i++)
		{
			optionBtns[i] = new Button();
			optionBtns[i].rect = new Rect( xOffset, (yOffset ) + (i * (yOffset + btnHeight)), btnWidth, btnHeight);
			optionBtns[i].text = "";
			optionBtns[i].command = "";
		}
		optionBtns[4].text = "End Turn";
		optionBtns[4].command = "EndTurn_Pressed";
		
	}
	
	void InitActions()
	{
		actionGroup = new Rect(Screen.width * .3f, Screen.height * .9f, Screen.width * .4f, Screen.height * .1f);
		actionBox = new Rect(0,0, Screen.width * .4f, Screen.height * .1f);
		
		int actionBtnCount = 6;
		float xOffset = actionBox.width * .02f;
		float yOffset = actionBox.height * .2f;
		float actionBtnWidth = (actionBox.width - xOffset * (actionBtnCount + 1))/actionBtnCount;
		float actionBtnHeight = actionBox.height - yOffset * 2; 
		actionBtns = new Button[actionBtnCount];
		for(int i = 0; i < actionBtnCount; i++)
		{
			actionBtns[i] = new Button();
			actionBtns[i].rect = new Rect( (xOffset) + (i * (xOffset + actionBtnWidth)), yOffset, actionBtnWidth, actionBtnHeight);
			actionBtns[i].text = "";
			actionBtns[i].command = "";
		}
		
		actionBtns[0].text = "Single";
		actionBtns[0].command = "SingleShot";
		actionBtns[1].text = "Multi";
		actionBtns[1].command = "MultiShot";
		actionBtns[2].text = "Reload";
		actionBtns[2].command = "Reload";
	}
	void Update () {
	
	}
	
	void UpdateData()
	{
		if(Selector.Instance.selectedUnit == null)
			return;
		
		Unit selectedUnit = Selector.Instance.selectedUnit;
		unitName = selectedUnit.charName;
		health = selectedUnit.health.ToString();
		weaponAmmo = "W1 " + selectedUnit.selectedWeapon.ammo.ToString() + "/" + selectedUnit.selectedWeapon.clipSize.ToString();
		
		gtName.text = unitName;
		gtHealth.text = health;
		gtPrimaryWeapon.text = weaponAmmo;
	}
	void UnitSelected()
	{
		 UpdateData();
	}
	
	void DisplayActionBox()
	{
		GUI.BeginGroup(actionGroup);
		GUI.Box(actionBox, "Actions");
		
		for(int i = 0; i < actionBtns.Length; i++)
		{
			if(GUI.Button(actionBtns[i].rect, actionBtns[i].text))
			{
				Debug.Log("Pressed " + actionBtns[i].text);
				NotificationCenter.PostNotification(this, actionBtns[i].command);
			}
			
		}
		GUI.EndGroup();
	}
	
	void DisplayOptions()
	{
		GUI.BeginGroup(optionGroup);
		GUI.Box(optionBox, "");
		for(int i = 0; i < optionBtns.Length; i++)
		{
			if(GUI.Button(optionBtns[i].rect, optionBtns[i].text))
			{
				Debug.Log("Pressed " + optionBtns[i].text);
				NotificationCenter.PostNotification(this, optionBtns[i].command);
			}
		}
		
		GUI.EndGroup();
	}
	
	void OnGUI()
	{
		//DisplayOptions();
		//DisplayActionBox();
		
		UpdateData();
	}
}
