using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManagerScript : MonoBehaviour {

	public static GameManagerScript instance;
	
	public GameObject TilePrefab;
	public GameObject UserPlayerPrefab;
	public GameObject AIPlayerPrefab;
	
	public int mapSize = 11;
	public int mapWidth = 10;
	public int mapHeight = 40;

	// List of all the hexes
	List <List<HexScript>> map = new List<List<HexScript>>();
	// List of all the units in the game
	List <UnitScript> units = new List<UnitScript>();

	// Clicking on a unit will make it focused
	UnitScript focusedUnit;
	
	void Awake() {
		instance = this;
	}
	
	// Use this for initialization
	void Start () {		
		generateMap();
		generateUnits();
	}
	
	// Update is called once per frame
	void Update () {

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
			focusedUnit.setPosition (hex);
			// Sets the focused unit to be null
			focusedUnit = null;
		}
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
					hex = ((GameObject)Instantiate(TilePrefab, new Vector3(i * 0.9f + 0.45f - Mathf.Floor(mapSize/2), -(j + 0.33f)/4f + Mathf.Floor(mapSize/2), 1), Quaternion.Euler(new Vector3()))).GetComponent<HexScript>();
				}
				else {
					hex = ((GameObject)Instantiate(TilePrefab, new Vector3(i * 0.9f - Mathf.Floor(mapSize/2), -j/4f + Mathf.Floor(mapSize/2), 1), Quaternion.Euler(new Vector3()))).GetComponent<HexScript>();
				}
				hex.setPosition(new Vector2((float) i , (float) j));
				row.Add (hex);
				Debug.Log ("Tile added at position: (" + hex.getPosition().x + ", " + hex.getPosition().y + ")");
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
		focusedUnit = unit;
	}

	// Set's a unit to be the current unit in focus
	public void selectFocus(UnitScript unit) {
		focusedUnit = unit;
		Debug.Log ("unit selected");
	}
}
