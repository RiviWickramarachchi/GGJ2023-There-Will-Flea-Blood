using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using System;

public class PlayerController : MonoBehaviour
{

	private Rigidbody rb;
	[SerializeField] private float jumpForce;
	[SerializeField] private float slideOnLand;
	[SerializeField] private InputActionAsset inputProvider;
	[SerializeField] private float moveSpeed = 6f;
	[SerializeField] private float movementForce = 1f;
	[SerializeField] private Camera playerCamera;
	private InputAction move;
	private Vector3 forceDirection = Vector3.zero;
	private CharacterController controller;
	private Vector2 moveVals;
	private float maxSpeed = 20f;
	private bool playerMovementLocked = false;
	public float turnSmoothTime = 0.1f;
	float turnSmoothVelocity;
    
	public PlayerStates currentState;
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
					//anim.Play("Idle");
					break;
				case PlayerStates.WALK:
					//anim.Play("Movement");
					break;
				case PlayerStates.SUCK:
					break;
			}
        }
    }
	//testing INK script variable changes

    void Awake()
	{
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
		if (currentState == PlayerStates.SUCK)
		{
			Debug.Log("sucking");
		}
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
		}
		if(context.canceled)
        {
			CurrentState = PlayerStates.IDLE;
			playerMovementLocked = false;
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

	/*
	if (Input.GetMouseButtonDown(1))
	{
		TryJump();
	}
}

public void TryJump()
{
	if (!_isGrounded) return;
	Debug.Log("Do jump tried");
	DoJump();
}

private void DoJump()
{
	_horizontalJumpVelocity = agent.velocity;
	DisableNavMeshAgent();
	EnableRB();
	_isGrounded = false;
	rb.AddForce(new Vector3(_horizontalJumpVelocity.x, jumpForce, _horizontalJumpVelocity.z), ForceMode.Impulse);
}

private void SetUpRigidbody()
{
	rb = GetComponent<Rigidbody>();
	if (rb == null)
	{
		gameObject.AddComponent<Rigidbody>();
		rb = GetComponent<Rigidbody>();
	}
	DisableRB();
	rb.freezeRotation = true;
}

// Effectively enables rigidbody.
private void EnableRB()
{
	rb.isKinematic = false;
	rb.useGravity = true;
}

// Effectively disables rigidbody. Technically still enabled but won't do much.
private void DisableRB()
{
	rb.isKinematic = true;
	rb.useGravity = false;
}

private void EnableNavMeshAgent()
{
	agent.Warp(transform.position);
	if(Mathf.Abs(_horizontalJumpVelocity.x) > 0f || Mathf.Abs(_horizontalJumpVelocity.z) > 0f) agent.SetDestination(transform.position + (transform.forward * slideOnLand));
	agent.updatePosition = true;
	agent.updateRotation = true;
	agent.isStopped = false;
}

private void DisableNavMeshAgent()
{
	agent.updatePosition = false;
	agent.updateRotation = false;
	agent.isStopped = true;
}

private void OnCollisionEnter(Collision other)
{
	Debug.Log(other.collider.tag);
	if (other.collider != null && other.collider.tag == "Ground" && !_isGrounded && rb.velocity.y <= 0f)
	{ // Condition assumes all ground is tagged with "Ground".
		Debug.Log("Condition met!");
		DisableRB();
		EnableNavMeshAgent();
	*/
	//_isGrounded = true;

	void OnDisable() {
		inputProvider.FindAction("Directional Movements").Disable();
		inputProvider.FindAction("Suck Blood").Disable();

	}
}
