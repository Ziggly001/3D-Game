using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Use this script to visibly rotate the player model in the direction they move
public class PlayerModelRotater : MonoBehaviour
{
    // "turnSpeed" should be a value lower than one. this value smoothes the turning of the player as they input a new movement direction (as I understand)

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //TurnAmount = Mathf.Atan2(move.x, move.z);
        //transform.Rotate(0, TurnAmount * turnSpeed * Time.deltaTime, 0);
    }
}
