using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* Select Squad
 * Choose to Move
 * Select Individual Units
 * Drag Individual Units
 * Select Other squad, repeat
 */
public class MapSelector : MonoBehaviour {
	
	bool bUnitSelected = false;
	
	
	public List<MapTile> lSelected;
	MapTile first; 
	MapTile second; 
	
	public bool bActive = true;
	int nPhase = 0;
	//Ray
	Ray ray;
	RaycastHit hit;
	
	Rect guiRect;
	
	// Use this for initialization
	void Start () 
	{
		lSelected = new List<MapTile>();
		
		guiRect = Camera.mainCamera.GetComponent<MapBuilderGUI>().guiRect;
	}
	
	void SelectUnit(GameObject target)
	{
		
	}
	
	void DeselectUnit()
	{

	}
	
	void Drag(Vector3 hitPoint)
	{
		//selectedObj.transform.position = new Vector3(hitPoint.x, selectedObj.transform.position.y, hitPoint.z);
		
	}
	
	void DragToEdge(Vector3 hitPoint)
	{
		//Vector3 templatePos = SixInchTemplate.transform.position;
		//vecToEdge = hitPoint - templatePos;
		//edgePos = new Vector3(templatePos.x + vecToEdge.normalized.x * 6, selectedObj.transform.position.y, templatePos.z + vecToEdge.normalized.z * 6);
		//selectedObj.transform.position = edgePos;
	}
		
	void ClearSelected()
	{
		foreach(MapTile tile in lSelected){	
			tile.ResetPaint();	
		}
		lSelected.Clear();
	}
	void CalculateSelected()
	{
		MapBuilder map = GameObject.Find("World").GetComponent<MapBuilder>();
		int mapHeight = map.height;
		int mapWidth = map.width;

		
		Debug.Log(first.xIndex + " " + first.yIndex + " " + second.xIndex + " " + second.yIndex);
		
		int left, right, top, bot;
		left = Mathf.Min(first.xIndex, second.xIndex);
		top = Mathf.Max(first.yIndex, second.yIndex);
		right = Mathf.Max(first.xIndex, second.xIndex);
		bot = Mathf.Min(first.yIndex, second.yIndex);
		
		int width = Mathf.Abs(first.xIndex - second.xIndex);
		int height = Mathf.Abs(first.yIndex - second.yIndex);
		
		Debug.Log("Left " + left);
		Debug.Log("Top " + top);
		Debug.Log("Right " + right);
		Debug.Log("Botm " + bot);
		
		Debug.Log("Width " + width);
		Debug.Log("Height " + height);
		
		Debug.Log("Map height" + mapHeight);
		Debug.Log("Map width" + mapWidth);
		ClearSelected();
		for( int y = 0; y <= height; y++)
		{
			for(int x = 0; x <= width; x++)
			{
				MapTile selectedTile = map.map[left + x + (bot * mapWidth) + (y * mapWidth)];
				selectedTile.PaintSelected();
				lSelected.Add(selectedTile);
			}
		}
		
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
						//if(lSelected.Count == 0)
						{
		           		 	Debug.DrawLine(ray.origin, hit.point);
							MapTile tile = hit.transform.GetComponent<MapTile>();
							tile.PaintSelected();
							first = tile;
							lSelected.Add(tile);
							//nPhase = 1;
							/*if(hit.transform.tag == "unit")
							{	
								Squad hitSquad = hit.transform.gameObject.GetComponent<Unit>().GetSquad();
								if(!hitSquad.bMoved)
									SelectSquad(hitSquad);
							}*/
						}
						//else
						{
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
				}
			
			
				
				if(Input.GetMouseButtonUp(0) && !guiRect.Contains(Input.mousePosition))
				{
	  				ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	        		if (Physics.Raycast(ray, out hit, 100))
					{
	           		 	Debug.DrawLine(ray.origin, hit.point);
						MapTile tile = hit.transform.GetComponent<MapTile>();
						tile.PaintSelected();
						second = tile;
						
						lSelected.Add(tile);
						CalculateSelected();
					}
				}
				break;
			case 1:
				
				break;
			}
		}
	}
}
