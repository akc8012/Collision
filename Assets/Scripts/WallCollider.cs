using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCollider : MonoBehaviour
{
	PlayerCollider player;
	enum Axis { X, Y, Z };
	Axis axis;
	Quaternion lastRotation;

	[SerializeField] bool isWall = true;
	[SerializeField] float cornerSize;
	[SerializeField] float snapLeniency;
	[SerializeField] bool updateSelf = false;

	delegate bool IsWithin(int a);
	delegate bool IsPositioned(int a);
	delegate float GetPos(int a);
	IsWithin isWithin;
	IsPositioned isPositioned;
	GetPos getPos;

	void Awake()
	{
		transform.position += -transform.forward * cornerSize;

		UpdateRotation();

		if (axis == Axis.Y)
			isWithin = IsWithinY;
		else
			isWithin = IsWithinXZ;
	}

	void Start()
	{
		player = GameObject.FindWithTag("Player").GetComponent<PlayerController>().GetCol;
	}

	bool InFront
	{
		get
		{
			switch (axis)
			{
				case Axis.Z:
					return Vector3.Dot(Vector3.forward, transform.forward) > 0;
				case Axis.X:
					return Vector3.Dot(Vector3.forward, transform.right) < 0;
				case Axis.Y:
				default:
					return Vector3.Dot(Vector3.up, transform.forward) > 0;
			}
		}
	}

	Vector3 PlayerCornerPoint
	{
		get
		{
			switch (axis)
			{
				case Axis.Z:
					if (InFront) return Vector3.Dot(Vector3.forward, transform.right) > 0 ? player.GetTopLeft : player.GetTopRight;
					else return Vector3.Dot(Vector3.forward, transform.right) > 0 ? player.GetBottomLeft : player.GetBottomRight;

				case Axis.X:
					if (InFront) return Vector3.Dot(Vector3.forward, transform.forward) > 0 ? player.GetTopRight : player.GetBottomRight;
					else return Vector3.Dot(Vector3.forward, transform.forward) > 0 ? player.GetTopLeft : player.GetBottomLeft;

				case Axis.Y:
				default:
					if (InFront) return Vector3.Dot(Vector3.up, transform.up) > 0 ? player.GetMax : player.GetMax;  // meh, good enough
					else return Vector3.Dot(Vector3.up, transform.up) > 0 ? player.GetTopRight : player.GetBottomLeft;
			}
		}
	}

	void UpdateRotation()
	{
		if (isWall)
		{
			float angle = Vector3.Angle(Vector3.forward, transform.forward);
			if (angle < 45 || angle > 135)
				axis = Axis.Z;
			else
				axis = Axis.X;
		} else axis = Axis.Y;

		if (InFront) {
			isPositioned = IsInFront;
			getPos = GetInFrontPos;
		} else {
			isPositioned = IsBehind;
			getPos = GetBehindPos;
		}
	}

	void LateUpdate()  // must set player position on a late update -- 
					   // this way it happens AFTER their input goes through
	{
		if (updateSelf) CustomLateUpdate();
	}

	public void CustomLateUpdate()
	{
		if (transform.rotation != lastRotation) UpdateRotation();
		lastRotation = transform.rotation;

		if (isWithin(Mathf.Abs((int)axis - 2)) && isPositioned((int)axis))
		{
			player.SetPos((int)axis, getPos((int)axis));
		}
	}

	void GetMinMax(int a, out float min, out float max)
	{
		min = transform.position[a] - (transform.lossyScale.x/2 * transform.right[a]);
		max = transform.position[a] + (transform.lossyScale.x/2 * transform.right[a]);
		if (max < min)
		{
			float temp = min;
			min = max;
			max = temp;
		}
	}

	Vector3 ProjectToPlane(Vector3 aPoint)
	{
		Vector3 V1 = aPoint - transform.position;
		Vector3 V2 = Vector3.ProjectOnPlane(V1, transform.forward);
		Vector3 point = transform.position + V2;

		float min, max;
		GetMinMax(0, out min, out max);
		point.x = Mathf.Clamp(point.x, min, max);

		GetMinMax(2, out min, out max);
		point.z = Mathf.Clamp(point.z, min, max);

		return point;
	}

	bool IsWithinXZ(int a)
	{
		float min, max;
		GetMinMax(a, out min, out max);
		bool withinSentA = (player.GetMax[a] > min) && (player.GetMin[a] < max);

		min = transform.position.y - transform.lossyScale.y/2;
		max = transform.position.y + transform.lossyScale.y/2;
		bool withinY = (player.GetMax.y > min) && (player.GetMin.y < max);

		return withinSentA && withinY;
	}

	bool IsWithinY(int a)
	{
		float weirdShit = 1+Mathf.Abs(transform.right.x-1);
		float min = transform.position.x - (transform.lossyScale.x/2 * weirdShit);
		float max = transform.position.x + (transform.lossyScale.x/2 * weirdShit);
		bool xWithin = (player.GetMax.x > min) && (player.GetMin.x < max);

		min = transform.position.z - (transform.lossyScale.y/2 * weirdShit);
		max = transform.position.z + (transform.lossyScale.y/2 * weirdShit);

		bool zWithin = (player.GetMax.z > min) && (player.GetMin.z < max);

		return xWithin && zWithin;
	}

	bool IsInFront(int a)
	{
		Vector3 pos = ProjectToPlane(PlayerCornerPoint);
		return (player.GetMax[a] > pos[a] && (player.GetMax[a]-snapLeniency < pos[a]) && player.GetMin[a] < pos[a]);
	}

	bool IsBehind(int a)
	{
		Vector3 pos = ProjectToPlane(PlayerCornerPoint);
		return (player.GetMin[a] < pos[a] && (player.GetMin[a]+snapLeniency > pos[a]) && player.GetMax[a] > pos[a]);
	}

	float GetInFrontPos(int a)
	{
		Vector3 pos = ProjectToPlane(PlayerCornerPoint);
		return pos[a] - player.GetExtents[a];
	}

	float GetBehindPos(int a)
	{
		Vector3 pos = ProjectToPlane(PlayerCornerPoint);
		return pos[a] + player.GetExtents[a];
	}
}
