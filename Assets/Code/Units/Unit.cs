using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit : MonoBehaviour {
	
	static int unitCounter = 0;
	
	public int unitID; 
	public List<Armor> armor;
	public Weapon selectedWeapon;
	public Weapon primaryWeapon;
	public Weapon secondaryWeapon;
	
	Tile currTile, prevTile;
	
	public int team;
	public int moveRange = 50;
	public int runRange = 100;
	
	public int totalActions = 2;
	public Turn currentTurn;
	
	//Stats
	public string charName = "";
	public int aim = 65;
	public int morale = 10;
	public int health = 12;
	public int sight = 15;
	
	
	protected void AssignID()
	{
		unitID = unitCounter;
		unitCounter++;	
	}
	public virtual void Initialize(string _Name, int Team)
	{
		AssignID();
		armor = new List<Armor>();
		int count = (int)Armor.Slot.COUNT;
		for (int i = 0; i < count; i++)
			armor.Add(null);
		
		charName = _Name;
		if(charName.Length == 0)
			charName = "Blorgon";
	
		//Item head = ItemManager.self.GetItem(1);
		//Item chest = ItemManager.self.GetItem(2);
		//Item arm = ItemManager.self.GetItem(3);
		//Item leg = ItemManager.self.GetItem(4);
		
		//Item primary = ItemManager.self.GetItem(11);
		//Item secondary = ItemManager.self.GetItem(10);
		
		//Equip(head);
		//Equip(chest);
		//Equip(arm);
		//Equip(leg);
		//Equip(primary);
		//Equip(secondary);
		
		primaryWeapon = new  Weapon(999999, "Fists", Item.Types.WEAPON, new DamageDice(1,1,1), new DamageDice(1,2,1), 1, Weapon.Slot.PRIMARY, 10, 9999);
		team = Team;
		//ShowEquipment();
		gameObject.name = charName;
		print(gameObject.name + " " + charName);
		NotificationCenter.AddObserver(this, "EndTurn");
		
		selectedWeapon = primaryWeapon;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	public void SetTile(Tile tile)
	{
		MoveToTile(tile);
		currTile = tile;
		tile.Occupy(this);
		tile.bOccupied = true;
		//NotificationCenter.PostNotification(this, "UpdateMovementMap");
	}
	
	public Tile GetTile()
	{
		return currTile;		
	}
	
	public void Move(Tile newTile)
	{
		if(!newTile.Equals(currTile) && Map.Instance.IsTileInList(newTile))
			currentTurn.Move(newTile);
		else
		{
			if(prevTile != null)
				currentTurn.Move(prevTile);
		}
	}
	
	public void DragMoveToTile(Tile newTile)
	{
		if(currentTurn.actionsLeft == 0)
			Debug.LogWarning("No Turns Left");	
		else if(!Map.Instance.IsTileInList(newTile))
		{
			Debug.LogWarning("Invalid Move. Tile out of range");	
		}
		else
		{
			float diffY = newTile.transform.position.y - transform.position.y;
			if(diffY == 0) 
				diffY = 1;
			transform.position = new Vector3(newTile.transform.position.x, 1.5f, newTile.transform.position.z);
			prevTile = newTile;
		}
	}
	public void MoveToTile(Tile newTile)
	{
		float diffY = newTile.transform.position.y - transform.position.y;
		if(diffY == 0) 
			diffY = 1;
		transform.position = new Vector3(newTile.transform.position.x, 1.5f, newTile.transform.position.z);
	}
	
	public virtual void Selected()
	{
		PaintSelected();
		print("Can take action? " + currentTurn.CanTakeAction());			
		if(currentTurn.CanTakeAction())
			Map.Instance.SetTileMark(APath.Mark.MOVE);	
		else
			Map.Instance.SetTileMark(APath.Mark.GREY_MOVE);	
		
		currTile.pathCost = 0;
		Map.Instance.ShowMovement(this);
	}
	public virtual void Targeted()
	{
		PaintTargeted();	
	}
	
	public virtual void Unselected()
	{
		ResetPaint();
	}
	
	protected virtual void ResetPaint()
	{
		GetComponentInChildren<Renderer>().material.SetColor("_Color", Color.white);
	}
	
	protected virtual void PaintSelected()
	{
		renderer.material.SetColor("_Color", Color.green);
	}
	
	void PaintTargeted()
	{
		GetComponentInChildren<Renderer>().material.SetColor("_Color", Color.red);
	}
	
	public void Equip(Item item)
	{
		switch(item.type)
		{
		case Item.Types.WEAPON:
			Weapon newWeapon = (Weapon)item;
			//Debug.Log("Equipping " + newWeapon.name);
			if(newWeapon.hands >= 2)
				primaryWeapon = newWeapon;
			else if(primaryWeapon == null)
				primaryWeapon = newWeapon;
			else
			{
				if(newWeapon.hands == 1)
					secondaryWeapon = newWeapon;
				else
					Debug.Log("Cant Equip this weapon. No hands");
			}
			break;
		case Item.Types.ARMOR:
			Armor newArmor = (Armor)item;
			//Debug.Log("Equipping " + newArmor.type.ToString());
			int armorIndex = (int)newArmor.slot;
			armor[armorIndex] = newArmor;
			break;
		}
	}
	
	void ShowEquipment()
	{
		int count = (int)Armor.Slot.COUNT;
		for (int i = 0; i < count; i++)
		{
			if(armor[i] != null)
			{
				Debug.Log(armor[i].slot.ToString() + " Equipped :" +armor[i].name);
			}
		}
		
		GetTotalArmor();
		GetBaseDamage();
	}
	
	public int GetTotalArmor()
	{
		int sum = 0;
		int count = (int)Armor.Slot.COUNT;
		for (int i = 0; i < count; i++)
		{
			if(armor[i] != null)
			{
				sum += armor[i].defense;
			}
		}
		Debug.Log("Total Armor: " + sum);
		return sum;
	}
	
	int GetBaseDamage()
	{
		//Debug.Log("Base Damage: " + primaryWeapon.damage);
		//Debug.Log("Rate of Fire: " + primaryWeapon.rof[0] + primaryWeapon.rof[1]);
		//return primaryWeapon.damage;
		return 0;
	}
	
	public void NewTurn()
	{
		currentTurn = new Turn(this);
	}
	
	public bool RollToHit()
	{
		int roll = Random.Range(1,100);
		if(roll < aim)
			return true;
		else
			return false;
	}
	
	public virtual void Died()
	{
		GetComponentInChildren<Renderer>().enabled = false;	
	}
	
	public void PaintBlue()
	{
		GetComponentInChildren<Renderer>().material.SetColor("_Color", Color.blue);
	}
	public virtual void StartTurn()
	{
		Debug.Log(charName + " Starts Turn ");
		currentTurn = new Turn(this);
		currentTurn.StartTurn();
	}
	protected virtual void EndTurn()
	{
		if(currentTurn != null && currentTurn.turnIndex == TurnManager.turnCounter)
		{
			Debug.Log(charName + " Ended Turn");
			TurnManager.instance.AddToHistory(currentTurn);
			currentTurn = new Turn(this);
		}
		
	}
	
	public int GetActionsLeft()
	{
		return currentTurn.actionsLeft;	
	}
	
	public virtual void ShootSingle(Unit _target)
	{
		
		Debug.Log("Shooting Single shot");
		currentTurn.Attack( SingleShot.Instance, _target);
		//GameObject.Find("Game").GetComponent<CombatResolver>().SingleShot(this, _target);
	}
	
	public virtual void ShootMulti(Unit _target)
	{
		Debug.Log("Shooting Multi shot");
		currentTurn.Attack(MultiShot.Instance ,_target);
		//GameObject.Find("Game").GetComponent<CombatResolver>().MultiShot(this, _target);
	}
	
		
	public void StartDamageEffect(int damage)
	{
		StartCoroutine(DamageEffect(damage));
	}
	
	IEnumerator DamageEffect(int damage)
	{
		int state = 0; //0 move up, 1 wait for duraiton before destroy
		float floatDuration = 1;
		
		GameObject textEffect = GameObject.Find("DamageEffect");
		Transform tfTextEffect = textEffect.transform;
		textEffect.GetComponent<TextMesh>().text = damage.ToString();
		
		tfTextEffect.position = transform.position;
		while(state < 2)
		{
			if(state == 0)
			{
				tfTextEffect.position += Vector3.up * Time.deltaTime;
				if(tfTextEffect.position.y > transform.position.y + 1)
				{
					state++;	
				}
			}
			else if(state == 1)
			{
				yield return new WaitForSeconds(floatDuration);
				state++;
			}
			yield return 0;	
		}
		textEffect.GetComponent<TextMesh>().text = "";
	}
	
	public void StartMissEffect()
	{
		StartCoroutine(MissEffect());
	}
	
	IEnumerator MissEffect()
	{
		int state = 0; //0 move up, 1 wait for duraiton before destroy
		float floatDuration = 1;
		
		GameObject textEffect = GameObject.Find("DamageEffect");
		Transform tfTextEffect = textEffect.transform;
		textEffect.GetComponent<TextMesh>().text = "Miss";
		
		tfTextEffect.position = transform.position;
		while(state < 2)
		{
			if(state == 0)
			{
				tfTextEffect.position += Vector3.up * Time.deltaTime;
				if(tfTextEffect.position.y > transform.position.y + 1)
				{
					state++;	
				}
			}
			else if(state == 1)
			{
				yield return new WaitForSeconds(floatDuration);
				state++;
			}
			yield return 0;	
		}
		textEffect.GetComponent<TextMesh>().text = "";
	}
	
}
