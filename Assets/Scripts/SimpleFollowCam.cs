using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFollowCam : MonoBehaviour
{
	Transform player;
	[SerializeField] float zoffset;
	[SerializeField] float yoffset;
	[SerializeField] float followSpeed;

	void Awake()
	{
		player = GameObject.FindWithTag("Player").transform;
	}
	
	void LateUpdate()
	{
		Vector3 newPos = player.position + (Vector3.back * zoffset) + (Vector3.up * yoffset);
		transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * followSpeed);
	}
}
