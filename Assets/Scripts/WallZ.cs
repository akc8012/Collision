using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallZ : MonoBehaviour
{
	SimpleController player;
	[SerializeField] bool inFront;

	void Start()
	{
		player = GameObject.FindWithTag("Player").GetComponent<SimpleController>();
		Vector3 meep = transform.position;
		meep.x = transform.position.x + transform.localScale.x/2;
		GameObject.Find("Dude").transform.position = meep;
	}
	
	void LateUpdate()	// must set player position on a late update -- 
						// this way it happens AFTER their input goes through
	{
		bool checkPos = false;

		if (inFront)
			checkPos = IsInFrontZ();
		else
			checkPos = IsBehindZ();

		if (WithinX() && checkPos)        // function pointer for InFrontZ -- BehindZ
		{
			float setZ = 0;

			if (inFront)
				setZ = GetInFrontPosZ();
			else
				setZ = GetBehindPosZ();

			player.SetZ(setZ);
		}
	}

	bool WithinX()
	{
		float min = transform.position.x - transform.localScale.x / 2;
		float max = transform.position.x + transform.localScale.x / 2;
		return (player.GetMax.x > min) && (player.GetMin.x < max);
	}

	bool IsInFrontZ()
	{
		return (player.GetMax.z > transform.position.z && player.GetMin.z < transform.position.z);
	}

	bool IsBehindZ()
	{
		return (player.GetMin.z < transform.position.z && player.GetMax.z > transform.position.z);
	}

	float GetInFrontPosZ()
	{
		return transform.position.z - player.GetExtents.z;
	}

	float GetBehindPosZ()
	{
		return transform.position.z + player.GetExtents.z;
	}
}
