using UnityEngine;
using System.Collections;

public class NewSkirmishScript : MonoBehaviour
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
		//hi
		this.transform.GetComponent<UnityEngine.UI.Image> ().sprite = mouseExited;
	}

	void OnMouseDown ()
	{
		Application.LoadLevel ("grid_scene");

		System.Console.WriteLine ("NewSkirmish");
	}
}
