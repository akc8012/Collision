using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube_Movement : MonoBehaviour {

	public GameObject
	playerCamera;

	public float 
	maxVelocity = 6,
	maxTranslateSpeed = 10,
	JumpForce = 10,
	rotationSpeed = 5,
	directionSpeed = 3.0f,
	rotationDegreePerSecond = 120f;


	//Movement Controls
	float 
	horizAxis,
	vertAxis,
	speed = 0f,
	direction = 0f;


	bool 
	jumpBool = false;


	// GET / SET
	public float Speed {get{ return speed; }}


	void Start () {
		
	}

	void FixedUpdate () {

		float frameTime = Time.deltaTime;

//		secondPulse = false;
//		secondTimer -= frameTime;
//		if (secondTimer <= 0) {
//			secondTimer = 0;
//			secondPulse = true;
//		}

		horizAxis = Input.GetAxis ("Horizontal");
		vertAxis = Input.GetAxis ("Vertical");

		speed = new Vector2 (horizAxis, vertAxis).magnitude;

//		StickToWorldspace (this.transform, Camera.main.transform, ref direction, ref speed);

		MoveROTATION (horizAxis, vertAxis, frameTime);

		MoveFORCE (horizAxis, vertAxis, frameTime);
		//MoveTRANSLATE (horizAxis, vertAxis, frameTime);

		if (Input.GetButtonDown ("Jump") && !jumpBool) {
			GetComponent<Rigidbody> ().AddForce (0, JumpForce, 0);
			jumpBool = true;
		}
		if (((direction > 0 && horizAxis > 0) || (direction < 0 && horizAxis < 0))) {

			Vector3 rotationAmount = Vector3.Lerp (Vector3.zero, new Vector3 (0f, rotationDegreePerSecond * (horizAxis < 0f ? -1f : 1f), 0f), Mathf.Abs(horizAxis));
			Quaternion deltaRotation = Quaternion.Euler (rotationAmount * Time.deltaTime);
			this.transform.rotation = (this.transform.rotation * deltaRotation);
		}
	}

	void MoveFORCE(float horizAxis, float vertAxis, float frameTime){

		Vector3 movement = this.transform.forward * vertAxis;
		//movement += this.transform.right * horizAxis;
		movement *= maxVelocity;

		if (vertAxis != 0) {
			//GetComponent<Rigidbody> ().velocity = new Vector3 (maxVelocity * movement.x, GetComponent<Rigidbody> ().velocity.y, maxVelocity * movement.z);
			GetComponent<Rigidbody> ().velocity = new Vector3(movement.x,GetComponent<Rigidbody> ().velocity.y,movement.z);
		}

		if (vertAxis == 0) {
			GetComponent<Rigidbody> ().velocity = new Vector3 (0, GetComponent<Rigidbody> ().velocity.y, 0);
		}
	}

	void MoveTRANSLATE(float horizAxis, float vertAxis, float frameTime){
		
		Vector3 movement = this.transform.forward * vertAxis;
		//movement += this.transform.right * horizAxis;

		movement *= maxTranslateSpeed * frameTime;
		transform.Translate (movement);
	}

	void MoveROTATION(float horizAxis, float vertAxis, float frameTime){
		
		if (horizAxis > 0) {
			transform.Rotate(0,rotationSpeed,0);
		}

		if (horizAxis < 0) {
			transform.Rotate(0,-rotationSpeed,0);
		}
	}
	 
	void OnTriggerStay()
	{
		jumpBool = false;
	}
		
//	public void StickToWorldspace(Transform root, Transform camera, ref float directionOut, ref float speedOut){
//
//		Vector3 rootDirection = root.forward;
//
//		Vector3 stickDirection = new Vector3 (horizAxis, 0, vertAxis);
//
//		speedOut = stickDirection.sqrMagnitude;
//
//		// Get camera rot
//		Vector3 CameraDirection = camera.forward;
//		CameraDirection.y = 0.0f; // set Y to 0
//		Quaternion referentialShift = Quaternion.FromToRotation(Vector3.forward, CameraDirection);
//
//		// Convert joystick input in Worldspace coordinates
//		Vector3 moveDirection = referentialShift * stickDirection;
//		Vector3 axisSign = Vector3.Cross (moveDirection, rootDirection);
//
//		float angleRootToMove = Vector3.Angle (rootDirection, moveDirection) * (axisSign.y >= 0 ? -1f : 1f);
//
//		angleRootToMove /= 180f;
//
//		directionOut = angleRootToMove * directionSpeed;
//	}
}
