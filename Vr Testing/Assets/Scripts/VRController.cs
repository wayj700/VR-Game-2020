using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class VRController : MonoBehaviour
{
    public float m_Gravity = 30.0f;
    private float m_Sensitivity = 0.1f;
    public float m_MaxSpeed = 1.0f;
    public float m_RotateIncrement = 90;
    public SteamVR_Action_Boolean m_MovePress = null;
    public SteamVR_Action_Vector2 m_MoveValue = null;

    private float m_Speed = 0.0f;
    private CharacterController m_CharacterController = null;

    public Transform CameraRig;
    public Transform Head;
        public float runSpeed = 1.0f;
        public float walkSpeed = 0.6f;

        private void Awake()
        {
            m_CharacterController = GetComponent<CharacterController>();
        }

        private void Start()
        {
        
        }

        private void Update()
        {
            HandleHeight();
            CalulateMovement();
        AnimateHandWithoutController();
        }

        private void HandleHead()
        {
            Vector3 oldPosition = Head.position;
            Quaternion oldRotation = Head.rotation;


            transform.eulerAngles = new Vector3(0.0f, Head.rotation.eulerAngles.y, 0.0f);

        Head.position = oldPosition;
        Head.rotation = oldRotation;
        }

        private void HandleHeight()
        {
            // get the head in local space
            float headHeight = Mathf.Clamp(Head.localPosition.y, 1, 2);
            m_CharacterController.height = headHeight;

            // Cut in half
            Vector3 newCenter = Vector3.zero;
            newCenter.y = m_CharacterController.height / 2;
            newCenter.y += m_CharacterController.skinWidth;

            // Move capsule in local space
            newCenter.x = Head.localPosition.x;
            newCenter.z = Head.localPosition.z;

            // Rotate
            newCenter = Quaternion.Euler(0, -transform.eulerAngles.y, 0) * newCenter;

            // Apply
            m_CharacterController.center = newCenter;
        }

    private void CalulateMovement()
        {
            // Figure out movemnet orientation
            Vector3 orientationEuler = new Vector3(0, transform.eulerAngles.y, 0);
            Quaternion orientation = Quaternion.Euler(orientationEuler);
            Vector3 movement = Vector3.zero;
            Vector2 move = (m_MoveValue.GetAxis(SteamVR_Input_Sources.Any));

            

            // If button pressed, run
            if (m_MovePress.state)
            {
                m_Sensitivity = runSpeed;
            }
            else
            {
                m_Sensitivity = walkSpeed;
            }

            // Calculate movement
            if (m_MoveValue.axis.y != 0)
            {
                Debug.Log("is moving");
                // Orientation
                Vector3 newMove = new Vector3(m_MoveValue.axis.x * m_Sensitivity, 0, m_MoveValue.axis.y * m_Sensitivity);
                movement = orientation * newMove * Time.deltaTime;
            }
            else
            {
                movement = new Vector3(0f, 0f, 0f);
            }

            // Gravity
            movement.y -= m_Gravity * Time.deltaTime;

            // Apply
            m_CharacterController.Move(movement * Time.deltaTime);
        }

    public void AnimateHandWithoutController()
    {
        for (int handIndex = 0; handIndex < Player.instance.hands.Length; handIndex++)
        {
            Hand hand = Player.instance.hands[handIndex];
            if (hand != null)
            {
                hand.SetSkeletonRangeOfMotion(Valve.VR.EVRSkeletalMotionRange.WithoutController);
                //Debug.Log("Boom BAP");
            }
        }
    }
}

