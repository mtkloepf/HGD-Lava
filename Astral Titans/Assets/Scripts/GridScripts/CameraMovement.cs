using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{

	public float xVel;
	public float yVel;
	public float zVel;
	public bool locked = false;

	// Use this for initialization
	void Start ()
	{
	    
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKey ("w")) {
			Vector3 position = this.transform.position;
			position.y += yVel;
			this.transform.position = position;
		}
		if (Input.GetKey ("a")) {
			Vector3 position = this.transform.position;
			position.x -= xVel;
			this.transform.position = position;
		}
		if (Input.GetKey ("s")) {
			Vector3 position = this.transform.position;
			position.y -= yVel;
			this.transform.position = position;
		}
		if (Input.GetKey ("d")) {
			Vector3 position = this.transform.position;
			position.x += xVel;
			this.transform.position = position;
		}
			
		float scrollMovement = Input.GetAxis ("Mouse ScrollWheel");
		if (scrollMovement < 0.0f) {
			GetComponent<Camera> ().orthographicSize += zVel;
		}
		if (scrollMovement > 0.0f) {
			if (GetComponent<Camera> ().orthographicSize - zVel < 0) {
				GetComponent<Camera> ().orthographicSize = 0.1f;
			} else {
				GetComponent<Camera> ().orthographicSize -= zVel;
			}
		}

	}
}
