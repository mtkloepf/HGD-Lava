﻿using UnityEngine;
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
		size = 4;
	}

	/* Creates a map of the given dimensions and type */
	public MapManager(int w, int h, string m_type) {
		width = w;
		height = h;
		map_type = m_type;
	}

	// TODO: base map generation off of size and map type

	public void generatePseudoRandomMap() {
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
				h.setType(1);
			}
		}

		/* build lakes */
		counter = average / 10 + 1;
		while (--counter >= 0) {
			pos_x = UnityEngine.Random.Range(0, width - 1);
			pos_y = UnityEngine.Random.Range (0, height - 1);

			area = findArea(map[pos_x][pos_y], average / 15 + 3, new Probability(0.5f, 0.04f));

			foreach (HexScript h in area) {
				h.setType(2);
			}
		}

		/* build mountain ranges */
		counter = 2 * average / 11;
		while (--counter >= 0) {
			pos_x = UnityEngine.Random.Range(0, width - 1);
			pos_y = UnityEngine.Random.Range (0, height - 1);

			area = findArea(map[pos_x][pos_y], average / 20 + 3, new Probability(0.55f, 0.06f));

			foreach (HexScript h in area) {
				h.setType(3);
			}
		}
	}

	private void desertification() {
		int pos_x, pos_y, counter;
		List<HexScript> area;
		int average = (2 * width + height / 2) / 2;

		/* build  a pontentially large desert */
		counter = 1;
		while (--counter >= 0) {
			pos_x = width / 2;
			pos_y = height / 2;

			area = findArea(map[pos_x][pos_y], 2 * System.Math.Max(2 * width, height / 2) / 3, new Probability(0.35f, -0.02f));

			foreach (HexScript h in area) {
				h.setType(1);
			}
		}
	}

	/* DON'T try this map: it is not polished!! */
	private void centralMountainMap() {
		int pos_x, pos_y, counter;
		List<HexScript> area;

		/* build 1 ~ 3 pontentially large deserts */
		counter = (int)( (2.0f * width + height / 2.0f) / 30.0f + 0.5f );
		while (--counter >= 0) {
			pos_x = UnityEngine.Random.Range(0, width - 1);
			pos_y = UnityEngine.Random.Range (0, height - 1);

			area = findArea(map[pos_x][pos_y], System.Math.Max(2 * width, height / 2) / 3, null);

			foreach (HexScript h in area) {
				h.setType(1);
			}
		}

		/* build a few medium lakes */
		counter = 3;
		while (--counter >= 0) {
			pos_x = UnityEngine.Random.Range(0, width - 1);
			pos_y = UnityEngine.Random.Range (0, height - 1);

			area = findArea(map[pos_x][pos_y], 2, null);

			foreach (HexScript h in area) {
				h.setType(2);
			}
		}

		/* build the central mountain */
		counter = 1;
		while (--counter >= 0) {
			pos_x = width / 2;
			pos_y = height / 2;
			Debug.Log("(" + pos_x + " , " + pos_y + ") " + (2 * height / 10) + "\n");
			area = findArea(map[pos_x][pos_y], (2 * height / 10), null);

			foreach (HexScript h in area) {
				// leave outside margins passable by non-infantry units
				if (h.position.x > 0 && h.position.x < width - 1 && h.position.y > 2 && h.position.y < (height - 2)) {
					h.setType(3);
				}
			}
		}
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
		// avoid cutting off the generation of the area, because of an empty layer
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

	/* Return the x and y coordiantes of the center of the map */
	private Point center() {
		return new Point(width / 2, height / 2);
	}

	/* Return the point (0,0) */
	private Point topLeft() { return new Point (0, 0); }

	/* Return the bottom-right most hex in the map */
	private Point bottomRight() { return new Point(width - 1, height - 1); }

	/**
	 * A simple class to hold a coordinate pair (x,y)
	 */
	private class Point {
		public int x;
		public int y;

		public Point(int x, int y) {
			this.x = x;
			this.y = y;
		}

		public override string ToString() {
			return string.Format("[Point]");
		}
	}

	/**
	 * A simple class designed to a float value between 0.0 and 1.0 that can be reduced by a fix value down to 0.0.
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