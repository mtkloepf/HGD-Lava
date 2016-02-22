using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A class used to manage a player's deck, discard pile, and hand.
/// </summary>
public class DeckManager {

	public CardCollection hand;
	public CardCollection deck;
	public CardCollection discardPile;
	private int handSize;

	/* Builds a deck given the types and their respective quantities in the array arguments.
	 * NOTE: types and weights MUST be of the same length!
	 * Also, there should be no null values in types and weights should contain no nonpositive values either! */
	public DeckManager(CardScript.CardType[] types, int[] weights) {
		deck = new CardCollection ();

		for (int idx = 0; idx < types.Length; ++idx) {
			for (int qty = 0; qty < weights[idx]; ++qty) {
				deck.add( new CardScript().init(types[idx]) );
			}
		}

		hand = new CardCollection();
		discardPile = new CardCollection();
		handSize = 5;
	}
	
	/// <summary>
	/// Discards your hand and adds the discarded cards to the discard pile.
	/// </summary>
	public void discardHand ()
	{
		List<CardScript> cards = hand.removeAll ();
		discardPile.addList (cards);
	}
	
	/// <summary>
	/// Puts all your discarded cards into the deck then shuffles it
	/// </summary>
	public void discardToDeck ()
	{
		List<CardScript> cards = discardPile.removeAll ();
		deck.addList (cards);
		deck.shuffle ();
	}
	
	/// <summary>
	/// Discards your hand and deals out new cards. If you don't have enough cards in your
	/// deck, this shuffles your discard pile and sets that to be your new deck
	/// </summary>
	public void deal ()
	{
		discardHand ();
		hand.addList (deck.removeCount (handSize));
		int count = handSize - hand.getSize ();
		if (count > 0) {
			discardToDeck ();
			hand.addList (deck.removeCount (count));
		}
	}
	
	/// <summary>
	/// Gets the maximum size of the hand.
	/// </summary>
	/// <returns>The maximum hand size.</returns>
	public int getHandSize ()
	{
		return handSize;
	}
	
	/// <summary>
	/// Sets the maximum size of the hand.
	/// </summary>
	/// <param name="size">The new maximum hand size.</param>
	public void setHandSize (int size)
	{
		handSize = size;
	}

	public CardCollection getAllCards() {
		CardCollection allCards = new CardCollection ();
		allCards.addList (hand.getCards());
		allCards.addList (deck.getCards ());
		allCards.addList (discardPile.getCards ());
		return allCards;
	}

	/* Removes a number of cards of the given type equal to or less than quantity from the discard pile.
	 * Keep in mind that if there exists less cards in the discard pile than quantity the cards will
	 * still be removed!! */
	public int removeCardsFromDiscard(CardScript.CardType toRemove, int quantity) {
		List<CardScript> discard = discardPile.getCards();
		int left = quantity;
		// Attempt to remove the number of cards equal to quantity
		for (int idx = 0; idx < discardPile.getSize();) {
			// Exit after removing the quantity of a card type
			if (left <= 0) {
				break;
			} else if (discard[idx].getType().CompareTo(toRemove) == 0) {
				discard.RemoveAt(idx);
				--left;
			} else {
				++idx;
			}
		}

		return quantity - left;
	}
}
