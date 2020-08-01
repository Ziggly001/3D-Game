using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamXRot : MonoBehaviour
{
    float CamXInput;

    public float IdleRotSpeed = 75f;
    public float MoveRotSpeed = 25f;
    float RotSpeed;

    float YRot;

    public CinemachineFreeLook FreeLook;

    public GameObject Player;
    private BestMove MoveScript;

    // Start is called before the first frame update
    void Start()
    {
        MoveScript = Player.GetComponent<BestMove>();
    }

    // Update is called once per frame
    void Update()
    {
        if (MoveScript.isMoving == true)
        {
            RotSpeed = MoveRotSpeed;
        }
        else
        {
            RotSpeed = IdleRotSpeed;
        }

        CamXInput = Input.GetAxis("Camera X");

        YRot = CamXInput * RotSpeed * Time.deltaTime;

        transform.Rotate(0, YRot, 0, Space.Self);
        FreeLook.m_XAxis.Value += YRot;
    }
}
