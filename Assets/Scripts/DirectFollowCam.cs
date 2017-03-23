using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectFollowCam : MonoBehaviour
{
	Transform playerBall;
	Vector3 lastPos;

	void Start()
	{
		playerBall = GameObject.Find("PlayerBall").transform;
	}

	public void ReInit(Vector3 pos, Quaternion rot)
	{
		lastPos = playerBall.position;
		transform.position = pos;
		transform.rotation = rot;
	}

	void LateUpdate()
	{
		Vector3 dist = playerBall.position - lastPos;
		transform.position += dist;
		lastPos = playerBall.position;
	}
}
