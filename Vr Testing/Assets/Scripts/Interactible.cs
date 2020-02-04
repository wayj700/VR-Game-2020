using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactible : MonoBehaviour
{
    public bool isGrabbed = false;
    public bool isHitByCast = false;

    Shader notHighlighted;
    Shader highlighted;

    private void Start()
    {
        notHighlighted = Shader.Find("Standard");
        highlighted = Shader.Find("Outlined/Silhouetted Diffuse");
    }

    private void onHitEnter()
    {
        isHitByCast = true;
        transform.gameObject.GetComponent<Renderer>().material.shader = highlighted;
        transform.gameObject.GetComponent<Renderer>().material.SetFloat("_Outline", 0.1f);
    }

    private void onHitExit()
    {
        isHitByCast = false;
        transform.gameObject.GetComponent<Renderer>().material.shader = notHighlighted;
    }
}
