using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleController : MonoBehaviour
{
	CharacterController controller;
	[SerializeField] float speed;

	void Start()
	{
		controller = GetComponent<CharacterController>();
	}

	void Update()
	{
		Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
		//controller.Move(input * speed * Time.deltaTime);

		RaycastHit hitInfo;
		if (Physics.Raycast(new Ray(transform.position, transform.forward), out hitInfo, 1))
		{
			input.z = input.z > 0 ? 0 : input.z;
		}
		transform.position += input * speed * Time.deltaTime;
	}
}
