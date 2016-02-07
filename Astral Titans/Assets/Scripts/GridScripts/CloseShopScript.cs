using UnityEngine;
using System.Collections;

public class CloseShopScript : MonoBehaviour {

	public GameManagerScript game;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown() {
		game.toggleShop ();
	}
}
