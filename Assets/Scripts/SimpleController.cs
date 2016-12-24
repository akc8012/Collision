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
		vel *= (speed * Time.deltaTime);
		transform.position += vel;
	}

	public void SetZ(float z)
	{
		Vector3 newPos = transform.position;
		newPos.z = z;
		transform.position = newPos;
	}

	public Vector3 GetExtents { get { return col.bounds.extents; } }
	public Vector3 GetMax { get { return col.bounds.max; } }
	public Vector3 GetMin { get { return col.bounds.min; } }
}
