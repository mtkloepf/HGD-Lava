using UnityEngine;
using System.Collections;

public class CardScript : MonoBehaviour {

	public enum CardType {HumanInfantry, AlienInfantry, HumanTank, AlienTank, Currency1, Currency2};

	private CardType type;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public CardScript init(CardType type) {
		this.type = type;
		return this;
	}

	public void setType(CardType type) {
		this.type = type;
	}

	public CardType getType() {
		return type;
	}

	void OnMouseDown() {
		Debug.Log ("Card clicked of type: " + type);
		GameManagerScript.instance.selectCard (this);
	}
}
