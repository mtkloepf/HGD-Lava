﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManagerScript : MonoBehaviour
{

	public static GameManagerScript instance;
	public static MapManager Map;

	public GameObject EndTurn;
	public GameObject AIPlayerPrefab;

	// Shop Canvas UI Element
	public Canvas shopCanvas;

	public static PlayerScript Player1;
	public static PlayerScript Player2;
	private static HandScript[] hand_display;

	public TurnIndicatorScript TurnIndicator;
	public GameObject UI;
	private static int turn;
	private GameObject musicSlider;
	public static bool paused = false;
	private float timer;

	private float cardStartX = 0;
	private float cardStartY = 0;
	public float cardVelX;
	public float cardVelY;


	private UnitScript p1Base;
	private UnitScript p2Base;

	// List of all the units in the game
	static List <UnitScript> units;

	// Set containing all hexes that a unit can move to
	HashSet<HexScript> hexSet = new HashSet<HexScript>();

	// Set containing all hexes that can be seen
	HashSet<HexScript> visibleHexes = new HashSet<HexScript>();

	// Clicking on a unit will make it focused
	private static UnitScript focusedUnit;
	private static HexScript focusedHex;
	private static CardScript focusedCard;

	static GameManagerScript() {
		Map = new MapManager(SceneTransitionStorage.map_width, SceneTransitionStorage.map_height, SceneTransitionStorage.map_type);
	}

	void Awake () {
		instance = this;
	}
	
	// Use this for initialization
	void Start () {
		Debug.Log (SpriteManagerScript.pinkPlainsSprite == null);
		// Initialize hand display
		GameObject[] card_frames = GameObject.FindGameObjectsWithTag("Hand");
		hand_display = new HandScript[DeckManager.MAX_HAND_SIZE];

		for (int idx = 0; idx < card_frames.Length; ++idx) {
			hand_display[idx] = card_frames[idx].GetComponent<HandScript>();
		}

		musicSlider = GameObject.Find ("Slider");
		UI.GetComponentInChildren<Canvas> ().enabled = false;
		shopCanvas.enabled = false;

		units = new List<UnitScript>();

		// map setup
		Map.generatePseudoRandomMap();

		// give starting deck specifications
		DeckManager d1 = new DeckManager( new CardScript.CardType[] {CardScript.CardType.Currency1, CardScript.CardType.Currency2, CardScript.CardType.HumanInfantry, CardScript.CardType.HumanTank}, new int[] {7, 2, 2, 1} );
		DeckManager d2 = new DeckManager( new CardScript.CardType[] {CardScript.CardType.Currency1, CardScript.CardType.Currency2, CardScript.CardType.AlienInfantry, CardScript.CardType.AlienTank}, new int[] {7, 2, 2, 1} );
		
		// player setup
		Player1 = new PlayerScript(d1);
		Player2 = new PlayerScript(d2);
		Player1.getDeck().deck.shuffle();
		Player2.getDeck().deck.shuffle();
		turn = 1;

		getPlayer().getDeck().deal();
		drawCards();
		updateHexes();
		// place mobile bases
		p1Base = placeUnit ( (int)UnitScript.Types.H_Base, 1, 2 );
		p1Base.setPlayer (1);
		p2Base = placeUnit ( (int)UnitScript.Types.A_Base, Map.width - 2, Map.height - 3 );
		p2Base.setPlayer (2);
	}

	int stupidFix = 0;

	// Update is called once per frame
	void Update () {
		if (stupidFix == 0) {
			stupidFix++;
			selectFocus (p1Base);
			updateVisibleHexes ();
			foreach (List<HexScript> hexList in Map.map) {
				foreach (HexScript tempHex in hexList) {
					tempHex.checkForFog(); 
					tempHex.makeDefault();
				}
			}
			updateHexes ();
		}
		if (stupidFix == 1) {
			stupidFix++;
			focusedUnit = null;
		}

		timer = Time.deltaTime;
		
		if (Input.GetKey ("w")) {
			cardStartY += cardVelY;
		}
		if (Input.GetKey ("a")) {
			cardStartX -= cardVelX;
		}
		if (Input.GetKey ("s")) {
			cardStartY -= cardVelY;
		}
		if (Input.GetKey ("d")) {
			cardStartX += cardVelX;
		}
		/* Allows the player to convert bronze to silver and silver to gold
		 * hold 'c' and press '2' to
		 * 		remove 3 currency and replace 5 bronze (in discard pile) with 1 silver (placed in discard pile).
		 * hold 'c' and press '3' to
		 * 		remove 7 currency and replace 2 silver (in discard pile) with 1 gold (placed in discard pile).
		 */
		if (Input.GetKey("c")) {
			if (Input.GetKeyDown("2")) {
				Debug.Log( "Bronze: " + getPlayer().getDeck().discardPile.getCount(CardScript.CardType.Currency1) + "\n" );

				if (getPlayer().getCurrency() >= 3 && getPlayer().getDeck().discardPile.getCount(CardScript.CardType.Currency1) >= 5) {
					int removed = getPlayer().getDeck().removeCardsFromDiscard(CardScript.CardType.Currency1, 5);
					Debug.Log("Removed: " + removed + "\n");
					getPlayer().getDeck().discardPile.add( new CardScript(CardScript.CardType.Currency2) );
					getPlayer().changeCurrency(-3);
				} 
			} else if (Input.GetKeyDown("3")) {
				Debug.Log( "Silver: " + getPlayer().getDeck().discardPile.getCount(CardScript.CardType.Currency2) + "\n" );

				if (getPlayer().getCurrency() >= 7 && getPlayer().getDeck().discardPile.getCount(CardScript.CardType.Currency2) >= 2) {
					int removed = getPlayer().getDeck().removeCardsFromDiscard(CardScript.CardType.Currency2, 2);
					Debug.Log("Removed: " + removed + "\n");
					getPlayer().getDeck().discardPile.add( new CardScript(CardScript.CardType.Currency3) );
					getPlayer ().changeCurrency(-7);
				}
			}
		}
	}

	// Returns currently seletec unit
	public static UnitScript getFocusedUnit () {
		return focusedUnit;
	}

	// Returns currently selected card
	public static CardScript getFocusedCard() {
		return focusedCard;
	}

	// Gets the turn
	public static int getTurn () {
		return turn;
	}

	/* Returns the current player's deck size */
	public static int getDeckCount () {
		return getPlayer().getDeck().deck.getSize();
	}

	/* Returns the current player's discard pile size */
	public static int getDiscardCount () {
		return getPlayer().getDeck().discardPile.getSize();
	}

	/* Returns the current player. */
	public static PlayerScript getPlayer() {
		return (turn == 1) ? Player1 : Player2;
	}

	/* Returns the hand of the current player. */
	public static CardCollection currentHand() {
		return getPlayer().getDeck().hand;
	}

	// Updates the colors of the hexes
	public static void updateHexes ()
	{
		foreach (List<HexScript> hexlist in Map.map) {
			foreach (HexScript hex in hexlist) {
				hex.setFocus (false);
			}
		}
		foreach (UnitScript unit in units) {
			if (unit.hasMoved && unit.getPlayer () == turn) {
				// make hex red
				List<HexScript> mapRow = Map.map [(int)unit.getPosition ().x];
				HexScript hex = mapRow [(int)unit.getPosition ().y];

				if (unit.getAttack() == 0 || unit.hasAttacked) {
					hex.makeRed ();
				} else if (unit.hasMoved) {
					hex.makePink ();
				}
			}
                        // DOES NOTHING AT THE MOMENT
			else if (!unit.hasMoved && unit.getPlayer () == turn) {
				List<HexScript> mapRow = Map.map [(int)unit.getPosition ().x];
				HexScript hex = mapRow [(int)unit.getPosition ().y];
			}
		}
	}

	// Call at the end of a turn to update the game.
	public void endTurn ()
	{
		if (!paused) {
			foreach (UnitScript unit in units) {
				unit.updateTurn ();
				List<HexScript> mapRow = Map.map [(int)unit.getPosition ().x];
				HexScript hex = mapRow [(int)unit.getPosition ().y];
				hex.makeDefault ();
			}

			// switch turns
			turn = (turn) % 2 + 1;

			getPlayer().getDeck().deal();

			Player1.setCurrency(0);
			Player2.setCurrency(0);


			drawCards();
			updateVisibleHexes();
			foreach (List<HexScript> hexList in Map.map) {
				foreach (HexScript tempHex in hexList) {
					tempHex.checkForFog();
					//tempHex.refreshFog ();
				}
			}
			updateHexes();

			TurnIndicator.updateTurn(turn);
		}
	}

	// Moves a unit to a hex
	public void moveCurrentUnit (HexScript hex)
	{
		// DONE: Limit range of a unit's movement
		// DONE: Zone of control
		// DONE: Only allow unit's to be moved on their turn
		// TODO: wacking

		if (!paused) {
			// Makes sure there is currently a focused unit
			if (focusedUnit != null) {
				// If the hex is in the set of moveable hexes, move to it
				if (hexSet.Contains (hex)) {
					Map.map[(int)focusedUnit.getPosition().x][(int)focusedUnit.getPosition().y].setOccupied(false);
					focusedUnit.move(hex);
					hex.setOccupied(true);
				}


				updateVisibleHexes();
				foreach (List<HexScript> hexList in Map.map) {
					foreach (HexScript tempHex in hexList) {
						tempHex.checkForFog (); 
						tempHex.refreshFog ();
					}
				}
				updateHexes();

				focusedUnit = null;
				focusedHex.setFocus (false);
			}
		}
	}

	/* Display Current player's hand. */
	void drawCards () {
		for (int idx = 0; idx < currentHand().getSize(); ++idx) {
			hand_display[idx].reset(idx, currentHand().getCards()[idx].type);
		}
	}


	// Finds the adjacent hexes to a hex and adds them to an iterable Set
	// The returns that set
	HashSet<HexScript> findAdj (HexScript hex)
	{
		if (!paused) {
			int x = (int)hex.getPosition ().x;
			int y = (int)hex.getPosition ().y;
			HashSet<HexScript> set = new HashSet<HexScript> ();
			// Note: The logic here is clunky and long, but it works and runs in O(1)! Hopefully
			//       it won't need any changes.
			if (y % 2 == 0) {
				if (y - 2 >= 0) {
					set.Add (Map.map [x] [y - 2]);
				}
				if (y - 1 >= 0) {
					set.Add (Map.map [x] [y - 1]);
				}
				if (y + 1 < Map.map [x].Count) {
					set.Add (Map.map [x] [y + 1]);
				}
				if (y + 2 < Map.map [x].Count) {
					set.Add (Map.map [x] [y + 2]);
				}
				if (x - 1 >= 0) {
					if (y + 1 < Map.map [x].Count) {
						set.Add (Map.map [x - 1] [y + 1]);
					}
					if (y - 1 >= 0) {
						set.Add (Map.map [x - 1] [y - 1]);
					}
				}
			}
			if (y % 2 == 1) {
				if (y - 2 >= 0) {
					set.Add (Map.map [x] [y - 2]);
				}
				if (y - 1 >= 0) {
					set.Add (Map.map [x] [y - 1]);
				}
				if (y + 1 < Map.map [x].Count) {
					set.Add (Map.map [x] [y + 1]);
				}
				if (y + 2 < Map.map [x].Count) {
					set.Add (Map.map [x] [y + 2]);
				}
				if (x + 1 < Map.map.Count) {
					if (y + 1 < Map.map [x].Count) {
						set.Add (Map.map [x + 1] [y + 1]);
					}
					if (y - 1 >= 0) {
						set.Add (Map.map [x + 1] [y - 1]);
					}
				}
			}

			// Remove any hexes that cannot be landed on (water/mountains)
			/*foreach (HexScript hexes in set) {
                  if(hexes.getOccupied())
                     set.Remove(hexes);
                }*/
			return set;
		} else
			return null;
	}

	// Recursively finds the hexes that a unit can move to given
	// a starting hex and a movement distance
	// TODO: Account for terrain
	// DONE: Account for Zone of Control
	// TODO: Write an implementation of the A* algorithm to find the best path
	void findMovement (int movement, HexScript location, bool moved)
	{
		if (!paused) {
			bool stopped = false;
			bool adjToEnemy = false;
			HashSet<HexScript> adjSet = findAdj (location);
			hexSet.Add (location);
			if (movement == 0) {
			} else {
				foreach (HexScript adjHex in adjSet) {
					foreach (UnitScript enemy in units) {
						if (enemy.getPlayer () != turn) {
							if (moved && enemy.getPosition () == adjHex.getPosition ()) {
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
							focusedUnit.terrainMap.TryGetValue (adjHex.getType (), out cost);
							if (movement - cost >= 0) {
								findMovement (movement - cost, adjHex, true);
							}
						}
					}
				} else if (!stopped) {
					foreach (HexScript adjHex in adjSet) {
						int cost;
						focusedUnit.terrainMap.TryGetValue (adjHex.getType (), out cost);
						if (movement - cost >= 0) {
							findMovement (movement - cost, adjHex, true);
						}
					}
				}
			}
		}
	}

	// Attacks another unit. This is currently extremely primitive.
	public void attack (UnitScript unit)
	{
		if (!paused) {
			if (focusedUnit != null && focusedUnit.getAttack() > 0 && !focusedUnit.hasAttacked) {
				bool inRange = false;
				List<HexScript> focMapRow = Map.map [(int)focusedUnit.getPosition ().x];
				HexScript focHex = focMapRow [(int)focusedUnit.getPosition ().y];

				List<HexScript> curMapRow = Map.map [(int)(unit.getPosition ().x)];
				HexScript curHex = curMapRow [(int)unit.getPosition ().y];


                HashSet<HexScript> tempRange = findAdj(curHex);
                HashSet<HexScript> totalRange = new HashSet<HexScript>();
                int range = focusedUnit.getRange();
                Debug.Log("attack " + range);
                if (range != 1)
                {
                    attackRange(range - 1, tempRange, totalRange);
                }
                else
                {
                    totalRange = findAdj(curHex);
                }
                foreach (HexScript hex in totalRange)
                {
                    if (hex == focHex)
                    {
                        inRange = true;
                    }
                }
            
				if (inRange) {
					Debug.Log (focusedUnit.getPosition () + " attacked " + unit.getPosition ());

					int h = unit.getHealth ();
					focusedUnit.attackEnemy ();
					// Reduces the attacked units health by the attacking units attack
					// dmg = 2 (P / A ) - 2 ( A / P ) + 3P - 2A + 38
					var dmg = 2 * ( (float)focusedUnit.getAttack() / unit.getDefense() ) - 2 * ( (float)unit.getDefense() / focusedUnit.getAttack() ) +
							  3 * focusedUnit.getAttack() - 2 * unit.getDefense() + 38;
					// damage randomness
					dmg *= ( 100 + UnityEngine.Random.Range(-5, 5) ) / 100.0f;
					unit.setHealth(h - (int)dmg);
					//unit.setHealth ((int)(unit.getHealth () - (focusedUnit.getAttack () * (1 - unit.getDefense () / 100))));
					focusedUnit.hasAttacked = true;
					focusedUnit.hasMoved = true;

					Debug.Log ("Attack: " + focusedUnit.getAttack () + 
						"\nDefense: " + unit.getDefense () + 
						"\nPrevious health: " + h + 
						"\nResulting health lost: " + (focusedUnit.getAttack () * (1 - unit.getDefense () / 100)));

					// If the unit is out of health, destroy it
					if (unit.getHealth () <= 0) {
						unit.destroyUnit ();
						units.Remove (unit);
						Destroy (unit);
					}

					focusedUnit.setFocus (false);
					focusedUnit = null;
				}
			} else {
				Debug.Log ("No unit currently focused...");
			}
			if (p1Base.getHealth () <= 0 || p2Base.getHealth () <= 0) {
				Invoke ("endGame", 2.0f);
			}
			updateHexes ();
		}
	}

    public void attackRange(int range, HashSet<HexScript> tempRange, HashSet<HexScript> totalRange)
    {
        if (range == 0)
        {
            Debug.Log("end attackrange method");
        }
        else
        {
            Debug.Log("attackRange " + range);
            HashSet<HexScript> prevHexes = new HashSet<HexScript>();
            foreach (HexScript hex in tempRange)
            {
                HashSet<HexScript> adjHexes = findAdj(hex);
                foreach (HexScript adj in adjHexes)
                {

                    if (!totalRange.Contains(adj))
                    {
                        totalRange.Add(adj);
                        prevHexes.Add(adj);
                    }
                }
            }
            range--;
            attackRange(range, prevHexes, totalRange);
        }
    }

    // Returns if the hex that was clicked is occupied by a unit
    public bool hexClicked (HexScript hex) {
		// TODO handle fog tiles
		if (!paused) {
			
			if (focusedUnit != null) {
				moveCurrentUnit(hex);

				return true;
			} else if ( focusedCard != null && adjacent_to_base(hex) && placeUnitWithCard(focusedCard, (int)hex.position.x, (int)hex.position.y) ) {
					// Attempt to place the unit of the focusedCard
					focusedCard = null;
					return true;
			}
			return false;
		} else
			return false;
	}

	/* Determines if the given hex is adjacent to current's player's base */
	private bool adjacent_to_base(HexScript hex) {

		if (!paused) {
			HexScript baseHex = Map.map[ (int)((getTurn() == 1) ? p1Base : p2Base).getPosition().x ][ (int)((getTurn() == 1) ? p1Base : p2Base).getPosition().y];
			// Determine if the hex is adjacent to the base
			for (int adj = 0; adj < 6; ++adj) {
				HexScript adjHex = Map.adjacentHexTo(baseHex, adj);
				// adjHex is the desired hex
				if (hex != null && MapManager.same_position(adjHex, hex)) { return true; }
			}
		}

		return false;
	}

	/* Places the Unit that is associated with the given card at the given x and y coordinates. */
	private bool placeUnitWithCard(CardScript card, int x, int y) {
		// Player must have enough currency to place the Unit
		if (card != null && getPlayer().getCurrency() >= card.cost) {
			int idx = currentHand().getCards().IndexOf(focusedCard);

			if (idx == -1) {
				Debug.Log("Invalid index!\n");
				return false;
			}

			// use the card
			hand_display[idx].reset(-1, CardScript.CardType.Empty);
			getPlayer().changeCurrency(-card.cost);
			// place the unit
			return placeUnit((int)card.type, x, y) != null;
		}

		return false;
	}

	/* Places the unit of the given type at the given coordinate pair (x, y) on the map */
	private UnitScript placeUnit(int type, int x, int y) {
		if (paused) {
			return null;
		}

		UnitScript unit = null;
		Object origin = null;
		// Determines which prefab to use base on the type value
		if (type == (int)UnitScript.Types.H_Infantry) {
			origin = PrefabManager.HumanInfantryPrefab;
		} else if (type == (int)UnitScript.Types.H_Exo) {
			origin = PrefabManager.HumanExoPrefab;
		} else if (type == (int)UnitScript.Types.H_Tank) {
			origin = PrefabManager.HumanTankPrefab;
		} else if (type == (int)UnitScript.Types.H_Artillery) {
			origin = PrefabManager.HumanArtilleryPrefab;
		} else if (type == (int)UnitScript.Types.H_Base) {
			origin = PrefabManager.HumanMobileBasePrefab;
		} else if (type == (int)UnitScript.Types.A_Infantry) {
			origin = PrefabManager.AlienInfantryPrefab;
		} else if (type == (int)UnitScript.Types.A_Elite) {
			origin = PrefabManager.AlienElitePrefab;
		} else if (type == (int)UnitScript.Types.A_Artillery) {
			origin = PrefabManager.AlienArtilleryPrefab;
		} else if (type == (int)UnitScript.Types.A_Tank) {
			origin = PrefabManager.AlienTankPrefab;
		} else if (type == (int)UnitScript.Types.A_Base) {
			origin = PrefabManager.AlienMobileBasePrefab;
		}
		// Create the Unit if its type is valid
		if (origin != null) {
			unit = ((GameObject)Instantiate(origin, new Vector3(4 - Mathf.Floor(MapManager.size / 2), -5 + Mathf.Floor(MapManager.size / 2), -0.5f), Quaternion.Euler(new Vector3()))).GetComponent<UnitScript>();
			unit.setType(type);
			unit.setPlayer(turn);
			unit.move(Map.map[x][y]);
			units.Add(unit);

			focusedUnit = unit;
			// update vision of new unit
			updateVisibleHexes();
			foreach (List<HexScript> hexList in Map.map) {
				foreach (HexScript tempHex in hexList) {
					tempHex.checkForFog (); 
					tempHex.refreshFog ();
				}
			}
			updateHexes();

			focusedUnit = null;
		}

		return unit;
	}

	/* Evaluates the card at the given index in the hand being clicked.
	 * Sets focusedCard if the cards is not a currenct card.
	 * Returns true if the card was currency, false otherwise. */
	public static bool cardClicked(int idx) {
		if (paused) { // cards are unresponsive when the game is paused
			return false;
		}

		CardScript c = currentHand().getCards()[idx];

		focusedUnit = null;
		updateHexes();
		// Determine if the card is currency and if so add to the player's currency and return true
		if (c.type == CardScript.CardType.Currency1) {
			getPlayer().changeCurrency(1);
			return true;
		} else if (c.type == CardScript.CardType.Currency2) {
			getPlayer().changeCurrency(5);
			return true;
		} else if (c.type == CardScript.CardType.Currency3) {
			getPlayer().changeCurrency(10);
			return true;
		} else { // card is not currency
			focusedCard = c;
		}

		return false;
	}

	// Set's a unit to be the current unit in focus
	public void selectFocus (UnitScript unit)
	{
		if (!paused) {

			focusedUnit = unit;
			updateHexes ();
			if (unit != null && !unit.hasMoved) {
				List<HexScript> mapRow = Map.map [(int)unit.getPosition ().x];
				HexScript curHex = mapRow [(int)unit.getPosition ().y];
				// Reinitialize the hexSet to an empty set
				hexSet = new HashSet<HexScript> ();
				// Populate the hexSet with moveable hexes
				findMovement (focusedUnit.getMovement (), (Map.map [(int)focusedUnit.getPosition ().x]) [(int)focusedUnit.getPosition ().y], false);
				// For each moveable hex, indicate that it is moveable
				foreach (HexScript moveable in hexSet) {
					moveable.setFocus (true);
				}
				focusedHex = curHex;
				curHex.setFocus (true);
			}
		}
	}

	public UnitScript getHoveredUnit() {
		foreach (UnitScript unit in units) {
			if (unit.mouseOver) {
				return unit;
			}
		}
		return null;
	}

	public void buyCard(CardScript.CardType type) {
		CardScript temp = new CardScript(type);
		int cost = temp.cost;
		if (turn == 1) {
			if (cost <= Player1.getCurrency()) {
				Player1.changeCurrency(-cost);
				getPlayer().getDeck().discardPile.add (new CardScript(type));
			}
		}

		if (turn == 2) {
			if (cost <= Player2.getCurrency()) {
				Player2.changeCurrency(-cost);
				getPlayer().getDeck().discardPile.add(new CardScript(type));
			}
		}
	}

	public void endGame() {			
		Debug.Log ("Game Over!");
		SceneManager.LoadScene("start_menu");
	}

	public void togglePauseMenu ()
	{
		if (UI.GetComponentInChildren<Canvas> ().enabled) {
			UI.GetComponentInChildren<Canvas> ().enabled = false;
			musicSlider.SetActive (false);
			Time.timeScale = 1;
		} else {
			UI.GetComponentInChildren<Canvas> ().enabled = true;
			musicSlider.SetActive (true);
			Time.timeScale = 0;
		}
		if (shopCanvas.enabled || UI.GetComponentInChildren<Canvas> ().enabled) {
			paused = true;
		} else {
			paused = false;
		}
	}

	public void toggleShop() {
		if (shopCanvas.enabled) {
			shopCanvas.enabled = false;
			Time.timeScale = 1;
		} else {
			shopCanvas.enabled = true;
			Time.timeScale = 0;
		}
		if (shopCanvas.enabled || UI.GetComponentInChildren<Canvas> ().enabled) {
			paused = true;
		} else {
			paused = false;
		}
	}

	public HashSet<HexScript> getVisibleHexes() {
		return visibleHexes;
	}

	public void updateVisibleHexes() {
		Debug.Log ("test");

		UnitScript origFocusedUnit = focusedUnit;

		visibleHexes = new HashSet<HexScript> ();

		foreach (UnitScript unit in units) {
			if (unit.player == turn) {
				focusedUnit = unit;
				List<HexScript> mapRow = Map.map [(int)unit.getPosition ().x];
				HexScript curHex = mapRow [(int)unit.getPosition ().y];
				List<HexScript> visible = Map.findArea (curHex, unit.getMovement());
				// For each moveable hex, indicate that it is moveable
				foreach (HexScript visibleHex in visible) {
					visibleHexes.Add(visibleHex);
				}
			}
		}
		focusedUnit = origFocusedUnit;
	}


}
