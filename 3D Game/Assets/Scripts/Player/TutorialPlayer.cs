using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPlayer : MonoBehaviour
{

    public GameObject Planet;
    public GameObject PlayerPlaceholder;

    public GameObject GroundCheck;


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

    // initializes "FlatGravity"
    private FlatGravity FlatGravity;

    // Update is called once per frame
    void Update()
    {

        // States that "PlanetScript" is the "Planet" class from the Planet script
        PlanetScript = Planet.GetComponent<Planet>();

        // States that "FlatGravity" is the "FlatGravity" class from the FlatGravity script
        FlatGravity = Planet.GetComponent<FlatGravity>();

        //MOVEMENT

        float x = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        float z = Input.GetAxis("Vertical") * Time.deltaTime * speed;

        transform.Translate(x, 0, z);

        //Local Rotation (not planning on using)

        if (Input.GetKey(KeyCode.E))
        {
        
            transform.Rotate(0, 150 * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.Q))
        {
        
            transform.Rotate(0, -150 * Time.deltaTime, 0);
        }

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

        // Tests if the current planet has a "FlatGravity" component. If it does, then it uses flat gravity. else, it uses spherical gravity.
        if (Planet.GetComponent<FlatGravity>() != null)
        {
            // use flat gravity

            Vector3 PlayerY = new Vector3(0f, Planet.transform.localPosition.y, 0f);
            // I'm subtracting the y component of the players position from the players position to get a direction. Maybe multiply PlayerY?
            // I mutiply PlayerY by 2 so I'm not just removing the y component; I'm making it lower.
            Vector3 gravDirection = (PlayerY * 2).normalized;

            if (OnGround == false)
            {
                // thanks to using that PlanetScript GetComponent thing in Update, PlanetScript.PlanetGravity is referencing the current planet's gravity!
                rb.AddForce(gravDirection * PlanetScript.PlanetGravity * GravityScript.gravityScale);

            }
        }
        else
        {
            // use planet gravity

            Vector3 gravDirection = (transform.position - Planet.transform.position).normalized;

            if (OnGround == false)
            {
                // thanks to using that PlanetScript GetComponent thing in Update, PlanetScript.PlanetGravity is referencing the current planet's gravity!
                rb.AddForce(gravDirection * PlanetScript.PlanetGravity * GravityScript.gravityScale);

            }
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
            // Tests if the new planet has a "FlatGravity" component. If it does, then it uses flat gravity. else, it uses spherical gravity.
            if (collision.GetComponent<FlatGravity>() != null)
            {
                // use flat gravity

                Planet = collision.transform.gameObject;

                Vector3 PlayerY = new Vector3(0f, Planet.transform.localPosition.y, 0f);
                // I'm subtracting the y component of the players position from the players position to get a direction. Maybe multiply PlayerY?
                // I mutiply PlayerY by 2 so I'm not just removing the y component; I'm making it lower.
                Vector3 gravDirection = (PlayerY * 2).normalized;

                Quaternion toRotation = Quaternion.FromToRotation(transform.up, gravDirection) * transform.rotation;
                transform.rotation = toRotation;

                rb.velocity = Vector3.zero;

                // I believe that PlanetScript.PlanetGravity needs to be reversed now (by the - symbol) because it is flinging the player up, towards the new planet
                // I added the gravityScale multiplier because i thought it made sense. keep an eye on it in case problems with planet changing occur
                rb.AddForce(gravDirection * -PlanetScript.PlanetGravity);


                PlayerPlaceholder.GetComponent<TutorialPlayerPlaceholder>().NewPlanet(Planet);
            }
            else
            {
                // use planet gravity

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


}