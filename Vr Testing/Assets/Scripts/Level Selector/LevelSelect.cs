using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    public SteamVR_Action_Boolean Select;

    [SerializeField] GameObject player;

    // Update is called once per frame
    void Update()
    {
       // Debug.Log(Select.state);
        Ray selectorRay = new Ray(this.transform.position, ((transform.up * -1.0f) + transform.forward));
        Debug.DrawRay(this.transform.position, ((transform.up * -1.0f) + transform.forward) * 25, Color.blue);

        RaycastHit selection;
        if (Physics.Raycast(this.transform.position, ((transform.up * -1.0f) + transform.forward), out selection))
        {
            if (selection.collider.tag == "Interactible" && Select.state)
            {
                SceneManager.LoadScene("Level 1 Concept", LoadSceneMode.Additive);
                GameObject level = selection.transform.gameObject;
                Debug.Log(level.name);
            }
        }
    }
}
