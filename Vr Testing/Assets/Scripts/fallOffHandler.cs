using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallOffHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            Debug.Log("Player collided with invisible falloff point");
            collision.collider.gameObject.GetComponent<RigidbodyVRController>().health = 0;
        }
    }
}
