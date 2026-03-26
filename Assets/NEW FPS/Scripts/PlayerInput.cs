using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private PlayerInputAction inputActions;

    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool SprintHeld { get; private set; }
    public bool CrouchHeld { get; private set; }

    private void Awake()
    {
        inputActions = new PlayerInputAction();
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => MoveInput = Vector2.zero;
        inputActions.Player.Look.performed += ctx => LookInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Look.canceled += ctx => LookInput = Vector2.zero;
        inputActions.Player.Jump.performed += ctx => JumpPressed = true;
        inputActions.Player.Sprint.performed += ctx => SprintHeld = true;
        inputActions.Player.Sprint.canceled += ctx => SprintHeld = false;
        inputActions.Player.Crouch.performed += ctx => CrouchHeld = true;
        inputActions.Player.Crouch.canceled += ctx => CrouchHeld = false;
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    public void ResetJump() => JumpPressed = false;
}