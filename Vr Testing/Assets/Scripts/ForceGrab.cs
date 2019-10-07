﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class ForceGrab : MonoBehaviour
{
    public GameObject oppositeHand;
    public float sphereCastRadius = 1.0f;
    public float maxDistance = 100.0f;
    public float dragAmount = 20.0f;
    public float lerpSpeed = 0.5f;
    public float ModSpeed = 3.5f;

    private float currentHoldDist;
    private Vector3 currentVelocity;
    private bool isForceGrabbing = false;

    public SteamVR_Action_Boolean Grab;
    public SteamVR_Action_Boolean Pull;
    public SteamVR_Action_Boolean Push;

    public Transform raySource;
    private Vector3 ForceHoldPos;
    public GameObject grabbedObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ForceRay = new Ray(raySource.position, ((transform.up * -1.0f) + transform.forward));
        Debug.DrawRay(raySource.position, ((transform.up * -1.0f) + transform.forward) * maxDistance, Color.blue);
        if (Grab.state && !isForceGrabbing && Physics.Raycast(raySource.position,((transform.up * -1.0f) + transform.forward), out hit, maxDistance))
        {
            
            if (hit.collider.tag == "Interactible")
            {
                if (Grab.state)
                {
                    Debug.Log("Force Grabbing Object");
                    grabbedObject = hit.collider.gameObject;
                    hit.rigidbody.useGravity = false;
                    hit.rigidbody.drag = dragAmount;
                    currentHoldDist = Vector3.Distance(raySource.position, hit.collider.transform.position);
                    isForceGrabbing = true;
                }
            }
        }
        else if (isForceGrabbing && Grab.state)
        {
            if (Push.state)
            {
                currentHoldDist = currentHoldDist + ModSpeed;
            }
            else if (Pull.state)
            {
                currentHoldDist = currentHoldDist - ModSpeed;
            }

            
            Vector3 heading = ForceRay.GetPoint(currentHoldDist) - grabbedObject.GetComponent<Rigidbody>().position;
            float distance = Vector3.Distance(ForceRay.GetPoint(currentHoldDist), grabbedObject.GetComponent<Rigidbody>().position);
            grabbedObject.GetComponent<Rigidbody>().AddForce(heading * (lerpSpeed * distance));
        }
        else if (isForceGrabbing && !Grab.state)
        {
            grabbedObject.GetComponent<Rigidbody>().useGravity = true;
            grabbedObject.GetComponent<Rigidbody>().drag = 0.0f;
            grabbedObject = null;
            isForceGrabbing = false;
        }
    }
}
