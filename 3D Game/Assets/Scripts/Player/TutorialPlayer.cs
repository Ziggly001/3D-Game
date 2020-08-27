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

    bool OnGround = false;


    float distanceToGround;
    Vector3 Groundnormal;

    Vector3 RotNormal;

    // The player will allign themself to the first object the raycast hits that is on this layer. Put floor planes and planet cores on this layer.
    public LayerMask OrbitRotMask;


    private Rigidbody rb;

    private CustomGravity GravityScript;

    public bool RotRayHit;

    public Vector3 gravDirection;

    public float gravChangeSpeed = 0.1f;

    public bool InSpace = true;

    public int gravFieldsTouching;

    public LayerMask GravityField;

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

        //Local Y Rotation (not planning on using as I want to go off of camera)

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

        // Orbital rotation

        RaycastHit OrbitRot = new RaycastHit();
        if (Physics.Raycast(transform.position, -transform.up, out OrbitRot, 10, OrbitRotMask))
        {

            RotNormal = OrbitRot.normal;

            RotRayHit = true;

        }
        else
        {

            RotRayHit = false;

        }


        //GRAVITY and ROTATION

        // Only applies gravity if not in space
        if (InSpace == false)
        {



            // Tests if the current planet has a "FlatGravity" component. If it does, then it uses flat gravity. else, it uses spherical gravity.
            if (Planet.GetComponent<FlatGravity>() != null)
            {
                // use flat gravity

                //Vector3 gravDirection = Planet.transform.localRotation.eulerAngles;
                //Vector3 gravDirection = transform.up.normalized;

                if (RotRayHit == true)
                {
                    // Rotation and gravity based off normals
                    gravDirection = (RotNormal).normalized;

                    // This method causes instant rotation change. It's unwanted but all I have.
                    Quaternion toRotation = Quaternion.FromToRotation(transform.up, RotNormal) * transform.rotation;
                    transform.rotation = toRotation;
                }
                else
                {
                    // Sets player's rotation to grav body's rotation, excluding y axis
                    // (Doesn't work; odd stuff happens when player attempts to rotate on y axis)
                    /*
                    Vector3 gravBodyRot = Planet.transform.rotation.eulerAngles;
                    transform.InverseTransformDirection(gravBodyRot);
                    Vector3 NewPlayerRot = new Vector3(gravBodyRot.x, transform.localEulerAngles.y, gravBodyRot.z);
                    transform.TransformDirection(NewPlayerRot);
                    Quaternion NewRotQ = Quaternion.Euler(NewPlayerRot);
                    transform.rotation = NewRotQ;


                    gravDirection = transform.up.normalized;
                    */
                }

                if (OnGround == false)
                {
                    // thanks to using that PlanetScript GetComponent thing in Update, PlanetScript.PlanetGravity is referencing the current planet's gravity!
                    rb.AddForce(gravDirection * PlanetScript.PlanetGravity * GravityScript.gravityScale);

                }
            }
            else
            {
                // use planet gravity

                Vector3 oldGravDirection;

                // use Vector3.Lerp to smooth between rotations and gravity directions (less jitter)

                if (RotRayHit == true)
                {

                    oldGravDirection = gravDirection;

                    //old (will probably keep)
                    gravDirection = (RotNormal).normalized;

                    //new (possibly broken)
                    //gravDirection = Vector3.Lerp(oldGravDirection, (RotNormal).normalized, gravChangeSpeed);

                    //Debug.Log("Raycast Hit!");

                    // Rotate based on normals
                    // This method causes instant rotation change. It's unwanted but all I have.
                    Quaternion toRotation = Quaternion.FromToRotation(transform.up, RotNormal) * transform.rotation;

                    transform.rotation = toRotation;

                }
                else
                {
                    oldGravDirection = gravDirection;

                    //old (will probably keep)
                    gravDirection = (transform.position - Planet.transform.position).normalized;

                    //new (possibly broken)
                    //gravDirection = Vector3.Lerp(oldGravDirection, (transform.position - Planet.transform.position).normalized, gravChangeSpeed);

                    // Rotate based on difference between player and planet rotation
                    // setting rotation to gravDirection just made the player's rotation essentially 0, whatever, 0.
                }


                if (OnGround == false)
                {
                    // thanks to using that PlanetScript GetComponent thing in Update, PlanetScript.PlanetGravity is referencing the current planet's gravity!
                    rb.AddForce(gravDirection * PlanetScript.PlanetGravity * GravityScript.gravityScale);

                }


                // Here I rotate the player

                // used to rotate player here, but I now do different rotation if rotrayhit is false

            }
            //

            // This code alligns the player to the normal below them. I don't want this, so I commented it out.
            //Quaternion toRotation = Quaternion.FromToRotation(transform.up, Groundnormal) * transform.rotation;
            //transform.rotation = toRotation;

        }

        // SPACE PHYSICS
        if (InSpace == true)
        {
            //rb.velocity = Vector3.zero;
        }

    }



    //CHANGE PLANET

    private void OnTriggerEnter(Collider collision)
    {
        // Checks if the collider is on the GravityField layer (basically; is it a gravity field?)
        if (((1 << collision.gameObject.layer) & GravityField.value) != 0)
        {
            // Adds 1 to the gravFieldsTouching float upon entering a gravity field
            gravFieldsTouching++;

            // If touching multiple or 1 gravity fields, InSpace is false.
            if (gravFieldsTouching > 1 || gravFieldsTouching == 1)
            {
                InSpace = false;
            }

            // Continues if the collider isn't the current gravity field
            if (collision.transform != Planet.transform)
            {

                // Tests if the new planet has a "FlatGravity" component. If it does, then it uses flat gravity. else, it uses spherical gravity.
                if (collision.GetComponent<FlatGravity>() != null)
                {
                    // use flat gravity

                    Planet = collision.transform.gameObject;

                    // this might mess stuff up when you fly a ship to a flat gravity body, so I plan to comment it out.
                    //Vector3 gravDirection = transform.up.normalized;


                    // commented this out because I should only do this in update, and only if RotRayHit is false
                    /*
                    // alligns player to surface of flat thing
                    //
                    // takes player's world rotation and converts it to local
                    Vector3 PlayerRot = transform.rotation.eulerAngles;
                    transform.InverseTransformDirection(PlayerRot);

                    // gets planet object's world rotation and converts it to this object's local space
                    Vector3 PlatformRot = Planet.transform.rotation.eulerAngles;
                    transform.InverseTransformDirection(PlatformRot);

                    // sets players rotation to planet object rotation, without changing local y rotation
                    Vector3 NewPlayerRot = new Vector3(PlatformRot.x, PlayerRot.y, PlatformRot.z);
                    transform.TransformDirection(NewPlayerRot);
                    Quaternion NewRotQ = Quaternion.Euler(NewPlayerRot);
                    transform.SetPositionAndRotation(transform.position, NewRotQ);
                    //
                    */


                    // sets velocity to zero
                    rb.velocity = Vector3.zero;

                    // tutorial used this and I don't see a reason to remove it. Figured I won't tamper with it.
                    PlayerPlaceholder.GetComponent<TutorialPlayerPlaceholder>().NewPlanet(Planet);
                }
                else
                {
                    // use planet gravity

                    Planet = collision.transform.gameObject;

                    // commented out these statements setting gravDirection at OnTriggerEnter. they would mess up incoming ships.
                    //Vector3 gravDirection = (transform.position - Planet.transform.position).normalized;

                    // this would rotate the player to have their feet facing the planet's center upon entrance, but that would mess with players in ships.
                    //Quaternion toRotation = Quaternion.FromToRotation(transform.up, gravDirection) * transform.rotation;
                    //transform.rotation = toRotation;

                    rb.velocity = Vector3.zero;


                    PlayerPlaceholder.GetComponent<TutorialPlayerPlaceholder>().NewPlanet(Planet);
                }

            }

        }
    }



    private void OnTriggerExit(Collider collision)
    {
        // This if statement checks if the trigger's layer is the same as the mask layer. (same if statement as before)
        if (((1 << collision.gameObject.layer) & GravityField.value) != 0)
        {
            // Removes 1 from the gravFieldsTouching float upon leaving a gravity field
            gravFieldsTouching--;

            if (gravFieldsTouching < 1)
            {
                // If not in any gravity fields, InSpace is true
                InSpace = true;

            }

            if (InSpace == true)
            {

                rb.velocity = Vector3.zero;

            }

        }

        
    }


}