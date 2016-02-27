using UnityEngine;
using System.Collections;

public class CardScript {

	public readonly CardType type;
	public readonly int cost;

	public enum CardType {
		HumanInfantry,
		AlienInfantry,
		HumanTank,
		AlienTank,
		AlienElite,
		HumanExo,
		HumanArtillery,
		Currency1,
		Currency2,
		Currency3,
		Empty
	};

	/* Creates a card based on the given type. */
	public CardScript(CardScript.CardType t) {
		type = t;
		cost = setCost(t);
	}

	/* Sets the cost of the card base on the card's type. */
	private static int setCost(CardType t) {
		switch (t) {
			case CardType.HumanInfantry:		return 1;
			case CardType.HumanTank:			return 4;
			case CardType.HumanExo:				return 2;
			case CardType.HumanArtillery:		return 5;
			case CardType.AlienInfantry:		return 1;
			case CardType.AlienTank:			return 4;
			case CardType.AlienElite:			return 2;
			case CardType.Currency1:			return 1;
			case CardType.Currency2:			return 5;
			case CardType.Currency3:			return 10;
			default:							return int.MaxValue;
		}
	}

	/* Returns the image associated with this card. */
	public Sprite card_image() {
		return SpriteManagerScript.card_by_type(type);
	}
}
