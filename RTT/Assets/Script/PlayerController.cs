using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float moveSpeed = 10;

	Rigidbody rigidbody;
	public Camera viewCamera;
	Vector3 velocity;
	Vector3 mousePos;
	private Vector3 input;
	void Start()
	{
		rigidbody = GetComponent<Rigidbody>();
		viewCamera = Camera.main;
	}

	void Update()
	{
		mousePos = viewCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, viewCamera.transform.position.y));
		transform.LookAt(mousePos + Vector3.up * transform.position.y);

		var rawInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
		input = rawInput.normalized;

		var matrix = Matrix4x4.Rotate(Quaternion.Euler(0.0f, 45.0f, 0.0f));

		var modifiedInput = matrix.MultiplyPoint3x4(input);

		var relativeV = (transform.position + modifiedInput) - transform.position;

		velocity = relativeV.normalized * moveSpeed;
	}

	void FixedUpdate()
	{
		rigidbody.MovePosition(rigidbody.position + velocity * Time.fixedDeltaTime);
	}
}
