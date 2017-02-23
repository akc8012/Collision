using UnityEngine;
using System.Collections;

// Placement:  Temp cube prototype for "player" that turns into a ball

// Purpose:  Allows for a temporary test object to spawn the actual player ball

namespace TristanBall
{
public class Temp_ChangeToBall : MonoBehaviour {

	public GameObject playerPrefab;

	bool startTimer = false;

	float timer = 0.6f;

	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {
			startTimer = true;
			GetComponent<Rigidbody> ().mass = 0.01f;
			GetComponent<Rigidbody> ().AddForce (new Vector3 (0, 2, 0));
		}

		if (startTimer) {
			timer -= Time.deltaTime;
		}

		if (timer <= 0) {
			startTimer = false;
			Destroy (this.gameObject);
			Instantiate (playerPrefab, transform.position, Quaternion.identity);
		}
	}
}
}