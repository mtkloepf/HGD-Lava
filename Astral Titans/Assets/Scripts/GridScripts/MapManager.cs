using UnityEngine;
using System;
using System.Collections.Generic;

/**
 * Holds the Map and any values related to the map. Also deals with
 * psuedo-random map generation baesd on map size and map type.
 * 
 * @author Joshua Hooker
 * 29 February 2016
 */
public class MapManager {
	// Determines if the map is fog of war
	public readonly bool FOG_OF_WAR;

	/* Copy of the map */
	public List<List<HexScript>> map;
	/* the dimensions of the map to create (within the bounds of [12, 46] */
	public readonly int width;
	public readonly int height;
	public static readonly int size;

	// the type of map to generate (i.e. )
	private readonly string map_type;

	/* Initialize the size of the map */
	static MapManager() {
		UnityEngine.Random.seed = (int)Time.unscaledDeltaTime;
		size = 4;
	}

	/* Creates a map of the given dimensions and type */
	public MapManager(int w, int h, string m_type, bool fog) {
		FOG_OF_WAR = fog;
		width = w;
		height = h;
		map_type = m_type;
	}

	// TODO: base map generation off of size and map type

	public void generatePseudoRandomMap() {
		// Sets the random number seed.
		UnityEngine.Random.seed = Time.frameCount;

		map = new List<List<HexScript>>();

		// generate all hexes initially
		for (int i = 0; i < width; i++) {
			List <HexScript> row = new List<HexScript>();

			for (int j = 0; j < height; j++) {
				// default to plains
				row.Add(HexScript.createHex(i, j, size, 0));
			}

			map.Add(row);
		}

		if (map_type == "mountain") {
			centralMountainMap();
		} else if (map_type == "desert") {
			desertification();
		} else {
			defaultMap();
		}
	}

	private void defaultMap() {
		int pos_x, pos_y, counter;
		List<HexScript> area;
		int average = (2 * width + height / 2) / 2;

		/* build pontentially large deserts */
		counter = 9 * average / 100;
		while (--counter >= 0) {
			pos_x = UnityEngine.Random.Range(0, width - 1);
			pos_y = UnityEngine.Random.Range(0, height - 1);

			area = findArea(map[pos_x][pos_y], System.Math.Max(2 * width, height / 2) / 3, new Probability(0.45f, 0.02f));

			foreach (HexScript h in area) {
				h.setType((int)HexScript.HexEnum.desert);
			}
		}

		/* build lakes */
		counter = average / 10 + 1;
		while (--counter >= 0) {
			pos_x = UnityEngine.Random.Range(0, width - 1);
			pos_y = UnityEngine.Random.Range (0, height - 1);

			area = findArea(map[pos_x][pos_y], average / 15 + 3, new Probability(0.5f, 0.04f));

			foreach (HexScript h in area) {
				h.setType((int)HexScript.HexEnum.water);
			}
		}

		/* build mountain ranges */
		counter = 2 * average / 11;
		while (--counter >= 0) {
			pos_x = UnityEngine.Random.Range(0, width - 1);
			pos_y = UnityEngine.Random.Range (0, height - 1);

			area = findArea(map[pos_x][pos_y], average / 20 + 3, new Probability(0.55f, 0.06f));

			foreach (HexScript h in area) {
				h.setType((int)HexScript.HexEnum.mountain);
			}
		}
	}

	private void desertification() {
		int pos_x, pos_y, counter;
		List<HexScript> area;
		int average = (2 * width + height / 2) / 2;

		/* Fill map with desert */
		for (int row = 0; row < width; ++row) {
			for (int col = 0; col < height; ++col) {
				map[row][col].setType((int)HexScript.HexEnum.desert);
			}
		}

		/* build mountain ranges */
		counter = (int)(1.5f + 0.149f * average);

		while (--counter >= 0) {
			pos_x = UnityEngine.Random.Range(0, width - 1);
			pos_y = UnityEngine.Random.Range (0, height - 1);

			area = findArea(map[pos_x][pos_y], (int)(0.15f * average + 1.5f), new Probability(0.55f, 0.15f));

			foreach (HexScript h in area) {
				h.setType((int)HexScript.HexEnum.mountain);
			}
		}

		/* Build oases */
		counter = average / 10 + UnityEngine.Random.Range(0, 3);

		while (--counter >= 0) {
			pos_x = UnityEngine.Random.Range(0, width - 1);
			pos_y = UnityEngine.Random.Range (0, height - 1);

			area = findArea(map[pos_x][pos_y], (int)(0.16f * average + 0.35f), new Probability(0.72f, 0.07f));
			// fill grass area
			foreach (HexScript h in area) {
				h.setType((int)HexScript.HexEnum.plains);
			}

			area = findArea(map[pos_x][pos_y], (int)(0.08f * average + 1.1f), new Probability(0.4f, 0.1f));
			// fill resevoir
			foreach (HexScript h in area) {
				h.setType((int)HexScript.HexEnum.water);
			}
		}
	}

	/* DON'T try this map: it is not polished!! */
	private void centralMountainMap() {
		int pos_x, pos_y, counter;
		List<HexScript> area;
		int average = (2 * width + height / 2) / 2;

		/* build 1 ~ 3 pontentially large deserts */
		counter = (int)( 0.068f * average + 0.5f );
		while (--counter >= 0) {
			pos_x = UnityEngine.Random.Range(0, width - 1);
			pos_y = UnityEngine.Random.Range (0, height - 1);

			area = findArea(map[pos_x][pos_y], (int)(0.4f * average), new Probability(0.78f, 0.13f));

			foreach (HexScript h in area) {
				h.setType((int)HexScript.HexEnum.desert);
			}
		}

		/* build a few medium lakes */
		counter = (int)(0.085f * average + 0.825f);
		while (--counter >= 0) {
			pos_x = UnityEngine.Random.Range(0, width - 1);
			pos_y = UnityEngine.Random.Range (0, height - 1);


			area = findArea(map[pos_x][pos_y], System.Math.Min(2 * width, height / 2) / 5, new Probability(0.8f, 0.2f));

			foreach (HexScript h in area) {
				h.setType((int)HexScript.HexEnum.water);
			}
		}

		/* build the central mountain */
		counter = 1;
		while (--counter >= 0) {
			pos_x = width / 2;
			pos_y = height / 2;
			// Debug.Log("(" + pos_x + " , " + pos_y + ") " + (2 * height / 10) + "\n");
			float percent = (1.2f * average + 40f) / 100f;
			int max = System.Math.Max(width * 2, height / 2);
			area = findArea(map[pos_x][pos_y], (int)(0.015f * max * max +  0.15f * max), new Probability(percent, 0.08f));

			foreach (HexScript h in area) {
				// leave outside margins passable by non-infantry units
				if ( ((h.position.y % 2 == 1 && h.position.x > 0) || h.position.x > 1) &&
					 ((h.position.y % 2 == 0 && h.position.x < width - 1) || h.position.x < width - 2) &&
					   h.position.y > 3 && h.position.y < (height - 4)) {

					   h.setType((int)HexScript.HexEnum.mountain);
				}
			}
		}
	}

	/* Finds a hex at a given offset from a point relative to the given hex in the map.
	 * 
	 * If opp_x is set to true, then the point's x position will be equal to the complement
	 * of the given hex's x position relative to the map's width bounds: likewise for opp_y,
	 * the point's y position, and the map's height bounds.
	 * 
	 * If the given hex is null, then null is returned. */
	public HexScript hex_at_offset_from(HexScript hex, bool opp_x, bool opp_y, int offset) {
		if (hex == null) { return null; }

		var pos_x = (int)hex.position.x;
		var pos_y = (int)hex.position.y;
		// flip x position
		if (opp_x) {
			pos_x = width - pos_x - 1;
		}
		// flip y position
		if (opp_y) {
			pos_y = height - pos_y - 1;
		}

		List<HexScript> area = findArea(map[pos_x][pos_y], offset, null);
		// Find a random hex at a radius of the given offset away from the given hex
		int hex_idx = UnityEngine.Random.Range(0, area.Count - 1);

		return area[hex_idx];
	}

	/* Given an integer value along with a lower and upper bound (lower <= upper),
	 * if the value rises above the upper bound or falls below the lower bound,
	 * then the violated bound is returned, otherwise the value is returned.*/
	public static int cap_at_bounds(int value, int l_bound, int u_bound) {
		if (value < l_bound) {
			return l_bound;
		} else if (value > u_bound) {
			return u_bound;
		} else {
			return value;
		}
	}

	/* This method calculates the movement range of the given unit based on the unit's maximum movement and the movement costs
	 * of terrian for the give unit. It also sets the focus value of all hexes in the unit's movement to the value of set_focus.
	 * A HashSet of all hexes that the unit can move to is returned. */
	public HashSet<HexScript> unit_move_range(UnitScript unit, bool set_focus) {
		HashSet<HexScript> reachable_hexes = new HashSet<HexScript>();

		if (unit != null) {
			Queue<HexScript> potential_hexes = new Queue<HexScript>();
			SortedDictionary<HexScript, int> cur_values = new SortedDictionary<HexScript, int>(); 
			// Initialize queue and dictionary entries
			potential_hexes.Enqueue( hex_of(unit) );
			cur_values.Add( hex_of(unit), unit.getMovement() );
			// Only search hexes that are new, or that have a new path, from a different hex, with a greater movement value
			while (potential_hexes.Count > 0) {
				HexScript hex = potential_hexes.Dequeue();

				int cur_val = -1;
				// Find hex's current cost
				cur_values.TryGetValue(hex, out cur_val);

				// Possibility to move to adjacent hexes
				if (cur_val > 0) {
					// Sift through all valid adjacent hexes
					for (int adj_idx = 0; adj_idx < 6; ++adj_idx) {
						HexScript adj_hex = adjacentHexTo(hex, adj_idx);

						if (adj_hex != null) {
							// Find a movement value for moving through this hex via a different path, if one exists
							int old_val = -1;
							bool exists = cur_values.TryGetValue(adj_hex, out old_val);
							// Calculate the new excess movement value after moving through the adjacent hex
							int new_val = cur_val - UnitScript.move_cost(unit.unitType(), adj_hex.getType());

							if (new_val >= 0) {
								// check if the current hex is occupied by an enemy unit
								if (adj_hex.getOccupied() > 0 && adj_hex.getOccupied() != unit.getPlayer()) {
									new_val = 0;
								} else {
									// check if the current adj_hex is adjacent to a hex occupied by an enemy unit
									for (int idx = 0; idx < 6; ++idx) {
										HexScript adj_hex_2 = adjacentHexTo(adj_hex, idx);
										// reduce the new movement value to 0, when the hex is adjacent to an enemy
										if (adj_hex_2 != null && adj_hex_2.getOccupied() > 0 && adj_hex_2.getOccupied() != unit.getPlayer()) {
											new_val = 0;
											break;
										}
									}
								}

								/* Only add hexes for which the cost of moving through subtracted from the current movement value yields a non-negative result.
								 * Also, the space cannot already be occupied. */
								if (!exists && (adj_hex.getOccupied() == 0 || adj_hex.getOccupied() == unit.getPlayer())) {
									//Debug.Log(adj_hex.position + " : " + new_val + " ");
									// Set the hex focus value of hexes within the units movement range and add them to the list of reachable hexes
									if (adj_hex.getOccupied() == 0) {
										reachable_hexes.Add(adj_hex);
										adj_hex.setFocus(set_focus);
									}

									potential_hexes.Enqueue(adj_hex);
									cur_values.Add(adj_hex, new_val);
								} else if (old_val >= 0 && new_val > old_val) {
									//Debug.Log(adj_hex.position + " : " + old_val + " -> " + new_val + " ");
									potential_hexes.Enqueue(adj_hex);
									cur_values [adj_hex] = new_val;
								}
							}
						}

					}

					//Debug.Log("\n");
				}
			}
		}

		return reachable_hexes;
	}

	/* Given a unit and a fog flag, this method flips all hexes within the unit's field
	 * of vision to the given flag value. The unit's vison range is based off of its
	 * movement value and the vision costs associated with a hex; both of which can be
	 * found in the move_cost() and vision_cost() methods of the UnitScript class. */
	public void update_field_of_view(UnitScript unit, bool fog) {
		
		if (unit != null) {
			Queue<HexScript> hexes = new Queue<HexScript>();
			SortedDictionary<HexScript, int> cur_values = new SortedDictionary<HexScript, int>(); 
			// Initialize queue and dictionary entries
			hexes.Enqueue( hex_of(unit) );
			cur_values.Add( hex_of(unit), unit.getMovement() );
			// Only search hexes that are new, or that have a new path, from a different hex, with a greater vision value
			while (hexes.Count > 0) {
				HexScript hex = hexes.Dequeue();
				hex.set_fog_cover(fog);

				int cur_val = 0;
				// Find hex's current cost
				cur_values.TryGetValue(hex, out cur_val);

				// Possibility to see adjacent hexes
				if (cur_val > 0) {
					// Sift through all valid adjacent hexes
					for (int adj_idx = 0; adj_idx < 6; ++adj_idx) {
						HexScript adj_hex = adjacentHexTo(hex, adj_idx);

						if (adj_hex != null) {
							// Find old vision value for the given hex (if it exists)
							int old_val = 0;
							bool exists = cur_values.TryGetValue( adj_hex, out old_val);
							// Calculate new vision after going through the adjacent hex
							int new_val = cur_val - UnitScript.vision_cost(unit.unitType(), adj_hex.getType());
							// Either add the hex if does not exists yet, or if the new value is less than the original
							if ( !exists && new_val >= 0 ) {
								//Debug.Log(adj_hex.position + " : " + new_val + " ");
								hexes.Enqueue(adj_hex);
								cur_values.Add(adj_hex, new_val);
							} else if (old_val >= 0 && new_val > old_val) {
								//Debug.Log(adj_hex.position + " : " + old_val + " -> " + new_val + " ");
								hexes.Enqueue(adj_hex);
								cur_values[adj_hex] = new_val;
							}
						}

					}

					//Debug.Log("\n");
				}
			}
		}
	}

	/* Finds all hexes within the given radius, centered at the center hex. */
	public List<HexScript> findArea(HexScript center, int radius) {
		return findArea(center, radius, null);
	}

	/**
	 * Returns all hexes that surround the given hex, in the map, at a given radius
	 * Passing a non-positive value for the radius will simply yeild a singleton
	 * containing the given hex.
	 */
	private List<HexScript> findArea(HexScript center, int radius, Probability variation) {
		List<HexScript> to_fill = new List<HexScript>();
		to_fill.Add(center);

		// find remaining layers
		if (radius > 0) {
			List<HexScript> initial = new List<HexScript>();
			initial.Add(center);
			addNextLayer(initial, to_fill, radius, variation);
		}

		return to_fill;
	}

	/**
	 * Finds all hexes that are adjacent to any hex in prev_layer at a given radius from the hexes in prev_layer, that
	 * do not exist in total, with no duplicates. Ideally, prev_layer is a subset of total. Passing a non-positive
	 * radius will yield no change. Variation should be a float between 0.0 and 1.0 inclusive coupled with a static decay
	 * value that will reduce the probability on susequent layer calls. Contrary to logic, the greater the variability
	 * the greater the chance of a uniform shape being formed from the hexes in total.
	 */
	private void addNextLayer(List<HexScript> prev_layer, List<HexScript> total, int radius, Probability variation) {
		// based case
		if (radius <= 0) { return; }

		List<HexScript> cur_layer = new List<HexScript>();
		// Find hexes, which have not already been found, adjacent to any hex in the previous layer
		foreach (HexScript hex in prev_layer) {
			// find all adjacent hexes
			for (int pos = 0; pos < 6; ++pos) {
				HexScript adj_hex = adjacentHexTo(hex, pos);

				if ( (variation == null || UnityEngine.Random.value <= variation.getProbability()) && adj_hex != null && !total.Contains(adj_hex)) {
					cur_layer.Add(adj_hex);
					total.Add(adj_hex); // add to total as well
				}
			}
		}
		// avoid cutting off the generation of the area because of an empty layer
		if (cur_layer.Count == 0) { cur_layer = prev_layer; }

		if (variation != null) { variation.reduce(); }
		// find the next layer
		addNextLayer(cur_layer, total, radius - 1, variation);
	}

	/**
	 * Returns the adjacent hex to the given hex, in the given map, at the given offset value.
	 * The offset represents the distance, in a clockwise fashion around the given hex, from
	 * the adjacent top hex.
	 * So, passing 0 for offset will return the hex above the given hex; 1 will return the hex
	 * to the top-right of the given hex, and so on.
	 */
	public HexScript adjacentHexTo(HexScript hex, int offset) {
		try {
			int pos_x = (int)hex.position.x;
			int pos_y = (int)hex.position.y;

			switch(offset) {
				// top
				case 0: return map[pos_x][pos_y - 2];
				// top-right
				case 1: return (pos_y % 2 == 0) ? map[pos_x][pos_y - 1] : map[pos_x + 1][pos_y - 1];
				// bottom-right
				case 2: return (pos_y % 2 == 0) ? map[pos_x][pos_y + 1] : map[pos_x + 1][pos_y + 1];
				// bottom
				case 3: return map[pos_x][pos_y + 2];
				// bottom-left
				case 4: return (pos_y % 2 == 0) ? map[pos_x - 1][pos_y + 1] : map[pos_x][pos_y + 1];
				// top-left
				case 5: return (pos_y % 2 == 0) ? map[pos_x - 1][pos_y - 1] : map[pos_x][pos_y - 1];
				// invalid position
				default: return null;
			}
		} catch(Exception Ex) {
			// Either the hex give is invalid (or null) or the hex is on the map's boundary
			if (Ex is NullReferenceException || Ex is IndexOutOfRangeException || Ex is ArgumentOutOfRangeException) {
				return null;
			}

			throw;
		}
	}

	/* Returns the hex. on which the given unit is located. */
	public HexScript hex_of(UnitScript unit) {
		return map[(int)unit.position.x][(int)unit.position.y];
	}

	/* Verifies that the two hexes are at the same position in the map */
	public static bool same_position(HexScript h1, HexScript h2) {
			return h1 != null && h2 != null && (h1.position.x == h2.position.x) && (h1.position.y == h2.position.y);
	}

	/* Enable or disable fog of war. */
	public void fog_of_war(bool fog) {

		foreach(List<HexScript> hex_list in map) {
			foreach(HexScript hex in hex_list) {
				hex.startRenderer();
				hex.set_fog_cover(fog);
			}
		}
	}

	/* Removes all tiles from the map */
	public void removeHexes() {
		for (int x = 0; x < width; ++x) {
			for (int y = 0; y < height; ++y) {
				GameObject.Destroy( map[x][y].gameObject );
			}
		}

	}

	/**
	 * A simple class designed to a float value between 0.0 and 1.0 that
	 * can be modified by a percentage of its current value down.
	 */
	private class Probability {
		private float probability;
		private readonly float decay;

		public Probability(float p, float d) {
			probability = p;
			decay = d;
		}

		public void reduce() {
			probability *= 1.0f - decay;
		}

		public float getProbability() { return probability; }

		public float getDecay() { return decay; }
	}
}
