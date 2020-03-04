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
    public float rotateSpeed = 90.0f;

    private float currentHoldDist;
    private Vector3 currentVelocity;
    private bool isForceGrabbing = false;

    Shader notHighlighted;
    Shader highlighted;

    public SteamVR_Action_Boolean Grab;
    public SteamVR_Action_Boolean Pull;
    public SteamVR_Action_Boolean Push;

    public Transform raySource;
    public Transform headPos;
    public Transform shoulderPos;
    private Vector3 ForceHoldPos;
    [HideInInspector]
    public GameObject grabbedObject;
    [HideInInspector]
    public GameObject hitObject;
    public bool hitting = false;

    // Start is called before the first frame update
    void Start()
    {
        notHighlighted = Shader.Find("Default");
        highlighted = Shader.Find("Outlined/Silhouetted Diffuse");
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ForceRay = new Ray(raySource.position, ((transform.up * -1.0f) + transform.forward));
        Debug.DrawRay(raySource.position, ((transform.up * -1.0f) + transform.forward) * maxDistance, Color.blue);
        if (!this.isForceGrabbing && Physics.Raycast(raySource.position, ((transform.up * -1.0f) + transform.forward), out hit, maxDistance))
        {
            //Outlines the object if it is interactible
            if (hit.collider.tag == "Interactible")
            {
                GameObject go = hit.transform.gameObject;

                //if nothing is highlighted, highlight hit object
                if (hitObject == null)
                {
                    go.SendMessage("onHitEnter");
                }
                //if object hit does not equal last hit object, de-highlight last hit and highlight hit
                else if (hitObject.GetInstanceID() != go.GetInstanceID())
                {
                    hitObject.SendMessage("onHitExit");
                    go.SendMessage("onHitEnter");
                }

                hitting = true;
                hitObject = go;
            }
            //if what's hitting isn't interactible, turn off whatever was hit last
            else if (hitting && hitObject != null)
            {
                hitObject.SendMessage("onHitExit");
                hitObject = null;
            }

            //collects object
            if (this.Grab.state && hit.collider.tag == "Interactible" && !hit.collider.GetComponent<Interactible>().isGrabbed)
            {
                if (Grab.state)
                {
                    //Debug.Log("Force Grabbing Object");
                    grabbedObject = hit.collider.gameObject;
                    hit.rigidbody.useGravity = false;
                    hit.rigidbody.drag = dragAmount;
                    currentHoldDist = Vector3.Distance(raySource.position, hit.collider.transform.position);
                    isForceGrabbing = true;
                    hit.collider.GetComponent<Interactible>().isGrabbed = true;
                }
            }
        }
        //moves and rotates object once attached
        else if (this.isForceGrabbing && this.Grab.state && grabbedObject != null)
        {
            float grabbedRotateY = 0;
            float grabbedRotateZ = 0;
            Rigidbody grabbedRigid = grabbedObject.GetComponent<Rigidbody>();

            //calculates pushing and pulling of object relative to the hand's disntance from head
            currentHoldDist = Vector3.Distance(new Vector3(shoulderPos.position.x, shoulderPos.position.y, shoulderPos.position.z), new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z)) * ModSpeed;
            Debug.Log(currentHoldDist);

            //calculates direction from object towards desired position
            Vector3 heading = ForceRay.GetPoint(currentHoldDist) - grabbedObject.GetComponent<Rigidbody>().position;

            //sets distance of held object relative to hand's distance from guessed shoulder position
            float distance = Vector3.Distance(ForceRay.GetPoint(Mathf.Clamp(currentHoldDist, minDistance, maxDistance)), grabbedObject.GetComponent<Rigidbody>().position) * ModSpeed;

            //adds calculated force to object, moving it towards desired position
            grabbedRigid.AddForce(heading * Mathf.Clamp(lerpSpeed * distance * grabbedObject.GetComponent<Rigidbody>().mass, 0, forceMaxSpeed * grabbedObject.GetComponent<Rigidbody>().mass) * Time.deltaTime * 100);

            //changes angle that grabbed object is rotated to on button press
            if (Pull.state)
            {
                grabbedRotateY -= rotateSpeed;
            }
            else if (Push.state)
            {
                grabbedRotateZ -= rotateSpeed;
            }

            //rotates the grabbed object to desired rotation
            Vector3 rotationHeading = new Vector3(0, grabbedRotateY, grabbedRotateZ) - new Vector3(grabbedRigid.rotation.x, grabbedRigid.rotation.y, grabbedRigid.rotation.y);
            grabbedRigid.angularDrag = dragAmount;
            grabbedRigid.AddRelativeTorque(rotationHeading * lerpSpeed * Time.deltaTime * 10);
        }
        //releases object, resets variables, and turns off outline
        else if (this.isForceGrabbing && !this.Grab.state && grabbedObject != null)
        {
            grabbedObject.GetComponent<Rigidbody>().useGravity = true;
            grabbedObject.GetComponent<Rigidbody>().drag = 0.0f;
            grabbedObject.GetComponent<Rigidbody>().angularDrag = 0.0f;
            grabbedObject.GetComponent<Interactible>().isGrabbed = false;
            grabbedObject = null;
            hitObject.SendMessage("onHitExit");
            hitting = false;
            hitObject = null;
            this.isForceGrabbing = false;
        }
        //Gets rid of outline if raycast isn't hitting anything
        else if (hitting && hitObject != null)
        {
            hitObject.SendMessage("onHitExit");
            hitObject = null;
            hitting = false;
        }
    }
}
