using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultActivator : MonoBehaviour
{
    public float waitTime = 3.0f;

    private bool canLaunch = true;

    public HingeJoint hingeJoint;

    //launches catapult on putton press
    public void ActivateCatapult()
    {
        if (canLaunch)
        {
            hingeJoint.useMotor = true;
            canLaunch = false;
            StartCoroutine("catapult");
        }
    }

    //uses an IEnumerator to let the catapult stay in launched position for waitTime seconds
    IEnumerator catapult()
    {
        yield return new WaitForSeconds(waitTime);
        hingeJoint.useMotor = false;
        yield return new WaitForSeconds(waitTime/2);
        canLaunch = true;
    }
}
