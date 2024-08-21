using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform playerBody;               // Reference to the player's body
    public float mouseSensitivity = 100f;      // Sensitivity of the mouse movement
    public float distanceFromPlayer = 5f;      // Distance of the camera from the player

    private float xRotation = 0f;              // To keep track of the camera's vertical rotation (pitch)
    private float yRotation = 0f;              // To keep track of the camera's horizontal rotation (yaw)

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
    }

    void LateUpdate()
    {
        // Get mouse input for camera rotation
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Adjust the camera's horizontal and vertical rotation (yaw and pitch)
        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -40f, 85f); // Clamp the pitch to prevent over-rotation

        // Calculate the new camera rotation
        Quaternion cameraRotation = Quaternion.Euler(xRotation, yRotation, 0);

        // Position the camera relative to the player's position
        Vector3 direction = cameraRotation * Vector3.back * distanceFromPlayer;
        transform.position = playerBody.position + direction;

        // Make the camera look at the player
        transform.LookAt(playerBody.position + Vector3.up * 1.5f); // Adjust height offset to match player's height
    }
}
