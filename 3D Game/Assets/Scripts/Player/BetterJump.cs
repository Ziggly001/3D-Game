using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterJump : MonoBehaviour
{

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    Rigidbody rb;

    private CustomGravity CustomGravity;

    public GameObject Player;

    private Vector3 localVelocity = new Vector3();

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        //CustomGravity = CustomGravity.GetComponent<CustomGravity>();
        CustomGravity = Player.GetComponent<CustomGravity>();
    }

    //    void FixedUpdate()
    //    {
    //        if (rb.velocity.y < 0)
    //        {
    //            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
    //        }
    //        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
    //        {
    //            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
    //        }
    //    }


        // Replace gravityScale (used for the 2d engine) with my own gravity scaling script
    void FixedUpdate()
    {
        // States that the "localVelocity" Vector3 is the player's current local velocity
        localVelocity = rb.transform.InverseTransformDirection(rb.velocity);

        // replaced "rb.velocity.y" with a local version, as rigidbody velocity is worldspace.
        if (localVelocity.y < 0)
        {
            CustomGravity.gravityScale = fallMultiplier;
        }
        else if (localVelocity.y > 0 && !Input.GetButton("Jump"))
        {
            CustomGravity.gravityScale = lowJumpMultiplier;
        }
        else
        {
            CustomGravity.gravityScale = 1f;
        }
    }
}
