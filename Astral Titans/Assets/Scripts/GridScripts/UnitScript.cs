using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitScript : MonoBehaviour
{
	
	readonly int InfantryHAttack = 20;
	readonly int InfantryAAttack = 20;
	readonly int HeavyTankHAttack = 30;
	readonly int HeavyTankAAttack = 30;
	readonly float InfantryHDef = 10;
	readonly float InfantryADef = 10;
	readonly float HeavyTankHDef = 40;
	readonly float HeavyTankADef = 40;
	readonly int InfantryHMovement = 4;
	readonly int InfantryAMovement = 4;
	readonly int HeavyTankHMovement = 2;
	readonly int HeavyTankAMovement = 2;
	public Sprite Sprite;
	public SortedDictionary<HexScript.HexEnum, int> terrainMap = new SortedDictionary<HexScript.HexEnum, int> ();
	public static UnitScript instance;
	private Animator animator;

	public Vector2 position = Vector2.zero;
	public bool focus = false;
	public int mapSize = 11;
	public int player;
	public bool hasMoved;
	int attack;
	int maxHealth = 100;
	int health = 100;
	int maxMovement;
	int movement;
	float defense;

	// Use this for initialization
	void Start ()
	{
		animator = GetComponent<Animator> ();
		hasMoved = false;
	}

	public void updateTurn ()
	{
		hasMoved = false;
	}

	public void destroyUnit ()
	{
		if (animator != null)
			animator.SetTrigger ("Death");
		Destroy (this.gameObject, 1);
	}

	public enum Types
	{
		InfantryH,
		InfantryA,
		HeavyTankH,
		HeavyTankA
	}
	;

	// Set the unit type
	public void setType (Types type)
	{
		switch (type) {
		
		case Types.InfantryH:
			attack = InfantryHAttack;
			maxMovement = InfantryHMovement;
			movement = InfantryHMovement;
			defense = InfantryHDef;
			terrainMap.Add (HexScript.HexEnum.water, 1);
			terrainMap.Add (HexScript.HexEnum.mountain, 2);
			terrainMap.Add (HexScript.HexEnum.plains, 1);
			terrainMap.Add (HexScript.HexEnum.desert, 2);
			break;
		
		case Types.InfantryA:
			attack = InfantryAAttack;
			maxMovement = InfantryAMovement;
			movement = InfantryAMovement;
			defense = InfantryADef;
			terrainMap.Add (HexScript.HexEnum.water, 1);
			terrainMap.Add (HexScript.HexEnum.mountain, 2);
			terrainMap.Add (HexScript.HexEnum.plains, 1);
			terrainMap.Add (HexScript.HexEnum.desert, 2);
			break;

		case Types.HeavyTankH:
			attack = HeavyTankHAttack;
			maxMovement = HeavyTankHMovement;
			movement = maxMovement;
			defense = HeavyTankHDef;
			terrainMap.Add (HexScript.HexEnum.water, 2);
			terrainMap.Add (HexScript.HexEnum.mountain, 2);
			terrainMap.Add (HexScript.HexEnum.plains, 1);
			terrainMap.Add (HexScript.HexEnum.desert, 1);
			break;

		case Types.HeavyTankA:
			attack = HeavyTankAAttack;
			maxMovement = HeavyTankAMovement;
			movement = maxMovement;
			defense = HeavyTankADef;
			terrainMap.Add (HexScript.HexEnum.water, 2);
			terrainMap.Add (HexScript.HexEnum.mountain, 2);
			terrainMap.Add (HexScript.HexEnum.plains, 1);
			terrainMap.Add (HexScript.HexEnum.desert, 1);
			break;

		default:
			Debug.Log ("Default case");
			break;
		}
	}

	// Gets the player of the unit
	public int getPlayer ()
	{
		return player;
	}

	// Sets the player of the unit. p should be 1 or 2
	public void setPlayer (int p)
	{
		player = p;
	}

	// Gets the remaining number of hexes the player can move this turn
	public int getMovement ()
	{
		return movement;
	}

	// Moves the player to a hex, if the player has not moved yet.
	public void move (HexScript hex)
	{
		if (!hasMoved) {
			setPosition (hex);
			hasMoved = true;
		}
	}

	// Sets the position of the unit to a hex.
	// NOTE: This works much better than the other set position method,
	// and should be used whenever possible.
	public void setPosition (HexScript hex)
	{
		position = hex.getPosition ();
		transform.position = new Vector3 (
			hex.transform.position.x,
			hex.transform.position.y,
			hex.transform.position.z - 1f);
	}

	// Returns the position of the unit
	public Vector2 getPosition ()
	{
		return position;
	}

	// Returns the position of the transform
	public Vector2 getTransformPosition ()
	{
		Vector2 pos = new Vector2 (transform.position.x, transform.position.y);
		return pos;
	}

	// Sets the health
	public void setHealth (int health)
	{
		this.health = health;
	}

	// Gets the health
	public int getHealth ()
	{
		return health;
	}

	// Gets the attack
	public int getAttack ()
	{
		return attack;
	}

	// Gets the defense
	public float getDefense ()
	{
		return defense;
	}

	// Sets the unit to be focused.
	// NOTE: I am not sure if this is necessary. We
	//       may want to remove it later
	public void setFocus (bool focus)
	{
		this.focus = focus;
	}

	public void attackEnemy ()
	{
		if (animator != null) {
			animator.SetTrigger ("Attack");
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	// Selects the unit and sets it to be focused in the game manager
	void OnMouseDown ()
	{
		if (GameManagerScript.instance.getTurn () == player) {
			GameManagerScript.instance.selectFocus (this);
			Debug.Log ("Player selected");
		} else {
			GameManagerScript.instance.attack (this);
		}
	}
}
