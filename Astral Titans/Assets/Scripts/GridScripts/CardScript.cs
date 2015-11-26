using UnityEngine;
using System.Collections;

public class CardScript : MonoBehaviour {

	public Sprite HumanInfantry, AlienInfantry, HumanTank, AlienTank, Currency1, Currency2;
	SpriteRenderer render;

	public enum CardType {HumanInfantry, AlienInfantry, HumanTank, AlienTank, Currency1, Currency2};

	private CardType type;

	// Use this for initialization
	void Start () {
		startRenderer ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void destroyCard() {
		if(render != null)
			render.sprite = null;
		Destroy (this.gameObject, 1);
	}

	public void startRenderer() {
		render = GetComponent<SpriteRenderer> ();
//		render.sprite = HumanInfantry;
	}

	public CardScript init(CardType type) {
		this.type = type;
//		startRenderer ();
//		setType (type);
		return this;
	}

	public void setType(CardType type) {
		this.type = type;
		switch (type) {
		case CardType.AlienInfantry:
			render.sprite = AlienInfantry;
			break;
		case CardType.AlienTank:
			render.sprite = AlienTank;
			break;
		case CardType.Currency1:
			render.sprite = Currency1;
			break;
		case CardType.Currency2:
			render.sprite = Currency2;
			break;
		case CardType.HumanInfantry:
			render.sprite = HumanInfantry;
			break;
		case CardType.HumanTank:
			render.sprite = HumanTank;
			break;
		default:
			break;
		}
	}

	public CardType getType() {
		return type;
	}

	void OnMouseDown() {
		Debug.Log ("Card clicked of type: " + type);
		GameManagerScript.instance.selectCard (this);
	}
}
