using UnityEngine;
using System.Collections;

/**
 * A class designed to allow for the changing of hex types during runtime.
 * 
 * @author Joshua Hooker
 * 1 March 2016
 */
public class HexManagerScript : MonoBehaviour {
	// Used for changing hex types at runtime
	private static bool EDIT_HEX;
	private static bool FOG_TOGGLE;
	private static int EDIT_TYPE;

	public void Start() {
		EDIT_HEX = false;
		FOG_TOGGLE = false;
		EDIT_TYPE = (int)HexScript.HexEnum.plains;
	}

	// Update is called once per frame
	void Update() {
		// enable or disable hex editing by pressing p
		if (Input.GetKeyDown(KeyCode.P)) {
			EDIT_HEX = !EDIT_HEX;
			Debug.Log( "Hex editing " + ((EDIT_HEX) ? "enabled" : "disabled") );
		}

		if (Input.GetKeyDown(KeyCode.F)) {
			FOG_TOGGLE = !FOG_TOGGLE;
			Debug.Log( "Fog tiles " + ((FOG_TOGGLE) ? "enabled" : "disabled") );
		}

		// cycle through terrain types by pressing r
		if (Input.GetKeyDown(KeyCode.R)) {
			EDIT_TYPE = (EDIT_TYPE + 1) % 4;
			Debug.Log("Edit type: " + (HexScript.HexEnum)EDIT_TYPE);
		}
	}

	public static bool edit_hex() { return EDIT_HEX; }

	public static bool fog_toggle() { return FOG_TOGGLE; }

	public static int edit_type() {return EDIT_TYPE; }
}

