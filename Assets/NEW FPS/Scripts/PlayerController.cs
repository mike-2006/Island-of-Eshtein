using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerSettings settings;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerCamera playerCamera;
    [SerializeField] private PlayerAudio playerAudio;
    [SerializeField] private LayerMask groundMask;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isCrouching;
    private float currentHeight;
    private Vector3 headBobOffset;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        currentHeight = settings.normalHeight;
        controller.height = currentHeight;
    }

    private void Update()
    {
        CheckGround();
        HandleCrouch();
        HandleMovement();
        HandleJump();
        ApplyGravity();
        Move();

        playerCamera.HandleFOVKick(playerInput.SprintHeld && playerInput.MoveInput != Vector2.zero);

        bool isMoving = playerInput.MoveInput != Vector2.zero && isGrounded;
        float speed = isMoving ? controller.velocity.magnitude : 0f;
        headBobOffset = playerCamera.HandleHeadBob(speed, isMoving);
        ApplyHeadBob();

        playerInput.ResetJump();
    }

    private void CheckGround()
    {
        isGrounded = Physics.SphereCast(
            transform.position,
            controller.radius,
            Vector3.down,
            out _,
            settings.groundCheckDistance,
            groundMask
        );

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            playerAudio.PlayLandSound();
        }
    }

    private void HandleCrouch()
    {
        if (!settings.enableCrouch) return;

        float targetHeight = playerInput.CrouchHeld ? settings.crouchHeight : settings.normalHeight;
        currentHeight = Mathf.Lerp(currentHeight, targetHeight, Time.deltaTime * 10f);
        controller.height = currentHeight;

        if (isCrouching != playerInput.CrouchHeld)
        {
            isCrouching = playerInput.CrouchHeld;
            Vector3 center = new Vector3(0f, currentHeight / 2f, 0f);
            controller.center = center;
        }
    }

    private void HandleMovement()
    {
        Vector2 input = playerInput.MoveInput;
        float targetSpeed = GetTargetSpeed();

        Vector3 targetVelocity = transform.forward * input.y + transform.right * input.x;
        targetVelocity.Normalize();
        targetVelocity *= targetSpeed;

        velocity.x = Mathf.Lerp(velocity.x, targetVelocity.x, settings.acceleration * Time.deltaTime);
        velocity.z = Mathf.Lerp(velocity.z, targetVelocity.z, settings.acceleration * Time.deltaTime);

        if (input != Vector2.zero && isGrounded)
        {
            playerAudio.PlayStepSound();
        }
    }

    private float GetTargetSpeed()
    {
        if (playerInput.CrouchHeld) return settings.crouchSpeed;
        if (playerInput.SprintHeld) return settings.runSpeed;
        return settings.walkSpeed;
    }

    private void HandleJump()
    {
        if (playerInput.JumpPressed && isGrounded && !playerInput.CrouchHeld)
        {
            velocity.y = Mathf.Sqrt(settings.jumpForce * -2f * Physics.gravity.y * settings.gravityMultiplier);
            playerAudio.PlayJumpSound();
        }
    }

    private void ApplyGravity()
    {
        if (!isGrounded)
        {
            velocity.y += Physics.gravity.y * settings.gravityMultiplier * Time.deltaTime;
        }
    }

    private void Move()
    {
        Vector3 finalVelocity = new Vector3(velocity.x, velocity.y, velocity.z);
        finalVelocity += headBobOffset;
        controller.Move(finalVelocity * Time.deltaTime);
    }

    private void ApplyHeadBob()
    {
        playerCamera.transform.localPosition = headBobOffset;
    }
}