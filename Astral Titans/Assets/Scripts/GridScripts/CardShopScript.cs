using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CardShopScript : MonoBehaviour {

	public Sprite p1Sprite;
	public Sprite p2Sprite;
	public GameManagerScript game;
	private Image image;


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
			image.sprite = p1Sprite;
			if (p1Sprite == null) {
				image.enabled = false;
			}
		} else {
			image.sprite = p2Sprite;
			if (p2Sprite == null) {
				image.enabled = false;
			}
		}
	}
}
