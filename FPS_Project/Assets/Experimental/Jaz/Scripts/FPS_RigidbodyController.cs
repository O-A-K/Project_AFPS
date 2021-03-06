﻿using System.Collections;
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

    private void Start()
    {
        IEnumerator checkForGrounded;
        checkForGrounded = CheckForGrounded(5);

        //StartCoroutine(checkForGrounded);
    }

    void FixedUpdate()
    {
        if (grounded)
        {
            Move();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("Pressed Button");
        }

        // We apply gravity manually for more tuning control.
        rigidbody.AddForce(new Vector3(0, -gravity * rigidbody.mass, 0));
        grounded = false;
    }

    void Move()
    {
        // Calculate how fast we should be moving.
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");

        Vector3 targetVelocity = new Vector3(horizontalMovement, 0, verticalMovement);
        targetVelocity = transform.TransformDirection(targetVelocity);

        targetVelocity *= speed;

        // Apply a force that attempts to reach our target velocity.
        Vector3 velocity = rigidbody.velocity;
        Vector3 velocityChange = (targetVelocity - velocity);

        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0;                                                                        // Freezing Y when not jumping.
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);

        rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);

        Jump();

        // Jump (No Air Control)
        if (canJump)
        {
            canJump = false;
            rigidbody.velocity = new Vector3(velocity.x, CalculateJumpVerticalSpeed(), velocity.z);
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Bit shift the index of the Layer.
            RaycastHit hit;

            if (Physics.Raycast(transform.position, Vector3.down, out hit, (capsuleCollider.height / 2) + 0.25f))
            {
                Debug.DrawRay(transform.position + new Vector3(0, 0, 0.1f), Vector3.down * hit.distance, Color.yellow);

                if (hit.collider.gameObject.layer == 9)
                {
                    print("canJump");
                    canJump = true;
                }
            }
        }
    }

    IEnumerator CheckForGrounded(float vCoroutineRefreshRate)
    {
        while (true)
        {
            float tRefreshRateInSecs = 1 / vCoroutineRefreshRate;

            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, (capsuleCollider.height / 2) + 0.1f))
            {
                if (hit.collider.gameObject.layer == 9)
                {
                    print("grounded");
                    grounded = true;
                }

                else
                {
                    print("Not grounded");
                    grounded = false;
                }
            }

            //print("Checking For Grounded");
            yield return new WaitForSeconds(tRefreshRateInSecs);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        grounded = true;
    }

    float CalculateJumpVerticalSpeed()
    {
        // From the jump height and gravity we deduce the upwards speed for the character to reach at the apex.
        return Mathf.Sqrt(2 * jumpHeight * gravity);
    }
}