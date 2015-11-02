using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitScript : MonoBehaviour {

	readonly int InfantryHHealth = 5;
	readonly int InfantryAHealth = 5;
	readonly int HeavyTankHHealth = 10;
	readonly int HeavyTankAHealth = 10;
	readonly int InfantryHAttack = 2;
	readonly int InfantryAAttack = 2;
	readonly int HeavyTankHAttack = 3;
	readonly int HeavyTankAAttack = 3;
	readonly int InfantryHMovement = 3;
	readonly int InfantryAMovement = 3;
	readonly int HeavyTankHMovement = 2;
	readonly int HeavyTankAMovement = 2;
	public Sprite InfantryHSprite;
	public Sprite InfantryASprite;
	public Sprite HeavyTankHSprite;
	public Sprite HeavyTankASprite;

	public SortedDictionary<HexScript.HexEnum, int> terrainMap = new SortedDictionary<HexScript.HexEnum, int>();

	public static UnitScript instance;
	SpriteRenderer render;

	public Vector2 position = Vector2.zero;
	public bool focus = false;
	public int mapSize = 11;
	public int player;
	public bool hasMoved;

	int attack;
	int maxHealth;
	int health;
	int maxMovement;
	int movement;
	
	// Use this for initialization
	void Start () {
		hasMoved = false;
		render = GetComponent<SpriteRenderer> ();
		Debug.Log (render.GetType ());
//		render.sprite = InfantryHSprite;

//		setType (Types.HeavyTankA);
	}

	public void startRenderer() {
		render = GetComponent<SpriteRenderer> ();
	}

	public void updateTurn() {
		hasMoved = false;
	}

	public void destroy() {
		Destroy (gameObject);
	}

	public enum Types { InfantryH, InfantryA, HeavyTankH, HeavyTankA};

	// Set the unit type
	public void setType(Types type) {
		switch (type) {
		
		case Types.InfantryH:
			attack = InfantryHAttack;
			maxHealth = InfantryHHealth;
			health = InfantryHHealth;
			maxMovement = InfantryHMovement;
			movement = InfantryHMovement;
			render.sprite = InfantryHSprite;
			terrainMap.Add(HexScript.HexEnum.forest, 1);
			terrainMap.Add(HexScript.HexEnum.mountain, 2);
			terrainMap.Add(HexScript.HexEnum.plains, 1);
			terrainMap.Add(HexScript.HexEnum.desert, 2);
			Debug.Log ("Human Infantry");
			break;
		
		case Types.InfantryA:
			attack = InfantryAAttack;
			maxHealth = InfantryAHealth;
			health = InfantryAHealth;
			maxMovement = InfantryAMovement;
			movement = InfantryAMovement;
			render.sprite = InfantryASprite;
			terrainMap.Add(HexScript.HexEnum.forest, 1);
			terrainMap.Add(HexScript.HexEnum.mountain, 2);
			terrainMap.Add(HexScript.HexEnum.plains, 1);
			terrainMap.Add(HexScript.HexEnum.desert, 2);
			Debug.Log ("Alien Infantry");
			break;

		case Types.HeavyTankH:
			attack = HeavyTankHAttack;
			maxHealth = HeavyTankHHealth;
			health = maxHealth;
			maxMovement = HeavyTankHMovement;
			movement = maxMovement;
			render.sprite = HeavyTankHSprite;
			terrainMap.Add(HexScript.HexEnum.forest, 2);
			terrainMap.Add(HexScript.HexEnum.mountain, 2);
			terrainMap.Add(HexScript.HexEnum.plains, 1);
			terrainMap.Add(HexScript.HexEnum.desert, 1);
			Debug.Log ("Human Heavy Tank");
			break;

		case Types.HeavyTankA:
			attack = HeavyTankAAttack;
			maxHealth = HeavyTankAHealth;
			health = maxHealth;
			maxMovement = HeavyTankAMovement;
			movement = maxMovement;
			render.sprite = HeavyTankASprite;
			terrainMap.Add(HexScript.HexEnum.forest, 2);
			terrainMap.Add(HexScript.HexEnum.mountain, 2);
			terrainMap.Add(HexScript.HexEnum.plains, 1);
			terrainMap.Add(HexScript.HexEnum.desert, 1);
			Debug.Log ("Alien Heavy Tank");
			break;

		default:
			Debug.Log ("Default case");
			break;
		}
	}

	// Gets the player of the unit
	public int getPlayer() {
		return player;
	}

	// Sets the player of the unit. p should be 1 or 2
	public void setPlayer(int p) {
		player = p;
	}

	// Gets the remaining number of hexes the player can move this turn
	public int getMovement() {
		return movement;
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

	// Returns the position of the transform
	public Vector2 getTransformPosition() {
		Vector2 pos = new Vector2 (transform.position.x, transform.position.y);
		return pos;
	}

	// Sets the health
	public void setHealth(int health) {
		this.health = health;
	}

	// Gets the health
	public int getHealth() {
		return health;
	}

	// Gets the attack
	public int getAttack() {
		return attack;
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
		} else {
			GameManagerScript.instance.attack(this);
		}
	}
}
