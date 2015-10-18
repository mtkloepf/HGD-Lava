using UnityEngine;
using System.Collections;

public class HexScript : MonoBehaviour {

	public Vector2 position = Vector2.zero;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// Sets the position of the hex.
	public void setPosition (Vector2 v2) {
		position = v2;
		Debug.Log ("New position: " + getPosition ().x + ", " + getPosition ().y);
	}

	// Returns the position of the hex
	public Vector2 getPosition() {
		return position;
	}
	
	void OnMouseEnter() {
		// TODO: Indicate that the mouse is in the current hex by changing the image
	}
	
	void OnMouseExit() {
		// TODO: Indicate that the mouse has left the hex by reverting the image
	}
	
	// Moves the currently focused unit to this hex
	void OnMouseDown() {
		Vector2 v = new Vector2 (position.x, position.y);
		GameManagerScript.instance.moveCurrentUnit(this);
		Debug.Log ("Tile clicked at: " + v.x + ", " + v.y);
	}
}
