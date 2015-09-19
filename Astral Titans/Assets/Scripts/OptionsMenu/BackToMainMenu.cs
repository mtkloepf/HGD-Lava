using UnityEngine;
using System.Collections;

public class BackToMainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

    void OnMouseDown()
    {
        Application.LoadLevel("start_menu");
    }
}
