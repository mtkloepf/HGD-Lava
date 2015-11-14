using UnityEngine;
using System.Collections;

public class TurnIndicatorScript : MonoBehaviour {
   public Sprite player1;
   public Sprite player2;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void updateTurn(int turn) {
		if (turn == 1) {
			GetComponent<SpriteRenderer>().sprite = 
			player1;
		} else {
			GetComponent<SpriteRenderer>().sprite = 
			player2;
		}
	}
}
