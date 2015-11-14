using UnityEngine;
using System.Collections;

public class CardScript : MonoBehaviour {



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown() {
		Debug.Log ("Card clicked");
		GameManagerScript.instance.selectCard (this);
	}
}
