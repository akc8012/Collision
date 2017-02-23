using UnityEngine;
using System.Collections;

// Placement:  Place on the Main Camera

// Purpose:  Allows the Main Camera to never lose track of the player object
//				Also allows for rotation and distance controls for the Main Camera.

namespace TristanBall
{
public class CameraDistanceToPlayer : MonoBehaviour {

	GameObject 
		player;

	Vector3 
		offset;

	[SerializeField]
	float 
		rotationSpeed = 10,
		liftSpeed = 4;

	void Start () 
	{
		player = GameObject.FindWithTag ("Player");
		transform.Translate (Vector3.right * rotationSpeed * Time.deltaTime);
		transform.LookAt (player.transform);
		offset = transform.localPosition - player.transform.localPosition;
	}

	void Update () 
	{
		player = GameObject.FindWithTag ("Player");
		transform.localPosition = player.transform.localPosition + offset;

		float horizMovement = Input.GetAxis ("altHorizontal");
		float vertMovement = Input.GetAxis ("altVertical");

		if (horizMovement != 0) {
			transform.Translate (Vector3.right * horizMovement * rotationSpeed * Time.deltaTime);
			transform.LookAt (player.transform);
			offset = transform.localPosition - player.transform.localPosition;
		}
		if (horizMovement != 0) {
			transform.Translate (Vector3.up * vertMovement * liftSpeed * Time.deltaTime);
			transform.LookAt (player.transform);
			offset = transform.localPosition - player.transform.localPosition;
		}
	}
}
}