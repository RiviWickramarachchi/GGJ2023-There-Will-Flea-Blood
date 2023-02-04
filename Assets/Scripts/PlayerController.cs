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
	[SerializeField] private float moveSpeed; //= 2f;

	private CharacterController controller;
	private Vector3 playerVelocity;
	private Vector2 moveVals;
	private bool playerMovementLocked = false;
    
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
		//player moves to the position where the seed is dropped
		
		MoveCharacter();
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
			//Get normalized directional vectors of the cameras
			Vector3 camForward = Camera.main.transform.forward;
			Vector3 camRight = Camera.main.transform.right;
			camForward.y = 0;
			camRight.y = 0;
			camForward = camForward.normalized;
			camRight = camRight.normalized;

			//get direction-relative input vectors
			Vector3 forwardRelativeVerticalInput = moveVals.y * camForward;
			Vector3 rightRelativeVerticalInput = moveVals.x * camRight;

			//get camera relative movement vector
			Vector3 camRelativeMovement = forwardRelativeVerticalInput + rightRelativeVerticalInput;
			if (camRelativeMovement != Vector3.zero)
			{
				//Rotation is not necessary for the 2D sprites
				//transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(camRelativeMovement), 0.15f);
				transform.Translate(camRelativeMovement * moveSpeed * Time.deltaTime, Space.World);
			}
			Vector3 moveDir = Vector3.zero;
			moveDir.x = moveVals.x;
			moveDir.z = moveVals.y;
			controller.Move(transform.TransformDirection(moveDir) * moveSpeed * Time.deltaTime);
		}
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
