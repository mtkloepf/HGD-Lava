using UnityEngine;
using System.Collections;

public class QuitScript : MonoBehaviour
{

	public Sprite mouseExited;
	public Sprite mouseEntered;

	private bool isClicked;

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

	void OnMouseDown ()
	{
		Debug.Log ("Quit was pressed");
		Application.Quit ();
	}
}
