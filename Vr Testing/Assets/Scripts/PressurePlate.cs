using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private bool isPressedDown;
    [SerializeField] private bool hasBeenPressed;
    private GameObject button;

    public UnityEvent onButtonDown;
    public UnityEvent onButtonUp;
    public UnityEvent onButtonPressed;


    // Start is called before the first frame update
    void Start()
    {
        //collects the button plate itself and defines it
        button = gameObject.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasBeenPressed == true && onButtonPressed != null)
        {
            onButtonPressed.Invoke();
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        hasBeenPressed = true;
        if(collision.gameObject == button)
        {
            isPressedDown = true;
            if (onButtonDown != null)
            {
                onButtonDown.Invoke();
            }

        }
    }

    private void OnCollisionExit(Collision collision)
    {
        isPressedDown = false;
        if (onButtonUp != null)
        {
            onButtonUp.Invoke();
        }
    }
}
