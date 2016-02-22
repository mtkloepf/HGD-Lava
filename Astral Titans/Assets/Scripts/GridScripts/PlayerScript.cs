using UnityEngine;
using System.Collections;

public class PlayerScript {

	private readonly DeckManager deck;
	private int currency;

	/* Creates a player with the given ID value */
	public PlayerScript(DeckManager DM) {
		deck = DM;
		currency = 0;
	}

	/* Adds the given value to the player's currency */
	public void changeCurrency(int change) { currency += change; }

	/* Set the player's currency to the given value */
	public void setCurrency (int val) { currency = val; }

	/* Returns the player's current currency */
	public int getCurrency() { return currency; }

	/* Returns this player's deck */
	public DeckManager getDeck() { return deck; }
}
