﻿using UnityEngine;
using System.Collections;

public class NewSkirmishScript : MonoBehaviour
{

	public Sprite mouseExited;
	public Sprite mouseEntered;

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
		Application.LoadLevel ("grid_scene");
	}
}
