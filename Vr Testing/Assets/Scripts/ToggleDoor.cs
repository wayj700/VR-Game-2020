using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleDoor : MonoBehaviour
{
    public Animator anim;

    private void Start()
    {
        anim.enabled = false;
    }

    public void openDoor()
    {
        anim.enabled = true;
        anim.Play("Door Open");
    }

    public void closeDoor()
    {
        anim.enabled = true;
        anim.Play("Door Close");
    }
}
