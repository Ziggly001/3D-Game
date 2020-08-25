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

    Vector3 RotNormal;

    // The player will allign themself to the closest normal with this mask. the core of the planet should be on this layer, and maybe floor planes too (add a plane to each floor, and assign that plane to this layer. since its a plane, the player won't turn sideways when they walk off it.)
    public LayerMask OrbitRotMask;


    private Rigidbody rb;

    private CustomGravity GravityScript;

    public bool RotRayHit;

    public Vector3 gravDirection;

    public float gravChangeSpeed = 0.1f;

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

            // Tests if the current planet has a "FlatGravity" component. If it does, then it uses flat gravity. else, it uses spherical gravity.
            if (Planet.GetComponent<FlatGravity>() != null)
        {
            // use flat gravity

            //Vector3 PlayerY = Planet.transform.localRotation.eulerAngles;

            //Vector3 gravDirection = Planet.transform.localRotation.eulerAngles;
            Vector3 gravDirection = transform.up.normalized;

            if (OnGround == false)
            {
                // thanks to using that PlanetScript GetComponent thing in Update, PlanetScript.PlanetGravity is referencing the current planet's gravity!
                rb.AddForce(gravDirection * PlanetScript.PlanetGravity * GravityScript.gravityScale);

            }
        }
        else
        {
            // use planet gravity

            // This is how gravity would be calculated if it didn't go off of the specified normals. I will still use this when the OrbitRot raycast doesn't hit anything.
            //Vector3 gravDirection = (transform.position - Planet.transform.position).normalized;

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

            }
            else
            {
                oldGravDirection = gravDirection;

                //old (will probably keep)
                gravDirection = (transform.position - Planet.transform.position).normalized;

                //new (possibly broken)
                //gravDirection = Vector3.Lerp(oldGravDirection, (transform.position - Planet.transform.position).normalized, gravChangeSpeed);
            }


            if (OnGround == false)
            {
                // thanks to using that PlanetScript GetComponent thing in Update, PlanetScript.PlanetGravity is referencing the current planet's gravity!
                rb.AddForce(gravDirection * PlanetScript.PlanetGravity * GravityScript.gravityScale);

            }

            // Set player rotation to have feet facing the planet center here ( or at least it would if it didn't make the game angry)
            /*
            // get planet position in local space
            Vector3 PlanetRelative = Planet.transform.position;
            transform.InverseTransformPoint(PlanetRelative);
            // get local space direction between player and planet
            PlanetRelative = PlanetRelative - transform.localPosition;
            // set player's current y rotation to y component of the new rotation
            PlanetRelative = new Vector3(PlanetRelative.x, transform.localRotation.eulerAngles.y, PlanetRelative.z);
            // converts new rotation to world space
            PlanetRelative = transform.TransformDirection(PlanetRelative);
            // converts local forward to world space
            Vector3 WorldSpaceForward = transform.TransformPoint(Vector3.forward);
            // sets player rotation to correct rotation relative to planet
            transform.rotation = Quaternion.LookRotation(PlanetRelative, WorldSpaceForward);
            */

            // Here I could have the player allign to the normal below them, but only if it has less than a 45 degree difference or some buffer like that.

            //old
            Quaternion toRotation = Quaternion.FromToRotation(transform.up, RotNormal) * transform.rotation;

            transform.rotation = toRotation;

        }
        //

        // This code alligns the player to the normal below them. I don't want this, so I commented it out.
        //Quaternion toRotation = Quaternion.FromToRotation(transform.up, Groundnormal) * transform.rotation;
        //transform.rotation = toRotation;



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

                //Vector3 PlayerY = Planet.transform.localRotation.eulerAngles;

                //Vector3 gravDirection = Planet.transform.localRotation.eulerAngles;
                Vector3 gravDirection = transform.up.normalized;

                // Again, I don't want to allign the player to the thing below them; I want to allign them to the planet / surface rotation.
                //Quaternion toRotation = Quaternion.FromToRotation(transform.up, gravDirection) * transform.rotation;
                //transform.rotation = toRotation;

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

                // sets velocity to zero
                rb.velocity = Vector3.zero;

                // I believe that PlanetScript.PlanetGravity needs to be reversed now (by the - symbol) because it is flinging the player up, towards the new planet
                // I added the gravityScale multiplier because i thought it made sense. keep an eye on it in case problems with planet changing occur

                // This isn't mario galaxy, so I don't need this.
                //rb.AddForce(gravDirection * -PlanetScript.PlanetGravity);


                PlayerPlaceholder.GetComponent<TutorialPlayerPlaceholder>().NewPlanet(Planet);
            }
            else
            {
                // use planet gravity

                Planet = collision.transform.gameObject;

                Vector3 gravDirection = (transform.position - Planet.transform.position).normalized;

                // Change this to allign the player's feet to the planet's core.
                Quaternion toRotation = Quaternion.FromToRotation(transform.up, gravDirection) * transform.rotation;
                transform.rotation = toRotation;

                rb.velocity = Vector3.zero;

                // I believe that PlanetScript.PlanetGravity needs to be reversed now (by the - symbol) because it is flinging the player up, towards the new planet
                // I added the gravityScale multiplier because i thought it made sense. keep an eye on it in case problems with planet changing occur

                // This isn't mario galaxy, so I dont need this.
                //rb.AddForce(gravDirection * -PlanetScript.PlanetGravity);


                PlayerPlaceholder.GetComponent<TutorialPlayerPlaceholder>().NewPlanet(Planet);
            }

        }
    }


}