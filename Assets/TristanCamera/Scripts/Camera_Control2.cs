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
	distanceSide,
	smooth,
	lookDirDampTime = 0.1f,
	camSmoothDampTime = 0.1f,
	freeThreshhold = -0.1f,
	rightStickThreshhold = 0.1f,
	freeRotationDegreePerSecond = -5f;

	float
	//leftX,
	//leftY,
	rightX,
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
	camMinDistanceFromCharacter = new Vector2(1,-0.5f);

	Vector2
	rightStickPrevFrame = Vector2.zero;

	Vector3
	targetPosition,
	//lookDir,
	curLookDir,
	//velocityCamSmooth = Vector3.zero,
	velocityLookDir = Vector3.zero;
	//savedRigToGoal;

	//float lookWeight = 0;


	[SerializeField] bool invertX;
	[SerializeField] bool invertY;

	public enum CameraStates{

		BEHIND,
		TARGET,
		FREE
	}

	CameraStates camState = CameraStates.BEHIND;

	void Start () {

		follow = GameObject.Find ("Follow").transform;

		curLookDir = follow.forward;

	}

	void Update () {
		
	}

	void LateUpdate () {

		Vector3 characterOffset = follow.position + new Vector3(0,distanceUp,0);

		//leftX = Input.GetAxis ("Horizontal");
		//leftY = Input.GetAxis ("Vertical");
		rightX = Input.GetAxisRaw ("CamHorizontal");
		rightY = Input.GetAxisRaw ("CamVertical");

		#region FREE CAMERA MOVEMENT

		#region DISTANCE UP
		rightStickPrevFrame.y = 0.1f * rightY * (invertY ? 1 : -1);

		if (distanceUp >= 0 && distanceUp <= 8)
			distanceUp += rightStickPrevFrame.y;
		if(distanceUp < 0) distanceUp = 0;
		if(distanceUp > 8) distanceUp = 8;

		#endregion

		#region CAMERA ROTATION ADJUST

		rightStickPrevFrame.x += 3 * rightX * (invertX ? 1 : -1);

		follow.rotation = Quaternion.Euler(Vector3.down * follow.parent.eulerAngles.y + new Vector3(0,follow.parent.eulerAngles.y,0));


		#endregion

		#region DISTANCE AWAY
		if (distanceAway >= 3.5f && distanceAway <= 6)
			distanceAway -= rightStickPrevFrame.y / 2;
		if(distanceAway < 3.5f) distanceAway = 3.5f;
		if(distanceAway > 6) distanceAway = 6;
		#endregion

		#endregion

		if (rightX != 0) {

			camState = CameraStates.FREE;
			//savedRigToGoal = Vector3.zero;

		}

		targetPosition = Vector3.zero;

		if (Input.GetKey (KeyCode.LeftShift)) {

			camState = CameraStates.BEHIND;

		} else {

			camState = CameraStates.TARGET;

		}

		switch (camState) {

		case CameraStates.TARGET:

			if (playerObj.GetComponent<PlayerController> ().Speed > 0) {
				
				curLookDir = Vector3.Normalize (characterOffset - this.transform.position);
				curLookDir.y = 0;

				curLookDir = Vector3.SmoothDamp (curLookDir, Vector3.zero, ref velocityLookDir, lookDirDampTime);
			}

			targetPosition = (characterOffset + follow.up * distanceUp) - (Vector3.Normalize (curLookDir) * distanceAway);

			break;

		case CameraStates.BEHIND:

			curLookDir = follow.parent.transform.forward;
			curLookDir.y = 0;

			targetPosition = (characterOffset + follow.up * distanceUp) - (Vector3.Normalize (curLookDir) * distanceAway);

			break;

		case CameraStates.FREE:


			break;
		}

		CompensateForWalls (characterOffset, ref targetPosition);

		transform.position = targetPosition;

		transform.LookAt (playerObj.transform);

		follow.rotation = Quaternion.Euler(0,rightStickPrevFrame.x,0);

	}

	void CompensateForWalls(Vector3 fromObject, ref Vector3 toTarget){

		Debug.DrawLine (fromObject - new Vector3(0,2,0), toTarget, Color.red);

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
