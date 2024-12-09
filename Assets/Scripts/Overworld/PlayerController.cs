using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody theRB;
    public float moveSpeed, jumpForce;

    private Vector2 moveInput;
    public Transform cameraTransform; // Reference to the camera transform

    public LayerMask whatIsGround;
    public Transform groundPoint;
    private bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        if (!cameraTransform)
        {
            cameraTransform = Camera.main.transform; // Automatically find the main camera if not assigned
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Get input
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");
        moveInput.Normalize();

        // Calculate movement direction relative to the camera
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        // Remove the y component to ensure movement stays horizontal
        camForward.y = 0;
        camRight.y = 0;

        // Normalize the vectors to maintain consistent speed
        camForward.Normalize();
        camRight.Normalize();

        // Combine input with camera direction
        Vector3 moveDirection = (camRight * moveInput.x + camForward * moveInput.y) * moveSpeed;
        theRB.velocity = new Vector3(moveDirection.x, theRB.velocity.y, moveDirection.z);

        // Ground check using raycast
        RaycastHit hit;
        if (Physics.Raycast(groundPoint.position, Vector3.down, out hit, .3f, whatIsGround))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        // Jump logic
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            theRB.velocity += new Vector3(0f, jumpForce, 0f);
        }
    }
}
