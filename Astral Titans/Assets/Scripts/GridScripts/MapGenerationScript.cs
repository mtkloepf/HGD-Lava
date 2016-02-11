using UnityEngine;
using System;
using System.Collections.Generic;

/**
 * Proceedurally generates maps based on a given size and type
 */
public static class MapGeneration {
	/* Determines if the game map will be created pseudo-randomly */
	public static bool generate = true;
	/* Used to set the Hex sprites: NEEDS to be set in GameManager!!! */
	public static SpriteManagerScript sprites;
	/* Copy of the map */
	public static List<List<HexScript>> map;
	/* the dimensions of the map to create (within the bounds of [5, 45] */
	public static int width { get; set; }
	public static int height { get; set; }

	/* List of valid map types */
	private static readonly string[] MAP_TYPES = new string[] { "inland", "highland", "coastal", "ring", "maze" };
	// the type of map to generate (i.e. )
	private static string map_type;

	/* Initialize the width and height of the map */
	static MapGeneration() {
		width = 8;
		height = 32;
	}

	// TODO: base map generation off of size and map type

	public static void generatePseudoRandomMap() {
		int pos_x, pos_y, counter;
		List<HexScript> area;

		/* build a (pontentially) giant desert */
		counter = 1;
		while (--counter >= 0) {
			pos_x = UnityEngine.Random.Range(0, width - 1);
			pos_y = UnityEngine.Random.Range (0, height - 1);

			area = findArea(map[pos_x][pos_y], 16, 0.4f);

			foreach (HexScript h in area) {
				setHexType(h, 1);
			}
		}

		/* build small lakes */
		counter = 5;
		while (--counter > 0) {
			pos_x = UnityEngine.Random.Range(0, width - 1);
			pos_y = UnityEngine.Random.Range (0, height - 1);

			area = findArea(map[pos_x][pos_y], 3, 0.55f);

			foreach (HexScript h in area) {
				setHexType(h, 2);
			}
		}

		/* build mountain ranges */
		counter = 7;
		while (--counter >= 0) {
			pos_x = UnityEngine.Random.Range(0, width - 1);
			pos_y = UnityEngine.Random.Range (0, height - 1);

			area = findArea(map[pos_x][pos_y], 2, 0.45f);

			foreach (HexScript h in area) {
				setHexType(h, 3);
			}
		}
	}

	/**
	 * Returns all hexes that surround the given hex, in the map, at a given radius
	 * Passing a non-positive value for the radius will simply yeild a singleton
	 * containing the given hex.
	 */
	public static List<HexScript> findArea(HexScript center, int radius, float variability) {
		List<HexScript> to_fill = new List<HexScript>();
		to_fill.Add(center);

		// find remaining layers
		if (radius > 0) {
			List<HexScript> initial = new List<HexScript>();
			initial.Add(center);
			addNextLayer(initial, to_fill, radius, variability);
		}

		return to_fill;
	}

	/**
	 * Finds all hexes that are adjacent to any hex in prev_layer at a given radius from the hexes in prev_layer, that
	 * do not exist in total, with no duplicates. Ideally, prev_layer is a subset of total. Passing a non-positive
	 * radius will yield no change. Variability should be between 0.0 and 1.0 inclusive. Contrary to logic, the greater
	 * the variability the greater the chance of a uniform shape being formed from the hexes in total.
	 */
	private static void addNextLayer(List<HexScript> prev_layer, List<HexScript> total, int radius, float variability) {
		// based case
		if (radius <= 0) { return; }

		List<HexScript> cur_layer = new List<HexScript>();
		// Find all hexes, which have not already been found, adjacent to any hex in the previous layer
		foreach (HexScript hex in prev_layer) {
			// find all adjacent hexes
			for (int pos = 0; pos < 6; ++pos) {
				HexScript adj_hex = adjacentHexTo(hex, pos);

				if (UnityEngine.Random.value <= variability && adj_hex != null && !total.Contains(adj_hex)) {
					cur_layer.Add(adj_hex);
					total.Add(adj_hex); // add to total as well
				}
			}
		}
		// find the next layer
		addNextLayer(cur_layer, total, radius - 1, variability);
	}

	/**
	 * Returns the adjacent hex to the given hex, in the given map, at the given offset value.
	 * The offset represents the distance, in a clockwise fashion around the given hex, from
	 * the adjacent top hex.
	 * So, passing 0 for offset will return the hex above the given hex; 1 will return the hex
	 * to the top-right of the given hex, and so on.
	 */
	private static HexScript adjacentHexTo(HexScript hex, int offset) {
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

	private static void setHexType(HexScript hex, int type) {
		// Generate a plains
		if (type == 0) {
			hex.setType (HexScript.HexEnum.plains,
				sprites.plainsSprite, 
				sprites.redPlainsSprite,
				sprites.bluePlainsSprite);
		}
		// Generate a desert
		else if (type == 1) {
			hex.setType (HexScript.HexEnum.desert,
				sprites.desertSprite, 
				sprites.redDesertSprite,
				sprites.blueDesertSprite);
		}
		// Generate water
		else if (type == 2) {
			hex.setType (HexScript.HexEnum.water,
				sprites.waterSprite, 
				sprites.redWaterSprite,
				sprites.blueWaterSprite);
		}
		// Generate a mountain
		else if (type == 3) {
			hex.setType (HexScript.HexEnum.mountain,
				sprites.mountainSprite, 
				sprites.redMountainSprite,
				sprites.blueMountainSprite);
		}
	}

	public static void setGeneration() { generate = true; }

	public static void setMap(List<List<HexScript>> m) { map = m; }

	/* Determines if the given type is a valid map type */
	public static bool containsType(string type) {
		foreach (string t in MAP_TYPES) {
			if (t == type) { return true; }
		}

		return false;
	}

	/* Provides a comma'd list of all map types */
	public static string allTypes() {
		string types = "";

		foreach (string t in MAP_TYPES) {
			types +=  t + ", ";
		}

		return types.TrimEnd( new char[] { ' ' , ',', ' ' } );
	}
}

