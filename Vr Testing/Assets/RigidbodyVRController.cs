using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class RigidbodyVRController : MonoBehaviour
{

    public float maxSpeed = 1.0f;
    public float Sensitivity = 1.0f;
    public float walkSpeed = 0.6f;
    public float runSpeed = 1.0f;

    public SteamVR_Action_Boolean MoveClick = null;
    public SteamVR_Action_Vector2 MoveStick = null;

    public Transform head;

    private Rigidbody CharacterRigidbody;
    private CapsuleCollider PlayerCollider;

    private void Awake()
    {
        CharacterRigidbody = GetComponent<Rigidbody>();
        PlayerCollider = GetComponent<CapsuleCollider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleHeight();
        CalculateMovement();
    }

    private void HandleHeight()
    {
        float headHeight = Mathf.Clamp(head.localPosition.y, 1, 2);
        PlayerCollider.height = head.position.y;

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

        // If button pressed, run
        if (MoveClick.state)
        {
            Sensitivity = runSpeed;
        }
        else
        {
            Sensitivity = walkSpeed;
        }

        if (MoveStick.axis.y != 0)
        {
            newMove = new Vector3(MoveStick.axis.x * Sensitivity, 0, MoveStick.axis.y * Sensitivity);
            movement = orientation * (newMove * Sensitivity) * Time.deltaTime;
            
            //movement += orientation * (Sensitivity * Vector3.forward) * Time.deltaTime;
        }
        else
        {
            movement = Vector3.zero;
        }

        CharacterRigidbody.AddForce(orientation * newMove);
        Debug.Log("Player is moving Movement: " + movement + " newMove: " + newMove + " orientation" + orientation);
    }
}
