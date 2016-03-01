using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit : MonoBehaviour {

	private SpriteRenderer _renderer;
	private Animator _animator;
	private Transform hp_bar;

	private Vector2 position;

	/* Assign Arbitrary values to each unit type */
	public enum Type: int {
		H_Infantry = 0,
		H_Exo = 1,
		H_Tank = 2,
		H_Artillery = 3,
		H_Base = 4,

		A_Infantry = 5,
		A_Elite = 6,
		A_Tank = 7,
		A_Artillery = 8,
		A_Base = 9
	};

	/* Determines to which player this unit belongs */
	private int army;
	private Type unit_type;

	/* A unit's HP is scaled to a percent value between 0 and 100% */
	public static readonly int MAX_HEALTH = 100;
	/* The stats of a unit */
	private int health;
	private int attack;
	private int armor;
	private int move_range;
	/* Used to determine a Unit's movement cost on each terrain type. */
	private SortedDictionary<HexScript.HexEnum, int> terrain_costs;

	private bool focus;
	private bool mouse_over;
	private bool moved;
	private bool attacked;

	/* Initialize all the unit's values */
	public void Start() {
		_renderer = GetComponent<SpriteRenderer>();
		_animator = GetComponent<Animator>();
		position = Vector2.zero;

		Transform[] HP_Bar = GetComponentsInChildren<Transform>();
		// Get the HP Bar transform for the green bar
		if (HP_Bar[0].transform.gameObject.name == "HP_Bar") {
			hp_bar = HP_Bar[0];
		} else {
			hp_bar = HP_Bar[1];
		}

		army = -1;
		unit_type = Type.H_Infantry;

		health = MAX_HEALTH;
		attack = 0;
		armor = 0;
		move_range = 0;

		terrain_costs = new SortedDictionary<HexScript.HexEnum, int>();
		
		focus = false;
		mouse_over = false;
		moved = false;
		attacked = false;
	}

	/* Update the image for the Unit's HP bar */
	public void Update() {
		var original_position = hp_bar;
		// x scale and position of the green bar change based on health percentage 
		var x_scale = 0.5f * health / MAX_HEALTH;
		var x_value = 0.5f * x_scale - 0.25f;
		// update x position and scale
		hp_bar.localPosition = new Vector3 (x_value, original_position.localPosition.y,   original_position.localPosition.z);
		hp_bar.localScale = new Vector3 ( x_scale, original_position.transform.localScale.y, original_position.transform.localScale.z);
	}

	/* Resets the move and attack indicator for the Unit */
	public void update_turn() {
		moved = false;
		attacked = false;
	}

	/* Plays the Unit's attack animation */
	public void attack_animation() {
		_animator.SetTrigger("Attack");
	}

	/* Play the Unit's death animation and remove it from the game */
	public void DestroyUnit() {
		_animator.SetTrigger("Death");
		Destroy(this.gameObject, 1);
	}

	public void OnMouseDown() {
		// TODO
	}

	/* Sets the mouse hover flag if the mouse is over this Unit */
	public void OnMouseEnter() { mouse_over = true; }

	/* Disables the mouse hover flag when the mouse leave the Unit */
	public void OnMouseExit() { mouse_over = false; }

	/* Returns if the mouse is over this Unit */
	public bool mouse_is_over() { return mouse_over; }

	/* Sets the Unit's position in the world */
	public void set_transform_position(Vector3 pos) {
		transform.position = new Vector3(pos.x, pos.y, -1);
	}

	/* Sets the Unit's position to the given position and returns the new position value */
	public Vector2 set_position(Vector2 new_position ) { return position = new_position; }

	/* Return the Unit's current position in the map grid */
	public Vector2 get_position() { return position; }

	/* Return the unit's type */
	public int unit_Type() { return (int)unit_type; }

	/* Sets the Unit's army indicator to the given value and returns the new army indicator value */
	public int set_army(int new_army) { return army = new_army; }

	/* Return the Unit's current army indicator value */
	public int get_army() { return army; }

	/* Changes the Unit's current health by the given value and returns the new value */
	public int changeHP(int value) { return health += value; }

	/* Return the Unit's current health value */
	public int get_health() { return health; }

	/* Return the Unit's attack value */
	public int get_attack() { return attack; }

	/* Return the Unit's armor value */
	public int get_armor() { return armor; }

	/* Return the Unit's movement range */
	public int get_move_range() { return move_range; }

	/* Returns the movement costs for the given terrian type */
	public int get_terrain_costs(HexScript.HexEnum hex_type) {
		int cost;
		terrain_costs.TryGetValue(hex_type, out cost);
		return cost;
	}

	/* Flips the Unit's focus value */
	public void switch_focus() { focus = !focus; }

	/* Return if the Unit is the current focus in the game */
	public bool is_focus() { return focus; }

	/* Return if the Unit has attacked yet this turn */
	public bool has_attacked() { return attacked; }

	/* Return if the Unit has moved on this turn */
	public bool has_moved() { return moved; }

	/* This method initializes all the stats of the Unit based on the given type.
	 * This should be called immediately after a unit is instantiated! */
	public void setStatus(Unit.Type t, int player, Vector2 pos) {
		position = pos;
		army = player;
		unit_type = t;
		_renderer.sprite = SpriteManagerScript.get_unit_sprite(t);
		_animator.runtimeAnimatorController = SpriteManagerScript.get_controller(t);


		if (t == Type.H_Infantry) { // Human Infantry
			//_animator.runtimeAnimatorController = 
			attack = 2;
			armor = 1;
			move_range = 4;

			terrain_costs.Add(HexScript.HexEnum.water, 100);
			terrain_costs.Add(HexScript.HexEnum.mountain, 2);
			terrain_costs.Add(HexScript.HexEnum.plains, 1);
			terrain_costs.Add(HexScript.HexEnum.desert, 2);
		} else if (t == Type.H_Exo) { // Human Exosuit
			attack = 4;
			armor = 3;
			move_range = 3;

			terrain_costs.Add(HexScript.HexEnum.water, 100);
			terrain_costs.Add(HexScript.HexEnum.mountain, 1);
			terrain_costs.Add(HexScript.HexEnum.plains, 1);
			terrain_costs.Add(HexScript.HexEnum.desert, 1);
		} else if (t == Type.H_Tank) { // Human Tank
			attack = 7;
			armor = 8;
			move_range = 5;

			terrain_costs.Add(HexScript.HexEnum.water, 100);
			terrain_costs.Add(HexScript.HexEnum.mountain, 100);
			terrain_costs.Add(HexScript.HexEnum.plains, 1);
			terrain_costs.Add(HexScript.HexEnum.desert, 2);
		} else if (t == Type.H_Artillery) { // Human Artillery
			attack = 8;
			armor = 5;
			move_range = 3;

			terrain_costs.Add(HexScript.HexEnum.water, 100);
			terrain_costs.Add(HexScript.HexEnum.mountain, 100);
			terrain_costs.Add(HexScript.HexEnum.plains, 1);
			terrain_costs.Add(HexScript.HexEnum.desert, 3);
		} else if (t == Type.H_Base) { // Human Mobile Base
			attack = 0;
			armor = 8;
			move_range = 4;

			terrain_costs.Add(HexScript.HexEnum.water, 100);
			terrain_costs.Add(HexScript.HexEnum.mountain, 1);
			terrain_costs.Add(HexScript.HexEnum.plains, 1);
			terrain_costs.Add(HexScript.HexEnum.desert, 1);
		} else if (t == Type.A_Infantry) { //  Alien Infantry
			attack = 2;
			armor = 1;
			move_range = 4;

			terrain_costs.Add(HexScript.HexEnum.water, 100);
			terrain_costs.Add(HexScript.HexEnum.mountain, 2);
			terrain_costs.Add(HexScript.HexEnum.plains, 1);
			terrain_costs.Add(HexScript.HexEnum.desert, 2);
		} else if (t == Type.A_Elite) { // Alein Elite Infantry
			attack = 4;
			armor = 3;
			move_range = 3;

			terrain_costs.Add(HexScript.HexEnum.water, 100);
			terrain_costs.Add(HexScript.HexEnum.mountain, 1);
			terrain_costs.Add(HexScript.HexEnum.plains, 1);
			terrain_costs.Add(HexScript.HexEnum.desert, 1);
		} else if (t == Type.A_Tank) { // Alien Tank
			attack = 7;
			armor = 8;
			move_range = 5;

			terrain_costs.Add(HexScript.HexEnum.water, 100);
			terrain_costs.Add(HexScript.HexEnum.mountain, 100);
			terrain_costs.Add(HexScript.HexEnum.plains, 1);
			terrain_costs.Add(HexScript.HexEnum.desert, 2);
		} else if (t == Type.A_Artillery) { // Alien Artillery
			attack = 8;
			armor = 5;
			move_range = 3;

			terrain_costs.Add(HexScript.HexEnum.water, 100);
			terrain_costs.Add(HexScript.HexEnum.mountain, 100);
			terrain_costs.Add(HexScript.HexEnum.plains, 1);
			terrain_costs.Add(HexScript.HexEnum.desert, 3);
		} else if (t == Type.A_Base) { // Alien Mobile Base
			attack = 0;
			armor = 8;
			move_range = 4;

			terrain_costs.Add(HexScript.HexEnum.water, 100);
			terrain_costs.Add(HexScript.HexEnum.mountain, 1);
			terrain_costs.Add(HexScript.HexEnum.plains, 1);
			terrain_costs.Add(HexScript.HexEnum.desert, 1);
		}
	}
}
