using UnityEngine;
using System.Collections;

/**
 * A class designed to cycle through different instances of the current
 * map specs (i.e. hieght, width, and type).
 * 
 * @author Joshua Hooker
 * 29 February 2016
 */
public class CycleMap : MonoBehaviour {

	public void OnMouseDown() {
		MapGeneration.generatePseudoRandomMap();
	}
}

