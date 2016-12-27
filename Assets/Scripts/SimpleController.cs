using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleController : MonoBehaviour
{
	[SerializeField] float speed;
	Vector3 vel;
	const float gravity = -0.5f;

	void Start()
	{
		
	}

	void Update()
	{
		float oldVelY = vel.y + (gravity * Time.deltaTime);

		vel = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
		vel = vel.magnitude * vel.normalized * speed * Time.deltaTime;

		vel.y = oldVelY;
		if (Input.GetButtonDown("Jump") && vel.y == (gravity * Time.deltaTime))
			vel.y += 12 * Time.deltaTime;

		transform.position += vel;
	}

	public void SetPos(int axis, float pos)
	{
		if (axis == 1) vel.y = 0;

		Vector3 newPos = transform.position;
		newPos[axis] = pos;
		transform.position = newPos;
	}

	public Vector3 GetExtents { get { return transform.lossyScale/2; } }
	public Vector3 GetMax { get { return transform.position + GetExtents; } }
	public Vector3 GetMin { get { return transform.position - GetExtents; } }
}
