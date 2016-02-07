using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CardShopScript : MonoBehaviour {

	public Sprite p1Sprite;
	public Sprite p2Sprite;
	public GameManagerScript game;
	private Image image;
	public Text cardCount;
	public CardScript.CardType p1Type;
	public CardScript.CardType p2Type;


	// Use this for initialization
	void Start () {
		image = GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!image.enabled) {
			image.enabled = true;
		}
		if (game.turn == 1) {
			CardCollection cards = game.getDeck1 ().getAllCards();
			int count = cards.getCount(p1Type);
			cardCount.text = "" + count;
			image.sprite = p1Sprite;
			if (p1Sprite == null) {
				image.enabled = false;
			}

		} else {
			CardCollection cards = game.getDeck2 ().getAllCards();
			int count = cards.getCount(p2Type);
			cardCount.text = "" + count;
			image.sprite = p2Sprite;
			if (p2Sprite == null) {
				image.enabled = false;
			}
		}
	}

	void OnMouseDown() {
		if (game.turn == 1) {
			game.buyCard(p1Type);
		}
		else if (game.turn == 2) {
			game.buyCard (p2Type);
		}
	}
}
