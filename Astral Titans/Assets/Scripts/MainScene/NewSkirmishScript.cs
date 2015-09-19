using UnityEngine;
using System.Collections;

public class NewSkirmishScript : MonoBehaviour {

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

        System.Console.WriteLine("NewSkirmish");
    }
}
