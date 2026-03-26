using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "FPS/Player Settings")]
public class PlayerSettings : ScriptableObject
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float crouchSpeed = 2.5f;
    public float acceleration = 10f;
    public float deceleration = 10f;

    [Header("Jump")]
    public float jumpForce = 5f;
    public float gravityMultiplier = 2.5f;
    public float groundCheckDistance = 0.2f;

    [Header("Look")]
    public float mouseSensitivity = 2f;
    public float minVerticalAngle = -90f;
    public float maxVerticalAngle = 90f;
    public bool invertY = false;

    [Header("Head Bob")]
    public bool enableHeadBob = true;
    public float bobSpeed = 10f;
    public float bobAmount = 0.1f;

    [Header("FOV Kick")]
    public bool enableFOVKick = true;
    public float runFOV = 75f;
    public float fovSmoothTime = 0.3f;

    [Header("Crouch")]
    public bool enableCrouch = true;
    public float crouchHeight = 1f;
    public float normalHeight = 2f;
}