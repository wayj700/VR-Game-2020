using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class PlayerController : MonoBehaviour
{
    public SteamVR_Action_Vector2 MoveStick = null;

    // The point it's orbitting.
    [SerializeField] Transform anchor;

    // Speed it rotates around the anchor. Modifier for the move.
    float speed = 75;

    //This comment is just me complaining that I coded this shit from scratch, and then realized Unity had a function FOR me, which works better anways :(. I was proud too lol.
    void Update()
    {
        if (MoveStick.axis.x <= -0.07 || MoveStick.axis.x >= 0.07)
        {
            if (MoveStick.axis.x > 0)
            {
                transform.RotateAround(anchor.position, Vector3.up, speed * Time.deltaTime);
            } else
            {
                transform.RotateAround(anchor.position, Vector3.up, -speed * Time.deltaTime);
            }
        }
    }
}
