using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManagerScript : MonoBehaviour
{

	public static GameManagerScript instance;

	public GameObject TilePrefab;
	public GameObject HumanInfantryPrefab;
	public GameObject HumanTankPrefab;
	public GameObject AlienInfantryPrefab;
	public GameObject AlienTankPrefab;
	public GameObject EndTurn;
	public GameObject AIPlayerPrefab;
	public GameObject CardPrefab;

	// Mobile base prefabs
	public GameObject HumanMobileBasePrefab;
	public GameObject AlienMobileBasePrefab;

	// Human Exo
	public GameObject HumanExoPrefab;

	public PlayerScript Player1;
	public PlayerScript Player2;
	public TurnIndicatorScript TurnIndicator;
	public GameObject UI;
	public SpriteManagerScript SpriteManager;
	public int mapSize = 11;
	public int mapWidth = 10;
	public int mapHeight = 40;
	public int turn;
	private GameObject musicSlider;
	private bool paused = false;
	private float timer;

	private float cardStartX = 0;
	private float cardStartY = 0;
	public float cardVelX;
	public float cardVelY;


	private UnitScript p1Base;
	private UnitScript p2Base;
	public int p1Startx;
	public int p1Starty;
	public int p2Startx;
	public int p2Starty;

	// List of all the hexes
	List <List<HexScript>> map = new List<List<HexScript>> ();
	// List of all the units in the game
	List <UnitScript> units = new List<UnitScript> ();
	// List of the cards in a hand
	List<CardScript> hand = new List<CardScript> ();

	// Deck managers for players 1 and 2
	DeckManager deck1 = new DeckManager ();
	DeckManager deck2 = new DeckManager ();

	// Set containing all hexes that a unit can move to
	HashSet<HexScript> hexSet = new HashSet<HexScript> ();

	// Clicking on a unit will make it focused
	UnitScript focusedUnit;
	HexScript focusedHex;
	CardScript focusedCard;
	
	void Awake ()
	{
		instance = this;
	}
	
	// Use this for initialization
	void Start ()
	{		
		musicSlider = GameObject.Find ("Slider");
		UI.GetComponentInChildren<Canvas> ().enabled = false;
		//generateMap();
		generateRandomMap ();
		generateUnits ();
		generateDecks ();
		deck1.deal ();
//		generateCards();
		turn = 1;
		Player1 = new PlayerScript ();
		Player2 = new PlayerScript ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		timer = Time.deltaTime;

		foreach (CardScript card in hand) {
			if (card == null) {
				continue;
			}
			if (Input.GetKey ("w")) {
				Vector3 position = card.transform.position;
				position.y += cardVelY;
				card.transform.position = position;
			}
			if (Input.GetKey ("a")) {
				Vector3 position = card.transform.position;
				position.x -= cardVelX;
				card.transform.position = position;
			}
			if (Input.GetKey ("s")) {
				Vector3 position = card.transform.position;
				position.y -= cardVelY;
				card.transform.position = position;
			}
			if (Input.GetKey ("d")) {
				Vector3 position = card.transform.position;
				position.x += cardVelX;
				card.transform.position = position;
			}
		}
		
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

	}

	public UnitScript getFocusedUnit ()
	{
		return focusedUnit;
	}

	public int getDeckCount ()
	{
		if (turn == 1) {
			return deck1.deck.getSize ();
		}
		if (turn == 2) {
			return deck2.deck.getSize ();
		}
		return 0;
	}

	public int getDiscardCount ()
	{
		if (turn == 1) {
			return deck1.discardPile.getSize ();
		}
		if (turn == 2) {
			return deck2.discardPile.getSize ();
		}
		return 0;
	}

	// Updates the colors of the hexes
	void updateHexes ()
	{
		foreach (List<HexScript> hexlist in map) {
			foreach (HexScript hex in hexlist) {
				hex.setFocus (false);
			}
		}
		foreach (UnitScript unit in units) {
			if (unit.hasMoved && unit.getPlayer () == turn) {
				// make hex red
				List<HexScript> mapRow = map [(int)unit.getPosition ().x];
				HexScript hex = mapRow [(int)unit.getPosition ().y];
				hex.makeRed ();
			}
                        // DOES NOTHING AT THE MOMENT
			else if (!unit.hasMoved && unit.getPlayer () == turn) {
				List<HexScript> mapRow = map [(int)unit.getPosition ().x];
				HexScript hex = mapRow [(int)unit.getPosition ().y];
			}
		}
	}

	// Gets the turn
	public int getTurn ()
	{
		return turn;
	}

	// Call at the end of a turn to update the game.
	public void endTurn ()
	{
		if (!paused) {
			foreach (UnitScript unit in units) {
				unit.updateTurn ();
				List<HexScript> mapRow = map [(int)unit.getPosition ().x];
				HexScript hex = mapRow [(int)unit.getPosition ().y];
				hex.makeDefault ();
			}
			foreach (CardScript card in hand) {
				if (card != null) {
					card.destroyCard ();
				}
//				Destroy (card);
			}

			if (turn == 1) {
				turn = 2;
				deck2.deal ();
			} else {
				turn = 1;
				deck1.deal ();
			}
			Player1.setCurrency (0);
			Player2.setCurrency (0);
			updateHexes ();
			drawCards ();
			TurnIndicator.updateTurn (turn);


		}
	}

	// Moves a unit to a hex
	public void moveCurrentUnit (HexScript hex)
	{
		// DONE: Limit range of a unit's movement
		// DONE: Zone of control
		// DONE: Only allow unit's to be moved on their turn
		// TODO: Attacking

		if (!paused) {
			// Makes sure there is currently a focused unit
			if (focusedUnit != null) {
				// If the hex is in the set of moveable hexes, move to it
				if (hexSet.Contains (hex)) {
					focusedUnit.move (hex);
				}
				focusedUnit = null;
				focusedHex.setFocus (false);
			}
			updateHexes ();
		}
	}

	// Create all of the hexes in the map
	// TODO: Create an implementation of this method that takes in a 2D array and generates a custom
	//       map with different terrain.
	void generateMap ()
	{
		map = new List<List<HexScript>> ();
		for (int i = 0; i < mapWidth; i++) {
			List <HexScript> row = new List<HexScript> ();
			for (int j = 0; j < mapHeight; j++) {
				HexScript hex;
				// If else statements to create an offset, because we are using hexes.
				// This needs polishing.
				if (j % 2 == 1) {
					hex = ((GameObject)Instantiate (TilePrefab, 
                                            new Vector3 (i * 0.9f + 0.45f - Mathf.Floor (mapSize / 2), 
                                               -(j + 0f) / 4f + Mathf.Floor (mapSize / 2), 1), 
                                            Quaternion.Euler (new Vector3 ()))).GetComponent<HexScript> ();
				} else {
					hex = ((GameObject)Instantiate (TilePrefab, 
                                            new Vector3 (i * 0.9f - Mathf.Floor (mapSize / 2), 
                                               -j / 4f + Mathf.Floor (mapSize / 2), 1), 
                                            Quaternion.Euler (new Vector3 ()))).GetComponent<HexScript> ();
				}
				hex.setPosition (new Vector2 ((float)i, (float)j));
				hex.startRenderer ();
				hex.setType (HexScript.HexEnum.plains,
                                SpriteManager.plainsSprite, 
                                SpriteManager.redPlainsSprite,
                                SpriteManager.bluePlainsSprite);
				row.Add (hex);
			}
			map.Add (row);
		}
	}

	// Randomly generate a map
	void generateRandomMap ()
	{
		map = new List<List<HexScript>> ();
		for (int i = 0; i < mapWidth; i++) {
			List<HexScript> row = new List<HexScript> ();
			for (int j = 0; j < mapHeight; j++) {
				HexScript hex;
				if (j % 2 == 1) {
					hex = ((GameObject)Instantiate (TilePrefab, 
                             new Vector3 (i * 0.9f + 0.45f - Mathf.Floor (mapSize / 2), 
                                -(j + 0f) / 4f + Mathf.Floor (mapSize / 2), 1), 
                             Quaternion.Euler (new Vector3 ()))).GetComponent<HexScript> ();
				} else {
					hex = ((GameObject)Instantiate (TilePrefab, 
                             new Vector3 (i * 0.9f - Mathf.Floor (mapSize / 2), 
                                -j / 4f + Mathf.Floor (mapSize / 2), 1), 
                             Quaternion.Euler (new Vector3 ()))).GetComponent<HexScript> ();
				}
				hex.setPosition (new Vector2 ((float)i, (float)j));
				hex.startRenderer ();

				//Randomization of hexes to add
				int tileNumber = Random.Range (0, 4);

				// Generate a plains
				if (tileNumber == 0) {
					hex.setType (HexScript.HexEnum.plains,
                          SpriteManager.plainsSprite, 
                          SpriteManager.redPlainsSprite,
                          SpriteManager.bluePlainsSprite);
				}
                 // Generate a desert
                 else if (tileNumber == 1) {
					hex.setType (HexScript.HexEnum.desert,
                          SpriteManager.desertSprite, 
                          SpriteManager.redDesertSprite,
                          SpriteManager.blueDesertSprite);
				}
                 // Generate water
                 else if (tileNumber == 2) {
					hex.setType (HexScript.HexEnum.water,
                          SpriteManager.waterSprite, 
                          SpriteManager.redWaterSprite,
                          SpriteManager.blueWaterSprite);
				}
                 // Generate a mountain
                 else if (tileNumber == 3) {
					hex.setType (HexScript.HexEnum.mountain,
                          SpriteManager.mountainSprite, 
                          SpriteManager.redMountainSprite,
                          SpriteManager.blueMountainSprite);
				}
				row.Add (hex);
			}
			map.Add (row);
		}
	}

	// Creates a unit for testing purposes. Additional units can be added if desired.
	// TODO: Create a method to purchase a unit and place it at a desired location. This
	//       will be a seperate method from this one. Eventually, we will not need this method anymore
	void generateUnits ()
	{
		UnitScript unit;

		p1Base = placeUnit (map [p1Startx] [p1Starty], UnitScript.Types.MobileBaseH);
		p1Base.setPlayer (1);
		p2Base = placeUnit (map [p2Startx] [p2Starty], UnitScript.Types.MobileBaseA);
		p2Base.setPlayer (2);
	}

	// Creates the starting decks.
	void generateDecks ()
	{
		CardCollection deck = new CardCollection ();
		for (int i = 0; i < 7; i ++) {
			deck.add (new CardScript ().init (CardScript.CardType.Currency1));
		}
		for (int i = 0; i < 3; i++) {
			deck.add (new CardScript ().init (CardScript.CardType.Currency2));
		}
		for (int i = 0; i < 2; i ++) {
			deck.add (new CardScript ().init (CardScript.CardType.HumanInfantry));
		}
		for (int i = 0; i < 1; i++) {
			deck.add (new CardScript ().init (CardScript.CardType.HumanTank));
		}

		//TODO:
		//Used to spawn exo and artillery (no cards implemented yet)
		/*for (int i = 0; i < 1; i++) {
			deck.add (new CardScript ().init (CardScript.CardType.HumanExo));
		}
		for (int i = 0; i < 1; i++) {
			deck.add (new CardScript ().init (CardScript.CardType.HumanArtillery));
		}*/


		deck.shuffle ();
		deck1.init (new CardCollection (), deck, new CardCollection ());
		deck = new CardCollection ();
		for (int i = 0; i < 7; i ++) {
			deck.add (new CardScript ().init (CardScript.CardType.Currency1));
		}
		for (int i = 0; i < 3; i++) {
			deck.add (new CardScript ().init (CardScript.CardType.Currency2));
		}
		for (int i = 0; i < 2; i ++) {
			deck.add (new CardScript ().init (CardScript.CardType.AlienInfantry));
		}
		for (int i = 0; i < 1; i++) {
			deck.add (new CardScript ().init (CardScript.CardType.AlienTank));
		}
		deck.shuffle ();
		deck2.init (new CardCollection (), deck, new CardCollection ());
	}

	void drawCards ()
	{
		CardScript card;
		float i = 0f;
		if (turn == 1) {
			foreach (CardScript playerCard in deck1.hand.getCards ()) {
				card = ((GameObject)Instantiate (CardPrefab, new Vector3 (-1.7f + i / 1.66f + cardStartX, -1.45f + cardStartY, 0), Quaternion.Euler (new Vector3 ()))).GetComponent<CardScript> ();
				card.startRenderer ();
				card.setType (playerCard.getType ());
				hand.Add (card);
				i ++;
			}
		} else {
			foreach (CardScript playerCard in deck2.hand.getCards ()) {
				card = ((GameObject)Instantiate (CardPrefab, new Vector3 (-1.7f + i / 1.66f + cardStartX, -1.45f + cardStartY, 0), Quaternion.Euler (new Vector3 ()))).GetComponent<CardScript> ();
				card.startRenderer ();
				card.setType (playerCard.getType ());
				hand.Add (card);
				i ++;
			}
		}
	}

	void generateCards ()
	{
//		CardScript card;
//
//		card = ((GameObject)Instantiate (CardPrefab, new Vector3 (0, -1.5f, 0), Quaternion.Euler (new Vector3 ()))).GetComponent<CardScript> ();
//
//		hand.Add (card);
//
//		card = ((GameObject)Instantiate (CardPrefab, new Vector3 (1f, -1.5f, 0), Quaternion.Euler (new Vector3 ()))).GetComponent<CardScript> ();
//		
//		hand.Add (card);
//
//		card = ((GameObject)Instantiate (CardPrefab, new Vector3 (-1f, -1.5f, 0), Quaternion.Euler (new Vector3 ()))).GetComponent<CardScript> ();
//		
//		hand.Add (card);
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
					set.Add (map [x] [y - 2]);
				}
				if (y - 1 >= 0) {
					set.Add (map [x] [y - 1]);
				}
				if (y + 1 < map [x].Count) {
					set.Add (map [x] [y + 1]);
				}
				if (y + 2 < map [x].Count) {
					set.Add (map [x] [y + 2]);
				}
				if (x - 1 >= 0) {
					if (y + 1 < map [x].Count) {
						set.Add (map [x - 1] [y + 1]);
					}
					if (y - 1 >= 0) {
						set.Add (map [x - 1] [y - 1]);
					}
				}
			}
			if (y % 2 == 1) {
				if (y - 2 >= 0) {
					set.Add (map [x] [y - 2]);
				}
				if (y - 1 >= 0) {
					set.Add (map [x] [y - 1]);
				}
				if (y + 1 < map [x].Count) {
					set.Add (map [x] [y + 1]);
				}
				if (y + 2 < map [x].Count) {
					set.Add (map [x] [y + 2]);
				}
				if (x + 1 < map.Count) {
					if (y + 1 < map [x].Count) {
						set.Add (map [x + 1] [y + 1]);
					}
					if (y - 1 >= 0) {
						set.Add (map [x + 1] [y - 1]);
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
	// TODO: Animations
	// TODO: Decide on a combat system and implement it
	// TODO: Limit units to attacking once per turn
	public void attack (UnitScript unit)
	{
		if (!paused) {
			if (focusedUnit != null && focusedUnit.canAttack && !focusedUnit.hasAttacked) {
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
					focusedUnit.attackEnemy ();
					// Reduces the attacked units health by the attacking units attack
					unit.setHealth ((int)(unit.getHealth () - 
						(focusedUnit.getAttack () * (1 - unit.getDefense () / 100))));
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

	// Returns if the hex that was clicked is occupied by a unit
	public bool hexClicked (HexScript hex)
	{
		if (!paused) {
			if (focusedUnit != null) {
				moveCurrentUnit (hex);
				return true;
			} else if (focusedCard != null) {

				placeUnitAdjacentToBase (hex);
				return true;
			}
			return false;
		} else
			return false;
	}

	// Used to place the player bases.
	public UnitScript placeUnit (HexScript hex, UnitScript.Types type)
	{
		if (!paused) {
			int x = (int)(hex.getPosition ().x);
			int y = (int)(hex.getPosition ().y);
			
			UnitScript unit = null;
			
			switch (type) {
			case UnitScript.Types.InfantryA:
				unit = ((GameObject)Instantiate (AlienInfantryPrefab, new Vector3 (4 - Mathf.Floor (mapSize / 2), -5 + Mathf.Floor (mapSize / 2), -1), Quaternion.Euler (new Vector3 ()))).GetComponent<UnitScript> ();
				unit.setType (UnitScript.Types.InfantryA);
				unit.setPlayer (turn);
				unit.move (map [x] [y]);
				units.Add (unit);
				break;
			case UnitScript.Types.HumanExo:
				unit = ((GameObject)Instantiate (HumanExoPrefab, new Vector3 (4 - Mathf.Floor (mapSize / 2), -5 + Mathf.Floor (mapSize / 2), -1), Quaternion.Euler (new Vector3 ()))).GetComponent<UnitScript> ();
				unit.setType (UnitScript.Types.HumanExo);
				unit.setPlayer (turn);
				unit.move (map [x] [y]);
				units.Add (unit);
				break;
			case UnitScript.Types.HeavyTankA:
				unit = ((GameObject)Instantiate (AlienTankPrefab, new Vector3 (4 - Mathf.Floor (mapSize / 2), -5 + Mathf.Floor (mapSize / 2), -1), Quaternion.Euler (new Vector3 ()))).GetComponent<UnitScript> ();
				unit.setType (UnitScript.Types.HeavyTankA);
				unit.setPlayer (turn);
				unit.move (map [x] [y]);
				units.Add (unit);
				break;
			case UnitScript.Types.InfantryH:
				unit = ((GameObject)Instantiate (HumanInfantryPrefab, new Vector3 (4 - Mathf.Floor (mapSize / 2), -5 + Mathf.Floor (mapSize / 2), -1), Quaternion.Euler (new Vector3 ()))).GetComponent<UnitScript> ();
				unit.setType (UnitScript.Types.InfantryH);
				unit.setPlayer (turn);
				unit.move (map [x] [y]);
				units.Add (unit);
				break;
			case UnitScript.Types.HeavyTankH:
				unit = ((GameObject)Instantiate (HumanTankPrefab, new Vector3 (4 - Mathf.Floor (mapSize / 2), -5 + Mathf.Floor (mapSize / 2), -1), Quaternion.Euler (new Vector3 ()))).GetComponent<UnitScript> ();
				unit.setType (UnitScript.Types.HeavyTankH);
				unit.setPlayer (turn);
				unit.move (map [x] [y]);
				units.Add (unit);
				break;
			case UnitScript.Types.MobileBaseH:
				unit = ((GameObject)Instantiate (HumanMobileBasePrefab, new Vector3 (4 - Mathf.Floor (mapSize / 2), -5 + Mathf.Floor (mapSize / 2), -1), Quaternion.Euler (new Vector3 ()))).GetComponent<UnitScript> ();
				unit.setType (UnitScript.Types.MobileBaseH);
				unit.setPlayer (turn);
				unit.move (map [x] [y]);
				units.Add (unit);
				break;
			case UnitScript.Types.MobileBaseA:
				unit = ((GameObject)Instantiate (AlienMobileBasePrefab, new Vector3 (4 - Mathf.Floor (mapSize / 2), -5 + Mathf.Floor (mapSize / 2), -1), Quaternion.Euler (new Vector3 ()))).GetComponent<UnitScript> ();
				unit.setType (UnitScript.Types.MobileBaseA);
				unit.setPlayer (turn);
				unit.move (map [x] [y]);
				units.Add (unit);
				break;

			default :
				break;
			}
			return unit;
		}
		return null;
	}

	public void placeUnit (HexScript hex)
	{
		if (!paused) {
			bool created = false;
			int x = (int)(hex.getPosition ().x);
			int y = (int)(hex.getPosition ().y);

			UnitScript unit;

			switch (focusedCard.getType ()) {
			case CardScript.CardType.AlienInfantry:
				if (focusedCard.cost <= Player1.getCurrency ()) {
					unit = ((GameObject)Instantiate (AlienInfantryPrefab, new Vector3 (4 - Mathf.Floor (mapSize / 2), -5 + Mathf.Floor (mapSize / 2), -1), Quaternion.Euler (new Vector3 ()))).GetComponent<UnitScript> ();
					unit.setPlayer (turn);
					unit.setType (UnitScript.Types.InfantryA);
					unit.move (map [x] [y]);
					units.Add (unit);
					created = true;
					Player1.subtractCurrency (focusedCard.cost);
				}
				break;
			case CardScript.CardType.AlienTank:
				if (focusedCard.cost <= Player1.getCurrency ()) {
					unit = ((GameObject)Instantiate (AlienTankPrefab, new Vector3 (4 - Mathf.Floor (mapSize / 2), -5 + Mathf.Floor (mapSize / 2), -1), Quaternion.Euler (new Vector3 ()))).GetComponent<UnitScript> ();
					unit.setPlayer (turn);
					unit.setType (UnitScript.Types.HeavyTankA);
					unit.move (map [x] [y]);
					units.Add (unit);
					created = true;
					Player1.subtractCurrency (focusedCard.cost);
				}
				break;
			case CardScript.CardType.HumanInfantry:
				if (focusedCard.cost <= Player1.getCurrency ()) {
					unit = ((GameObject)Instantiate (HumanInfantryPrefab, new Vector3 (4 - Mathf.Floor (mapSize / 2), -5 + Mathf.Floor (mapSize / 2), -1), Quaternion.Euler (new Vector3 ()))).GetComponent<UnitScript> ();
					unit.setType (UnitScript.Types.InfantryH);
					unit.setPlayer (turn);
					unit.move (map [x] [y]);
					units.Add (unit);
					created = true;
					Player1.subtractCurrency (focusedCard.cost);
				}
				break;
			case CardScript.CardType.HumanTank:
				if (focusedCard.cost <= Player1.getCurrency ()) {
					unit = ((GameObject)Instantiate (HumanTankPrefab, new Vector3 (4 - Mathf.Floor (mapSize / 2), -5 + Mathf.Floor (mapSize / 2), -1), Quaternion.Euler (new Vector3 ()))).GetComponent<UnitScript> ();
					unit.setType (UnitScript.Types.HeavyTankH);
					unit.setPlayer (turn);
					unit.move (map [x] [y]);
					units.Add (unit);
					created = true;
					Player1.subtractCurrency (focusedCard.cost);
				}
				break;
			default :
				Debug.Log ("Unknown card type");
				break;
			}
//			unit.setPlayer (turn);
//			unit.move (map [x] [y]);
//			units.Add (unit);
//			hand.Remove (focusedCard);
			if (created) {
				focusedCard.destroyCard ();
				Destroy (focusedCard);
			}
			focusedCard = null;
		}
	}

	public void placeUnitAdjacentToBase (HexScript hex)
	{
		if (!paused) {

			bool adj = false;
//			List<HexScript> focMapRow = map [(int)focusedUnit.getPosition ().x];
//			HexScript focHex = focMapRow [(int)focusedUnit.getPosition ().y];
			if (getTurn () == 1) {
				List<HexScript> curMapRow = map [(int)(p1Base.getPosition ().x)];
				HexScript curHex = curMapRow [(int)p1Base.getPosition ().y];
				
				
				HashSet<HexScript> adjHexes = findAdj (curHex);
				foreach (HexScript focHex in adjHexes) {
					if (hex == focHex) {
						adj = true;
					}
				}
			} else if (getTurn () == 2) {
				List<HexScript> curMapRow = map [(int)(p2Base.getPosition ().x)];
				HexScript curHex = curMapRow [(int)p2Base.getPosition ().y];
				
				
				HashSet<HexScript> adjHexes = findAdj (curHex);
				foreach (HexScript focHex in adjHexes) {
					if (hex == focHex) {
						adj = true;
					}
				}
			}

			if (adj) {
				placeUnit (hex);
			}
		}
	}

	public void selectCard (CardScript card)
	{
		if (!paused) {
			focusedUnit = null;
			updateHexes ();
			focusedCard = card;
			switch (focusedCard.getType ()) {
			case CardScript.CardType.Currency1:
				if (turn == 1) 
					Player1.addCurrency (1);
				else 
					Player2.addCurrency (1);
				focusedCard.destroyCard ();
				Destroy (focusedCard);
				focusedCard = null;
				break;
			case CardScript.CardType.Currency2:
				if (turn == 1) 
					Player1.addCurrency (3);
				else 
					Player2.addCurrency (3);
				focusedCard.destroyCard ();
				Destroy (focusedCard);
				focusedCard = null;
				break;
			default:
				break;
			}
		}
	}

	// Set's a unit to be the current unit in focus
	public void selectFocus (UnitScript unit)
	{
		if (!paused) {
			focusedUnit = unit;
			if (!unit.hasMoved) {
				updateHexes ();
				List<HexScript> mapRow = map [(int)unit.getPosition ().x];
				HexScript curHex = mapRow [(int)unit.getPosition ().y];

				// Reinitialize the hexSet to an empty set
				hexSet = new HashSet<HexScript> ();
				// Populate the hexSet with moveable hexes
				findMovement (focusedUnit.getMovement (), (map [(int)focusedUnit.getPosition ().x]) [(int)focusedUnit.getPosition ().y], false);
				// For each moveable hex, indicate that it is moveable
				foreach (HexScript moveable in hexSet) {
					moveable.setFocus (true);
				}
				focusedHex = curHex;
				//Debug.Log ("Focused hex: " + curHex.getPosition ().ToString ());
				curHex.setFocus (true);
			}
			//Debug.Log ("unit selected");
		}
	}

	public void endGame ()
	{			
		Debug.Log ("Game Over!");
		Application.LoadLevel ("grid_scene");
	}

	public void togglePauseMenu ()
	{
		if (UI.GetComponentInChildren<Canvas> ().enabled) {
			UI.GetComponentInChildren<Canvas> ().enabled = false;
			paused = false;
			musicSlider.SetActive (false);
			Time.timeScale = 1;
		} else {
			UI.GetComponentInChildren<Canvas> ().enabled = true;
			musicSlider.SetActive (true);
			paused = true;
			Time.timeScale = 0;
		}
	}
}
