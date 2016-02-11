using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class InputPrompt : MonoBehaviour {
	private string in_1, in_2, in_3;
	private string msg_1, msg_2, msg_3;
	/* [ width min, width max, height min, height max ] */
	private int[] bounds;

	void Start() {
		in_1 = "";
		in_2 = "";
		in_3 = "";
		msg_1 = "";
		msg_2 = "";
		msg_3 = "";
		bounds = new int[] { 10, 45, 10, 45 };
	}

	/* Draws the window for the prompt */
	void OnGUI() {
		GUI.Window(0, new Rect(400, 200, 600, 400), EvaluateInput, "Map Specs");
	}

	/**
	 * Prints information in labels regarding valid input, creates text fields for input,
	 * and handles any errors with user input.
	 */
	void EvaluateInput(int ID) {
		/* Display general guidelines */
		GUI.Label(new Rect(45, 45, 510, 88), "Please input the size and type of the map you wish to create: width and height must be within the bounds of 10 and 45 inclusive.\n\nGame types: " + MapGeneration.allTypes() );
		/* Display width and height prompts */
		GUI.Label(new Rect(45, 165, 120, 22), "Width of the map?");
		GUI.Label(new Rect(45, 200, 124, 22), "Height of the map?");
		GUI.Label(new Rect(45, 235, 110, 22), "Type of map?");
		/* Draw input fields */
		in_1 = GUI.TextField(new Rect(191, 165, 32, 22), in_1, 3);
		in_2 = GUI.TextField(new Rect(191, 200, 32, 22), in_2, 3);
		in_3 = GUI.TextField(new Rect(191, 235, 85, 22), in_3);

		if ( GUI.Button (new Rect (45, 275, 56, 26), "Confirm") ) {
			int w_in = -1;
			int h_in = -1;

			/* Parse user input for width value */
			if ( Int32.TryParse (in_1, out w_in) ) {
				if ( w_in >= bounds[0] && w_in <= bounds[1] ) {
					if (w_in % 2 == 0) {
						MapGeneration.width = w_in / 2;
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
					MapGeneration.height = h_in * 2;
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
			if ( MapGeneration.containsType(in_3) ) {
				/* if all input fields are valid, then jump to creation of the map */
				if (w_in > 0 && h_in > 0) {
					SceneManager.LoadScene("grid_scene");
					// MapGeneration.setGeneration();
				}

				msg_3 = "";
			} else {
				msg_3 = "Not a valid map type!";
			}
		}

		/* Print Error messages */
		GUI.Label(new Rect(235, 165, 146, 22), msg_1);
		GUI.Label(new Rect(235, 200, 146, 22), msg_2);
		GUI.Label(new Rect(288, 235, 146, 22), msg_3);
	}


}

