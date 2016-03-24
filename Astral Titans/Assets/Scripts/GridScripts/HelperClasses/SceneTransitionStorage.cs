using UnityEngine;
using System.Collections;

/**
 * Used to transition the valeus for the map between the map prompt and
 * the map grid scene.
 * 
 * @author Joshua Hooker
 * 29 Feburary 2016
 */
public static class SceneTransitionStorage {

	public static int map_width;
	public static int map_height;
	/* List of valid map types */
	private static readonly string[] MAP_TYPES = new string[] { "default", "desert", "mountain","" };
	public static string map_type;
	public static bool fog;

	static SceneTransitionStorage() {
		map_width = 8;
		map_height = 32;
		map_type = "";
		fog = false;
	}

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

