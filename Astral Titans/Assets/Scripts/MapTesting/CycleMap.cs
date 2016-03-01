﻿using UnityEngine;
using System.Collections;

/**
 * A class designed to cycle through different instances of the current
 * map specs (i.e. hieght, width, and type).
 * 
 * @author Joshua Hooker
 * 29 February 2016
 */
public class CycleMap : MonoBehaviour {
	/* Values avaiable to be modify during runtime that will change Map that this script will generate */
	public int map_width;
	public int map_height;
	public string map_type;
	/* The Map generated by this script */
	private MapManager Map;

	public void Start() {
		map_width = SceneTransitionStorage.map_width;
		map_height = SceneTransitionStorage.map_height;
		map_type = SceneTransitionStorage.map_type;
		createMap();
	}

	public void OnMouseDown() {
		createMap();
	}

	/* Creates a map based on the current width, height, and type values */
	private void createMap() {
		// If type is not valid, then the map is not created
		if ( SceneTransitionStorage.containsType(map_type) ) {
			if ( Map != null ) {
				Map.removeHexes();
			}

			Map = new MapManager(map_width, map_height, map_type);
			Map.generatePseudoRandomMap();
		}
	}
}

