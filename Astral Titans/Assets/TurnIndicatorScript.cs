using UnityEngine;
using System.Collections;

public class TurnIndicatorScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void updateTurn(int turn) {
		if (turn == 1) {
//			((SpriteRenderer)GetComponent (SpriteRenderer)).sprite = turn1;
		} else {
//			((SpriteRenderer)GetComponent (SpriteRenderer)).sprite = turn2;
		}
	}
}
