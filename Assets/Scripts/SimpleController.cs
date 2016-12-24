﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleController : MonoBehaviour
{
	Collider col;
	[SerializeField] float speed;
	Vector3 vel;
	bool[] restrictAxis = new bool[3] { false, false, false };
	public enum Direction { Forward, Backward };
	Direction[] restrictDirs = new Direction[3];

	void Start()
	{
		col = GetComponent<Collider>();
	}

	void Update()
	{
		vel = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
		#region nonsense
		/*for (int i = 0; i < restrictAxis.Length; i++)
		{
			if (restrictAxis[i])
			{
				if (restrictDirs[i] == Direction.Forward) vel[i] = vel[i] > 0 ? 0 : vel[i];
				if (restrictDirs[i] == Direction.Backward) vel[i] = vel[i] < 0 ? 0 : vel[i];
			}
		}*/
		#endregion
		transform.position += vel * speed * Time.deltaTime;
	}

	public void SetZ(float z)
	{
		Vector3 newPos = transform.position;
		newPos.z = z;
		transform.position = newPos;
	}

	public void Restrict(int axis, bool doRestriction, Direction restrictDir)
	{
		restrictAxis[axis] = doRestriction;
		this.restrictDirs[axis] = restrictDir;
	}

	public float x { get { return transform.position.x; } }
	public float z { get { return transform.position.z; } }

	public Vector3 Vel { get { return vel; } }
	public Vector3 Extents { get { return col.bounds.extents; } }

	public float GetX { get { return col.bounds.max.x; } }
	public float GetMinX { get { return col.bounds.min.x; } }
	public float GetZ { get { return col.bounds.max.z; } }
	public float GetMinZ { get { return col.bounds.min.z; } }

	public bool zRestricted { get { return restrictAxis[2]; } }
	public bool xRestricted { get { return restrictAxis[0]; } }
}
