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
	
	void Update()
	{
		float min = transform.position.x - transform.localScale.x/2;
		float max = transform.position.x + transform.localScale.x/2;

		bool checkPos = false;

		if (inFront)
			checkPos = InFrontZ();
		else
			checkPos = BehindZ();

		if (WithinX(min, max) && checkPos)        // function pointer for InFrontZ -- BehindZ
		{
			float setZ = 0;

			if (inFront)
				setZ = InFrontPosZ();
			else
				setZ = BehindPosZ();

			player.SetZ(setZ);
		}
	}

	bool WithinX(float min, float max)
	{
		return (player.GetX > min) && (player.GetMinX < max);
	}

	bool InFrontZ()
	{
		return (player.GetZ > transform.position.z && player.GetMinZ < transform.position.z);
	}

	bool BehindZ()
	{
		return (player.GetMinZ < transform.position.z && player.GetZ > transform.position.z);
	}

	float InFrontPosZ()
	{
		return transform.position.z - player.Extents.z;
	}

	float BehindPosZ()
	{
		return transform.position.z + player.Extents.z;
	}
}
