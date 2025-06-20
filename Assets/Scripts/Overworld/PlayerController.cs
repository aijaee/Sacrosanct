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

    public Animator anim;

    private bool movingBackwards;

    public bool flipped;
    public float flipspeed;

    Quaternion flipleft = Quaternion.Euler(0, -180, 0);
    Quaternion flipright = Quaternion.Euler(0, 0, 0);

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
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");
        moveInput.Normalize();

        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDirection = (camRight * moveInput.x + camForward * moveInput.y) * moveSpeed;
        theRB.velocity = new Vector3(moveDirection.x, theRB.velocity.y, moveDirection.z);

        anim.SetFloat("moveSpeed", theRB.velocity.magnitude);

        if (!flipped && moveInput.x < 0)
        {
            flipped = true;
        }else if (flipped && moveInput.x > 0)
        {
            flipped = false;
        }

        if (flipped) transform.rotation = Quaternion.Slerp(transform.rotation, flipleft, flipspeed * Time.deltaTime);
        else if (!flipped) transform.rotation = Quaternion.Slerp(transform.rotation, flipright, flipspeed * Time.deltaTime);


        RaycastHit hit;
        if (Physics.Raycast(groundPoint.position, Vector3.down, out hit, .3f, whatIsGround))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            theRB.velocity += new Vector3(0f, jumpForce, 0f);
        }

        if (!movingBackwards && moveInput.y > 0)
        {
            movingBackwards = true;
        } else if (movingBackwards && moveInput.y < 0)
        {
            movingBackwards = false;
        }
        anim.SetBool("movingBackwards", movingBackwards);
    }
}
