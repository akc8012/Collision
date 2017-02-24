using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Adjust wall collision positions based on player to wall distance

// Adjust camera snap positioning where once you snap to the spot behind the player,
//		you'll stay there once the snap key is released



public class Camera_Control2 : MonoBehaviour {

	public float
	distanceAway,
	distanceAwayMultiplier = 1.5f,
	distanceUp,
	distanceUpMultiplier = 1.5f,
	smooth,
	lookDirDampTime = 0.1f,
	camSmoothDampTime = 0.1f,
	freeThreshhold = -0.1f,
	rightStickThreshhold = 0.1f,
	freeRotationDegreePerSecond = -5f;

	float
	leftX,
	leftY,
	//rightX,
	rightY,
	distanceAwayFree,
	distanceUpFree,
	cameraRotationAdjust;

	Transform 
	follow;

	public GameObject
	playerObj,
	parentRig;

	public Vector2
	camMinDistanceFromCharacter = new Vector2(1,-0.5f),
	cameraSpeed = new Vector2(0.2f,0.2f);

	Vector2
	rightStickPrevFrame = Vector2.zero;

	Vector3
	targetPosition,
	lookDir,
	curLookDir,
	velocityCamSmooth = Vector3.zero,
	velocityLookDir = Vector3.zero,
	savedRigToGoal;



	public enum CameraStates{

		BEHIND,
		TARGET,
		FREE
	}

	CameraStates camState = CameraStates.BEHIND;

	void Start () {

		follow = GameObject.FindWithTag ("Player").transform;

		curLookDir = follow.forward;

	}

	void Update () {
		
	}

	void LateUpdate () {

		Vector3 characterOffset = follow.position + new Vector3(0,distanceUp,0);

		leftX = Input.GetAxis ("Horizontal");
		leftY = Input.GetAxis ("Vertical");
		//rightX = Input.GetAxis ("CamHorizontal");
		rightY = Input.GetAxis ("CamVertical");

		#region FREE CAMERA MOVEMENT

		#region DISTANCE UP
		if (rightY > 0)
			rightStickPrevFrame.y = cameraSpeed.y;
		else if (rightY < 0)
			rightStickPrevFrame.y = -cameraSpeed.y;
		else
			rightStickPrevFrame.y = 0;

		if (distanceUp >= 1 && distanceUp <= 5)
			distanceUp += rightStickPrevFrame.y;
		if(distanceUp < 1) distanceUp = 1;
		if(distanceUp > 5) distanceUp = 5;
		#endregion

		#region DISTANCE AWAY
		if (distanceAway >= 3.5f && distanceAway <= 6)
			distanceAway -= rightStickPrevFrame.y / 2;
		if(distanceAway < 3.5f) distanceAway = 3.5f;
		if(distanceAway > 6) distanceAway = 6;
		#endregion

		#region CAMERA ROTATION ADJUST

//		if(rightX > 0){
//			if(rightX > cameraRotationAdjust){
//				cameraRotationAdjust = rightX;
//			}
//		}
//		if(rightX < 0){
//			if(rightX < cameraRotationAdjust){
//				cameraRotationAdjust = rightX;
//			}
//		}

		#endregion

		#endregion

		targetPosition = Vector3.zero;

		if (Input.GetKey (KeyCode.LeftShift)) {

			camState = CameraStates.TARGET;

		} else if (camState != CameraStates.FREE){

			camState = CameraStates.BEHIND;

		}

		switch (camState) {

		case CameraStates.BEHIND:

			if (playerObj.GetComponent<Cube_Movement> ().Speed > 0) {
				lookDir = Vector3.Lerp (follow.right * (leftX < 0 ? 1 : -1), follow.forward * (leftY < 0 ? -1 : 1), Mathf.Abs (Vector3.Dot (this.transform.forward, follow.forward)));

				curLookDir = Vector3.Normalize (characterOffset - this.transform.position);
				curLookDir.y = 0;

				curLookDir = Vector3.SmoothDamp (curLookDir, lookDir, ref velocityLookDir, lookDirDampTime);
			}

			break;

		case CameraStates.TARGET:

			curLookDir = follow.forward;
			curLookDir.y = 0;

			break;
		}

		targetPosition = characterOffset + follow.up * distanceUp - Vector3.Normalize (curLookDir) * distanceAway;

		CompensateForWalls (characterOffset, ref targetPosition);

		smoothPosition (parentRig.transform.position, targetPosition);

		transform.LookAt (playerObj.transform);

	}

	void smoothPosition(Vector3 fromPos, Vector3 toPos){

		parentRig.transform.position = Vector3.SmoothDamp (fromPos, toPos, ref velocityCamSmooth, camSmoothDampTime);
	}

	void CompensateForWalls(Vector3 fromObject, ref Vector3 toTarget){

		RaycastHit wallHit = new RaycastHit ();
		if (Physics.Linecast (fromObject, toTarget, out wallHit)) {

			toTarget = new Vector3 (wallHit.point.x, toTarget.y, wallHit.point.z);

			Vector3 directionToPlayer = wallHit.point - follow.position;
			directionToPlayer.y = 0;
			directionToPlayer.Normalize ();

			toTarget -= directionToPlayer * 0.4f;

		}
	}
}
