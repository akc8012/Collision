using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleController : MonoBehaviour
{
	Collider col;
	[SerializeField] float speed;
	Vector3 vel;
	const float gravity = -0.5f;

	void Start()
	{
		col = GetComponent<Collider>();
	}

	void Update()
	{
		float oldVelY = vel.y + (gravity * Time.deltaTime);

		vel = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
		vel = vel.normalized * speed * Time.deltaTime;

		vel.y = oldVelY;
		if (Input.GetKeyDown(KeyCode.Space)) vel.y += 12 * Time.deltaTime;

		transform.position += vel;
	}

	public void SetPos(int axis, float pos)
	{
		if (axis == 1) vel.y = 0;

		Vector3 newPos = transform.position;
		newPos[axis] = pos;
		transform.position = newPos;
	}

	public Vector3 GetExtents { get { return col.bounds.extents; } }
	public Vector3 GetMax { get { return col.bounds.max; } }
	public Vector3 GetMin { get { return col.bounds.min; } }
}
