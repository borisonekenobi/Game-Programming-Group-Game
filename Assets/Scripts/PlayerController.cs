using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	private const float DistanceToGround = 1f;

	[Header("Movement Settings")]
	[SerializeField] private float maxMoveSpeed;
	[SerializeField] private float moveForce;
	[SerializeField] private float jumpForce;

	[Header("Controls")]
	[SerializeField] private Rigidbody rigidbody;
	[SerializeField] private InputAction moveAction;

	private bool _jumpRequested;
	private Vector3 _moveDirection;
	private bool IsGrounded => Physics.Raycast(transform.position, Vector3.down, DistanceToGround + 0.001f);

	private void Update()
	{
		_moveDirection = moveAction.ReadValue<Vector3>();

		if (Keyboard.current.spaceKey.wasPressedThisFrame) _jumpRequested = true;

		_moveDirection.y = 0;
		_moveDirection.Normalize();
	}

	private void FixedUpdate()
	{
		var moveVelocity = _moveDirection * maxMoveSpeed;
		var velocityChange = moveVelocity - rigidbody.linearVelocity;
		velocityChange.y = 0;

		rigidbody.AddForce(velocityChange * moveForce, ForceMode.Acceleration);

		if (!IsGrounded || !_jumpRequested) return;

		rigidbody.linearVelocity = new Vector3(rigidbody.linearVelocity.x, 0, rigidbody.linearVelocity.z);
		rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
		_jumpRequested = false;
	}

	private void OnEnable()
	{
		moveAction.Enable();
	}

	private void OnDisable()
	{
		moveAction.Disable();
	}
}
