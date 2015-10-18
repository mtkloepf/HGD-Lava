using UnityEngine;
using System.Collections;

public class ChangeHexColor : MonoBehaviour {

    public Sprite[] prefabs = new Sprite[3];

    public int position;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void OnMouseDown()
    {
        position++;
        Debug.Log("Positon: " + position);
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = (Sprite)prefabs[position % 3];
    }
}
