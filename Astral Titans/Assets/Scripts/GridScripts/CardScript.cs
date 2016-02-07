using UnityEngine;
using System.Collections;

public class CardScript : MonoBehaviour
{

	public Sprite HumanInfantry, AlienInfantry, HumanTank, AlienTank, HumanExo, HumanArtillery, Currency1, Currency2;
	SpriteRenderer render;
	public int cost = 0;

	public enum CardType
	{
		HumanInfantry,
		AlienInfantry,
		HumanTank,
		AlienTank,
		HumanExo,
		HumanArtillery,
		Currency1,
		Currency2}
	;

	private CardType type;

	// Use this for initialization
	void Start ()
	{
		transform.SetParent (GameObject.Find ("CardManager").transform);
		startRenderer ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void destroyCard ()
	{
		if (render != null)
			render.sprite = null;
		Destroy (this.gameObject, 1);
	}

	public void startRenderer ()
	{
		render = GetComponent<SpriteRenderer> ();
	}

	public CardScript init (CardType type)
	{
		this.type = type;
		return this;
	}

	public void setType (CardType type)
	{
		this.type = type;
		switch (type) {
		case CardType.AlienInfantry:
			render.sprite = AlienInfantry;
			cost = 1;
			break;
		case CardType.AlienTank:
			render.sprite = AlienTank;
			cost = 4;
			break;
		case CardType.Currency1:
			render.sprite = Currency1;
			break;
		case CardType.Currency2:
			render.sprite = Currency2;
			break;
		case CardType.HumanInfantry:
			render.sprite = HumanInfantry;
			cost = 1;
			break;
		case CardType.HumanTank:
			render.sprite = HumanTank;
			cost = 4;
			break;
		case CardType.HumanExo:
			render.sprite = HumanExo;
			cost = 2;
			break;
		case CardType.HumanArtillery:
			render.sprite = HumanArtillery;
			cost = 3;
			break;

		default:
			break;
		}
	}

	public CardType getType ()
	{
		return type;
	}

	void OnMouseDown ()
	{
		Debug.Log ("Card clicked of type: " + type);
		GameManagerScript.instance.selectCard (this);
	}

	public int getCost(CardScript.CardType type) {
		switch (type) {
		case CardType.AlienInfantry:
			cost = 1;
			break;
		case CardType.AlienTank:
			cost = 4;
			break;
		case CardType.Currency1:
			cost = 1;
			break;
		case CardType.Currency2:
			cost = 3;
			break;
		case CardType.HumanInfantry:
			cost = 1;
			break;
		case CardType.HumanTank:
			cost = 4;
			break;
		case CardType.HumanExo:
			cost = 2;
			break;
		case CardType.HumanArtillery:
			cost = 3;
			break;
		default:
			break;
		}

		return cost;
	}
}
