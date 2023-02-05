using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using System;

public class PlayerController : MonoBehaviour
{

	[SerializeField] private InputActionAsset inputProvider;
	[SerializeField] private float moveSpeed = 6f;
	[SerializeField] private float movementForce = 1f;
	[SerializeField] private Camera playerCamera;
	private InputAction move;
	private Rigidbody rb;
	private Vector3 forceDirection = Vector3.zero;
	private CharacterController controller;
	private Vector2 moveVals;
	private float maxSpeed = 20f;
	private bool playerMovementLocked = false;
	public float turnSmoothTime = 0.1f;
	float turnSmoothVelocity;
    private Animator anim;
	public PlayerStates currentState;

	public static event Action<bool> OnScoreUpdate;
	public enum PlayerStates
    {
		IDLE,
		WALK,
		SUCK
    }
	PlayerStates CurrentState
    {
		set
        {
			currentState = value;

			switch(currentState)
            {
				case PlayerStates.IDLE:
					anim.SetTrigger("Idle");
					break;
				case PlayerStates.WALK:
					anim.SetTrigger("Move");
					break;
				case PlayerStates.SUCK:
					anim.SetTrigger("Sucking");
					break;
			}
        }
    }
	//testing INK script variable changes

    void Awake()
	{
		anim = GetComponent<Animator>();
		rb = GetComponent<Rigidbody>();
		rb.freezeRotation = true;
		controller = GetComponent<CharacterController>();
		//SetUpRigidbody();
		//inputProvider.FindActionMap("PlayerMovements").FindAction("Directional Movements").performed += ManagePlayerMovement;
	}

	void OnEnable() {
		inputProvider.FindAction("Directional Movements").Enable();
		
		inputProvider.FindAction("Suck Blood").Enable();
		
	}

	void Update()
    {

	}

	void FixedUpdate()
	{
		MoveCharacter();
		LookAt();
	}

	public void OnSpacePressed(InputAction.CallbackContext context)
    {
		if(context.started)
        {
			CurrentState = PlayerStates.SUCK;
			playerMovementLocked = true;
			OnScoreUpdate?.Invoke(true);
		}
		if(context.canceled)
        {
			CurrentState = PlayerStates.IDLE;
			playerMovementLocked = false;
			OnScoreUpdate?.Invoke(false);
        }
	}

	public void OnPlayerMove(InputAction.CallbackContext context) {
		//get player input values
		if(!playerMovementLocked)
        {

			moveVals = context.ReadValue<Vector2>();
			if (moveVals != Vector2.zero)
			{
				CurrentState = PlayerStates.WALK;
				Debug.Log("moving");

			}
			else { 
				CurrentState = PlayerStates.IDLE;
			}
		}
	}


	private void MoveCharacter() {
		if(!playerMovementLocked)
        {

			forceDirection += moveVals.x * GetCameraRight(playerCamera) *movementForce;
			forceDirection += moveVals.y * GetCameraForward(playerCamera) * movementForce;
			rb.AddForce(forceDirection, ForceMode.Impulse);
			forceDirection = Vector3.zero;

			Vector3 horizontalVelocity = rb.velocity;
			horizontalVelocity.y = 0;
			if (horizontalVelocity.sqrMagnitude > maxSpeed * maxSpeed)
				rb.velocity = horizontalVelocity.normalized * maxSpeed + Vector3.up * rb.velocity.y;

		}
	}

	private void LookAt()
    {
		Vector3 direction = rb.velocity;
		direction.y = 0f;
		if (moveVals.sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
        {
			float targetAngle = Mathf.Atan2(moveVals.x, moveVals.y) * Mathf.Rad2Deg;
			float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
			this.rb.rotation = Quaternion.Euler(0, angle, 0);

		}
        else
        {
			rb.angularVelocity = Vector3.zero;
        }
    }

	private Vector3 GetCameraForward(Camera playerCamera)
    {
		Vector3 forward = playerCamera.transform.forward;
		forward.y = 0;
		return forward.normalized;
    }

	private Vector3 GetCameraRight(Camera playerCamera)
	{
		Vector3 right = playerCamera.transform.right;
		right.y = 0;
		return right.normalized;
	}

	void OnDisable() {
		inputProvider.FindAction("Directional Movements").Disable();
		inputProvider.FindAction("Suck Blood").Disable();

	}

	void OnCollisionEnter(Collision collision)
    {
       if(collision.gameObject.CompareTag("Hand")) {
		//player dies and has to be spawned again 
		Debug.Log("Ded");
	   }
    }
}
