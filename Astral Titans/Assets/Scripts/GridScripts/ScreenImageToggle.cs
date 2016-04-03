using UnityEngine;
using UnityEngine.UI;
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
		GetComponent<Image>().enabled = false;
		gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

		Text txt = GetComponentInChildren<Text>();
		if (txt != null) { txt.enabled = false; }
	}

	/* Restore the black screen */
	public void reset() {
		GetComponent<Image>().enabled = true;
		gameObject.layer = LayerMask.NameToLayer("Transition Screen");

		Text txt = GetComponentInChildren<Text>();
		// Display turn number for the current player
		if (txt != null) {
			txt.text = "Player " + (GameManagerScript.instance.getTurn()) + "'s Turn";
			txt.enabled = true;
		}
	}
}
