using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class RigidbodyVRController : MonoBehaviour
{
    public float health = 100.0f;
    private float maxSpeed;
    private float currentMaxSpeed;
    public float lerpSpeed = 1.5f;
    public float walkMaxSpeed = 10.0f;
    public float runMaxSpeed = 5.0f;
    public float wallSlideSpeed = 2.0f;
    public float Sensitivity = 1.0f;
    public float walkSpeed = 0.6f;
    public float runSpeed = 1.0f;
    public float jumpForce = 400.0f;
    public float doubleJumpForce = 300.0f;
    public float waitTilDoubleJump = 0.75f;
    public float wallJumpFowardVelocity = 200.0f;
    public float wallJumpUpwardsVelocity = 100.0f;

    [SerializeField] private bool isGrounded = true;
    [SerializeField] private bool canDoubleJump = true;
    [SerializeField] private bool touchingWall = false;

    public SteamVR_Action_Boolean MoveClick = null;
    public SteamVR_Action_Vector2 MoveStick = null;
    public SteamVR_Action_Boolean JumpClick = null;

    public GameObject GroundCheck;
    public GameObject Shoulders;

    public Transform head;

    public Transform spawn;

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
        if (health <= 0.0f)
        {
            killPlayer();
        }
    }

    void FixedUpdate()
    {
        HandleHeight();
        CheckIfGrounded();
        //CheckIfTouchingWall();
        CalculateMovement();
    }

    public void killPlayer()
    {
        health = 100.0f;
        this.transform.position = spawn.position;
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

        //Set shoulder position
        Shoulders.transform.position = head.position;
        Shoulders.transform.rotation = orientation;

        //Set max speed of charcter
        if (CharacterRigidbody.velocity.magnitude > currentMaxSpeed && isGrounded)
        {
            currentMaxSpeed = Mathf.Lerp(currentMaxSpeed, maxSpeed, lerpSpeed);
            CharacterRigidbody.velocity = CharacterRigidbody.velocity.normalized * currentMaxSpeed;
        }
        //ditched walljump code. could be fixed later
        /*else if(touchingWall && !isGrounded)
        {
            currentMaxSpeed = wallSlideSpeed;
            currentMaxSpeed = Mathf.Lerp(currentMaxSpeed, maxSpeed, lerpSpeed);
            CharacterRigidbody.velocity = CharacterRigidbody.velocity.normalized * currentMaxSpeed;
        }*/
        else
        {
            currentMaxSpeed = maxSpeed;
        }

        // If button pressed, run
        if (MoveClick.state)
        {
            Sensitivity = runSpeed * CharacterRigidbody.mass;
            maxSpeed = runMaxSpeed;
        }
        else
        {
            Sensitivity = walkSpeed * CharacterRigidbody.mass;
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
        if (JumpClick.state)
        {
            //ditched walljump code. could be fixed later
            /*if (touchingWall && !isGrounded)
            {
                newMove =  orientation * (new Vector3(0, wallJumpUpwardsVelocity, wallJumpFowardVelocity));
                touchingWall = false;
                StartCoroutine("loseTime");
                //Debug.Log("Wall Jumped");
            }
            else */
            if (!isGrounded && canDoubleJump)
            {
                CharacterRigidbody.velocity = new Vector3(CharacterRigidbody.velocity.x, 0, CharacterRigidbody.velocity.z);
                newMove.y += doubleJumpForce * CharacterRigidbody.mass;
                canDoubleJump = false;
                //Debug.Log("Double Jumped");
            }
            else if (isGrounded)
            {
                newMove.y += jumpForce * CharacterRigidbody.mass;
                isGrounded = false;
                StartCoroutine("loseTime");
                //Debug.Log("Jumped");
            }
        }

        //Apply
        CharacterRigidbody.AddForce(newMove * Time.deltaTime * 100);
    }

    private void CheckIfGrounded()
    {
        RaycastHit hit;
        float distance = 1.01f;
        Vector3 origin = new Vector3(head.position.x, 1 + transform.position.y, head.position.z);
        if (Physics.Raycast(origin, Vector3.down, out hit, distance))
        {

            isGrounded = true;
            canDoubleJump = false;
        }
        else
        {
            isGrounded = false;
        }
    }

    //ditched walljump code. could be fixed later
    /*private void CheckIfTouchingWall()
    {
        Vector3 origin = new Vector3(head.position.x, 1 + transform.position.y, head.position.z);
        Collider[] hitColliders = Physics.OverlapSphere(origin, PlayerCollider.radius + 0.05f);
        if (hitColliders.Length != 0)
        {
            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i].gameObject.tag != "Player")
                {
                    Debug.DrawLine(origin, hitColliders[i].transform.position, Color.red);
                    touchingWall = true;
                }
            }
        }
        else
        {
            touchingWall = false;
        }
    }*/

    IEnumerator loseTime()
    {
        yield return new WaitForSeconds(waitTilDoubleJump);
        canDoubleJump = true;
    }

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("collided");
        if (collision.tag == "FallOffCheck")
        {
            Debug.Log("collided with falloff check");
            killPlayer();
        }
    }
}