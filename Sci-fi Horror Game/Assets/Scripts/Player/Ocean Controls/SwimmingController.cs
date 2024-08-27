using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwimmingController : MonoBehaviour
{
    public float speed = 5.0f;
    public float rotationSpeed = 100.0f;

    private CharacterController characterController;
    private Camera swimmingCamera;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        swimmingCamera = GetComponent<Camera>();
    }

void Update()
{
    // Get input from the player
    float horizontalInput = Input.GetKey(KeyCode.D) ? 1 : Input.GetKey(KeyCode.A) ? -1 : 0;
    float verticalInput = Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0;

    // Calculate movement direction
    Vector3 movementDirection = swimmingCamera.transform.forward.normalized;

    // Move the player
    if (horizontalInput != 0 || verticalInput != 0)
    {
        Vector3 movement = Vector3.zero;
        if (horizontalInput != 0)
        {
            movement += swimmingCamera.transform.right * horizontalInput * speed * Time.deltaTime;
        }
        if (verticalInput != 0)
        {
            movement += swimmingCamera.transform.forward * verticalInput * speed * Time.deltaTime;
        }
        movement = Vector3.ClampMagnitude(movement, speed * Time.deltaTime);
        characterController.Move(movement);
    }

    // Update camera rotation
    swimmingCamera.transform.Rotate(-Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime, Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime, 0);
}
}