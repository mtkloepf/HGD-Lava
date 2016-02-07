using UnityEngine;
using System;
using System.Collections.Generic;

/**
 * Proceedurally generates maps based on a given size and type
 */
public static class MapGeneration {
	/* Determines if the game map will be created pseudo-randomly */
	public static bool generate = false;
	/* the dimensions of the map to create (within the bounds of [5, 45] */
	public static int width { get; set; }
	public static int height { get; set; }

	/* List of valid map types */
	private static readonly string[] MAP_TYPES = new string[] { "inland", "highland", "coastal", "ring", "maze" };
	// the type of map to generate (i.e. )
	private static string map_type;

	static MapGeneration() {
		width = -1;
		height = 1;
	}

	public static List<List<HexScript>> generateMap() {
		Debug.Log("width: " + width + "\nheight: " + height + "\n");
		// TODO: actually build the map . . .
		return null;
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

