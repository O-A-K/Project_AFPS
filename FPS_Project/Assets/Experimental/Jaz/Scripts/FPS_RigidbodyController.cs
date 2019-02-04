using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

public class FPS_RigidbodyController : MonoBehaviour
{
    static readonly float gravity = 9.81f;
    [Range(0, 25)] public float speed = 10.0f;
    [Range(0, 25)] public float maxVelocityChange = 10.0f;

    public bool canJump = true;
    [Range(0, 5)] public float jumpHeight = 2.0f;

    private bool grounded = false;
    private Rigidbody rigidbody;
    private CapsuleCollider capsuleCollider;

    void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        rigidbody = GetComponent<Rigidbody>();

        rigidbody.freezeRotation = true;
        rigidbody.useGravity = false;
    }

    void FixedUpdate()
    {
        if (grounded)
        {
            Move();
        }

        // We apply gravity manually for more tuning control
        rigidbody.AddForce(new Vector3(0, -gravity * rigidbody.mass, 0));

        grounded = false;
    }

    void Move()
    {
        // Calculate how fast we should be moving
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");

        Vector3 targetVelocity = new Vector3(horizontalMovement, 0, verticalMovement);
        targetVelocity = transform.TransformDirection(targetVelocity);

        targetVelocity *= speed;

        // Apply a force that attempts to reach our target velocity 
        Vector3 velocity = rigidbody.velocity;
        Vector3 velocityChange = (targetVelocity - velocity);

        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0;                                                                                   // Freezing Y when not jumping.
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);

        rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);

        JumpCheckGrounded();

        // Jump (No Air Control)
        if (canJump)
        {
            rigidbody.velocity = new Vector3(velocity.x, CalculateJumpVerticalSpeed(), velocity.z);
            canJump = false;
        }
    }

    void JumpCheckGrounded()
    {
        if (Input.GetButtonDown("Jump"))
        {
            // Bit shift the index of the Layer
            int groundLayerMask = 1 << 9;

            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, (capsuleCollider.height / 2) + 0.1f, groundLayerMask))
            {
                if (hit.collider)
                {
                    print("canJump");
                    canJump = true;
                }
            }
        }
    }

    void OnCollisionStay() // Switch this to Raycasting on a coroutine.
    {
        grounded = true;
    }

    float CalculateJumpVerticalSpeed()
    {
        // From the jump height and gravity we deduce the upwards speed 
        // for the character to reach at the apex.
        return Mathf.Sqrt(2 * jumpHeight * gravity);
    }
}
