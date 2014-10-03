using UnityEngine;
using System.Collections;

public class ShowCover : MonoBehaviour {

	// Use this for initialization
	void Start () {
		MapTile[] mapTiles = gameObject.GetComponentsInChildren<MapTile>();
		foreach(MapTile tile in mapTiles)
		{
			for(int i = 0; i < 4; i++)
			{
				if(tile.cover[i] > 0)
				{
					tile.renderer.material.SetColor("_Color", Color.gray);	
					break;
				}
			}
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
