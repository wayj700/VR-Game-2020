using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LaserActivator : MonoBehaviour
{
    public bool isHitByLaser = false;
    public bool needsLaser = true;

    public UnityEvent onLaserHit;
    public UnityEvent onLaserLeave;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void onHitEnter()
    {
        if (isHitByLaser == false)
        {
            onLaserHit.Invoke();
            Debug.Log("hit by laser");
            isHitByLaser = true;
        }
    }

    public void onHitExit()
    {
        isHitByLaser = false;
        if (needsLaser)
        {
            onLaserLeave.Invoke();
            Debug.Log("unhit by laser");
        }
    }
}
