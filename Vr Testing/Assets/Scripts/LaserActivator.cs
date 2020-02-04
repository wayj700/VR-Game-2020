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

    // Update is called once per frame
    void Update()
    {
        if (isHitByLaser)
        {
            onLaserHit.Invoke();
        }
        else if (!isHitByLaser && needsLaser)
        {
            onLaserLeave.Invoke();
        }
    }

    public void onHitEnter()
    {
        isHitByLaser = true;
    }

    public void onHitExit()
    {
        isHitByLaser = false;
    }
}
