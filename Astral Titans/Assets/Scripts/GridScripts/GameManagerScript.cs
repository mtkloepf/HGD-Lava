﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManagerScript : MonoBehaviour {

	public static GameManagerScript instance;
	public MapManager Map;

	public GameObject Turn_Transition;
	public GameObject EndTurn;
	public GameObject AIPlayerPrefab;
	public AudioClip[] coin_sounds;

	// Shop Canvas UI Element
	public Canvas shopCanvas;

	public PlayerScript Player1;
	public PlayerScript Player2;
	private HandScript[] hand_display;

	public TurnIndicatorScript TurnIndicator;
	public GameObject UI;
	private int turn;
	private GameObject musicSlider;
	public bool paused = false;

	private float cardStartX = 0;
	private float cardStartY = 0;
	public float cardVelX;
	public float cardVelY;


	private UnitScript p1Base;
	private UnitScript p2Base;

	// List of all the units in the game
	private List <UnitScript> units;

	// Set containing all hexes that a unit can move to
	HashSet<HexScript> hexSet = new HashSet<HexScript>();

	// Clicking on a unit will make it focused
	private UnitScript focusedUnit;
	private HexScript focusedHex;
	private CardScript focusedCard;

	void Awake () {
		instance = this;
	}
	
	// Use this for initialization
	void Start () {
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

		Map = new MapManager(SceneTransitionStorage.map_width, SceneTransitionStorage.map_height, SceneTransitionStorage.map_type, SceneTransitionStorage.fog);

		// map setup
		Map.generatePseudoRandomMap();

		// place alien base
		turn = 2;
		HexScript hex = Map.hex_at_offset_from(Map.map[0][0], false, false, System.Math.Min(Map.width / 2, Map.height / 2));
		p2Base = placeUnit ( UnitScript.Types.A_Base, (int)hex.position.x, (int)hex.position.y );
		//int unit = 4;
		// place one of each unit
		/*for (int adj_idx = 0; adj_idx < 6; ++adj_idx) {
			HexScript adj_hex = Map.adjacentHexTo(hex, adj_idx);

			if (adj_hex != null && adj_hex.getOccupied() == 0) {
				placeUnit((UnitScript.Types)unit, (int)adj_hex.position.x, (int)adj_hex.position.y );
				++unit;
			}

			if (unit > 7) { break; }
		}*/

		turn = 1;

		if (Map.FOG_OF_WAR) { Map.fog_of_war(true); }

		// place human base
		hex = Map.hex_at_offset_from(Map.map[Map.width - 1][Map.height - 1], false, false, System.Math.Min(Map.width / 2, Map.height / 2));
		p1Base = placeUnit ( UnitScript.Types.H_Base, (int)hex.position.x, (int)hex.position.y );
		// place one of each unit
		/*unit = 0;
		for (int adj_idx = 0; adj_idx < 6; ++adj_idx) {
			HexScript adj_hex = Map.adjacentHexTo(hex, adj_idx);

			if (adj_hex != null && adj_hex.getOccupied() == 0) {
				placeUnit((UnitScript.Types)unit, (int)adj_hex.position.x, (int)adj_hex.position.y );
				++unit;
			}

			if (unit > 3) { break; }
		}*/
	}

	// Update is called once per frame
	void Update () {
		
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
				//Debug.Log( "Bronze: " + getPlayer().getDeck().discardPile.getCount(CardScript.CardType.Currency1) + "\n" );

				if (getPlayer().getCurrency() >= 3 && getPlayer().getDeck().discardPile.getCount(CardScript.CardType.Currency1) >= 5) {
					int removed = getPlayer().getDeck().removeCardsFromDiscard(CardScript.CardType.Currency1, 5);
					//Debug.Log("Removed: " + removed + "\n");
					getPlayer().getDeck().discardPile.add( new CardScript(CardScript.CardType.Currency2) );
					getPlayer().changeCurrency(-3);
				} 
			} else if (Input.GetKeyDown("3")) {
				//Debug.Log( "Silver: " + getPlayer().getDeck().discardPile.getCount(CardScript.CardType.Currency2) + "\n" );

				if (getPlayer().getCurrency() >= 7 && getPlayer().getDeck().discardPile.getCount(CardScript.CardType.Currency2) >= 2) {
					int removed = getPlayer().getDeck().removeCardsFromDiscard(CardScript.CardType.Currency2, 2);
					//Debug.Log("Removed: " + removed + "\n");
					getPlayer().getDeck().discardPile.add( new CardScript(CardScript.CardType.Currency3) );
					getPlayer ().changeCurrency(-7);
				}
			}
		}
	}

	// Returns currently seletec unit
	public UnitScript getFocusedUnit () {
		return focusedUnit;
	}

	// Returns currently selected card
	public CardScript getFocusedCard() {
		return focusedCard;
	}

	// Gets the turn
	public int getTurn () {
		return turn;
	}

	/* Returns the current player's deck size */
	public int getDeckCount () {
		return getPlayer().getDeck().deck.getSize();
	}

	/* Returns the current player's discard pile size */
	public int getDiscardCount () {
		return getPlayer().getDeck().discardPile.getSize();
	}

	/* Returns the current player. */
	public PlayerScript getPlayer() {
		return (turn == 1) ? Player1 : Player2;
	}

	/* Returns the hand of the current player. */
	public CardCollection currentHand() {
		return getPlayer().getDeck().hand;
	}

	// Updates the colors of the hexes
	public void updateHexes () {
		
		foreach (List<HexScript> hexlist in Map.map) {
			foreach (HexScript hex in hexlist) {
				hex.setOccupied(0);
				hex.setFocus(false);

				if (hex.getOccupied() == 0) {
					hex.makeDefault();
				}
			}
		}

		foreach (UnitScript unit in units) {
			HexScript hex = Map.hex_of(unit);
			hex.setOccupied(unit.getPlayer());

			if (unit.getPlayer() == getTurn()) {
				// Update hex colors
				if (unit.getState() == 2 || (unit.getAttack() == 0 && unit.getState() == 1)) {
					hex.makeRed();
				} else if (unit.getState() == 1) {
					hex.makePink();
				}
			}
		}
	}

	// Call at the end of a turn to update the game.
	public void endTurn() {
		if (!paused) {
			// switch turns
			turn = (turn) % 2 + 1;
			TurnIndicator.updateTurn(turn);

			// revert all hexes to fog
			if (Map.FOG_OF_WAR) {
				// Create intermediate screen
				Turn_Transition.GetComponent<ScreenImageToggle>().reset();
				Map.fog_of_war(true);
			}

			// reset all unit statuses
			foreach (UnitScript unit in units) {
				unit.updateTurn();
				HexScript hex = Map.hex_of(unit);
				hex.makeDefault();
				// restore vision to next player's units
				if (Map.FOG_OF_WAR && unit.getPlayer() == getTurn()) {
					Map.update_field_of_view(unit, false);
				}
			}

			updateHexes();

			// draw new hand
			getPlayer().getDeck().deal();
			drawCards();
			Debug.Log(getPlayer().getDeck().hand.getSize());
			// reset currencies for each player
			Player1.setCurrency(0);
			Player2.setCurrency(0);
		}
	}

	/* Moves the current focused unit to the given hex, if possible. */
	public void moveCurrentUnit (HexScript hex) {

		if (!paused) {
			// Makes sure there is currently a focused unit
			if (focusedUnit != null) {
				// If the hex is in the set of moveable hexes, move to it
				if (hexSet.Contains (hex)) {
					// cover original vision in fog
					HexScript prev_hex = Map.map [(int)focusedUnit.getPosition().x] [(int)focusedUnit.getPosition().y];
					prev_hex.setOccupied(0);
					//Map.update_fog_cover(prev_hex, focusedUnit.getMovement(), true);

					focusedUnit.move(hex);
					hex.setOccupied(focusedUnit.getPlayer());
					// reveal new vision area
					if (Map.FOG_OF_WAR) { Map.update_field_of_view(focusedUnit, false); }
				}

				focusedUnit = null;
				updateHexes();
			}
		}
	}

	/* Display Current player's hand. */
	void drawCards() {
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
							int cost = UnitScript.move_cost(focusedUnit.unitType(), adjHex.getType());
							//focusedUnit.terrainMap.TryGetValue (adjHex.getType (), out cost);
							if (movement - cost >= 0) {
								findMovement (movement - cost, adjHex, true);
							}
						}
					}
				} else if (!stopped) {
					foreach (HexScript adjHex in adjSet) {
						int cost = UnitScript.move_cost(focusedUnit.unitType(), adjHex.getType());
						//focusedUnit.terrainMap.TryGetValue (adjHex.getType (), out cost);
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
			if (focusedUnit != null && focusedUnit.getAttack() > 0 && focusedUnit.getState() < 2) {//!focusedUnit.hasAttacked) {
				bool inRange = false;
				List<HexScript> focMapRow = Map.map [(int)focusedUnit.getPosition ().x];
				HexScript focHex = focMapRow [(int)focusedUnit.getPosition ().y];

				List<HexScript> curMapRow = Map.map [(int)(unit.getPosition ().x)];
				HexScript curHex = curMapRow [(int)unit.getPosition ().y];


                HashSet<HexScript> tempRange = findAdj(curHex);
                HashSet<HexScript> totalRange = new HashSet<HexScript>();
                int range = focusedUnit.getRange();
                //Debug.Log("attack " + range);
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
					//Debug.Log (focusedUnit.getPosition () + " attacked " + unit.getPosition ());

					int h = unit.getHealth ();
					focusedUnit.attackEnemy ();
					// Reduces the attacked units health by the attacking units attack
					// dmg = 2 (P / A ) - 2 ( A / P ) + 3P - 2A + 38
					var dmg = 2 * ( (float)focusedUnit.getAttack() / unit.getDefense() ) - 2 * ( (float)unit.getDefense() / focusedUnit.getAttack() ) +
							  3 * focusedUnit.getAttack() - 2 * unit.getDefense() + 38;
					// damage randomness
					dmg *= ( 100 + UnityEngine.Random.Range(-8, 8) ) / 100.0f;
					unit.setHealth(h - (int)dmg);
					//unit.setHealth ((int)(unit.getHealth () - (focusedUnit.getAttack () * (1 - unit.getDefense () / 100))));

					focusedUnit.setState(2);
					/*Debug.Log ("Attack: " + focusedUnit.getAttack () + 
						"\nDefense: " + unit.getDefense () + 
						"\nPrevious health: " + h + 
						"\nResulting health lost: " + (focusedUnit.getAttack () * (1 - unit.getDefense () / 100)));*/

					// If the unit is out of health, destroy it
					if (unit.getHealth () <= 0) {
						unit.destroyUnit ();
						units.Remove (unit);
						Map.hex_of(unit).setOccupied(0);
						Destroy (unit);
					}

					focusedUnit = null;
				}
			} else {
				//Debug.Log ("No unit currently focused...");
			}
			if (p1Base.getHealth () <= 0 || p2Base.getHealth () <= 0) {
				Invoke ("endGame", 2.0f);
			}

			updateHexes();
		}
	}

    public void attackRange(int range, HashSet<HexScript> tempRange, HashSet<HexScript> totalRange)
    {
        if (range == 0)
        {
           // Debug.Log("end attackrange method");
        }
        else
        {
           // Debug.Log("attackRange " + range);
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

    /* Determines the action to take if a hex is clicked. */
    public void hexClicked (HexScript hex) {
		
		if (!paused) {
			
			if (focusedUnit != null) {
				moveCurrentUnit(hex);

			// A space must be adjacent to the current Player's base and not a water tile
			} else if ( focusedCard != null && hex.getType() != HexScript.HexEnum.water &&
						adjacent_to_base(hex) && placeUnitWithCard(focusedCard, (int)hex.position.x, (int)hex.position.y) ) {
					// Attempt to place the unit of the focusedCard
					focusedCard = null;
			}

		}
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
				Debug.Log("Invalid card index!\n");
			} else if (placeUnit((UnitScript.Types)((int)card.type), x, y) != null) {
				// play a sound when the card is used
				if (coin_sounds.Length > 0) {
					int sound = UnityEngine.Random.Range(0, coin_sounds.Length);
					GetComponent<AudioSource>().PlayOneShot(coin_sounds[sound]);
				}
				// use the card
				hand_display[idx].reset(-1, CardScript.CardType.Empty);
				getPlayer().changeCurrency(-card.cost);
				return true;
			}
		}

		return false;
	}

	/* Places the unit of the given type at the given coordinate pair (x, y) on the map
	 * NOTE: this method is used to place the initial bases! */
	public UnitScript placeUnit(UnitScript.Types type, int x, int y) {
		if (paused) {
			return null;
		}

		//Debug.Log((UnitScript.Types)type);

		UnitScript unit = null;
		Object origin = null;
		// Determines which prefab to use base on the type value
		if (type == UnitScript.Types.H_Infantry) {
			origin = PrefabManager.HumanInfantryPrefab;
		} else if (type == UnitScript.Types.H_Exo) {
			origin = PrefabManager.HumanExoPrefab;
		} else if (type == UnitScript.Types.H_Tank) {
			origin = PrefabManager.HumanTankPrefab;
		} else if (type == UnitScript.Types.H_Artillery) {
			origin = PrefabManager.HumanArtilleryPrefab;
		} else if (type == UnitScript.Types.H_Base) {
			origin = PrefabManager.HumanMobileBasePrefab;
		} else if (type == UnitScript.Types.A_Infantry) {
			origin = PrefabManager.AlienInfantryPrefab;
		} else if (type == UnitScript.Types.A_Elite) {
			origin = PrefabManager.AlienElitePrefab;
		} else if (type == UnitScript.Types.A_Artillery) {
			origin = PrefabManager.AlienArtilleryPrefab;
		} else if (type == UnitScript.Types.A_Tank) {
			origin = PrefabManager.AlienTankPrefab;
		} else if (type == UnitScript.Types.A_Base) {
			origin = PrefabManager.AlienMobileBasePrefab;
		}
		// Create the Unit if its type is valid
		if (origin != null) {
			// initialize unit stats
			unit = ((GameObject)Instantiate(origin, new Vector3(4 - Mathf.Floor(MapManager.size / 2), -5 + Mathf.Floor(MapManager.size / 2), -0.5f), Quaternion.Euler(new Vector3()))).GetComponent<UnitScript>();
			unit.setType((int)type);
			unit.setPlayer(turn);
			units.Add(unit);
			// initialize unit on the map
			unit.move(Map.map[x][y]);
			unit.updateTurn();
			Map.map[x][y].setOccupied(unit.getPlayer());

			if (Map.FOG_OF_WAR) {
				// update vision of new unit
				Map.update_field_of_view(unit, false);
			}
		}

		return unit;
	}

	/* Evaluates the card at the given index in the hand being clicked.
	 * Sets focusedCard if the cards is not a currenct card.
	 * Returns true if the card was currency, false otherwise. */
	public bool cardClicked(int idx) {
		if (paused) { // cards are unresponsive when the game is paused
			return false;
		}

		CardScript c = currentHand().getCards()[idx];

		if (focusedUnit != null) {
			Map.unit_move_range(focusedUnit, false);
			Map.hex_of(focusedUnit).setFocus(false);
			focusedUnit = null;
		}

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
			updateHexes();

			if (unit != null && unit.getState() < 1) {
				List<HexScript> mapRow = Map.map [(int)unit.getPosition ().x];
				HexScript curHex = mapRow [(int)unit.getPosition ().y];
				// Reinitialize the hexSet to an empty set
				hexSet = new HashSet<HexScript>();
				// Populate the hexSet with moveable hexes
				hexSet = Map.unit_move_range(focusedUnit, true);
				//findMovement (focusedUnit.getMovement (), (Map.map [(int)focusedUnit.getPosition ().x]) [(int)focusedUnit.getPosition ().y], false);
				// For each moveable hex, indicate that it is moveable
				/*foreach (HexScript moveable in hexSet) {
					moveable.setFocus (true);
				}*/
				focusedHex = curHex;
				curHex.setFocus(true);
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
		//Debug.Log ("Game Over!");
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
}
