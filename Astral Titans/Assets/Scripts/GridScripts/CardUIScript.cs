using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CardUIScript : MonoBehaviour {

	public GameManagerScript game;
	public Text health;
	public Text attack;
	public Text defense;
	public Text movement;
	public Text deckCount;
	public Text discardCount;
	UnitScript unit;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (game.getFocusedUnit () != null) {
			unit = game.getFocusedUnit ();
			health.text = "" + unit.getHealth ();
			attack.text =  "" + unit.getAttack ();
			defense.text = "" + unit.getDefense ();
			movement.text = "" + unit.getMovement ();
		}
		deckCount.text = "" + game.getDeckCount ();
		discardCount.text = "" + game.getDiscardCount ();
	}
}
