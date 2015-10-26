using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManagerScript : MonoBehaviour {

	public static GameManagerScript instance;

	public GameObject TilePrefab;
	public GameObject UserPlayerPrefab;
	public GameObject EndTurn;
	public GameObject AIPlayerPrefab;
	public GameObject TurnIndicator;

	public int mapSize = 11;
	public int mapWidth = 10;
	public int mapHeight = 40;

	public int turn;

	// List of all the hexes
	List <List<HexScript>> map = new List<List<HexScript>>();
	// List of all the units in the game
	List <UnitScript> units = new List<UnitScript>();

	// Clicking on a unit will make it focused
	UnitScript focusedUnit;
	HexScript focusedHex;
	
	void Awake() {
		instance = this;
	}
	
	// Use this for initialization
	void Start () {		
		generateMap();
		generateUnits();
		turn = 1;
		endTurn ();
		endTurn ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void updateHexes() {
		foreach (UnitScript unit in units) {
			if (unit.hasMoved && unit.getPlayer() == turn) {
				// make hex red
				List<HexScript> mapRow = map [(int)unit.getPosition ().x];
				HexScript hex = mapRow [(int)unit.getPosition ().y];
				hex.makeRed ();
			}
		}
	}

	public int getTurn() {
		return turn;
	}

	// Call at the end of a turn to update the game.
	public void endTurn() {
		foreach (UnitScript unit in units) {
			unit.updateTurn();
			List<HexScript> mapRow = map [(int)unit.getPosition ().x];
			HexScript hex = mapRow [(int)unit.getPosition ().y];
			hex.makeDefault ();
		}
		if (turn == 1) {
			turn = 2;
		} else {
			turn = 1;
		}

		Debug.Log ("Turn ended");
	}

	// Moves a unit to a hex
	public void moveCurrentUnit(HexScript hex) {
		// TODO: Limit range of a unit's movement
		// TODO: Zone of control
		// TODO: Only allow unit's to be moved on their turn
		// TODO: Attacking

		// Makes sure there is currently a focused unit
		if (focusedUnit != null) {
			// Sets the unit's new position
			focusedUnit.move (hex);
			// Sets the focused unit to be null
			focusedUnit = null;
			focusedHex.setFocus (false);
		}
		updateHexes ();
	}

	// Create all of the hexes in the map
	void generateMap() {
		map = new List<List<HexScript>>();
		for (int i = 0; i < mapWidth; i++) {
			List <HexScript> row = new List<HexScript>();
			for (int j = 0; j < mapHeight; j++) {
				HexScript hex;
				// If else statements to create an offset, because we are using hexes.
				// This needs polishing.
				if (j % 2 == 1) {
					hex = ((GameObject)Instantiate(TilePrefab, new Vector3(i * 0.9f + 0.45f - Mathf.Floor(mapSize/2), -(j + 0f)/4f + Mathf.Floor(mapSize/2), 1), Quaternion.Euler(new Vector3()))).GetComponent<HexScript>();
				}
				else {
					hex = ((GameObject)Instantiate(TilePrefab, new Vector3(i * 0.9f - Mathf.Floor(mapSize/2), -j/4f + Mathf.Floor(mapSize/2), 1), Quaternion.Euler(new Vector3()))).GetComponent<HexScript>();
				}
				hex.setPosition(new Vector2((float) i , (float) j));
				row.Add (hex);
			}
			map.Add(row);
		}
	}

	// Creates a unit for testing purposes. Additional units can be added if desired.
	// TODO: Create a method to purchase a unit and place it at a desired location.
	void generateUnits() {

		UnitScript unit;
		unit = ((GameObject)Instantiate(UserPlayerPrefab, new Vector3(0 - Mathf.Floor(mapSize/2), -0 + Mathf.Floor(mapSize/2), -1), Quaternion.Euler(new Vector3()))).GetComponent<UnitScript>();
		units.Add(unit);
		unit.move (map [0] [1]);
		unit.setPlayer (1);

		unit = ((GameObject)Instantiate(UserPlayerPrefab, new Vector3(0 - Mathf.Floor(mapSize/2), -0 + Mathf.Floor(mapSize/2), -1), Quaternion.Euler(new Vector3()))).GetComponent<UnitScript>();
		units.Add(unit);
		unit.move (map [2] [3]);
		unit.setPlayer (1);

		unit = ((GameObject)Instantiate(UserPlayerPrefab, new Vector3(4 - Mathf.Floor(mapSize/2), -5 + Mathf.Floor(mapSize/2), -1), Quaternion.Euler(new Vector3()))).GetComponent<UnitScript>();
		unit.setPlayer (2);
		unit.move (map [4] [5]);
		units.Add (unit);
	}

	// Set's a unit to be the current unit in focus
	public void selectFocus(UnitScript unit) {
		focusedUnit = unit;
		if (!unit.hasMoved) {
			List<HexScript> mapRow = map [(int)unit.getPosition ().x];
			HexScript hex = mapRow [(int)unit.getPosition ().y];
			focusedHex = hex;
			Debug.Log("Focused hex: " + hex.getPosition().ToString());
			hex.setFocus (true);
		}
		Debug.Log ("unit selected");
	}
}
