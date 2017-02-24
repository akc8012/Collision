using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle_SpecialRotating : MonoBehaviour {

	public Vector3 RotateSpeed = new Vector3(0,10,0);

	void Start () {
		
	}

	void Update () {
		transform.Rotate (RotateSpeed);
	}
}
