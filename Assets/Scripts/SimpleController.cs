using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleController : MonoBehaviour
{
	Collider col;
	[SerializeField] float speed;
	Vector3 vel;
	bool[] restrictAxis = new bool[3] { false, false, false };
	public enum Direction { Forward, Backward };
	Direction restrictDir;

	void Start()
	{
		col = GetComponent<Collider>();
	}

	void Update()
	{
		vel = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

		for (int i = 0; i < restrictAxis.Length; i++)
		{
			if (restrictAxis[i])
			{
				if (restrictDir == Direction.Forward) vel[i] = vel[i] > 0 ? 0 : vel[i];
				if (restrictDir == Direction.Backward) vel[i] = vel[i] < 0 ? 0 : vel[i];
			}
		}

		transform.position += vel * speed * Time.deltaTime;
	}

	public void RestrictX(bool doRestriction)
	{
		restrictAxis[0] = doRestriction;
	}

	public void RestrictZ(bool doRestriction, Direction restrictDir)
	{
		restrictAxis[2] = doRestriction;
		this.restrictDir = restrictDir;
	}

	public float x { get { return transform.position.x; } }
	public float z { get { return transform.position.z; } }

	public float GetX { get { return col.bounds.max.x + (speed * Time.deltaTime); } }
	public float GetMinX { get { return col.bounds.min.x - (speed * Time.deltaTime); } }
	public float GetZ { get { return col.bounds.max.z + (speed * Time.deltaTime); } }
	public float GetMinZ { get { return col.bounds.min.z - (speed * Time.deltaTime); } }
}
