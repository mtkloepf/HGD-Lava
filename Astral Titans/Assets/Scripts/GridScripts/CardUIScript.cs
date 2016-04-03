using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CardUIScript : MonoBehaviour {
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
		if (GameManagerScript.instance.getHoveredUnit () != null) {
			unit = GameManagerScript.instance.getHoveredUnit ();
			health.text = "" + unit.getHealth ();
			attack.text = "" + unit.getAttack ();
			defense.text = "" + unit.getDefense ();
            range.text = "" + unit.getRange();
			movement.text = "" + unit.getMovement ();
			if (unitSprite != null) {
				unitSprite.sprite = unit.gameObject.GetComponent<SpriteRenderer>().sprite;
			}
		}
		else if (GameManagerScript.instance.getFocusedUnit () != null) {
			unit = GameManagerScript.instance.getFocusedUnit ();
			health.text = "" + unit.getHealth ();
			attack.text = "" + unit.getAttack ();
			defense.text = "" + unit.getDefense ();
            range.text = "" + unit.getRange();
            movement.text = "" + unit.getMovement ();
			if (unitSprite != null) {
				unitSprite.sprite = unit.gameObject.GetComponent<SpriteRenderer>().sprite;
			}
		}

		currency.text = "" + GameManagerScript.instance.getPlayer().getCurrency();
		deckCount.text = "" + GameManagerScript.instance.getDeckCount ();
		discardCount.text = "" + GameManagerScript.instance.getDiscardCount ();
	}
}
