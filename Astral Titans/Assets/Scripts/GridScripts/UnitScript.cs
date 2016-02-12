using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitScript : MonoBehaviour
{
	public GameObject HPBar;

	public Sprite Sprite;
	public SortedDictionary<HexScript.HexEnum, int> terrainMap = new SortedDictionary<HexScript.HexEnum, int> ();
	public static UnitScript instance;
	private Animator animator;

	public Vector2 position = Vector2.zero;
	public bool focus = false;
	public int mapSize = 11;
	public int player;
	public bool hasMoved;

	public bool canAttack = true;
	public bool hasAttacked;

	public bool mouseOver = false;

	public enum Types {
		H_Infantry,
		H_Exo,
		H_Tank,
		H_Artillery,
		H_Base,

		A_Infantry,
		A_Elite,
		A_Tank,
		A_Artillery,
		A_Base
	};

	private int attack;
	private int defense;

	public readonly int maxHealth = 100;
	private int health;
	private int maxMovement;
	private int movement;

	// Use this for initialization
	void Start () {
		health = 100;
		animator = GetComponent<Animator> ();
		hasMoved = false;
		hasAttacked = false;
		// Set HP z value, so it does not overlay the cards
		var original = HPBar.transform.localPosition;
		HPBar.transform.localPosition = new Vector3(original.x, original.y, -1.5f);
	}

	public void updateTurn ()
	{
		hasMoved = false;
		hasAttacked = false;
	}

	public void destroyUnit ()
	{
		if (animator != null)
			animator.SetTrigger ("Death");
		Destroy (this.gameObject, 1);
	}



	// Set the unit type
	public void setType(Types type) {
		switch (type) {
		
		case Types.H_Infantry:
			attack = 1;
			defense = 1;
			maxMovement = 4;
			movement = 4;
			terrainMap.Add(HexScript.HexEnum.water, 100);
			terrainMap.Add(HexScript.HexEnum.mountain, 2);
			terrainMap.Add(HexScript.HexEnum.plains, 1);
			terrainMap.Add(HexScript.HexEnum.desert, 2);
			break;
		
		case Types.H_Exo:
			attack = 2;
			defense = 2;
			maxMovement = 3;
			movement = 3;
			terrainMap.Add (HexScript.HexEnum.water, 100);
			terrainMap.Add (HexScript.HexEnum.mountain, 1);
			terrainMap.Add (HexScript.HexEnum.plains, 1);
			terrainMap.Add (HexScript.HexEnum.desert, 1);
			break;
		
		case Types.H_Tank:
			attack = 5;
			defense = 6;
			maxMovement = 5;
			movement = 5;
			terrainMap.Add (HexScript.HexEnum.water, 100);
			terrainMap.Add (HexScript.HexEnum.mountain, 100);
			terrainMap.Add (HexScript.HexEnum.plains, 1);
			terrainMap.Add (HexScript.HexEnum.desert, 2);
			break;
			
		case Types.H_Artillery:
			attack = 6;
			defense = 4;
			maxMovement = 3;
			movement = 3;
			terrainMap.Add (HexScript.HexEnum.water, 100);
			terrainMap.Add (HexScript.HexEnum.mountain, 100);
			terrainMap.Add (HexScript.HexEnum.plains, 1);
			terrainMap.Add (HexScript.HexEnum.desert, 3);
			break;
		
		case Types.H_Base:
			attack = 0;
			defense = 8;
			maxMovement = 4;
			movement = 4;
			terrainMap.Add (HexScript.HexEnum.water, 1);
			terrainMap.Add (HexScript.HexEnum.mountain, 1);
			terrainMap.Add (HexScript.HexEnum.plains, 1);
			terrainMap.Add (HexScript.HexEnum.desert, 1);
			break;
		
		case Types.A_Infantry:
			attack = 1;
			defense = 1;
			maxMovement = 4;
			movement = 4;
			terrainMap.Add(HexScript.HexEnum.water, 100);
			terrainMap.Add(HexScript.HexEnum.mountain, 2);
			terrainMap.Add(HexScript.HexEnum.plains, 1);
			terrainMap.Add(HexScript.HexEnum.desert, 2);
			break;

		case Types.A_Elite:
			attack = 2;
			defense = 2;
			maxMovement = 3;
			movement = 3;
			terrainMap.Add (HexScript.HexEnum.water, 100);
			terrainMap.Add (HexScript.HexEnum.mountain, 1);
			terrainMap.Add (HexScript.HexEnum.plains, 1);
			terrainMap.Add (HexScript.HexEnum.desert, 1);
			break;

		case Types.A_Tank:
			attack = 5;
			defense = 6;
			maxMovement = 5;
			movement = 5;
			terrainMap.Add (HexScript.HexEnum.water, 100);
			terrainMap.Add (HexScript.HexEnum.mountain, 100);
			terrainMap.Add (HexScript.HexEnum.plains, 1);
			terrainMap.Add (HexScript.HexEnum.desert, 2);
			break;

		case Types.A_Artillery:
			attack = 6;
			defense = 4;
			maxMovement = 3;
			movement = 3;
			terrainMap.Add (HexScript.HexEnum.water, 100);
			terrainMap.Add (HexScript.HexEnum.mountain, 100);
			terrainMap.Add (HexScript.HexEnum.plains, 1);
			terrainMap.Add (HexScript.HexEnum.desert, 3);
			break;

		case Types.A_Base:
			attack = 0;
			defense = 8;
			maxMovement = 4;
			movement = 4;
			terrainMap.Add (HexScript.HexEnum.water, 1);
			terrainMap.Add (HexScript.HexEnum.mountain, 1);
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
	void Update () {
		// Original position/scale
		Transform HpGreenPosition = HPBar.transform;
		// x scale and position of the green bar change based on health percentage 
		var x_scale = 0.5f * health / maxHealth;
		var x_value = 0.5f * x_scale - 0.25f;
		// update x position and scale
		HpGreenPosition.localPosition = new Vector3 (x_value, HpGreenPosition.localPosition.y,   HpGreenPosition.localPosition.z);
		HpGreenPosition.localScale = new Vector3 ( x_scale, HpGreenPosition.transform.localScale.y, HpGreenPosition.transform.localScale.z);

	}

	// Selects the unit and sets it to be focused in the game manager
	void OnMouseDown () {
		if (GameManagerScript.instance.getTurn () == player) {
			GameManagerScript.instance.selectFocus (this);
			Debug.Log ("Player selected");
		} else {
			GameManagerScript.instance.attack (this);
		}
	}

	// Check if the mouse is over the unit and display the stats if it is
	void OnMouseEnter() {
		mouseOver = true;
	}

	// Check if the mouse has left the unit and stop displaying the stats if it has
	void OnMouseExit() {
		mouseOver = false;
	}
}
