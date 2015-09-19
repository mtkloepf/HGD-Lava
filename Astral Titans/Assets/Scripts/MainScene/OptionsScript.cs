using UnityEngine;
using System.Collections;

public class OptionsScript : MonoBehaviour {

    public Sprite mouseExited;
    public Sprite mouseEntered;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnMouseEnter()
    {
        GetComponent<SpriteRenderer>().sprite = mouseEntered;
    }

    void OnMouseExit()
    {
        GetComponent<SpriteRenderer>().sprite = mouseExited;
    }

    void OnMouseDown()
    {
        Application.LoadLevel("options_scene");
    }
}
