using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCollider : MonoBehaviour
{
	SimpleController player;
	[SerializeField] bool inFront;
	enum Axis { X, Y, Z };
	[SerializeField] Axis axis;
	[SerializeField] float cornerSize;

	delegate bool IsWithin(int a);
	delegate bool IsPositioned(int a);
	delegate float GetPos(int a);
	IsWithin isWithin;
	IsPositioned isPositioned;
	GetPos getPos;

	void Start()
	{
		player = GameObject.FindWithTag("Player").GetComponent<SimpleController>();
		transform.position += -transform.forward * cornerSize;

		if (axis == Axis.Y)
			isWithin = IsWithinY;
		else
			isWithin = IsWithinXZ;

		if (inFront) {
			isPositioned = IsInFront;
			getPos = GetInFrontPos;
		} else {
			isPositioned = IsBehind;
			getPos = GetBehindPos;
		}
	}
	
	void LateUpdate()	// must set player position on a late update -- 
						// this way it happens AFTER their input goes through
	{
		if (isWithin(Mathf.Abs((int)axis-2)) && isPositioned((int)axis))
		{
			player.SetPos((int)axis, getPos((int)axis));
		}
	}

	bool IsWithinXZ(int a)
	{
		float min = transform.position[a] - transform.lossyScale.x/2;
		float max = transform.position[a] + transform.lossyScale.x/2;
		bool withinSentA = (player.GetMax[a] > min) && (player.GetMin[a] < max);

		min = transform.position.y - transform.lossyScale.y/2;
		max = transform.position.y + transform.lossyScale.y/2;
		bool withinY = (player.GetMax.y > min) && (player.GetMin.y < max);

		return withinSentA && withinY;
	}

	bool IsWithinY(int a)
	{
		float min = transform.position.x - transform.lossyScale.x/2;
		float max = transform.position.x + transform.lossyScale.x/2;
		bool xWithin = (player.GetMax.x > min) && (player.GetMin.x < max);

		min = transform.position.z - transform.lossyScale.x/2;
		max = transform.position.z + transform.lossyScale.x/2;
		bool zWithin = (player.GetMax.z > min) && (player.GetMin.z < max);

		return xWithin && zWithin;
	}

	bool IsInFront(int a)
	{
		return (player.GetMax[a] > transform.position[a] && player.GetMin[a] < transform.position[a]);
	}

	bool IsBehind(int a)
	{
		return (player.GetMin[a] < transform.position[a] && player.GetMax[a] > transform.position[a]);
	}

	float GetInFrontPos(int a)
	{
		return transform.position[a] - player.GetExtents[a];
	}

	float GetBehindPos(int a)
	{
		return transform.position[a] + player.GetExtents[a];
	}
}
