using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Path is used to store a list of hexes that form a path from one 
 * hex to another. This will be used to find the optimal route from 
 * one hex to another.
 **/
public class Path : MonoBehaviour
{
	List<HexScript> path = new List<HexScript>();
	int distance = 0;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	// Gets the total distance of the path
	public int getDistance() {
		return distance;
	}

	// Gets the list of hexes that form the path
	public List<HexScript> getPath() {
		return path;
	}

	// Adds a new hex to the path
	public void addHex(HexScript hex, int cost) {
		path [path.Count] = hex;
//		path.Add (hex, path.Count);
		distance += cost;
	}

	// Uses the position of the final element of the path to create
	// a vector2 which will be used to store the possible paths in a 
	// custom hashmap
	public Vector2 getHash() {
		HexScript last = path [path.Count - 1];
		return last.getPosition ();
	}

}

