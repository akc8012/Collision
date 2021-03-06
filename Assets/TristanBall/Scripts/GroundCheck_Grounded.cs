﻿using UnityEngine;
using System.Collections;

// Placement:  Place on 'GroundCheck' gameobject

// Purpose:  Tells the player if it is grounded or not, and modifies movement
// 				based on slope angle.

namespace TristanBall
{
public class GroundCheck_Grounded : MonoBehaviour {

	GameObject 
		player;

	void Start () 
	{
		player = GameObject.FindWithTag ("PlayerBall");
	}

	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag == "Ground" || col.gameObject.tag == "Ramp") {
			player.GetComponent<PlayerController> ().IsJumping (false);
		}
	}

	void OnTriggerStay(Collider col)
	{
		if (col.gameObject.tag == "Ground" || col.gameObject.tag == "Ramp") {
			player.GetComponent<PlayerController> ().IsGrounded (true);
		}

		if(col.gameObject.tag == "Ramp"){
			if (col.GetComponent<Terrain_ManageableTerrain>().ManageableTerrain == false) {
				player.GetComponent<PlayerController> ().ToggleControls (false);
			}
		}
	}

	void OnTriggerExit(Collider col)
	{
		player.GetComponent<PlayerController> ().ToggleControls (true);

		if (col.gameObject.tag == "Ground" || col.gameObject.tag == "Ramp") {
			player.GetComponent<PlayerController> ().IsGrounded (false);
		}
	}
}
}