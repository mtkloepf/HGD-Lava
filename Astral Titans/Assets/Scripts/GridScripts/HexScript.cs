using UnityEngine;
using System.Collections;

public class HexScript : MonoBehaviour {

	public Vector2 position = Vector2.zero;
	public Sprite standardSprite;
	public Sprite redSprite;
	public Sprite blueSprite;

	public enum HexEnum{plains, water, mountain, desert};
	HexEnum type = HexEnum.plains; //Default to plains

	SpriteRenderer render;
	bool focus = false;
        bool occupied = false;
	
	// Use this for initialization
	void Start () {
		transform.SetParent (GameObject.Find("HexManager").transform);
        startRenderer();
		// Do not remove this or the hexes will not be displayed!!
		gameObject.transform.localScale = new Vector3(1, 1, 0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

        // Start the sprite renderer
	public void startRenderer() {
		render = GetComponent<SpriteRenderer> ();
	}

	// Gets the type of terrain
	public HexEnum getType() {
		return type;
	}

        // Returns if a hex cannot be traveled to
        public bool getOccupied() {
           return occupied;
        }

	// Sets the position of the hex.
	public void setPosition (Vector2 v2) {
		position = v2;
	}

	// Returns the position of the hex
	public Vector2 getPosition() {
		return position;
	}
	
    // Mouse is hovered over a hex
    void OnMouseEnter() {
    	if(!getFocus() && render.sprite != redSprite)
			render.sprite = blueSprite;
	}
	
    // Mouse leaves a hovered hex
    void OnMouseExit() {
        if(!getFocus() && render.sprite != redSprite)
			render.sprite = standardSprite;
	}

	// Sets the focus of the hex
	public void setFocus (bool focused) {
		focus = focused;
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
		// Vector2 v = new Vector2 (position.x, position.y);
		bool occ = GameManagerScript.instance.hexClicked(this);
        occupied = occ;
		// Debug.Log("(" + position.x + " , " + position.y + ")\n");
	}

	/* Creates a hex at the given row and column value on the map with the given type.
	 * 0 -> plains; 1 -> desert; 2 -> water; and 3 -> mountain */
	public static HexScript createHex(int row, int column, int map_size, int type) {
		HexScript hex;

		if (column % 2 == 1) {
			hex = ((GameObject)Instantiate (PrefabManager.TilePrefab, 
				new Vector3 (row * 0.9f + 0.45f - Mathf.Floor (map_size / 2), 
					-(column + 0f) / 4f + Mathf.Floor (map_size / 2), 1), 
				Quaternion.Euler (new Vector3 ()))).GetComponent<HexScript>();
		} else {
			hex = ((GameObject)Instantiate (PrefabManager.TilePrefab, 
				new Vector3 (row * 0.9f - Mathf.Floor (map_size / 2), 
					-column / 4f + Mathf.Floor (map_size / 2), 1), 
				Quaternion.Euler (new Vector3 ()))).GetComponent<HexScript>();
		}

		hex.setPosition(new Vector2 ((float)row, (float)column));
		hex.startRenderer();
		hex.setType(type);

		//Debug.Log(hex.gameObject.layer);
		return hex;
	}

	public void setType(int type) {
		// Generate a plains
		if (type == 0) {
			setType (HexScript.HexEnum.plains,
				SpriteManagerScript.plainsSprite, 
				SpriteManagerScript.redPlainsSprite,
				SpriteManagerScript.bluePlainsSprite);
		}
		// Generate a desert
		else if (type == 1) {
			setType (HexScript.HexEnum.desert,
				SpriteManagerScript.desertSprite, 
				SpriteManagerScript.redDesertSprite,
				SpriteManagerScript.blueDesertSprite);
		}
		// Generate water
		else if (type == 2) {
			setType (HexScript.HexEnum.water,
				SpriteManagerScript.waterSprite, 
				SpriteManagerScript.redWaterSprite,
				SpriteManagerScript.blueWaterSprite);
		}
		// Generate a mountain
		else if (type == 3) {
			setType (HexScript.HexEnum.mountain,
				SpriteManagerScript.mountainSprite, 
				SpriteManagerScript.redMountainSprite,
				SpriteManagerScript.blueMountainSprite);
		}
	}

	// Sets the type of terrain
	private void setType(HexEnum type, Sprite standard, 
		Sprite red, Sprite blue) {
		this.type = type;
		if(type == HexEnum.mountain || 
			type == HexEnum.water) occupied = true;
		standardSprite = standard;
		redSprite = red;
		blueSprite = blue;

		render.sprite = standardSprite;
	}

	public override string ToString() {
		return type.ToString () + ": (" + position.x + ", " + position.y + ")";
	}
}
