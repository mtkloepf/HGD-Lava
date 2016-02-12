using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class OptionsScript : MonoBehaviour
{

	public Sprite mouseExited;
	public Sprite mouseEntered;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	void OnMouseEnter ()
	{
		this.transform.GetComponent<UnityEngine.UI.Image> ().sprite = mouseEntered;
	}

	void OnMouseExit ()
	{
		this.transform.GetComponent<UnityEngine.UI.Image> ().sprite = mouseExited;
	}

	void OnMouseDown () {
		SceneManager.LoadScene ("options_menu");
	}
}
