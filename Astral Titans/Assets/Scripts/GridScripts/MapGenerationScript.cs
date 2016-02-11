using UnityEngine;
using System;
using System.Collections.Generic;

/**
 * Proceedurally generates maps based on a given size and type
 */
public static class MapGeneration {
	/* Determines if the game map will be created pseudo-randomly */
	public static bool generate = false;
	/* Used to set the Hex sprites: NEEDS to be set in GameManager!!! */
	public static SpriteManagerScript sprites;
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

	public static void generatePseudoRandomMap(List<List<HexScript>> map) {
		// TODO: actually build the map . . .

		/* build a small lake */
		int pos_x = UnityEngine.Random.Range(0, width - 1);
		int pos_y = UnityEngine.Random.Range (0, height - 1);
		Debug.Log("(" + pos_x + " , " + pos_y + ")\n");

		List<HexScript> lake = new List<HexScript>();
		lake.Add(map[pos_x][pos_y]);

		for (int pos = 0; pos < 6; ++pos) {
			HexScript hex = adjacentHexTo(map, map[pos_x][pos_y], pos);

			if (hex != null) { lake.Add (hex); }
		}

		foreach (HexScript h in lake) {
			setHexType(h, 2);
		}
	}

	/**
	 * Returns the adjacent hex to the given hex, in the given map, at the given offset value.
	 * The offset represents the distance, in a clockwise fashion around the given hex, from
	 * the adjacent top hex.
	 * So, passing 0 for offset will return the hex above the given hex; 1 will return the hex
	 * to the top-right of the given hex, and so on.
	 */
	private static HexScript adjacentHexTo(List<List<HexScript>> map, HexScript hex, int offset) {
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

