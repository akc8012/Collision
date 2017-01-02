using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCollider : MonoBehaviour
{
	SimpleController player;
	enum Axis { X, Y, Z };
	Axis axis;
	Quaternion lastRotation;
	[SerializeField] bool isWall = true;
	[SerializeField] float cornerSize;
	[SerializeField] bool updateSelf = false;

	delegate bool IsWithin(int a);
	delegate bool IsPositioned(int a);
	delegate float GetPos(int a);
	IsWithin isWithin;
	IsPositioned isPositioned;
	GetPos getPos;

	public Transform min;
	public Transform max;

	bool InFront
	{
		get
		{
			if (axis == Axis.Z)
				return Vector3.Dot(Vector3.forward, transform.forward) > 0;
			else	//if (axis == Axis.X)
				return Vector3.Dot(Vector3.forward, transform.right) < 0;
		}
	}

	Vector3 PlayerCornerPoint
	{
		get
		{
			if (axis == Axis.Z)
			{
				if (InFront) return Vector3.Dot(Vector3.forward, transform.right) > 0 ? player.GetTopLeft : player.GetTopRight;
				else return Vector3.Dot(Vector3.forward, transform.right) > 0 ? player.GetBottomLeft : player.GetBottomRight;
			}
			else// if (axis == Axis.X)
			{
				if (InFront) return Vector3.Dot(Vector3.forward, transform.forward) > 0 ? player.GetTopRight : player.GetBottomRight;
				else return Vector3.Dot(Vector3.forward, transform.forward) > 0 ? player.GetTopLeft : player.GetBottomLeft;
			}
		}
	}

	void Start()
	{
		player = GameObject.FindWithTag("Player").GetComponent<SimpleController>();
		transform.position += -transform.forward * cornerSize;

		UpdateRotation();

		if (axis == Axis.Y)
			isWithin = IsWithinY;
		else
			isWithin = IsWithinXZ;
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
		if (min && max)
		{
			Vector3 minVec = transform.position;
			Vector3 maxVec = transform.position;
			minVec.x -= transform.lossyScale.x / 2 * transform.right.x;
			minVec.z -= transform.lossyScale.x / 2 * transform.right.z;
			maxVec.x += transform.lossyScale.x / 2 * transform.right.x;
			maxVec.z += transform.lossyScale.x / 2 * transform.right.z;

			min.position = player.GetMin;
			max.position = player.GetMax;
		}

		if (transform.rotation != lastRotation) UpdateRotation();
		lastRotation = transform.rotation;

		if (isWithin(Mathf.Abs((int)axis - 2)) && isPositioned((int)axis))
		{
			player.SetPos((int)axis, getPos((int)axis));
		}
	}

	Vector3 ProjectToPlane(Vector3 aPoint)
	{
		Vector3 V1 = aPoint - transform.position;
		Vector3 V2 = Vector3.ProjectOnPlane(V1, transform.forward);
		return transform.position + V2;
	}

	bool IsWithinXZ(int a)
	{
		float min = transform.position[a];
		float max = transform.position[a];
		min -= transform.lossyScale.x/2 * transform.right[a];
		max += transform.lossyScale.x/2 * transform.right[a];
		if (max < min)
		{
			float temp = min;
			min = max;
			max = temp;
		}
		bool withinSentA = (player.GetMax[a] > min) && (player.GetMin[a] < max);

		min = transform.position.y - transform.lossyScale.y/2;
		max = transform.position.y + transform.lossyScale.y/2;
		bool withinY = (player.GetMax.y > min) && (player.GetMin.y < max);

		print(withinSentA);

		return withinSentA && withinY;
	}

	bool IsWithinY(int a)
	{
		float min = transform.position.x - transform.lossyScale.x/2;
		float max = transform.position.x + transform.lossyScale.x/2;
		bool xWithin = (player.GetMax.x > min) && (player.GetMin.x < max);

		min = transform.position.z - transform.lossyScale.y/2;
		max = transform.position.z + transform.lossyScale.y/2;
		bool zWithin = (player.GetMax.z > min) && (player.GetMin.z < max);

		return xWithin && zWithin;
	}

	bool IsInFront(int a)
	{
		Vector3 pos = ProjectToPlane(PlayerCornerPoint);
		return (player.GetMax[a] > pos[a] && player.GetMin[a] < pos[a]);
	}

	bool IsBehind(int a)
	{
		Vector3 pos = ProjectToPlane(PlayerCornerPoint);
		return (player.GetMin[a] < pos[a] && player.GetMax[a] > pos[a]);
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
