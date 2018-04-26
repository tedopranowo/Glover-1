using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	#region variables
	Rigidbody playerRigidbody;
	Vector3 movement;                   // The vector to store the direction of the player's movement.
										
	// Player
	public float playerSpeed = 7.0f;
	//public float coffeeMeter = 5.0f;
	//public float energyMeter = 5.0f;
	//public float followerMeter = 5.0f;
	#endregion




	private void Start ()
	{
	}
	

	private void Update ()
	{
		if (MyGameManager._instance.GameMode == GameMode.NormalPlay /*&& !DialogueManager.IsConversationActive*/)
		{
			if (IsKeyDownOfButtonControl(ButtonControlNames.Interact))
			{
				// TODO
				Debug.Log(ButtonControlNames.Interact + " Pressed");
				//StartInteraction(GameManager._instance.ClosestNpcName);
			}
			if (IsKeyDownOfButtonControl(ButtonControlNames.UseAction))
			{
				// TODO
				Debug.Log(ButtonControlNames.UseAction + " Pressed");
			}
			if (IsKeyDownOfButtonControl(ButtonControlNames.UseItem))
			{
				// TODO
				Debug.Log(ButtonControlNames.UseItem + " Pressed");
			}
		}
		if (IsKeyDownOfButtonControl(ButtonControlNames.Start) /*&& !DialogueManager.IsConversationActive*/)
		{
			// TODO
			Debug.Log(ButtonControlNames.Start + " Pressed");
		}
		
		float inputX = Input.GetAxisRaw("Horizontal");
		float inputZ = Input.GetAxisRaw("Vertical");

		Move(inputX, inputZ);
	}


	private void Awake()
	{
		playerRigidbody = GetComponent<Rigidbody>();
	}


	private void Move(float x, float z)
	{
		// Set the movement vector based on the axis input.
		movement.Set(x, 0f, z);

		// Normalise the movement vector and make it proportional to the speed per second.
		movement = movement.normalized * playerSpeed * Time.deltaTime;

		// Move the player to it's current position plus the movement.
		playerRigidbody.MovePosition(transform.position + movement);

		//Quaternion rotation = Quaternion.LookRotation(movement);
		//Debug.Log(rotation.x + ", " + rotation.y + ", " + rotation.z);
	}


	// <summary> 
	// Checks if the Keycode is clicked (get key down) of both controller and keyboard controls.
	// Also checks against the user's current OS for controller mapping.
	// </summary>
	public bool IsKeyDownOfButtonControl(ButtonControlNames buttonDown)
	{
		return (Input.GetKeyDown(MyGameManager._instance.ButtonControls_Keyboard[buttonDown]) || Input.GetKeyDown(MyGameManager._instance.ButtonControls_Controller[buttonDown]));
	}

}
