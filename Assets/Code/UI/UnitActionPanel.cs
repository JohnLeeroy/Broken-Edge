using UnityEngine;
using System.Collections;

public class UnitActionPanel : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void onSingleShotPressed()
	{
		NotificationCenter.PostNotification (this, "SingleShot");
	}

	public void onMultiShotPressed()
	{
		NotificationCenter.PostNotification (this, "MultiShot");
	}

	public void onReloadPressed()
	{
		NotificationCenter.PostNotification (this, "Reload");
	}
}
