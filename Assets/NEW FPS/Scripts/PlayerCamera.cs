using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private PlayerSettings settings;
    [SerializeField] private Transform playerTransform;

    private float xRotation = 0f;
    private float currentFOV;
    private float targetFOV;
    private float fovVelocity;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        currentFOV = Camera.main.fieldOfView;
        targetFOV = currentFOV;
    }

    public void HandleLook(Vector2 lookInput)
    {
        float mouseX = lookInput.x * settings.mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * settings.mouseSensitivity * Time.deltaTime;

        if (settings.invertY) mouseY *= -1;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, settings.minVerticalAngle, settings.maxVerticalAngle);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerTransform.Rotate(Vector3.up * mouseX);
    }

    public void HandleFOVKick(bool isRunning)
    {
        if (!settings.enableFOVKick) return;

        targetFOV = isRunning ? settings.runFOV : Camera.main.fieldOfView;
        currentFOV = Mathf.SmoothDamp(currentFOV, targetFOV, ref fovVelocity, settings.fovSmoothTime);
        Camera.main.fieldOfView = currentFOV;
    }

    public Vector3 HandleHeadBob(float speed, bool isMoving)
    {
        if (!settings.enableHeadBob || !isMoving) return Vector3.zero;

        float bobX = Mathf.Sin(Time.time * settings.bobSpeed * speed) * settings.bobAmount;
        float bobY = Mathf.Abs(Mathf.Cos(Time.time * settings.bobSpeed * speed)) * settings.bobAmount;
        return new Vector3(bobX, bobY, 0f);
    }
}