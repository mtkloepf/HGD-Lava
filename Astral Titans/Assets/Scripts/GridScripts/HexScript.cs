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
		render.sprite = defaultSprite;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void startRenderer() {
		render = GetComponent<SpriteRenderer> ();
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

	// Sets the focus of the hex
	public void setFocus (bool focused) {
		this.focus = focused;
		if (focused) {
			render.sprite = blueSprite;
		} else {
			render.sprite = defaultSprite;
		}
	}

	// Makes the hex red, indicating that that unit has already moved
	public void makeRed() {
		render.sprite = redSprite;
	}

	// Makes the hex green, indicating that that unit may still move
	public void makeGreen() {
		render.sprite = greenSprite;
	}

	// Makes the hex the default color
	public void makeDefault() {
		render.sprite = defaultSprite;
	}

	// Gets the position of the transform of the hex
	public Vector2 getTransformPosition() {
		Vector2 pos = new Vector2 (transform.position.x, transform.position.y);
		return pos;
	}
	
	// Moves the currently focused unit to this hex
	void OnMouseDown() {
		Vector2 v = new Vector2 (position.x, position.y);
		GameManagerScript.instance.moveCurrentUnit(this);
		Debug.Log ("Tile clicked at: " + v.x + ", " + v.y);
	}
}
