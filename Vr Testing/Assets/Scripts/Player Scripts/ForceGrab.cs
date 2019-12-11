using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class ForceGrab : MonoBehaviour
{
    public GameObject oppositeHand;
    public float forceMaxSpeed = 5.0f;
    public float minDistance = 1.0f;
    public float maxDistance = 25.0f;
    public float dragAmount = 20.0f;
    public float lerpSpeed = 0.5f;
    public float ModSpeed = 0.1f;

    private float currentHoldDist;
    private Vector3 currentVelocity;
    private bool isForceGrabbing = false;

    public SteamVR_Action_Boolean Grab;
    public SteamVR_Action_Boolean Pull;
    public SteamVR_Action_Boolean Push;

    public Transform raySource;
    public Transform headPos;
    public Transform shoulderPos;
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
        if (this.Grab.state && !this.isForceGrabbing && Physics.Raycast(raySource.position,((transform.up * -1.0f) + transform.forward), out hit, maxDistance))
        {
            //collects object
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
        else if (this.isForceGrabbing && this.Grab.state)
        {
            Rigidbody grabbedRigid = grabbedObject.GetComponent<Rigidbody>(); 

            //calculates pushing and pulling of object relative to the hand's disntance from head
            currentHoldDist = Vector3.Distance(new Vector3(shoulderPos.position.x, shoulderPos.position.y, shoulderPos.position.z), new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z)) * ModSpeed;

            //calculates direction from object towards desired position
            Vector3 heading = ForceRay.GetPoint(currentHoldDist) - grabbedObject.GetComponent<Rigidbody>().position;

            //sets distance of held object relative to hand's distance from guessed shoulder position
            float distance = Vector3.Distance(ForceRay.GetPoint(Mathf.Clamp(currentHoldDist, minDistance, maxDistance)), grabbedObject.GetComponent<Rigidbody>().position);

            //adds calculated force to object, moving it towards desired position
            grabbedRigid.AddForce(heading * Mathf.Clamp(lerpSpeed * distance * grabbedObject.GetComponent<Rigidbody>().mass, 0, forceMaxSpeed * grabbedObject.GetComponent<Rigidbody>().mass) * Time.deltaTime * 100);


            Vector3 rotationHeading = new Vector3(0, this.transform.rotation.y, 0) - new Vector3(grabbedRigid.rotation.x, grabbedRigid.rotation.y, grabbedRigid.rotation.y);
            grabbedRigid.angularDrag = dragAmount;
            grabbedRigid.AddTorque(rotationHeading * lerpSpeed * Time.deltaTime * 10);


            //Quaternion rotation = Quaternion.Euler(transform.rotation.x, 0 , transform.rotation.y) * Quaternion.Euler(0, 0, 0);
            //grabbedRigid.MoveRotation(rotation);


        }
        else if (this.isForceGrabbing && !this.Grab.state)
        {
            //releases object
            grabbedObject.GetComponent<Rigidbody>().useGravity = true;
            grabbedObject.GetComponent<Rigidbody>().drag = 0.0f;
            grabbedObject.GetComponent<Rigidbody>().angularDrag = 0.0f;
            grabbedObject = null;
            this.isForceGrabbing = false;
        }
    }
}
