using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

    public float xVel;
    public float yVel;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKey("w")) {
            Vector3 position = this.transform.position;
            position.y += yVel;
            this.transform.position = position;
        }
        if (Input.GetKey("a")) {
            Vector3 position = this.transform.position;
            position.x -= xVel;
            this.transform.position = position;
        }
        if (Input.GetKey("s")) {
            Vector3 position = this.transform.position;
            position.y -= yVel;
            this.transform.position = position;
        }
        if (Input.GetKey("d")) {
            Vector3 position = this.transform.position;
            position.x += xVel;
            this.transform.position = position;
        }
	}
}
