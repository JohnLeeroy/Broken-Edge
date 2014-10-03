using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {
	
	public int xIndex, yIndex;
	public int mapIndex;
	public int height = 0;
	public bool bExplored = false;
	public bool bOccupied;
	
	Color alternateColor;
	
	public float moveCost = 10;
	public float pathCost = 0;
	public Tile parent;
	
	public int elevation = 0;
	public int[] cover;
	
	public Unit unit;
	
	public enum eState{NORMAL, MOVE, GREY_MOVE, ATTACK, RUN, PATH}
	
	eState state;
	// Use this for initialization
	
	void Start () {
		state = eState.NORMAL;
		//cover = new int[]{0,0,0,0};
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void Init(int arrayIndex, int xIndex, int yIndex)
	{
		mapIndex = arrayIndex;	
		this.xIndex = xIndex;
		this.yIndex = yIndex;
	}
	public void SetColor()
	{
		alternateColor = new Color(.8f,.8f,.8f);
		renderer.material.SetColor("_Color", alternateColor);	
	}
	
	public void PaintMovementTile()
	{
		state = eState.MOVE;
		renderer.material.SetColor("_Color", Color.green);	
	}
	
	public void PaintGreyTile()
	{
		state = eState.GREY_MOVE;
		renderer.material.SetColor("_Color", Color.grey);	
	}
	
	public void ResetPaint()
	{
		switch(state)
		{
		case eState.NORMAL:
			renderer.material.SetColor("_Color", Color.white);
			break;
		case eState.MOVE:
			PaintMovementTile();
			break;
		case eState.ATTACK:
			PaintAttack();
			break;
		case eState.PATH:
			PaintPath();
			break;
		case eState.GREY_MOVE:
			PaintGreyTile();
			break;
		case eState.RUN:
			
			break;
		};
	}
	
	public void PaintPath()
	{
		state = eState.PATH;
		renderer.material.SetColor("_Color", Color.green);
	}
	
	public void PaintSelected()
	{
		renderer.material.SetColor("_Color", Color.blue);
	}
	
	public void PaintAttack()
	{
		state = eState.ATTACK;
		renderer.material.SetColor("_Color", Color.red);
	}
	
	public void ResetTile()
	{
		parent = null;
		pathCost = 0;
		bExplored = false;
		state = eState.NORMAL;
		ResetPaint();
	}
	
	public void Occupy(Unit _unit)
	{
		unit = _unit;
	}
	
}
