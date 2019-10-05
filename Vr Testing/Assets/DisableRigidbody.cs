using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableRigidbody : MonoBehaviour
{
    public Rigidbody Rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void pickup()
    {
        Rigidbody.detectCollisions = false;
    }

    public void dropped()
    {
        Rigidbody.detectCollisions = true;
    }
}
