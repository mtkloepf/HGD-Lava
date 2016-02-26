using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TurnIndicatorScript : MonoBehaviour {
	public Sprite player1;
	public Sprite player2;

	public void updateTurn (int turn) {
		if (turn == 1) {
			GetComponent<Image>().sprite = player1;
		} else {
			GetComponent<Image>().sprite =  player2;
		}
	}
}
