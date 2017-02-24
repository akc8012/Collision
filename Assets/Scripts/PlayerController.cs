using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
	Text displayText;

	PlayerCollider col;
	PlayerTerrainCollider terrainCol;
	Transform cam;
	[SerializeField] Transform rotateMesh;
	[SerializeField] Animator animator;
	[SerializeField] Transform bottom;

	Vector3 vel = Vector3.zero;
	[SerializeField] float maxSpeed = 10;		// what to increment velocity by
	float maxVel = 5;			// maximum velocity in any direction
	float rotSmooth = 20;		// smoothing on the lerp to rotate towards stick direction
	float rotSmoothSlow = 5;       // smoothing on the lerp to rotate towards stick direction
	[SerializeField] float gravity = 35;
	[SerializeField] float jumpSpeed = 10.5f;
	[SerializeField] bool doAnimations = true;

	float lastSpeed = 0;
	float acceleration = 0.2f;
	float deceleration = 1.5f;
	float speedJumpedAt;

	const float jumpDetraction = 0.25f;
	const float fallDownFast = 0.90f;
	float currJumpSpeed;
	bool isGrounded = false;
	public bool IsGrounded { get { return isGrounded; } }
	public bool IsRising { get { return vel.y > 0; } }
	bool collisionActive = true;
	public bool CollisionActive { get { return collisionActive; } }

	public delegate void OnFloor();

	void Awake()
	{
		cam = GameObject.FindWithTag("MainCamera").transform;
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = 60;

		col = new PlayerCollider();
		col.Init(transform, onFloor);
		currJumpSpeed = jumpSpeed;

		terrainCol = new PlayerTerrainCollider();
		terrainCol.Init(transform, bottom);

		if (GameObject.Find("Text"))
			displayText = GameObject.Find("Text").GetComponent<Text>();
	}

	void Start()
	{
		DownRay();
	}

	void Update()
	{
		if (Time.deltaTime > 0.1f)
		{
			//print("player skip this frame");
			return;
		}

		if (!CollisionActive) return;

		float speed = 0;
		Vector3 moveDir = GetMoveDirection(ref speed);
		
		if (speed > 0.19f)		// greater than deadzone
			SpeedUp(ref speed);
		else
			SlowDown(ref speed);

		RotateMesh(moveDir);
		if (doAnimations) animator.SetFloat("Speed", speed);

		float lastVelY = vel.y;
		vel = rotateMesh.forward * speed;
		vel = Vector3.ClampMagnitude(vel, maxVel);
		vel.y = lastVelY;

		if (Input.GetButtonDown("Jump") && IsGrounded)
		{
			if (doAnimations) animator.SetTrigger("Jump");
			speedJumpedAt = speed;
			StartCoroutine("Jump");
		}

		if ((!Input.GetButton("Jump") && !isGrounded) || !IsRising)
		{
			if (IsRising)      // set vel to fall down faster
				vel.y *= fallDownFast;

			StopCoroutine("Jump");
			currJumpSpeed = jumpSpeed;
		}

		vel.y -= gravity * Time.deltaTime;
		transform.position += vel * Time.deltaTime;
		isGrounded = false;
		lastSpeed = speed;

		if (!IsRising && DownRay())
			SnapToTerrainFloor();

		if (displayText)
			displayText.text = speed+"";
	}

	IEnumerator Jump()
	{
		float valueBasedOnRunningJumpSpeed = Mathf.Clamp((speedJumpedAt / maxSpeed) + 0.4f, 0.9f, 1.14f);
		while (true)
		{
			vel.y += currJumpSpeed * valueBasedOnRunningJumpSpeed;
			currJumpSpeed *= jumpDetraction;
			yield return null;
		}
	}

	void onFloor()
	{
		vel.y = 0;
		isGrounded = true;
	}

	Vector3 GetMoveDirection(ref float speed)
	{
		Vector3 stickDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
		speed = Mathf.Clamp(Vector3.Magnitude(stickDir), 0, 1);

		Vector3 cameraDir = cam.forward; cameraDir.y = 0.0f;
		Vector3 moveDir = Quaternion.FromToRotation(Vector3.forward, cameraDir) * stickDir;		// referential shift

		// fixes bug when the camera forward is exactly -forward (opposite to Vector3.forward) by flipping the x around
		if (Vector3.Dot(Vector3.forward, cameraDir.normalized) == -1)
			moveDir = new Vector3(-moveDir.x, moveDir.y, moveDir.z);

		return moveDir;
	}

	void SpeedUp(ref float speed)
	{
		float speedClamp = speed;
		float airClamp = (IsGrounded ? 1 : (speedJumpedAt / maxSpeed) + 0.2f);
		speed = lastSpeed + acceleration;
		speed = Mathf.Clamp(speed, 0, maxSpeed * speedClamp * airClamp);
	}

	void SlowDown(ref float speed)
	{
		speed = lastSpeed - deceleration;
		speed = Mathf.Clamp(speed, 0, maxSpeed);
	}

	void RotateMesh(Vector3 moveDir)
	{
		float angle = Vector3.Angle(moveDir, rotateMesh.forward);

		//if (angle > 15 && angle != 90)
		//	return;

		if (angle > 135)        // if we're a very big angle change, we'll want to snap right to it, instead of lerping
			rotateMesh.forward = moveDir;
		else
		{
			Vector3 targetRotation = Vector3.Lerp(rotateMesh.forward, moveDir,
				Time.deltaTime * (IsGrounded ? rotSmooth : rotSmoothSlow));
			if (targetRotation != Vector3.zero)
				rotateMesh.rotation = Quaternion.LookRotation(targetRotation);
		}
	}

	public Vector3 GetVel { get { return vel; } }
	public PlayerCollider GetCol { get { return col; } }

	bool DownRay()
	{
		RaycastHit hitMan;
		Ray rayMan = new Ray(bottom.position, Vector3.down);
		if (Physics.Raycast(rayMan, out hitMan, 20))
		{
			Terrain foundTerrain = hitMan.collider.gameObject.GetComponent<Terrain>();

			if (foundTerrain && foundTerrain != terrainCol.CurrentTerrain)
			{
				terrainCol.SetTerrain(foundTerrain);
			}
		}

		return terrainCol.CurrentTerrain && terrainCol.IsAcceptableDistance;
	}

	void SnapToTerrainFloor()
	{
		terrainCol.CustomUpdate();
		onFloor();
	}

	void OnEnable()
	{
		
	}

	void OnDisable()
	{
		vel = Vector3.zero;
	}

	public void SetVelFromBall(Vector3 ballVel)
	{
		vel = ballVel;
	}
}
