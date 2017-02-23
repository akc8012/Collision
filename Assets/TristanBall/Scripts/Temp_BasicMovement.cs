using UnityEngine;
using System.Collections;

// Placement:  Place on temp player cube

// Purpose:  Allows for basic controls to shift the temp player box around.

namespace TristanBall
{
public class Temp_BasicMovement : MonoBehaviour {

	Rigidbody rb;

	Vector3 movement;

	void Start () {
		
		rb = GetComponent<Rigidbody> ();
	
	}

	void Update () {
	
		float speed = 15;

		movement = new Vector3 (Input.GetAxis ("Horizontal") * speed, 0, Input.GetAxis ("Vertical") * speed);

		if (rb.velocity.magnitude < 4) {
			rb.AddForce (movement);
		}
	}

	public Vector3 Movement {get{ return movement;}}
}
}