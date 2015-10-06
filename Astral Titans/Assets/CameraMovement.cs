using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

    public int xVel;
    public int yVel;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {

	    if (Input.GetKeyDown("w")) {
            Debug.Log("w");
            Vector3 position = this.transform.position;
            position.y += yVel;
            this.transform.position = position;
        }
        if (Input.GetKeyDown("a")) {
            Debug.Log("a");
            Vector3 position = this.transform.position;
            position.x -= xVel;
            this.transform.position = position;
        }
        if (Input.GetKeyDown("s")) {
            Debug.Log("s");
            Vector3 position = this.transform.position;
            position.y -= yVel;
            this.transform.position = position;
        }
        if (Input.GetKeyDown("d")) {
            Debug.Log("d");
            Vector3 position = this.transform.position;
            position.x += xVel;
            this.transform.position = position;
        }
	}
}
