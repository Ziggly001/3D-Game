using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSkybox : MonoBehaviour
{
    Renderer renderer;

    //float transparency = 1f;
    float alpha = 0.5f;

    // reference the player and player script
    public GameObject Player;
    private TutorialPlayer PlayerScript;

    public GameObject ThisPlanet;

    float SolidDist;

    public float SolidModifier = 1.3f;

    Color oldColor;
    Color newColor;

    // Start is called before the first frame update
    void Start()
    {
        this.transform.localScale = transform.parent.transform.localScale * 2.375f;
        renderer = GetComponent<Renderer>();

        // states that the PlayerScript is the TutorialPlayer script on the Player GameObject
        PlayerScript = Player.GetComponent<TutorialPlayer>();

        // SolidDist is a float. it describes the distance from the planet's center at which the sky shouldn't change opacity.
        // I use x of the localScale because the scaling on all axes is the same for my planets, so x, y, and z of "localScale" should all be the same value.
        // I multiply the x scale by 0.5 to get the radius of the planet. I then multiply that radius by a decided modifier to get the distance at which the opacity shouldn't change.
        SolidDist = transform.parent.transform.localScale.x * 0.5f * SolidModifier;
    }

    // Update is called once per frame
    void Update()
    {
        // Add an If statement that tests for the player to be far enough away from the planet to start making the SkySphere less visible
        // set alpha based off of distance from center of planet. farther distance = smaller alpha value.
        // this should be in the else statement of the planned below if statement.



        // make sure to clamp the alpha variable to be between 0 and 1

        // Add an If statement that tests for player being WITHIN the "don't change opacity" distance from planet's center.
        // "don't change opacity" distance should be a variable, set to be a distance proportional to the size of the planet
        // So Basically, planet's size times some number not too larger than one.
        // If player's distance is less than this variable, then set the alpha to a specific value. I'm thinking maybe somewhat see-through, but just a bit.




        // Sets the opacity of the skybox to zero if this isn't the current planet
        if (PlayerScript.Planet.transform != ThisPlanet.transform)
        {
            oldColor = renderer.material.color;
            newColor = new Color(oldColor.r, oldColor.g, oldColor.b, 0);

            renderer.material.color = newColor;
        }
        else
        {
            // If this is the current planet, sets the skybox opacity based on distance

            float DistToPlanetCenter = Vector3.Distance(Player.transform.position, ThisPlanet.transform.position);

            this.transform.rotation = Player.transform.rotation;

            if (DistToPlanetCenter > SolidDist)
            {

                //change alpha value based off distance

                alpha = (DistToPlanetCenter / SolidDist);

                oldColor = renderer.material.color;
                newColor = new Color(oldColor.r, oldColor.g, oldColor.b, alpha);

                renderer.material.color = newColor;

            }
            else
            {
                oldColor = renderer.material.color;
                newColor = new Color(oldColor.r, oldColor.g, oldColor.b, 1);

                renderer.material.color = newColor;
            }
        }
    }
}
