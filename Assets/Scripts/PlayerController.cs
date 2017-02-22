using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	PlayerCollider col;
	Transform cam;

	Vector3 vel = Vector3.zero;
	[SerializeField] float moveSpeed = 10;		// what to increment velocity by
	[SerializeField] float maxVel = 5;			// maximum velocity in any direction
	float rotSmooth = 20;		// smoothing on the lerp to rotate towards stick direction
	[SerializeField] float gravity = 35;
	[SerializeField] float jumpSpeed = 11;
	bool jumpKeyUp = true;
	bool isGrounded = false;
	public bool IsGrounded { get { return isGrounded; } }

	public delegate void OnFloor();

	void Awake()
	{
		cam = GameObject.FindWithTag("MainCamera").transform;
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = 60;

		col = new PlayerCollider();
		col.Init(transform, onFloor);
	}

	/*void Update()
	{
		float oldVelY = vel.y + (gravity * Time.deltaTime);

		float speed = 0;
		vel = GetMoveDirection(ref speed);
		vel = vel.magnitude * vel.normalized * moveSpeed * Time.deltaTime;

		vel.y = oldVelY;
		if (Input.GetButtonDown("Jump") && IsGrounded)
			vel.y += 12 * Time.deltaTime;

		transform.position += vel;
	}*/

	void Update()
	{
		float speed = 0.0f;
		Vector3 moveDir = GetMoveDirection(ref speed);
		RotateMesh(moveDir, speed);
		speed *= moveSpeed;
		//animator.SetFloat("Speed", speed);

		float lastVelY = vel.y;
		vel = transform.forward * speed;
		vel = Vector3.ClampMagnitude(vel, maxVel);
		vel.y = lastVelY;

		if (Input.GetButton("Jump") && IsGrounded && jumpKeyUp)
		{
			//animator.SetTrigger("Jump");
			vel.y = jumpSpeed;
			jumpKeyUp = false;
		}
		if (Input.GetButtonUp("Jump")) jumpKeyUp = true;

		vel.y -= gravity * Time.deltaTime;
		transform.position += vel * Time.deltaTime;
		isGrounded = false;
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

	void RotateMesh(Vector3 moveDir, float speed)
	{
		if (Vector3.Angle(moveDir, transform.forward) > 135)        // if the difference is above a certain angle,
			transform.forward = moveDir;                            // we'll want to snap right to it, instead of lerping
		else
		{
			Vector3 targetRotation = Vector3.Lerp(transform.forward, moveDir, Time.deltaTime * rotSmooth);
			if (targetRotation != Vector3.zero) transform.rotation = Quaternion.LookRotation(targetRotation);
		}
	}

	public Vector3 GetVel { get { return vel; } }
	public PlayerCollider GetCol { get { return col; } }
}
