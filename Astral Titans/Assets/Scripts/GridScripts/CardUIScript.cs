using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CardUIScript : MonoBehaviour
{

	public GameManagerScript game;
	public Text health;
	public Text attack;
	public Text defense;
    public Text range;
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
            range.text = "" + unit.getRange();
			movement.text = "" + unit.getMovement ();
			if (unitSprite != null) {
				unitSprite.sprite = unit.Sprite;
			}
		}
		else if (GameManagerScript.getFocusedUnit () != null) {
			unit = GameManagerScript.getFocusedUnit ();
			health.text = "" + unit.getHealth ();
			attack.text = "" + unit.getAttack ();
			defense.text = "" + unit.getDefense ();
            range.text = "" + unit.getRange();
            movement.text = "" + unit.getMovement ();
			if (unitSprite != null) {
				unitSprite.sprite = unit.Sprite;
			}
		}

		currency.text = "" + GameManagerScript.getPlayer().getCurrency();
		deckCount.text = "" + GameManagerScript.getDeckCount ();
		discardCount.text = "" + GameManagerScript.getDiscardCount ();
	}
}
