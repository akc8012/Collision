using UnityEngine;
using System.Collections;

// Placement:  Place on objects that need to follow the player precisely

// Purpose:  Any object that has this script will move precisely along with the player object
namespace TristanBall
{
public class MaintainDistance_Player : MonoBehaviour {

	GameObject 
		player;

	Vector3 
		offset;

	void Start () 
	{
		player = GameObject.FindWithTag ("PlayerBall");
		offset = transform.localPosition - player.transform.localPosition;
	}

	// runs every frame, but after all items have been processed.
	void LateUpdate () 
	{
		transform.localPosition = player.transform.localPosition + offset;
	}
}
}