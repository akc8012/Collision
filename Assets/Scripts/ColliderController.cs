﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderController : MonoBehaviour
{
	[SerializeField] bool doRotation = false;

	WallCollider frontWall;
	WallCollider backWall;
	WallCollider leftWall;
	WallCollider rightWall;
	WallCollider topWall;

	void Awake()
	{
		frontWall = transform.Find("Front Wall").GetComponent<WallCollider>();
		backWall = transform.Find("Back Wall").GetComponent<WallCollider>();
		leftWall = transform.Find("Left Wall").GetComponent<WallCollider>();
		rightWall = transform.Find("Right Wall").GetComponent<WallCollider>();
		topWall = transform.Find("Top Wall").GetComponent<WallCollider>();
	}

	void Update()
	{
		if (doRotation)
			transform.Rotate(transform.up);
	}
	
	void LateUpdate()
	{
		topWall.CustomLateUpdate();
		frontWall.CustomLateUpdate();
		backWall.CustomLateUpdate();
		leftWall.CustomLateUpdate();
		rightWall.CustomLateUpdate();
	}
}
