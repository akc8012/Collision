using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndCollider : MonoBehaviour
{
	Collider col;
	SimpleController player;
	[SerializeField] Transform min;
	[SerializeField] Transform max;

	void Start()
	{
		col = GetComponent<Collider>();
		player = GameObject.FindWithTag("Player").GetComponent<SimpleController>();
		min.position = col.bounds.min;
		max.position = col.bounds.max;

	}
	
	void Update()
	{
		CheckZ();
		CheckX();
	}

	void CheckZ()
	{
		if (player.GetX > col.bounds.min.x && player.GetMinX < col.bounds.max.x)
		{
			if (player.z < transform.position.z)
			{
				if (player.GetZ > col.bounds.min.z)
					player.Restrict(2, true, SimpleController.Direction.Forward);
				else if (player.zRestricted)
					player.Restrict(2, false, SimpleController.Direction.Forward);
			}
			else
			{
				if (player.GetMinZ < col.bounds.max.z)
					player.Restrict(2, true, SimpleController.Direction.Backward);
				else if (player.zRestricted)
					player.Restrict(2, false, SimpleController.Direction.Backward);
			}
		}
		else if (player.zRestricted) player.Restrict(2, false, SimpleController.Direction.Forward);
	}

	void CheckX()
	{
		if (player.GetZ > col.bounds.min.z && player.GetMinZ < col.bounds.max.z)
		{
			if (player.x < transform.position.x)
			{
				if (player.GetX > col.bounds.min.x)
					player.Restrict(0, true, SimpleController.Direction.Forward);
				else if (player.xRestricted)
					player.Restrict(0, false, SimpleController.Direction.Forward);
			}
			else
			{
				if (player.GetMinX < col.bounds.max.x)
					player.Restrict(0, true, SimpleController.Direction.Backward);
				else if (player.xRestricted)
					player.Restrict(0, false, SimpleController.Direction.Backward);
			}
		}
		else if (player.xRestricted) player.Restrict(0, false, SimpleController.Direction.Forward);
	}
}
