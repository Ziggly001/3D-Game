using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPlayer : MonoBehaviour
{

    public GameObject Planet;
    public GameObject PlayerPlaceholder;


    public float speed = 4;
    //public float JumpHeight = 1.2f;

    //float gravity = -100;
    bool OnGround = false;


    float distanceToGround;
    Vector3 Groundnormal;



    private Rigidbody rb;

    private CustomGravity GravityScript;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        GravityScript = this.GetComponent<CustomGravity>();
    }

    // Adds the "Planet" class into this script's scope (?) (its basically initializing the "PlanetScript")
    private Planet PlanetScript;

    // Update is called once per frame
    void Update()
    {

        // States that "PlanetScript" is the "Planet" class from the Planet script
        PlanetScript = Planet.GetComponent<Planet>();

        //MOVEMENT (not using)

        //float x = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        //float z = Input.GetAxis("Vertical") * Time.deltaTime * speed;

        //transform.Translate(x, 0, z);

        //Local Rotation (not using)

        //if (Input.GetKey(KeyCode.E))
        //{
        //
        //    transform.Rotate(0, 150 * Time.deltaTime, 0);
        //}
        //if (Input.GetKey(KeyCode.Q))
        //{
        //
        //    transform.Rotate(0, -150 * Time.deltaTime, 0);
        //}

        //Jump

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    rb.AddForce(transform.up * 40000 * JumpHeight * Time.deltaTime);
        //
        //}



        //GroundControl

        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(transform.position, -transform.up, out hit, 10))
        {

            distanceToGround = hit.distance;
            Groundnormal = hit.normal;

            if (distanceToGround <= 0.2f)
            {
                OnGround = true;
            }
            else
            {
                OnGround = false;
            }


        }


        //GRAVITY and ROTATION

        Vector3 gravDirection = (transform.position - Planet.transform.position).normalized;

        if (OnGround == false)
        {
            // thanks to using that PlanetScript GetComponent thing in Update, PlanetScript.PlanetGravity is referencing the current planet's gravity!
            rb.AddForce(gravDirection * PlanetScript.PlanetGravity * GravityScript.gravityScale);

        }

        //

        Quaternion toRotation = Quaternion.FromToRotation(transform.up, Groundnormal) * transform.rotation;
        transform.rotation = toRotation;



    }


    //CHANGE PLANET

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform != Planet.transform)
        {

            Planet = collision.transform.gameObject;

            Vector3 gravDirection = (transform.position - Planet.transform.position).normalized;

            Quaternion toRotation = Quaternion.FromToRotation(transform.up, gravDirection) * transform.rotation;
            transform.rotation = toRotation;

            rb.velocity = Vector3.zero;

            // I believe that PlanetScript.PlanetGravity needs to be reversed now (by the - symbol) because it is flinging the player up, towards the new planet
            // I added the gravityScale multiplier because i thought it made sense. keep an eye on it in case problems with planet changing occur
            rb.AddForce(gravDirection * -PlanetScript.PlanetGravity);


            PlayerPlaceholder.GetComponent<TutorialPlayerPlaceholder>().NewPlanet(Planet);

        }
    }


}