using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTerrainCollider
{
	[SerializeField] Terrain terrain;
	public Terrain CurrentTerrain { get { return terrain; } }
	Transform transform;
	Transform bottom;
	const float acceptableDistance = 0.3f;

	public float DistanceFromBottom
		{ get { return (terrain) ? Mathf.Abs(terrain.SampleHeight(bottom.position) - bottom.position.y) : float.MaxValue; } }

	public bool IsAcceptableDistance { get { return DistanceFromBottom < acceptableDistance; } }

	public void Init(Transform transform, Transform bottom)
	{
		this.transform = transform;
		this.bottom = bottom;
	}

	public void SetTerrain(Terrain newTerrain)
	{
		terrain = newTerrain;
	}

	public void CustomUpdate()
	{
		Vector3 target = bottom.position;
		target.y = terrain.SampleHeight(transform.position);

		Vector3 distance = target - bottom.position;
		transform.position += distance;
	}
}
