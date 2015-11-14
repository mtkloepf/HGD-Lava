using UnityEngine;
using System.Collections;

public class HexScript : MonoBehaviour {

	public Vector2 position = Vector2.zero;
	public Sprite defaultSprite;
	public Sprite blueSprite;
	public Sprite redSprite;
	public Sprite greenSprite;
	public Sprite plainSprite;
	public Sprite forestSprite;
	public Sprite mountainSprite;
	public Sprite desertSprite;

	public enum HexEnum{plains, forest, mountain, desert};
	HexEnum type = HexEnum.plains;


	SpriteRenderer render;
	bool focus = false;
        bool occupied = false;
	
	// Use this for initialization
	void Start () {
		render = GetComponent<SpriteRenderer> ();
		render.color = Color.white;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void startRenderer() {
		render = GetComponent<SpriteRenderer> ();
	}

	// Gets the type of terrain
	public HexEnum getType() {
		return type;
	}

	// Sets the type of terrain
	public void setType(HexEnum type) {
		this.type = type;
		switch (type) {

		case HexEnum.desert:
			render.sprite = desertSprite;
			break;

		case HexEnum.plains:
			render.sprite = plainSprite;
			break;

		case HexEnum.forest:
			render.sprite = forestSprite;
			break;

		case HexEnum.mountain:
			render.sprite = mountainSprite;
			break;

		default:
			render.sprite = defaultSprite;
			break;
		}
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
//			render.color = Color.blue;
			render.color = new Color(0f, 0f, 1f, 0.9f);
		} else {
			render.color = Color.white;
		}
	}

	// Says if the hex is focused
	public bool getFocus() {
		return focus;
	}

	// Makes the hex red, indicating that that unit has already moved
	public void makeRed() {
		render.color = Color.red;
		render.color = new Color (1, 0, 0, 0.9f);
	}

	// Makes the hex green, indicating that that unit may still move
	public void makeGreen() {
		render.color = Color.green;
		render.color = new Color (0, 1, 0, 0.9f);
	}

	// Makes the hex the default color
	public void makeDefault() {
		render.color = Color.white;
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
