using UnityEngine;
using System.Collections;

public class HexScript : MonoBehaviour {

	public Vector2 position = Vector2.zero;
	public Sprite defaultSprite;
	public Sprite blueSprite;
	public Sprite redSprite;
	public Sprite greenSprite;


	SpriteRenderer render;
	bool focus = false;
	
	// Use this for initialization
	void Start () {
		render = GetComponent<SpriteRenderer> ();
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
//		render.sprite = blueSprite;
	}
	
	void OnMouseExit() {
		// TODO: Indicate that the mouse has left the hex by reverting the image
//		render.sprite = defaultSprite;
	}

	public void setFocus (bool focused) {
		this.focus = focused;
		if (focused) {
			render.sprite = blueSprite;
		} else {
			render.sprite = defaultSprite;
		}
	}

	public void makeRed() {
		render.sprite = redSprite;
	}

	public void makeDefault() {
		render.sprite = defaultSprite;
	}
	
	// Moves the currently focused unit to this hex
	void OnMouseDown() {
		Vector2 v = new Vector2 (position.x, position.y);
		GameManagerScript.instance.moveCurrentUnit(this);
		Debug.Log ("Tile clicked at: " + v.x + ", " + v.y);
	}
}
