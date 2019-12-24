using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class terminalCheck : MonoBehaviour
{
    //terminal booleans. all must be true for main door to open
    private bool terminalBool1;
    private bool terminalBool2;
    private bool terminalBool3;
    private bool terminalBool4;

    public GameObject mainDoor;

    // Update is called once per frame
    void Update()
    {
        if (terminalBool1 && terminalBool2 && terminalBool3 && terminalBool4)
        {
            mainDoor.GetComponent<ToggleDoor>().openDoor();
        }
    }

    public void activateTerminal1()
    {
        terminalBool1 = true;
    }

    public void activateTerminal2()
    {
        terminalBool2 = true;
    }

    public void activateTerminal3()
    {
        terminalBool3 = true;
    }

    public void activateTerminal4()
    {
        terminalBool4 = true;
    }

}
