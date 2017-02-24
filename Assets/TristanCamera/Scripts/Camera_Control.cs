using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 	
 Placement:  	Camera that focuses on the player

 Purpose:  		Controls camera motion based on user input.  
				Also handles camera collision and reaction to player character actions.
*/

public class Camera_Control : MonoBehaviour {

	GameObject 
		player;

	public float 
		ZoomTOSpeed = 2.0f,
		ZoomFROMSpeed = 4.0f;

	float 
		defaultNeutralHeight,			// basic height level
		neutralHeight,					// basic height level
//		collisionShiftAmount,			// height to shift when camera is too close to a wall
		triggerCooldown = 1.0f;

//	Vector3	
//		cameraTilt;		// tilt of camera based on player height compared to neutral camera height

	bool 
		triggered = false,
		triggerCooldownBool = false;

	void Start () 
	{
		player = GameObject.FindWithTag ("Player");

		defaultNeutralHeight = transform.position.y;
		neutralHeight = defaultNeutralHeight;
//		collisionShiftAmount = 5;
//		cameraTilt = transform.rotation.eulerAngles;
	}

	void LateUpdate () 
	{
		// i, j, k, l <- Camera control
		float horizAxis = Input.GetAxis ("CamHorizontal");
		//float vertAxis = Input.GetAxis ("CamVertical");

		transform.LookAt (player.transform);

		Vector3 pseudoPlayer = new Vector3 (player.transform.position.x, 0, player.transform.position.z);
		Vector3 pseudoCamera = new Vector3 (this.transform.position.x, 0, this.transform.position.z);
		float distance = Vector3.Distance (pseudoPlayer, pseudoCamera);

		Vector3 offset = transform.position - player.transform.position;

		//Debug.Log ("Forward: " + Camera.main.transform.forward + ", Right: " + Camera.main.transform.right);
		//Debug.Log ("Camera: " + transform.position + ", Player: " + player.transform.position + ", Offset: " + offset + ", Distance: " + distance);
		//Debug.Log ("Camera Current: " + transform.position + ", Camera After: " + (transform.position - offset.normalized / 10));

		if (distance > 4.3f) {
			transform.position = Vector3.Lerp (
				transform.position, 
				transform.position - offset.normalized * (distance - ZoomTOSpeed + 1), 
				Time.deltaTime);
		}

		if (distance < 3.0f) {
			transform.position = Vector3.Lerp (
				transform.position, 
				transform.position + offset.normalized * (distance + ZoomFROMSpeed + 1), 
				Time.deltaTime);
		}

		// Rotate camera around player when keys pressed
		if (horizAxis != 0) {
			transform.Translate (Vector3.right * horizAxis * (distance*2) * Time.deltaTime);
		}

		// Constant check for when the camera is not within another object (after cooldown)
		if (!triggerCooldownBool) {
			triggered = false;
			triggerCooldownBool = true;
			neutralHeight = defaultNeutralHeight;
		}

		// Bring camera back to default position if not touching / within an object
		if (transform.position.y > defaultNeutralHeight) {
			if (!triggered)
				transform.position = Vector3.Lerp (transform.position, transform.position - new Vector3 (0, 3.0f, 0), Time.deltaTime);
		} else {
			transform.position = new Vector3 (transform.position.x, defaultNeutralHeight, transform.position.z);
		}

		// Cooldown for entering and exiting an object's collision area
		if (triggerCooldownBool) {
			triggerCooldown -= Time.deltaTime;
			if (triggerCooldown <= 0) {
				triggerCooldown = 1;
				triggered = !triggered;
				triggerCooldownBool = false;
			}
		}
	}

	void OnTriggerStay(Collider col){

		if (col.GetComponent<MeshRenderer> ())
			col.GetComponent<MeshRenderer> ().enabled = false;

		triggered = true;
		triggerCooldownBool = true;
		neutralHeight = 6;
		triggerCooldown = 1;

		transform.position = Vector3.Lerp (
			transform.position, 
			new Vector3 (transform.position.x, neutralHeight, transform.position.z), 
			Time.deltaTime);
	}

	void OnTriggerExit(Collider col){
		
		if (col.GetComponent<MeshRenderer> ())
			col.GetComponent<MeshRenderer> ().enabled = true;

	}
}

/*

Sentiments:  	When the camera tilts up to look at the player; the further the player is, the closer to the top edge of the screen
					the player should get until a set amount has been reached in which the player will leave the camera range.

				If the player leaves the camera range via height, all controls should be locked and horizontal movement should
					be reset to zero to allow the player to reenter the visible area.

*/
