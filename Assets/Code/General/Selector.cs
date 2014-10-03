using UnityEngine;
using System.Collections;

/* Select Squad
 * Choose to Move
 * Select Individual Units
 * Drag Individual Units
 * Select Other squad, repeat
 */
public class Selector : MonoBehaviour {
	
	public static Selector Instance;
	
	public bool bActive = false;
	public bool bUnitSelected = false;
	public bool bTargetSelected = false;
	
	public int nPhase = 0;
	
	public GameObject selectedObj;
	public Unit selectedTarget = null;
	public Unit selectedUnit = null;
	public Tile selectedTile = null;
	
	//Ray
	Ray ray;
	RaycastHit hit;
	//Template
	Vector3 vecToEdge;
	Vector3 edgePos;
	
	
	Rect guiRect;
	
	// Use this for initialization
	void Awake () 
	{
		Instance = this;
		//guiRect = GameObject.Find("Game").GetComponent<GameUI>().guiRect;
		
	}
	
	void DeselectUnit()
	{

	}
	
	void Drag(Vector3 hitPoint)
	{
		
	}
	
	void DragToEdge(Vector3 hitPoint)
	{
		
	}
		
	// Update is called once per frame
	void Update () 
	{
		if(bActive)
			{
			switch(nPhase)
			{
			case 0:
				if ( Input.GetMouseButtonDown(0) && !guiRect.Contains(Input.mousePosition))
				{
	  				ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	        		if (Physics.Raycast(ray, out hit, 100))
					{
	           		 	//Debug.DrawLine(ray.origin, hit.point);
						switch(hit.transform.tag)
						{
						case "unit":
							//if(!bUnitSelected || !bTargetSelected)
							{
								if(bTargetSelected)
								{
									bTargetSelected = false;
									selectedTarget.Unselected();
									selectedTarget = null;
								}
							
								Unit hitUnit = hit.transform.GetComponent<Unit>();
								Debug.Log(hitUnit.name + " " + hitUnit.team);
								if(hitUnit)
								{
									switch(hitUnit.team)
									{
									case 0:
										print("Selected");
										if(selectedUnit)
											selectedUnit.Unselected();
										
										selectedObj = hitUnit.gameObject;
										selectedUnit = hitUnit;
										selectedUnit.Selected();
										bUnitSelected = true;
										NotificationCenter.PostNotification(this, "UnitSelected");
										break;
										
									case 1:
										selectedTarget = hitUnit;
										selectedTarget.Targeted();
										bTargetSelected = true;
										break;
									}

								}
							}
							
							break;
						case "tile":
							if(selectedTile)
								selectedTile.ResetPaint();
							
							selectedTile = hit.transform.GetComponent<Tile>();
							selectedTile.PaintSelected();
							
							break;
						};
						/*
						Unit targetUnit = hit.transform.gameObject.GetComponent<Unit>();
						if(targetUnit)
						{
							Squad hitSquad = targetUnit.GetSquad();
							if(hitSquad == selectedSquad)
							{
								print("SELECTING UNIT unit");
								SelectUnit(hit.transform.gameObject);
							}
							else
							{
								print("Selecting a different Squad");
								DeselectUnit();	
								SelectSquad(targetUnit.GetSquad());
								SelectUnit(hit.transform.gameObject);
							}
						}
						*/
						
					}
				}
				
				//RIGHT CLICK PRESSED
				if(bUnitSelected && Input.GetMouseButtonUp(1))
				{
	  				ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	        		if (Physics.Raycast(ray, out hit, 100))
					{
						switch(hit.transform.tag)
						{
						case "unit":
							if(hit.transform.GetComponent<Unit>().Equals(selectedUnit))
								selectedUnit.Move(selectedTile);
							break;
						case "tile":
							if(selectedTile)
								selectedTile.ResetPaint();
							
							selectedTile = hit.transform.GetComponent<Tile>();
							selectedTile.PaintSelected();
							selectedUnit.Move(selectedTile);
							break;
						};
					}
				}
				//RIGHT CLICK DOWN
				else if(bUnitSelected && Input.GetMouseButton(1))
				{
	  				ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	        		if (Physics.Raycast(ray, out hit, 100))
					{
						switch(hit.transform.tag)
						{
						case "unit":
							break;
						case "tile":
							if(selectedTile)
								selectedTile.ResetPaint();
							
							selectedTile = hit.transform.GetComponent<Tile>();
							selectedTile.PaintSelected();
							selectedUnit.DragMoveToTile(selectedTile);
							break;
						};
					}	
				}
				break;
			case 1:
				
				break;
			}
		}
	}
	
	//used after ending movement phase
	public void Reset()
	{
		DeselectUnit();
	}
	
	public Unit GetSelectedUnit()
	{
		return selectedUnit;
	}
	
	public void SelectUnit(Unit unit)
	{
		selectedObj = unit.gameObject;
		selectedUnit = unit;
		selectedUnit.Selected();
		bUnitSelected = true;
		//overlord.UnitSelected();
	}
}
