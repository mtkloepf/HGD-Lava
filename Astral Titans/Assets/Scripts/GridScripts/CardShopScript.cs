using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CardShopScript : MonoBehaviour {

	public Sprite p1Sprite;
	public Sprite p2Sprite;
	private Image image;
	public Text cardCount;
	public CardScript.CardType p1Type;
	public CardScript.CardType p2Type;
	public Canvas parentCanvas;


	// Use this for initialization
	void Start () {
		image = GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!image.enabled) {
			image.enabled = true;
		}
		if (GameManagerScript.instance.getTurn() == 1) {
			CardCollection cards = GameManagerScript.instance.getPlayer().getDeck().getAllCards();
			int count = cards.getCount(p1Type);
			cardCount.text = "" + count;
			image.sprite = p1Sprite;
			if (p1Sprite == null || parentCanvas.enabled == false) {
				image.enabled = false;
				GetComponent<BoxCollider2D>().enabled = false;
				cardCount.text = "";
			}
			else {
				GetComponent<BoxCollider2D>().enabled = true;
			}

		} else {
			CardCollection cards = GameManagerScript.instance.getPlayer().getDeck().getAllCards();
			int count = cards.getCount(p2Type);
			cardCount.text = "" + count;
			image.sprite = p2Sprite;
			if (p2Sprite == null || parentCanvas.enabled == false) {
				image.enabled = false;
				GetComponent<BoxCollider2D>().enabled = false;
				cardCount.text = "";
			}
			else {
				GetComponent<BoxCollider2D>().enabled = true;
			}
		}
	}

	void OnMouseDown() {
		if (GameManagerScript.instance.getTurn() == 1) {
			GameManagerScript.instance.buyCard(p1Type);
		}
		else if (GameManagerScript.instance.getTurn() == 2) {
			GameManagerScript.instance.buyCard (p2Type);
		}
	}
}
