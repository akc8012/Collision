using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle_RandomScale : MonoBehaviour {

	public Vector3 scale;

	void Start () 
	{
		scale = new Vector3 (Random.Range(0.5f, 5.5f),Random.Range(0.5f, 5.5f),Random.Range(0.5f, 5.5f));
		GetComponent<Transform> ().localScale = scale;
	}

	void Update () 
	{
		
	}
}
