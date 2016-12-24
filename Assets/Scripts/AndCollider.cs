using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndCollider : MonoBehaviour
{
	Collider col;
	SimpleController player;

	void Start()
	{
		col = GetComponent<Collider>();
		player = GameObject.FindWithTag("Player").GetComponent<SimpleController>();
	}
	
	void Update()
	{
		if (player.GetX > col.bounds.min.x && player.GetMinX < col.bounds.max.x)
		{
			if (player.z < transform.position.z)
			{
				if (player.GetZ > col.bounds.min.z)
					player.RestrictZ(true, SimpleController.Direction.Forward);
				else
					player.RestrictZ(false, SimpleController.Direction.Forward);
			}
			else
			{
				if (player.GetMinZ < col.bounds.max.z)
					player.RestrictZ(true, SimpleController.Direction.Backward);
				else
					player.RestrictZ(false, SimpleController.Direction.Backward);
			}
		}
		else player.RestrictZ(false, SimpleController.Direction.Forward);
	}
}
