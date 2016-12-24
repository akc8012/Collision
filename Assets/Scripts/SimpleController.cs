using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleController : MonoBehaviour
{
	Collider col;
	[SerializeField] float speed;
	Vector3 vel;

	void Start()
	{
		col = GetComponent<Collider>();
	}

	void Update()
	{
		vel = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
		vel = vel.normalized * speed * Time.deltaTime;
		transform.position += vel;
	}

	public void SetPos(int axis, float pos)
	{
		Vector3 newPos = transform.position;
		newPos[axis] = pos;
		transform.position = newPos;
	}

	public Vector3 GetExtents { get { return col.bounds.extents; } }
	public Vector3 GetMax { get { return col.bounds.max; } }
	public Vector3 GetMin { get { return col.bounds.min; } }
}
