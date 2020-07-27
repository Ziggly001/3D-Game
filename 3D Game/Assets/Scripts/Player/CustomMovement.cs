using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    // reference the Main Camera, not the Cinemachine one

    public float speed = 6f;

    public float turnSmoothTime = 0.1f;

    float turnSmoothVelocity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 inputDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        if(inputDirection.magnitude >= 0.1f)
        {
            // gets angle to rotate the player by
            //float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg; (this commented out code would be used if camera didn't influence player's rotation)
            float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            // smoothes the angle the player rotates at to make it less snappy
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            // rotates the character to the target angle
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // uses camera direction to influence movement
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            //
            //controller.Move(inputDirection * speed * Time.deltaTime); (inputDirection for cam-independent movement, moveDir.normalized for cam-dependent)
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
    }
}
