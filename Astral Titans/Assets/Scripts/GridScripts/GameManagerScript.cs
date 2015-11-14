using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManagerScript : MonoBehaviour {

	public static GameManagerScript instance;

	public GameObject TilePrefab;
	public GameObject UserPlayerPrefab;
	public GameObject EndTurn;
	public GameObject AIPlayerPrefab;
	public GameObject CardPrefab;
	public TurnIndicatorScript TurnIndicator;
        public GameObject UI;

	public int mapSize = 11;
	public int mapWidth = 10;
	public int mapHeight = 40;

	public int turn;

	// List of all the hexes
	List <List<HexScript>> map = new List<List<HexScript>>();
	// List of all the units in the game
	List <UnitScript> units = new List<UnitScript>();
	// List of the cards in a hand
	List<CardScript> hand = new List<CardScript> ();

	// Set containing all hexes that a unit can move to
	HashSet<HexScript> hexSet = new HashSet<HexScript>();

	// Clicking on a unit will make it focused
	UnitScript focusedUnit;
	HexScript focusedHex;
	CardScript focusedCard;
	
	void Awake() {
		instance = this;
	}
	
	// Use this for initialization
	void Start () {		
           UI.GetComponentInChildren<Canvas>().enabled = false;
		generateMap();
		generateUnits();
		generateCards ();
		turn = 1;
		//endTurn ();
		//endTurn ();
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
                TurnIndicator.updateTurn(turn);
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
				hex.startRenderer ();
				hex.setType (HexScript.HexEnum.plains);
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
		unit.startRenderer ();
		unit.setType (UnitScript.Types.InfantryH);
		unit.setPlayer (1);

		unit = ((GameObject)Instantiate(UserPlayerPrefab, new Vector3(0 - Mathf.Floor(mapSize/2), -0 + Mathf.Floor(mapSize/2), -1), Quaternion.Euler(new Vector3()))).GetComponent<UnitScript>();
		units.Add(unit);
		unit.move (map [2] [3]);
		unit.startRenderer ();
		unit.setType (UnitScript.Types.HeavyTankH);
		unit.setPlayer (1);

		unit = ((GameObject)Instantiate(UserPlayerPrefab, new Vector3(4 - Mathf.Floor(mapSize/2), -5 + Mathf.Floor(mapSize/2), -1), Quaternion.Euler(new Vector3()))).GetComponent<UnitScript>();
		unit.setPlayer (2);
		unit.startRenderer ();
		unit.setType (UnitScript.Types.HeavyTankA);
		unit.move (map [4] [5]);
		units.Add (unit);

		unit = ((GameObject)Instantiate(UserPlayerPrefab, new Vector3(4 - Mathf.Floor(mapSize/2), -5 + Mathf.Floor(mapSize/2), -1), Quaternion.Euler(new Vector3()))).GetComponent<UnitScript>();
		unit.setPlayer (2);
		unit.startRenderer ();
		unit.setType (UnitScript.Types.InfantryA);
		unit.move (map [2] [5]);
		units.Add (unit);

		unit = ((GameObject)Instantiate(UserPlayerPrefab, new Vector3(4 - Mathf.Floor(mapSize/2), -5 + Mathf.Floor(mapSize/2), -1), Quaternion.Euler(new Vector3()))).GetComponent<UnitScript>();
		unit.setPlayer (2);
		unit.startRenderer ();
		unit.setType (UnitScript.Types.InfantryA);
		unit.move (map [3] [5]);
		units.Add (unit);

		unit = ((GameObject)Instantiate(UserPlayerPrefab, new Vector3(4 - Mathf.Floor(mapSize/2), -5 + Mathf.Floor(mapSize/2), -1), Quaternion.Euler(new Vector3()))).GetComponent<UnitScript>();
		unit.setPlayer (2);
		unit.startRenderer ();
		unit.setType (UnitScript.Types.HeavyTankA);
		unit.move (map [1] [5]);
		units.Add (unit);
	}

	void generateCards() {
		CardScript card;

		card = ((GameObject)Instantiate (CardPrefab, new Vector3 (0, -1.5f, 0), Quaternion.Euler (new Vector3 ()))).GetComponent<CardScript> ();

		hand.Add (card);

		card = ((GameObject)Instantiate (CardPrefab, new Vector3 (1f, -1.5f, 0), Quaternion.Euler (new Vector3 ()))).GetComponent<CardScript> ();
		
		hand.Add (card);

		card = ((GameObject)Instantiate (CardPrefab, new Vector3 (-1f, -1.5f, 0), Quaternion.Euler (new Vector3 ()))).GetComponent<CardScript> ();
		
		hand.Add (card);
	}


	// Finds the adjacent hexes to a hex and adds them to an iterable Set
	// The returns that set
	HashSet<HexScript> findAdj(HexScript hex) {
		int x = (int) hex.getPosition ().x;
		int y = (int) hex.getPosition ().y;
		HashSet<HexScript> set = new HashSet<HexScript> ();
		// Note: The logic here is clunky and long, but it works and runs in O(1)! Hopefully
		//       it won't need any changes.
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
						int cost;
						focusedUnit.terrainMap.TryGetValue(adjHex.getType (), out cost);
						if (movement - cost >= 0) {
							findMovement (movement - cost, adjHex, true);
						}
					}
				}
			}
			else if (!stopped) {
				foreach (HexScript adjHex in adjSet) {
					int cost;
					focusedUnit.terrainMap.TryGetValue(adjHex.getType(), out cost);
					if (movement - cost >= 0) {
						findMovement (movement - cost, adjHex, true);
					}
				}
			}
		}
	}

	// Attacks another unit. This is currently extremely primitive.
	// TODO: Animations
	// TODO: Decide on a combat system and implement it
	// TODO: Limit units to attacking once per turn
	public void attack(UnitScript unit) {
		if (focusedUnit != null) {
			bool adj = false;
			List<HexScript> focMapRow = map [(int)focusedUnit.getPosition ().x];
			HexScript focHex = focMapRow [(int)focusedUnit.getPosition ().y];

			List<HexScript> curMapRow = map [(int)(unit.getPosition ().x)];
			HexScript curHex = curMapRow [(int)unit.getPosition ().y];


			HashSet<HexScript> adjHexes = findAdj (curHex);
			foreach (HexScript hex in adjHexes) {
				if (hex == focHex) {
					adj = true;
				}
			}
			if (adj) {
				Debug.Log (focusedUnit.getPosition () + " attacked " + unit.getPosition ());

				int h = unit.getHealth ();

				// Reduces the attacked units health by the attacking units attack
				unit.setHealth ((int)(unit.getHealth () - 
				                (focusedUnit.getAttack () * (1 - unit.getDefense ()/100))));

				Debug.Log ("Attack: " + focusedUnit.getAttack() + 
				           "\nDefense: " + unit.getDefense () + 
				           "\nPrevious health: " + h + 
				           "\nResulting health lost: " + (focusedUnit.getAttack () * (1 - unit.getDefense ()/100)));

				// If the unit is out of health, destroy it
				if (unit.getHealth() <= 0) {
					unit.destroy ();
					units.Remove(unit);
					Destroy (unit);
				}
			}
		} else {
			Debug.Log ("No unit currently focused...");
		}
	}

	public void hexClicked(HexScript hex) {
		if (focusedUnit != null) {
			moveCurrentUnit (hex);
		} else if (focusedCard != null) {
			placeUnit (hex);
		}
	}

	public void placeUnit(HexScript hex) {
		int x = (int)(hex.getPosition ().x);
		int y = (int)(hex.getPosition ().y);

		UnitScript unit;

		unit = ((GameObject)Instantiate(UserPlayerPrefab, new Vector3(4 - Mathf.Floor(mapSize/2), -5 + Mathf.Floor(mapSize/2), -1), Quaternion.Euler(new Vector3()))).GetComponent<UnitScript>();
		unit.setPlayer (turn);
		unit.startRenderer ();
		unit.setType (UnitScript.Types.HeavyTankA);
		unit.move (map [x] [y]);
		units.Add (unit);

		focusedCard = null;


	}

	public void selectCard(CardScript card) {
		focusedUnit = null;
		updateHexes ();
		focusedCard = card;
	}

	// Set's a unit to be the current unit in focus
	public void selectFocus(UnitScript unit) {
		focusedUnit = unit;
		if (!unit.hasMoved) {
			updateHexes ();
			List<HexScript> mapRow = map [(int)unit.getPosition ().x];
			HexScript curHex = mapRow [(int)unit.getPosition ().y];

			// Reinitialize the hexSet to an empty set
			hexSet = new HashSet<HexScript>();
			// Populate the hexSet with moveable hexes
			findMovement (focusedUnit.getMovement (), (map[(int)focusedUnit.getPosition().x])[(int)focusedUnit.getPosition ().y], false);
			// For each moveable hex, indicate that it is moveable
			foreach (HexScript moveable in hexSet) {
				moveable.setFocus (true);
			}
			focusedHex = curHex;
			Debug.Log("Focused hex: " + curHex.getPosition().ToString());
			curHex.setFocus (true);
		}
		Debug.Log ("unit selected");
	}

        public void togglePauseMenu() {
           if (UI.GetComponentInChildren<Canvas>().enabled)
           {
              UI.GetComponentInChildren<Canvas>().enabled = false;
              Time.timeScale = 1;
           }
           else
           {
              UI.GetComponentInChildren<Canvas>().enabled = true;
              Time.timeScale = 0;
           }
        }
}
