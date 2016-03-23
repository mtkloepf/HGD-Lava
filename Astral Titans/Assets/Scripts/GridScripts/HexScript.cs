using UnityEngine;
using System.Collections;

public class HexScript : MonoBehaviour {

	public Vector2 position = Vector2.zero;
	public Sprite standardSprite;
	public Sprite redSprite;
	public Sprite blueSprite;
	public Sprite pinkSprite;

	public enum HexEnum : int { plains = 0, water = 1, mountain = 2, desert = 3 };
	HexEnum type = HexEnum.plains; //Default to plains
	// Determine if the tile is out of sight range
	private bool fog_cover = false;

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

    // Start the sprite renderer
	public void startRenderer() {
		render = GetComponent<SpriteRenderer> ();
	}

	// Gets the type of terrain
	public HexEnum getType() {
		return type;
	}

	// Sets the occupied variable to the given value
	public void setOccupied(bool occ) {
		occupied = occ;
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
		if (!getFocus () && render.sprite != redSprite) {
			makeFocused ();
		}
	}
	
    // Mouse leaves a hovered hex
    void OnMouseExit() {
		if (!getFocus() && render.sprite != redSprite) {
			makeDefault();
		}
	}

	// Sets the focus of the hex
	public void setFocus (bool focused) {
		focus = focused;

		if (focused) {
			makeFocused();
		} else {
			makeDefault();
		}
	}

	// Says if the hex is focused
	public bool getFocus() {
		return focus;
	}

	// Makes the hex red, indicating that that unit has already attacked
	public void makeRed() {
		render.sprite = (fog_cover) ? SpriteManagerScript.fogSprite : redSprite;
	}

	// Makes the hex pink, indicating that the unit has already moved but can still attack
	public void makePink() {
		render.sprite = (fog_cover) ? SpriteManagerScript.fogSprite : pinkSprite;
	}

	// Makes the hex the default color
	public void makeDefault() {
		render.sprite = (fog_cover) ? SpriteManagerScript.fogSprite : standardSprite;
	}

	public void  makeFocused() {
		render.sprite = (fog_cover) ? SpriteManagerScript.blueFogSprite : blueSprite;
	}

	public void refreshFog() {
		render.sprite = (fog_cover) ? SpriteManagerScript.fogSprite : standardSprite;
	}

	/* Set if the tile is covered in fog */
	public void set_fog_cover(bool covered) {
		if (covered != fog_cover) {
				fog_cover = covered;
				makeDefault();
		}
	}

	/* Return if the tile is covered in fog */
	public bool covered_in_fog() { return fog_cover; }

	// Gets the position of the transform of the hex
	public Vector2 getTransformPosition() {
		Vector2 pos = new Vector2 (transform.position.x, transform.position.y);
		return pos;
	}
	
	// Moves the currently focused unit to this hex
	void OnMouseDown() {
		// Will only work for non-occupied tiles and will override unit movement!
		if (HexManagerScript.edit_hex()) {
			// If fog tile editing is enabled then toggle the hexes fog cover flag
			if (HexManagerScript.fog_toggle()) {
				set_fog_cover(!fog_cover);
			} else { // Switch the hex type to the current edit type
				setType(HexManagerScript.edit_type());
			}
		} else {
			// Necessary for clicking hexes to work in test map generation scene
			if (GameManagerScript.instance != null) {
				bool occ = GameManagerScript.instance.hexClicked(this);
				occupied = occ;
			}
		}

		//Debug.Log("(" + position.x + " , " + position.y + ")\n");
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

	/* Sets the type of the hex based on the integer-hex type pairing
	 * established by the HexEnum enumeration.*/
	public void setType(int type) {
		// Generate a plains
		if (type == (int)HexEnum.plains) {
			setType (HexScript.HexEnum.plains,
				SpriteManagerScript.plainsSprite, 
				SpriteManagerScript.redPlainsSprite,
				SpriteManagerScript.bluePlainsSprite,
				SpriteManagerScript.pinkPlainsSprite);
		}
		// Generate a desert
		else if (type == (int)HexEnum.desert) {
			setType (HexScript.HexEnum.desert,
				SpriteManagerScript.desertSprite, 
				SpriteManagerScript.redDesertSprite,
				SpriteManagerScript.blueDesertSprite,
				SpriteManagerScript.pinkDesertSprite);
		}
		// Generate water
		else if (type == (int)HexEnum.water) {
			setType (HexScript.HexEnum.water,
				SpriteManagerScript.waterSprite, 
				SpriteManagerScript.redWaterSprite,
				SpriteManagerScript.blueWaterSprite,
				SpriteManagerScript.pinkWaterSprite);
		}
		// Generate a mountain
		else if (type == (int)HexEnum.mountain) {
			setType (HexScript.HexEnum.mountain,
				SpriteManagerScript.mountainSprite, 
				SpriteManagerScript.redMountainSprite,
				SpriteManagerScript.blueMountainSprite,
				SpriteManagerScript.pinkMountainSprite);
		}
	}

	// Sets the type of terrain
	private void setType(HexEnum type, Sprite standard, 
		Sprite red, Sprite blue, Sprite pink) {
		this.type = type;
		if(type == HexEnum.mountain || 
			type == HexEnum.water) occupied = true;
		standardSprite = standard;
		redSprite = red;
		blueSprite = blue;
		pinkSprite = pink;

		render.sprite = standardSprite;
	}

	public override string ToString() {
		return type.ToString () + ": (" + position.x + ", " + position.y + ")";
	}
}
