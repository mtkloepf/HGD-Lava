using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitScript : MonoBehaviour
{
	public GameObject HPBar;

	public Sprite Sprite;
	public SortedDictionary<HexScript.HexEnum, int> terrainMap = new SortedDictionary<HexScript.HexEnum, int> ();
	private Animator animator;
	// Used to disable the Unit in fog tiles
	private SpriteRenderer _renderer;

	public Vector2 position = Vector2.zero;
	public bool focus = false;
	public int player;
	public bool hasMoved;

	//public bool canAttack = true;
	public bool hasAttacked;

	public bool mouseOver = false;

	public enum Types : int {
		H_Infantry = 0,
		H_Exo = 1,
		H_Tank = 2,
		H_Artillery = 3,

		A_Infantry = 4,
		A_Elite = 5,
		A_Tank = 6,
		A_Artillery = 7,

		H_Base = 8,
		A_Base = 9
	};

	private static readonly int MAX_HEALTH = 100;
	private int health;

	private int attack;
	private int defense;
    private int range;
	private int movement;

	// Use this for initialization
	void Start() {
		health =  MAX_HEALTH;
		animator = GetComponent<Animator>();
		_renderer = GetComponent<SpriteRenderer>();
		hasMoved = false;
		hasAttacked = false;
		// Fix local positioning of the unit
		Vector3 pos = gameObject.transform.localPosition;
		gameObject.transform.localPosition = new Vector3(pos.x, pos.y, -0.5f);
	}

	public void updateTurn () {
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
	public void setType(int type) {
		switch (type) {
		
		case (int)Types.H_Infantry:
			attack = 2;
			defense = 1;
            range = 2;
			movement = 4;
			terrainMap.Add(HexScript.HexEnum.water, 100);
			terrainMap.Add(HexScript.HexEnum.mountain, 2);
			terrainMap.Add(HexScript.HexEnum.plains, 1);
			terrainMap.Add(HexScript.HexEnum.desert, 2);
			break;
		
		case (int)Types.H_Exo:
			attack = 4;
			defense = 3;
            range = 1;
			movement = 3;
			terrainMap.Add (HexScript.HexEnum.water, 100);
			terrainMap.Add (HexScript.HexEnum.mountain, 1);
			terrainMap.Add (HexScript.HexEnum.plains, 1);
			terrainMap.Add (HexScript.HexEnum.desert, 1);
			break;
		
		case (int)Types.H_Tank:
			attack = 7;
			defense = 8;
            range = 1;
			movement = 5;
			terrainMap.Add (HexScript.HexEnum.water, 100);
			terrainMap.Add (HexScript.HexEnum.mountain, 100);
			terrainMap.Add (HexScript.HexEnum.plains, 1);
			terrainMap.Add (HexScript.HexEnum.desert, 2);
			break;
			
		case (int)Types.H_Artillery:
			attack = 8;
			defense = 2;
            range = 3;
			movement = 3;
			terrainMap.Add (HexScript.HexEnum.water, 100);
			terrainMap.Add (HexScript.HexEnum.mountain, 100);
			terrainMap.Add (HexScript.HexEnum.plains, 1);
			terrainMap.Add (HexScript.HexEnum.desert, 3);
			break;
		
		case (int)Types.H_Base:
			attack = 0;
			defense = 9;
            range = 1;
			movement = 4;
			terrainMap.Add (HexScript.HexEnum.water, 1);
			terrainMap.Add (HexScript.HexEnum.mountain, 1);
			terrainMap.Add (HexScript.HexEnum.plains, 1);
			terrainMap.Add (HexScript.HexEnum.desert, 1);
			break;
		
		case (int)Types.A_Infantry:
			attack = 2;
			defense = 1;
            range = 2;
			movement = 4;
			terrainMap.Add(HexScript.HexEnum.water, 100);
			terrainMap.Add(HexScript.HexEnum.mountain, 2);
			terrainMap.Add(HexScript.HexEnum.plains, 1);
			terrainMap.Add(HexScript.HexEnum.desert, 2);
			break;

		case (int)Types.A_Elite:
			attack = 4;
			defense = 3;
            range = 1;
			movement = 3;
			terrainMap.Add (HexScript.HexEnum.water, 100);
			terrainMap.Add (HexScript.HexEnum.mountain, 1);
			terrainMap.Add (HexScript.HexEnum.plains, 1);
			terrainMap.Add (HexScript.HexEnum.desert, 1);
			break;

		case (int)Types.A_Tank:
			attack = 7;
			defense = 8;
            range = 1;
			movement = 5;
			terrainMap.Add (HexScript.HexEnum.water, 100);
			terrainMap.Add (HexScript.HexEnum.mountain, 100);
			terrainMap.Add (HexScript.HexEnum.plains, 1);
			terrainMap.Add (HexScript.HexEnum.desert, 2);
			break;

		case (int)Types.A_Artillery:
			attack = 8;
			defense = 2;
            range = 3;
			movement = 3;
			terrainMap.Add (HexScript.HexEnum.water, 100);
			terrainMap.Add (HexScript.HexEnum.mountain, 100);
			terrainMap.Add (HexScript.HexEnum.plains, 1);
			terrainMap.Add (HexScript.HexEnum.desert, 3);
			break;

		case (int)Types.A_Base:
			attack = 0;
			defense = 9;
            range = 1;
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
			-0.5f);
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

	// Sets the health and modifies the Unit's hp bar
	public void setHealth (int h) {
		health = h;

		if (health > MAX_HEALTH) {
			health = MAX_HEALTH;
		} else if (health < 0) {
			health = 0;
		}

		// Original position/scale
		Transform HpGreenPosition = HPBar.transform;
		// x scale and position of the green bar change based on health percentage 
		var x_scale = 0.5f * health / MAX_HEALTH;
		var x_value = 0.5f * x_scale - 0.25f;
		// update x position and scale
		HpGreenPosition.localPosition = new Vector3 (x_value, HpGreenPosition.localPosition.y, HpGreenPosition.localPosition.z);
		HpGreenPosition.localScale = new Vector3 ( x_scale, HpGreenPosition.transform.localScale.y, HpGreenPosition.transform.localScale.z);
	}

	// Gets the health
	public int getHealth () {
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

    public int getRange()
    {
        return range;
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
		// Disable the Unit in fog tiles
		disable_in_fog( GameManagerScript.instance.Map.map[(int)position.x][(int)position.y].covered_in_fog() );
	}

	/* Disable/Enable unit rendering and interaction with the mouse */
	private void disable_in_fog(bool covered) {
		
		if (_renderer.enabled == covered) {
			// Modify all sprite renderers for a Unit (including HP bars)
			_renderer.enabled = !covered;
			SpriteRenderer[] renderers = (SpriteRenderer[])GetComponentsInChildren<SpriteRenderer>();

			foreach (SpriteRenderer sr in renderers) {
				sr.enabled = !covered;
			}

			// Mouse ignores objects in Ignore Raycast layer
			gameObject.layer = (covered) ? LayerMask.NameToLayer("Ignore Raycast") : 0;
		}
	}

	// Selects the unit and sets it to be focused in the game manager
	void OnMouseDown () {
		// Unit not hidden in fog
		if (_renderer.enabled) {
			// deselect unit if it is already selected
			if (GameManagerScript.instance.getFocusedUnit() == this) {
				GameManagerScript.instance.selectFocus(null);
			} else { // select a unit
				if (GameManagerScript.instance.getTurn() == player) {
					GameManagerScript.instance.selectFocus(this);
					//Debug.Log("Player selected");
				} else {
					GameManagerScript.instance.attack(this);
				}
			}
		}
	}

	// Check if the mouse is over the unit and display the stats if it is
	void OnMouseEnter() {
		mouseOver = _renderer.enabled;
	}

	// Check if the mouse has left the unit and stop displaying the stats if it has
	void OnMouseExit() {
		mouseOver = false;
	}
}
