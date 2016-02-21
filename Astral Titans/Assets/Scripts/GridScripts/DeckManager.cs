using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A class used to manage a player's deck, discard pile, and hand.
/// </summary>
public class DeckManager : MonoBehaviour
{

	public CardCollection hand = new CardCollection ();
	public CardCollection deck = new CardCollection ();
	public CardCollection discardPile = new CardCollection ();

	private int handSize = 5;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	/// <summary>
	/// Initialize your deck with a given hand, deck, and discardPile.
	/// The hand and discardPile should usually be an empty CardCollection
	/// </summary>
	/// <param name="hand">The hand.</param>
	/// <param name="deck">The deck.</param>
	/// <param name="discardPile">The discard pile.</param>
	public void init (CardCollection hand, CardCollection deck, CardCollection discardPile)
	{
		this.hand = hand;
		this.deck = deck;
		this.discardPile = discardPile;
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
