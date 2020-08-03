using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class NewMove : MonoBehaviour
{

    private Transform m_Cam;                  // A reference to the main camera in the scenes transform
    private Vector3 m_CamForward;             // The current forward direction of the camera
    private Vector3 m_Move;

    public float speed = 4;

    Vector3 NoCamAngle;

    // Start is called before the first frame update
    void Start()
    {
        // get the transform of the main camera
        if (Camera.main != null)
        {
            m_Cam = Camera.main.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //float xTranslate = CrossPlatformInputManager.GetAxis("Horizontal") * Time.deltaTime * speed;
        //float zTranslate = CrossPlatformInputManager.GetAxis("Vertical") * Time.deltaTime * speed;

        //transform.Translate(xTranslate, 0, zTranslate);
        //transform.Rotate(0, -150 * Time.deltaTime, 0);

    }

    private void FixedUpdate()
    {
        // read inputs
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        float v = CrossPlatformInputManager.GetAxis("Vertical");
        bool crouch = Input.GetKey(KeyCode.C);

        // calculate move direction to pass to character
        if (m_Cam != null)
        {
            // calculate camera relative direction to move:
            // CamForward is a Vector3 describing the rotation of the main camera.
            m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
            m_Move = v * m_CamForward + h * m_Cam.right;
        }
        else
        {
            Debug.LogWarning(
                    "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\".", gameObject);
        }

        //sends move inputs to Move function to use to move
        Move(m_Move, h, v, crouch);
    }

    public void Move(Vector3 move, float h, float v, bool crouch)
    {

        // convert the world relative moveInput vector into a local-relative
        move = transform.InverseTransformDirection(move);

        if (m_Move.magnitude > 0.1f)
        {
            float targetAngleNoCam = Mathf.Atan2(h, v) * Mathf.Rad2Deg;
            NoCamAngle.Set(0f, targetAngleNoCam, 0f);
            Vector3 targetAngleWithCam = NoCamAngle + m_CamForward;

            transform.Translate(targetAngleWithCam.normalized * speed * Time.deltaTime);
        }

    }
}