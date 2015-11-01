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

	// Set containing all hexes that a unit can move to
	HashSet<HexScript> hexSet = new HashSet<HexScript>();

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
	}
	
	// Update is called once per frame
	void Update () {

	}

	// Updates the colors of the hexes
	void updateHexes() {
		foreach (List<HexScript> hexlist in map) {
			foreach (HexScript hex in hexlist) {
				hex.setFocus (false);
			}
		}
		foreach (UnitScript unit in units) {
			if (unit.hasMoved && unit.getPlayer() == turn) {
				// make hex red
				List<HexScript> mapRow = map [(int)unit.getPosition ().x];
				HexScript hex = mapRow [(int)unit.getPosition ().y];
				hex.makeRed ();
			}
			else if (!unit.hasMoved && unit.getPlayer() == turn) {
				List<HexScript> mapRow = map [(int)unit.getPosition ().x];
				HexScript hex = mapRow [(int)unit.getPosition ().y];
				hex.makeGreen ();
			}
		}
	}

	// Gets the turn
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

		updateHexes ();

		Debug.Log ("Turn ended");
	}

	// Moves a unit to a hex
	public void moveCurrentUnit(HexScript hex) {
		// DONE: Limit range of a unit's movement
		// DONE: Zone of control
		// DONE: Only allow unit's to be moved on their turn
		// TODO: Attacking

		// Makes sure there is currently a focused unit
		if (focusedUnit != null) {
			// If the hex is in the set of moveable hexes, move to it
			if (hexSet.Contains(hex)) {
				focusedUnit.move(hex);
				focusedUnit = null;
				focusedHex.setFocus (false);
			}
		}
		updateHexes ();
	}

	// Create all of the hexes in the map
	// TODO: Create an implementation of this method that takes in a 2D array and generates a custom
	//       map with different terrain.
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
	// TODO: Create a method to purchase a unit and place it at a desired location. This
	//       will be a seperate method from this one. Eventually, we will not need this method anymore
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

	// Finds the euclidean distance between two hexes
	float euclideanDist(HexScript hex1, HexScript hex2) {
		float x1 = hex1.getTransformPosition().x;
		float x2 = hex2.getTransformPosition ().x;
		float y1 = hex1.getTransformPosition ().y;
		float y2 = hex2.getTransformPosition ().y;
		float delx = x1 - x2;
		float dely = y1 - y2;

		float dist = Mathf.Sqrt (delx * delx + dely * dely);
		return dist;
	}

	// Finds the euclidean distance between a unit and a hex
	float euclideanDist(UnitScript unit, HexScript hex) {
		float x1 = unit.getTransformPosition().x;
		float x2 = hex.getTransformPosition ().x;
		float y1 = unit.getTransformPosition ().y;
		float y2 = hex.getTransformPosition ().y;
		float delx = x1 - x2;
		float dely = y1 - y2;
				
		float dist = Mathf.Sqrt (delx * delx + dely * dely);
		return dist;
	}

	// Finds the adjacent hexes to a hex and adds them to an iterable Set
	// The returns that set
	HashSet<HexScript> findAdj(HexScript hex) {
		int x = (int) hex.getPosition ().x;
		int y = (int) hex.getPosition ().y;
		HashSet<HexScript> set = new HashSet<HexScript> ();
		// Note: The logic here is clunky and long, but it works and runs in O(1)! Hopefully
		//       it won't need any changes.
		Debug.Log ("map.Count: " + map.Count + " map[x].Count: " + map [x].Count);
		Debug.Log ((y-2) + " <= y <= " + (y+2));
		Debug.Log ((x - 1) + " <= x <= " + (x + 1));
		if (y % 2 == 0) {
			if (y - 2 >= 0) {
				set.Add(map[x][y - 2]);
			}
			if (y - 1 >= 0) {
				set.Add(map[x][y - 1]);
			}
			if (y + 1 < map[x].Count) {
				set.Add(map[x][y + 1]);
			}
			if (y + 2 < map[x].Count) {
				set.Add(map[x][y + 2]);
			}
			if (x - 1 >= 0) {
				if (y + 1 < map[x].Count) {
					set.Add(map[x - 1][y + 1]);
				}
				if (y - 1 >= 0) {
					set.Add(map[x - 1][y - 1]);
				}
			}
		}
		if (y % 2 == 1) {
			if (y - 2 >= 0) {
				set.Add(map[x][y - 2]);
			}
			if (y - 1 >= 0) {
				set.Add(map[x][y - 1]);
			}
			if (y + 1 < map[x].Count) {
				set.Add(map[x][y + 1]);
			}
			if (y + 2 < map[x].Count) {
				set.Add(map[x][y + 2]);
			}
			if (x + 1 < map.Count) {
				if (y + 1 < map[x].Count) {
					set.Add(map[x + 1][y + 1]);
				}
				if (y - 1 >= 0) {
					set.Add(map[x + 1][y - 1]);
				}
			}
		}
		return set;
	}

	// Recursively finds the hexes that a unit can move to given
	// a starting hex and a movement distance
	// TODO: Account for terrain
	// DONE: Account for Zone of Control
	// TODO: Write an implementation of the A* algorithm to find the best path
	void findMovement(int movement, HexScript location, bool moved) {
		bool stopped = false;
		bool adjToEnemy = false;
		HashSet<HexScript> adjSet = findAdj (location);
		hexSet.Add (location);
		if (movement == 0) {} 
		else {
			foreach (HexScript adjHex in adjSet){
				foreach (UnitScript enemy in units) {
					if (enemy.getPlayer() != turn) {
						if (moved && enemy.getPosition() == adjHex.getPosition ()) {
							stopped = true;
							break;
						}
						if (!moved && enemy.getPosition () == adjHex.getPosition ()) {
							adjToEnemy = true;
							break;
						}
					}
				}
			}
			if (adjToEnemy) {
				foreach (HexScript adjHex in adjSet) {
					bool adj = false;
					foreach (UnitScript enemy in units) {
						if (enemy.getPlayer () != turn) {
							if ((enemy.getPosition () == adjHex.getPosition ())) {
								adj = true;
							}
						}
					}
					if (!adj) {
						findMovement (movement - 1, adjHex, true);
					}
				}
			}
			else if (!stopped) {
				foreach (HexScript adjHex in adjSet) {
					findMovement (movement - 1, adjHex, true);
				}
			}
		}
	}

	// Set's a unit to be the current unit in focus
	public void selectFocus(UnitScript unit) {
		focusedUnit = unit;
		if (!unit.hasMoved) {
			List<HexScript> mapRow = map [(int)unit.getPosition ().x];
			HexScript hex = mapRow [(int)unit.getPosition ().y];

			// Reinitialize the hexSet to an empty set
			hexSet = new HashSet<HexScript>();
			// Populate the hexSet with moveable hexes
			findMovement (3, (map[(int)focusedUnit.getPosition().x])[(int)focusedUnit.getPosition ().y], false);
			// For each moveable hex, indicate that it is moveable
			foreach (HexScript moveable in hexSet) {
				moveable.setFocus (true);
			}
			focusedHex = hex;
			Debug.Log("Focused hex: " + hex.getPosition().ToString());
			hex.setFocus (true);
		}
		Debug.Log ("unit selected");
	}
}
