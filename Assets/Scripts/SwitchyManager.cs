using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchyManager : MonoBehaviour
{
	GameObject player;
	GameObject playerBall;
	GameObject mainCam;
	GameObject ballCam;
	int state = 0;
	float lastAxis;

	void Start()
	{
		player = GameObject.FindWithTag("Player");
		playerBall = GameObject.FindWithTag("PlayerBall");
		mainCam = GameObject.Find("Player Camera2");
		ballCam = GameObject.Find("BallCam");

		Switch(state);
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.P))
			Switch(state == 0 ? 1 : 0);
		
		float currentAxis = Input.GetAxisRaw("BallToggle");

		if (currentAxis != lastAxis && (currentAxis == 0 || currentAxis == -1))
			Switch(state == 0 ? 1 : 0);

		lastAxis = currentAxis;
	}

	void Switch(int which)
	{
		state = which;
		if (which == 0)
		{
			Vector3 pos = playerBall.transform.position;
			pos.y += 0.5f;
			player.transform.position = pos;
			player.SetActive(true);
			playerBall.SetActive(false);
			mainCam.SetActive(true);
			ballCam.SetActive(false);
		}
		else
		{
			playerBall.transform.position = player.transform.position;
			player.SetActive(false);
			playerBall.SetActive(true);
			mainCam.SetActive(false);
			ballCam.SetActive(true);
			ballCam.GetComponent<DirectFollowCam>().ReInit(mainCam.transform.position, mainCam.transform.rotation);
		}
	}
}
