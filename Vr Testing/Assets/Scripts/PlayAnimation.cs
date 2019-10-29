using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimation : MonoBehaviour
{
    public Animator anim;


    private void Start()
    {
        //anim = gameObject.GetComponent<Animation>();
        anim.enabled = false;
    }

    public void playAnimation()
    {
        anim.enabled = true;
        anim.Play("Door Open");
    }
}
