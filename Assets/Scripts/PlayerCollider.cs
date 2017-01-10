using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider
{
	Transform transform;
	PlayerController.OnFloor onFloor;

	public void Init(Transform transform, PlayerController.OnFloor onFloor)
	{
		this.transform = transform;
		this.onFloor = onFloor;
	}

	public void SetPos(int axis, float pos)
	{
		if (axis == 1) onFloor();

		Vector3 newPos = transform.position;
		newPos[axis] = pos;
		transform.position = newPos;
	}

	public Vector3 GetExtents { get { return transform.lossyScale / 2; } }

	public Vector3 GetMax { get { return transform.position + GetExtents; } }   // top right
	public Vector3 GetMin { get { return transform.position - GetExtents; } }   // bottom left

	public Vector3 GetBottomLeft { get { return GetMin; } }
	public Vector3 GetTopRight { get { return new Vector3(transform.position.x + GetExtents.x, transform.position.y - GetExtents.y, transform.position.z + GetExtents.z); } }
	public Vector3 GetTopLeft { get { return new Vector3(transform.position.x - GetExtents.x, transform.position.y - GetExtents.y, transform.position.z + GetExtents.z); } }
	public Vector3 GetBottomRight { get { return new Vector3(transform.position.x + GetExtents.x, transform.position.y - GetExtents.y, transform.position.z - GetExtents.z); } }

}