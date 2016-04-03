using UnityEngine;
using System.Collections;

/**
 * This class is designed to manipulate the appearence of a screen that appears over the player's
 * field of view after each turn during fog of war games. This screen is designed to serve as a way
 * to ensure a certain level of secrecy between two player's, who are playing the game on the same
 * computer.
 * 
 * author : Joshua Hooker
 * 3 April 2016
 */
public class ScreenImageToggle : MonoBehaviour {

	/* Remove the black screen when it is clicked on */
	public void OnMouseDown() {
		GetComponent<SpriteRenderer>().enabled = false;
		gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
	}

	/* Restore the black screen */
	public void reset() {
			GetComponent<SpriteRenderer>().enabled = true;
			gameObject.layer = LayerMask.NameToLayer("Transition Screen");
	}
}
