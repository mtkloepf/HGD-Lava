using UnityEngine;
using System.Collections;

/**
 * A class designed to allow for the changing of hex types during runtime.
 * 
 * @author Joshua Hooker
 * 1 March 2016
 */
public class HexManagerScript : MonoBehaviour {
	
	// Update is called once per frame
	void Update() {
		// enable or disable hex editing by pressing e
		if (Input.GetKeyDown(KeyCode.E)) {
			HexScript.toggle_editing();
		}

		// cycle through terrain types by pressing r
		if (Input.GetKeyDown(KeyCode.R)) {
			HexScript.cycle_edit_type();
		}
	}
}

