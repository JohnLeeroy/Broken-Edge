using UnityEngine;
using System.Collections;

public class MapTile : MonoBehaviour {
	
	public int xIndex, yIndex;
	public int mapIndex;
	
	public float moveCost = 10;
	public int height = 0;
	public int[] cover;
	
	Color alternateColor;
	
	void Awake()
	{
		cover = new int[]{0,0,0,0};
	}
	void Start () {
	}
	
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
	
	public void ResetPaint()
	{
		renderer.material.SetColor("_Color", Color.white);
		/*
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
		case eState.RUN:
			
			break;
		};
		*/
	}
	public void PaintSelected()
	{
		renderer.material.SetColor("_Color", Color.green);
	}
	public void PaintPath()
	{
		renderer.material.SetColor("_Color", Color.green);
	}
	
	public void IncreaseHeight()
	{
		height += 1;	
		transform.position += Vector3.up;
	}
	
	public void DecreaseHeight()
	{
		height -= 1;	
		transform.position -= Vector3.up;
		
	}
	
	public void IncreaseCover()
	{
		for(int i = 0; i < cover.Length; i++)
			cover[i]++;
	}
	
	public void DecreaseCover()
	{
		for(int i = 0; i < cover.Length; i++)
			cover[i]--;
	}
	
	public void PaintAttack()
	{
		renderer.material.SetColor("_Color", Color.red);
	}
	
	public void ResetTile()
	{
		ResetPaint();
	}
	
	public void SetSpawn()
	{
		renderer.material.SetColor("_Color", Color.magenta);
	}
}
