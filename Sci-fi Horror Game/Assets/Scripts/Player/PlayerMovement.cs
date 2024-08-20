using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float jumpForce = 5f;

    private CharacterController controller;
    private Vector3 moveDirection;
    private float gravity = -9.81f;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Check if the player is on the ground
        isGrounded = controller.isGrounded;
        if (isGrounded && moveDirection.y < 0)
        {
            moveDirection.y = -2f;  // Ensure the player stays grounded
        }

        // Get input for movement
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Move in the direction the player is facing
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * moveSpeed * Time.deltaTime);

        // Jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            moveDirection.y = jumpForce;
        }

        // Apply gravity
        moveDirection.y += gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);

        // Rotate the player based on mouse movement
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        transform.Rotate(Vector3.up * mouseX);
    }
}
