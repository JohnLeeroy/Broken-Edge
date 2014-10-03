using UnityEngine;
using System.Collections;

public class RemoveCollider : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Collider[] colliders = gameObject.GetComponentsInChildren<Collider>();
		foreach(Collider collider in colliders)
		{
			collider.enabled = false;	
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
