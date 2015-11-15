using UnityEngine;
using System.Collections;

public class HexScript : MonoBehaviour {

	public Vector2 position = Vector2.zero;
	public Sprite standardSprite;
	public Sprite redSprite;
	public Sprite blueSprite;

	public enum HexEnum{plains, water, mountain, desert};
	HexEnum type = HexEnum.plains;

	SpriteRenderer render;
	bool focus = false;
        bool occupied = false;
	
	// Use this for initialization
	void Start () {
           startRenderer();
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
	public void setType(HexEnum type, Sprite standard, 
              Sprite red, Sprite blue) {
		this.type = type;
                standardSprite = standard;
                redSprite = red;
                blueSprite = blue;

                render.sprite = standardSprite;
	}

	// Sets the position of the hex.
	public void setPosition (Vector2 v2) {
		position = v2;
	}

	// Returns the position of the hex
	public Vector2 getPosition() {
		return position;
	}
	
        void OnMouseEnter() {
           if(!getFocus())
		render.sprite = blueSprite;
	}
	
        void OnMouseExit() {
           if(!getFocus())
		render.sprite = standardSprite;
	}

	// Sets the focus of the hex
	public void setFocus (bool focused) {
		this.focus = focused;
		if (focused) {
                        render.sprite = blueSprite;
		} else {
                        render.sprite = standardSprite;
		}
	}

	// Says if the hex is focused
	public bool getFocus() {
		return focus;
	}

	// Makes the hex red, indicating that that unit has already moved
	public void makeRed() {
           render.sprite = redSprite;
	}

	// Makes the hex the default color
	public void makeDefault() {
		render.sprite = standardSprite;
	}

	// Gets the position of the transform of the hex
	public Vector2 getTransformPosition() {
		Vector2 pos = new Vector2 (transform.position.x, transform.position.y);
		return pos;
	}
	
	// Moves the currently focused unit to this hex
	void OnMouseDown() {
		Vector2 v = new Vector2 (position.x, position.y);
		GameManagerScript.instance.hexClicked(this);
	}
}
