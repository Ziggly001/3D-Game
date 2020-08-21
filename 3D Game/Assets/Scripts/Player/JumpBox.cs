using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBox : MonoBehaviour
{
    [Range(1, 10)]
    public float jumpVelocity;
    //public float groundedSkin = 0.05f;
    //public LayerMask mask;

    bool jumpRequest;
    bool grounded;

    Vector3 playerSize;

    float playerHeight;
    float playerWidth;
    float playerLength;

    //Vector3 boxSize;


    // New Ground Check Variables
    public Transform groundCheck;
    public float sphereSize = 0.4f;
    public LayerMask groundMask;
    //

    void Awake()
    {
        playerHeight = GetComponent<CapsuleCollider>().height;
        playerWidth = GetComponent<CapsuleCollider>().radius * 2;
        playerLength = GetComponent<CapsuleCollider>().radius * 2;

        playerSize.Set(playerWidth, playerHeight, playerLength);
        //boxSize.Set(playerSize.x, groundedSkin, playerSize.z);
    }

    void Update()
    {
        // Sphere Ground Check
        grounded = Physics.CheckSphere(groundCheck.position, sphereSize, groundMask);
        //

        if (Input.GetButtonDown ("Jump") && grounded)
        {
            jumpRequest = true;
        }
    }

    void FixedUpdate()
    {
        if (jumpRequest)
        {
            //GetComponent<Rigidbody>().velocity += Vector3.up * jumpVelocity;
            // I switched from Vector3.up to transform.up

            // Use a custom AddForceToReachVelocity function, to prevent any oddities.
            GetComponent<Rigidbody>().AddForce(transform.up * jumpVelocity, ForceMode.Impulse);

            jumpRequest = false;
            grounded = false;
        }
        else
        {
            // box method
            //Vector3 boxCenter = (Vector3)transform.position + Vector3.down * (playerSize.y + boxSize.y) * 0.5f;
            //grounded = (Physics.OverlapBox (boxCenter, boxSize, Quaternion.identity, mask) != null);
        }
    }
}
