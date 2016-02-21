using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * A class that contains a list of cards and allows for cards
 * to be added and removed, and for the list of cards to be shuffled.
 * 
 * This class will be used for the player's hand, deck, and discard pile.
 */
public class CardCollection {
	
	private List<CardScript> cards = new List<CardScript> ();

	// Removes a card from the collection
	public CardScript remove (CardScript card)
	{
		if (cards.Remove (card))
			return card;
		else
			return null;
	}

	// Removes a given number of cards
	public List<CardScript> removeCount (int count)
	{
		List<CardScript> removed = new List<CardScript> ();
		while (count != 0 && cards.Count > 0) {
			CardScript card = cards [0];
			cards.RemoveAt (0);
			removed.Add (card);
			count --;
		}
		return removed;
	}

	// Removes all cards from the collection
	public List<CardScript> removeAll ()
	{
		List<CardScript> removed = cards;
		cards = new List<CardScript> ();
		return removed;
	}

	// Adds a card to the collection
	public void add (CardScript card)
	{
		cards.Add (card);
	}

	// Adds a list of cards to the collection
	public void addList (List<CardScript> cardList)
	{
		foreach (CardScript card in cardList) {
			cards.Add (card);
		}
	}

	// Returns the size of the list
	public int getSize ()
	{
		return cards.Count;
	}

	// Tells whether the list is empty
	public bool isEmpty ()
	{
		return (cards.Count == 0);
	}

	// Shuffles the list using the Fisher-Yates algorithm
	public void shuffle ()
	{
		for (int i = cards.Count - 1; i >= 0; i --) {
			int j = Random.Range (0, i + 1);
			CardScript temp = cards [i];
			cards [i] = cards [j];
			cards [j] = temp;
		}
	}

	public List<CardScript> getCards ()
	{
		return cards;
	}

	public int getCount(CardScript.CardType type) {
		int counter = 0;
		foreach (CardScript card in cards) {
			if (card.getType () == type) {
				counter ++;
			}
		}
		return counter;
	}

	public string list() {
		string l = "";

		for (int idx = 0; idx < cards.Count; ++idx) {
			l += cards[idx].getType() + "(" + idx + ")";
			if (idx < cards.Count - 1) { l += " , "; }
		}

		return l;
	}
}
