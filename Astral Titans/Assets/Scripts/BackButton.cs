using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/**
 * A simple class designed to return to the menu scene, when the
 * gameObject, which it is tied to is clicked on.
 *
 * @author Joshua Hooker
 * 29 February 2016
 */
public class BackButton : MonoBehaviour {
	/* Displayed when the gameObject is clicked on. */
	public Sprite object_clicked;

	// Go back to start menu
	public void OnMouseDown() {
		GetComponent<SpriteRenderer>().sprite = object_clicked;
		SceneManager.LoadScene("start_menu");
	}
}

