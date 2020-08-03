using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FINALMoveBackup : MonoBehaviour
{

    public float InputX;
    public float InputZ;
    public Vector3 desiredMoveDirection;
    public bool blockRotationPlayer;
    public float desiredRotationSpeed = 0.1f;
    public float Speed;
    public float allowPlayerRotation = 0.1f;
    public Camera cam;

    private float verticalVel;
    private Vector3 moveVector;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        InputMagnitude();
    }
    void PlayerMoveAndRotation()
    {
        //InputX = Input.GetAxis("Horizontal");
        //InputZ = Input.GetAxis("Vertical");

        var camera = Camera.main;
        var forward = cam.transform.forward;
        var right = cam.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        desiredMoveDirection = forward * InputZ + right * InputX;

        if (blockRotationPlayer == false)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), desiredRotationSpeed);
        }
    }

    void InputMagnitude()
    {
        //Calculate Input Vectors
        InputX = Input.GetAxis("Horizontal");
        InputZ = Input.GetAxis("Vertical");

        //anim.SetFloat("InputZ", InputZ, VerticalAnimTime, Time.deltaTime * 2f);
        //anim.SetFloat("InputX", InputX, HorizontalAnimSmoothTime, Time.deltaTime * 2f);

        //Calculate the Input Magnitude
        Speed = new Vector2(InputX, InputZ).sqrMagnitude;

        //Physically move player
        if (Speed > allowPlayerRotation)
        {
            //anim.SetFloat("InputMagnitude", Speed, StartAnimTime, Time.deltaTime);
            PlayerMoveAndRotation();
        }
        else if (Speed < allowPlayerRotation)
        {
            //anim.SetFloat("InputMagnitude", Speed, StopAnimTime, Time.deltaTime);
        }
    }
}

