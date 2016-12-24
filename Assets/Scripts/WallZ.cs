using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallZ : MonoBehaviour
{
	SimpleController player;
	[SerializeField] bool inFront;
	enum Axis { X, Y, Z };
	[SerializeField] Axis axis;
	[SerializeField] float cornerSize;

	delegate bool IsPositioned(int a);
	delegate float GetPos(int a);
	IsPositioned isPositioned;
	GetPos getPos;

	void Start()
	{
		player = GameObject.FindWithTag("Player").GetComponent<SimpleController>();
		transform.position += -transform.forward * cornerSize;

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
		if (IsWithin(Mathf.Abs((int)axis-2)) && isPositioned((int)axis))
		{
			player.SetPos((int)axis, getPos((int)axis));
		}
	}

	bool IsWithin(int a)
	{
		float min = transform.position[a] - transform.lossyScale.x / 2;
		float max = transform.position[a] + transform.lossyScale.x / 2;
		return (player.GetMax[a] > min) && (player.GetMin[a] < max);
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
