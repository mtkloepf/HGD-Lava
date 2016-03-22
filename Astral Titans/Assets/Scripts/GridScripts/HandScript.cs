using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/**
 * This class is designed to be attached to the hand display gameobjects and will switch
 * the image of the display based on the card that this script currently holds.
 * 
 * @author Joshua Hooker
 * 27 February 2016
 */
public class HandScript : MonoBehaviour {
	// The card to display
	public CardScript.CardType card;
	public int index;

	// Use this for initialization
	void Start() {
		updateImage();
	}
	
	// Update is called once per frame
	void Update() {
		updateImage();
	}

	/* Sets the image of the gameObject this is attached to the image of its card. */
	public void updateImage() {
		bool has_card = card != CardScript.CardType.Empty;
		// enables the card to show if it is not null
		GetComponent<Image>().enabled = has_card;
		GetComponent<Image>().sprite = (has_card) ? SpriteManagerScript.card_by_type( card ) : null;
	}

	public void OnMouseDown() {
		if (card != CardScript.CardType.Empty) {
			bool cardRemoved = GameManagerScript.instance.cardClicked(index);
			// Debug.Log(this.gameObject.layer);

			if (cardRemoved) { // Card was removed
				reset(-1, CardScript.CardType.Empty);
			}
		}
	}

	public void reset(int idx, CardScript.CardType t) {
		card = t;
		index = idx;

		if (t == CardScript.CardType.Empty) { // Ignored by Mouse
			gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
		} else { // Default layer
			gameObject.layer = LayerMask.NameToLayer("UI");
		}
	}
}

