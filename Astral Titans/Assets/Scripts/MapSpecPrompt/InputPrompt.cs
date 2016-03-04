using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

/**
 * A class designed to prompt the user for information used to create a pseudo-random map.
 * You can add the prefix '$' to the map type to jump to the testing scene instead of an actual game.
 * 
 * @author Joshua Hooker
 * 29 February 2016
 */
public class InputPrompt : MonoBehaviour {

	private Rect window_dimensions;
	private Rect[] components;
	private string in_1, in_2, in_3;
	private string msg_1, msg_2, msg_3;
	/* [ width min, width max, height min, height max ] */
	private int[] bounds;

	void Start() {
		window_dimensions = new Rect(100, 100, 100, 100);
		components = new Rect[11];

		in_1 = "";
		in_2 = "";
		in_3 = "";
		msg_1 = "";
		msg_2 = "";
		msg_3 = "";

		bounds = new int[] { 12, 46, 12, 46 };
	}

	/* Draws the window for the prompt */
	void OnGUI() {
		var limbo = new Rect(window_dimensions.x, window_dimensions.y, window_dimensions.width, window_dimensions.height);
		window_dimensions = resizeWindow();
		// resizes the components of the window if the window's dimensions changed
		if (!Rect.Equals (limbo, window_dimensions)) {
			Debug.Log(window_dimensions);
			updateComponents();
		}

		GUI.Window(0, window_dimensions, EvaluateInput, "Map Specs");
	}
		
	/* Resize the window based on the size of the screen */
	private Rect resizeWindow() {
		// minimum value for window width is 400
		float width = Mathf.Max(2.0f * Screen.width / 5.0f, 400.0f);
		return new Rect((Screen.width - width) / 2.0f, Screen.height / 5, width, 3 * Screen.height / 5);
	}

	/* Reconfigures the positions of all elements in the window. */
	private void updateComponents() {
		// dimensions for the general description
		components[0] = new Rect(45, 45, window_dimensions.width - 90, 128);
		// dimensions for the labels
		components[1] = new Rect(components[0].x, components[0].y + components[0].height + 22, 110, 22);
		components[2] = new Rect(components[1].x, components[1].y + components[1].height + 10, 110, 22);
		components[3] = new Rect(components[2].x, components[2].y + components[2].height + 10, 110, 22);
		// dimensions for the input fields
		components[4] = new Rect(components[1].x + components[1].width + 18, components[1].y, 32, 22);
		components[5] = new Rect(components[4].x, components[2].y, 32, 22);
		components[6] = new Rect(components[5].x, components[3].y, 85, 22);
		// dimensions for error output
		components[7] = new Rect(components[4].x + components[4].width + 6, components[4].y, 146, 22);
		components[8] = new Rect(components[5].x + components[5].width + 6, components[5].y, 146, 22);
		components[9] = new Rect(components[6].x + components[6].width + 6, components[6].y, 146, 22);
		// dimensions for button
		components[10] = new Rect(components[0].x, components[6].y + components[6].height + 30, 56, 26);
	}

	/**
	 * Prints information in labels regarding valid input, creates text fields for input,
	 * and handles any errors with user input.
	 */
	void EvaluateInput(int ID) {
		/* Display general guidelines */
		GUI.Label(components[0], "Please input the size and type of the map you wish to create: width and height must be within the bounds of 12 and 46 inclusive and the width must be even.\n\nGame types: " + SceneTransitionStorage.allTypes() );
		/* Display width and height prompts */
		GUI.Label(components[1], "Width of the map?");
		GUI.Label(components[2], "Height of the map?");
		GUI.Label(components[3], "Type of map?");
		/* Draw input fields */
		in_1 = GUI.TextField(components[4], in_1, 3);
		in_2 = GUI.TextField(components[5], in_2, 3);
		in_3 = GUI.TextField(components[6], in_3);

		if ( GUI.Button (components[10], "Confirm") ) {
			int w_in = -1;
			int h_in = -1;

			/* Parse user input for width value */
			if ( Int32.TryParse (in_1, out w_in) ) {
				if ( w_in >= bounds[0] && w_in <= bounds[1] ) {
					if (w_in % 2 == 0) {
						SceneTransitionStorage.map_width = w_in / 2;
						msg_1 = "";
					} else {
						msg_1 = "Width must be even!";
						w_in = -1;
					}
				} else {
					msg_1 = "Not a valid width value!";
					w_in = -1;
				}
			} else {
				msg_1 = "Input must be an integer!";
				w_in = -1;
			}

			/* Parse user input for height value */
			if ( Int32.TryParse (in_2, out h_in) ) {
				if (h_in >= bounds[2] && h_in <= bounds[3]) {
					SceneTransitionStorage.map_height = h_in * 2;
					msg_2 = "";
				} else {
					msg_2 = "Not a valid height value!";
					h_in = -1;
				}
			} else {
				msg_2 = "Input must be an integer!";
				h_in = -1;
			}
			/* Parse user input for map type */
			if ( SceneTransitionStorage.containsType(in_3) ) {
				/* if all input fields are valid, then jump to creation of the map */
				if (w_in > 0 && h_in > 0) {
					SceneTransitionStorage.map_type = in_3;
					SceneManager.LoadScene("grid_scene");
				}

				msg_3 = "";
			} else if ( in_3.Length > 0 && in_3[0] == '$' && SceneTransitionStorage.containsType(in_3.Substring(1, in_3.Length - 1) ) ) { // jump to map generation testing
				if (w_in > 0 && h_in > 0) {
					SceneTransitionStorage.map_type = in_3.Substring(1, in_3.Length - 1);
					SceneManager.LoadScene("test_map_generation");
				}
			} else {
				msg_3 = "Not a valid map type!";
			}
		}

		/* Print error messages */
		GUI.Label(components[7], msg_1);
		GUI.Label(components[8], msg_2);
		GUI.Label(components[9], msg_3);
	}
}

