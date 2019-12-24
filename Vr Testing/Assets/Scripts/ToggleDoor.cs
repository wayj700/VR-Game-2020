using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleDoor : MonoBehaviour
{
    public Animator anim;

    [SerializeField] private bool canToggle = true;

    private void Start()
    {
        anim.enabled = false;
    }

    public void checkIfDone()
    {
        canToggle = true;
    }

   private bool isAnimDone()
    {
        return canToggle;
    }

    public void openDoor()
    {
        if (canToggle)
        {
            Debug.Log("Opening Door");
            anim.enabled = true;
            anim.Play("Door Open");
            canToggle = false;
        }
    }

    public void closeDoor()
    {
        if (canToggle)
        {
            Debug.Log("Closing Door");
            anim.enabled = true;
            anim.Play("Door Close");
            canToggle = false;
        }
    }   
}