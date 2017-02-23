using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainMan : MonoBehaviour
{
	public Terrain terrain;
	[SerializeField] Transform bottom;

	void Start()
	{

	}

	void Update()
	{
		RaycastHit hitMan;
		Ray rayMan = new Ray(bottom.position, Vector3.down);
		if (Physics.Raycast(rayMan, out hitMan))
		{
			Terrain foundTerrain = hitMan.collider.gameObject.GetComponent<Terrain>();

			if (foundTerrain && foundTerrain != terrain)
			{
				print("set terrain");
				terrain = hitMan.collider.gameObject.GetComponent<Terrain>();
			}
		}
		
		if (terrain)
		{
			Vector3 target = bottom.position;
			target.y = terrain.SampleHeight(transform.position);

			Vector3 distance = target - bottom.position;
			transform.position += distance;
		}
	}
}
