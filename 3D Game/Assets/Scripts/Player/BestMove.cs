using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BestMove : MonoBehaviour
{
    // reference Camera Direction Empty
    public Transform Empty;

    float xInput;
    float zInput;

    Vector3 moveVector3;

    public bool isMoving;

    float speed = 4f;

    bool strafeToggle = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        xInput = Input.GetAxis("Horizontal");
        zInput = Input.GetAxis("Vertical");

        // Used to see if the user is attempting to move.
        moveVector3 = new Vector3(xInput, 0f, zInput).normalized;

        if (moveVector3.magnitude > 0.1f)
        {
            isMoving = true;
            Move();
        }
        else
        {
            isMoving = false;
        }

    }

    void Move()
    {
        //movement
        // Different movement for strafing vs free movement
        if (strafeToggle == true)
        {
            // Strafe Movement

            // 

        }
        else
        {
            // Free Movement

            // Rotate relative to camera
            Vector3 CamX = Empty.localEulerAngles;
            transform.Rotate(CamX, Space.Self);



            // Move relative to facing direction
            transform.Translate(xInput * Time.deltaTime * speed, 0, zInput * Time.deltaTime * speed);


        }

    }
}
