using UnityEngine;
using System.Collections;

public class EndTurnScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Ends the turn
	void OnMouseDown() {
		GameManagerScript.instance.endTurn ();
	}
}
