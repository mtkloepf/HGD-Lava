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
}

