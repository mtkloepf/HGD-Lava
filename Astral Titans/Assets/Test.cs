using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {
	static Animator animator;
	public int health;

	// Use this for initialization
	void Start () {
		animator = this.GetComponent<Animator>();
		health = 3;
	}
	
	// Update is called once per frame
	void Update () {
		if (health <= 0) {
			health = 3;
			animator.SetBool("Died", true);
		}
	}

	void OnMouseDown() {
		--health;
	}
}
