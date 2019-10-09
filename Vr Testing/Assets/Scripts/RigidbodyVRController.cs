using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class RigidbodyVRController : MonoBehaviour
{
    private float maxSpeed;
    private float currentMaxSpeed;
    public float lerpSpeed = 1.5f;
    public float walkMaxSpeed = 10.0f;
    public float runMaxSpeed = 5.0f;
    public float Sensitivity = 1.0f;
    public float walkSpeed = 0.6f;
    public float runSpeed = 1.0f;
    public float jumpForce = 5.0f;

    private bool isGrounded = true;

    public SteamVR_Action_Boolean MoveClick = null;
    public SteamVR_Action_Vector2 MoveStick = null;
    public SteamVR_Action_Boolean JumpClick = null;

    public Transform head;

    private Rigidbody CharacterRigidbody;
    private CapsuleCollider PlayerCollider;

    private void Awake()
    {
        CharacterRigidbody = GetComponent<Rigidbody>();
        PlayerCollider = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleHeight();
    }

    void FixedUpdate()
    {
        CalculateMovement();
    }

    private void HandleHeight()
    {
        float headHeight = Mathf.Clamp(head.localPosition.y, 1, 2);
        PlayerCollider.height = head.position.y - transform.position.y;

        Vector3 newCenter = Vector3.zero;
        newCenter.y = PlayerCollider.height / 2;

        newCenter.x = head.localPosition.x;
        newCenter.z = head.localPosition.z;

        newCenter = Quaternion.Euler(0, -transform.eulerAngles.y, 0) * newCenter;

        PlayerCollider.center = newCenter;
    }

    private void CalculateMovement()
    {
        Vector3 orientationEuler = new Vector3(0, head.eulerAngles.y, 0);
        Quaternion orientation = Quaternion.Euler(orientationEuler);
        Vector3 movement = Vector3.zero;
        Vector3 newMove = Vector3.zero;

        //Set max speed of charcter
        if (CharacterRigidbody.velocity.magnitude > currentMaxSpeed && isGrounded)
        {
            currentMaxSpeed = Mathf.Lerp(currentMaxSpeed, maxSpeed, lerpSpeed);
            CharacterRigidbody.velocity = CharacterRigidbody.velocity.normalized * currentMaxSpeed;
        }
        else
        {
            currentMaxSpeed = maxSpeed;
        }

        // If button pressed, run
        if (MoveClick.state)
        {
            Sensitivity = runSpeed;
            maxSpeed = runMaxSpeed;
        }
        else
        {
            Sensitivity = walkSpeed;
            maxSpeed = walkMaxSpeed;
        }

        //Calculates movement based on joystick axis and orientation of head
        if (MoveStick.axis.y != 0 && isGrounded)
        {
            newMove = orientation * (new Vector3(MoveStick.axis.x * Sensitivity, 0, MoveStick.axis.y * Sensitivity));
        }
        else
        {
            newMove = Vector3.zero;
        }

        //On button press, character jumps
        if (JumpClick.state && isGrounded)
        {
            newMove += new Vector3(0, jumpForce, 0);
            Debug.Log("Jumped");
        }

        //Apply
        CharacterRigidbody.AddForce(newMove);
    }

    private void OnCollisionStay(Collision collision)
    {
        //CheckIfGrounded();
        if (collision.gameObject.tag != "unwalkable")
        {
            isGrounded = true;
        }
    }

    private void CheckIfGrounded()
    {
        RaycastHit[] hits;

        //Raycast 1 down to check for collider
        Vector3 positionToCheck = new Vector3(head.position.x, 0 , head.position.z);
        hits = Physics.RaycastAll(positionToCheck, new Vector3(0, -0.1f, 0), 0.1f);
        Debug.DrawRay(positionToCheck, new Vector3(0, -0.1f, 0), Color.red);


        //If collider hit, we are grounded
        if (hits.Length > 0)
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
}
