using UnityEngine;
using System.Collections;

/**
 * A class designed to be paired with a Card prefab and will change that card's image based
 * on the card script that this holds. 
 * 
 * @author Joshua Hooker
 * 27 February 2016
 */
public class CardImageScript : MonoBehaviour {

	public CardScript card_image;

	// Use this for initialization
	void Start() {
		card_image = new CardScript(CardScript.CardType.Empty);
		GetComponent<SpriteRenderer>().sprite = SpriteManagerScript.card_by_type(card_image.type);
	}

}

