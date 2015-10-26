using UnityEngine;
using System.Collections;

public class UnitScript : MonoBehaviour {

	public static UnitScript instance;

	public Vector2 position = Vector2.zero;
	public bool focus = false;
	public int mapSize = 11;
	public int player;
	public bool hasMoved;
	
	// Use this for initialization
	void Start () {
		hasMoved = false;
	}

	public void updateTurn() {
		hasMoved = false;
	}

	// Gets the player of the unit
	public int getPlayer() {
		return player;
	}

	// Sets the player of the unit. p should be 1 or 2
	public void setPlayer(int p) {
		player = p;
	}

	// Moves the player to a hex, if the player has not moved yet.
	public void move(HexScript hex) {
		if (!hasMoved) {
			position = hex.getPosition ();
			transform.position = new Vector3 (
				hex.transform.position.x,
				hex.transform.position.y,
				hex.transform.position.z - 10f);
			hasMoved = true;
		}
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
		transform.position = new Vector3 (
			hex.transform.position.x,
			hex.transform.position.y,
			hex.transform.position.z - 10f);
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
		if (GameManagerScript.instance.getTurn () == player) {
			GameManagerScript.instance.selectFocus (this);
			Debug.Log ("Player selected");
		}
	}
}
