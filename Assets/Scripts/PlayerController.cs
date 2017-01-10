using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	PlayerCollider col;
	Transform cam;

	[SerializeField] float speed;
	public Vector3 vel;
	const float gravity = -0.5f;

	public delegate void OnFloor();

	void Awake()
	{
		cam = GameObject.FindWithTag("MainCamera").transform;
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = 60;

		col = new PlayerCollider();
		col.Init(transform, onFloor);
	}

	void Update()
	{
		float oldVelY = vel.y + (gravity * Time.deltaTime);

		vel = GetMoveDirection();
		vel = vel.magnitude * vel.normalized * speed * Time.deltaTime;

		vel.y = oldVelY;
		if (Input.GetButtonDown("Jump") && vel.y == (gravity * Time.deltaTime))
			vel.y += 12 * Time.deltaTime;

		transform.position += vel;
	}

	void onFloor()
	{
		vel.y = 0;
	}

	Vector3 GetMoveDirection()
	{
		Vector3 stickDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
		Vector3 cameraDir = cam.forward; cameraDir.y = 0.0f;
		Vector3 moveDir = Quaternion.FromToRotation(Vector3.forward, cameraDir) * stickDir;

		// fixes bug when the camera forward is exactly -forward (opposite to Vector3.forward) by flipping the x around
		if (Vector3.Dot(Vector3.forward, cameraDir.normalized) == -1)
			moveDir = new Vector3(-moveDir.x, moveDir.y, moveDir.z);

		return moveDir;
	}

	public Vector3 GetVel { get { return vel; } }
	public PlayerCollider GetCol { get { return col; } }
}
