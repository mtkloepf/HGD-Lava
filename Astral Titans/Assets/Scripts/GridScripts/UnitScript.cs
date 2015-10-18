﻿using UnityEngine;
using System.Collections;

public class UnitScript : MonoBehaviour {

	public Vector2 position = Vector2.zero;
	public bool focus = false;
	public int mapSize = 11;
	
	// Use this for initialization
	void Start () {
		position = new Vector2 (0, 0);
	}

	// Sets the position of the unit to a vector
	// TODO: Modify this to allow for the offset of hexes
	public void setPosition(Vector2 v2) {
		position = v2;
		transform.position = new Vector3(this.position.x - Mathf.Floor(mapSize/2), -this.position.y + Mathf.Floor(mapSize/2), -9);
	}

	// Sets the position of the unit to a hex.
	// NOTE: This works much better than the other set position method,
	// and should be used whenever possible.
	public void setPosition(HexScript hex) {
		position = hex.getPosition ();
		transform.position = hex.transform.position;
	}

	// Returns the position of the unit
	public Vector2 getPosition() {
		return position;
	}

	// Sets the unit to be focused.
	// NOTE: I am not sure if this is necessary. We
	//       may want to remove it later
	public void setFocus(bool focus) {
		this.focus = focus;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// Selects the unit and sets it to be focused in the game manager
	void OnMouseDown() {
		GameManagerScript.instance.selectFocus (this);
		Debug.Log ("Player selected");
	}
}
