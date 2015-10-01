using UnityEngine;
using System.Collections;

public class ChangeHexColor : MonoBehaviour {

    public GameObject[] prefabs = new GameObject[3];

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

    }
}
