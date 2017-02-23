using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchyManager : MonoBehaviour
{
	GameObject player;
	GameObject playerBall;
	SimpleFollowCam mainCam;
	TristanBall.CameraDistanceToPlayer ballCam;
	int state = 0;
	float lastAxis;

	void Start()
	{
		player = GameObject.FindWithTag("Player");
		playerBall = GameObject.FindWithTag("PlayerBall");
		mainCam = Camera.main.gameObject.GetComponent<SimpleFollowCam>();
		ballCam = Camera.main.gameObject.GetComponent<TristanBall.CameraDistanceToPlayer>();

		Switch(state);
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.P))
			Switch(state == 0 ? 1 : 0);
		
		float currentAxis = Input.GetAxisRaw("BallToggle");

		if (currentAxis != lastAxis)
			Switch(state == 0 ? 1 : 0);

		lastAxis = currentAxis;
	}

	void Switch(int which)
	{
		state = which;
		if (which == 0)
		{
			player.transform.position = playerBall.transform.position;
			player.SetActive(true);
			playerBall.SetActive(false);
			mainCam.enabled = true;
			ballCam.enabled = false;
		}
		else
		{
			playerBall.transform.position = player.transform.position;
			player.SetActive(false);
			playerBall.SetActive(true);
			mainCam.enabled = false;
			ballCam.enabled = true;
		}
	}
}