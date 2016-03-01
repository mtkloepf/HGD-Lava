using UnityEngine;
using System.Collections;

/**
 * A class designed to test methods created in the MapManager class.
 * 
 * @author Joshua Hooker
 * 1 March 2016
 */
public class Test_Button_Action : MonoBehaviour {
	/* An image for normal display and one for indicating when the game object is clicked on */
	public Sprite origin;
	public Sprite clicked_on;
	private SpriteRenderer _renderer;
	// parameters for the hex_at_offset_from() method
	public int pos_x;
	public int pos_y;
	public bool opp_x;
	public bool opp_y;
	public int offset;
	// May be used to modify a hex with the hex_at_offset_from() method
	public HexScript.HexEnum type;

	// Use this for initialization
	public void Start () {
		/* Get this gameObject's sprite renderer */
		_renderer = GetComponent<SpriteRenderer>();
		_renderer.sprite = origin;

		pos_x = 0;
		pos_y = 0;
		opp_x = false;
		opp_y = false;
		offset = 0;
		type = HexScript.HexEnum.plains;
	}

	/* Tests the MapManager's hex_at_offset_from() method when the game object is clicked on */
	public void OnMouseDown() {
		_renderer.sprite = clicked_on;
		// Tests the hex_at_offset_from method
		MapManager current = CycleMap.getMap();
		// print out type and position of the returned hex
		Debug.Log( current.hex_at_offset_from(current.map[pos_x][pos_y], opp_x, opp_y, offset) );
	}

	/* Resets the image of the game object */
	public void OnMouseUp() {
		_renderer.sprite = origin;
	}
}
