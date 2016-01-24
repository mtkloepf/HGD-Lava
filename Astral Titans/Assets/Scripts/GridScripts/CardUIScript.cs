using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CardUIScript : MonoBehaviour
{

	public GameManagerScript game;
	public Text health;
	public Text attack;
	public Text defense;
	public Text movement;
	public Text deckCount;
	public Text discardCount;
	public Text currency;
	public Image unitSprite;
	UnitScript unit;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (game.getHoveredUnit () != null) {
			unit = game.getHoveredUnit ();
			health.text = "" + unit.getHealth ();
			attack.text = "" + unit.getAttack ();
			defense.text = "" + unit.getDefense ();
			movement.text = "" + unit.getMovement ();
			unitSprite.sprite = unit.Sprite;
		}
		else if (game.getFocusedUnit () != null) {
			unit = game.getFocusedUnit ();
			health.text = "" + unit.getHealth ();
			attack.text = "" + unit.getAttack ();
			defense.text = "" + unit.getDefense ();
			movement.text = "" + unit.getMovement ();
			unitSprite.sprite = unit.Sprite;
		}
		if (game.turn % 2 == 1)
			currency.text = "" + game.Player1.getCurrency ();
		else
			currency.text = "" + game.Player2.getCurrency ();
		deckCount.text = "" + game.getDeckCount ();
		discardCount.text = "" + game.getDiscardCount ();
	}
}
