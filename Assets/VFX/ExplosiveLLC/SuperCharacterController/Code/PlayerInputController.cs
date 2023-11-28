﻿using UnityEngine;

public class PlayerInputController:MonoBehaviour
{
	public PlayerInputData Current;
	public Vector2 RightStickMultiplier = new Vector2(3, -1.5f);

	private void Start()
	{ Current = new PlayerInputData(); }

	private void Update()
	{
		// Retrieve our current WASD or Arrow Key input.
		// Using GetAxisRaw removes any kind of gravity or filtering being applied to the input
		// Ensuring that we are getting either -1, 0 or 1.
		Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

		Vector2 mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

		Vector2 rightStickInput = new Vector2(Input.GetAxisRaw("FacingHorizontal"), Input.GetAxisRaw("FacingVertical"));

		// Pass rightStick values in place of mouse when non-zero.
		mouseInput.x = rightStickInput.x != 0 ? rightStickInput.x * RightStickMultiplier.x : mouseInput.x;
		mouseInput.y = rightStickInput.y != 0 ? rightStickInput.y * RightStickMultiplier.y : mouseInput.y;

		bool jumpInput = Input.GetButtonDown("Jump");

		Current = new PlayerInputData() {
			MoveInput = moveInput,
			MouseInput = mouseInput,
			JumpInput = jumpInput
		};
	}
}

public struct PlayerInputData
{
	public Vector3 MoveInput;
	public Vector2 MouseInput;
	public bool JumpInput;
}