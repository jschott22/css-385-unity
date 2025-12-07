/* 
ControlScript.cs
By: Jake Schott
*/

using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class ControlScript : MonoBehaviour
{
    //CLASS CONSTANTS
    private static float RAYCAST_RANGE = 1.5f;

    private GameObject plr_camera;
    public GameObject control_info;

    private void Start()
    {
        plr_camera = transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        if (plr_camera != null)
        {
            control_info.SetActive(false); //hide UI indicator if not looking at a control
            if (Cursor.lockState == CursorLockMode.None)
            {
                return;
            }

            if (Physics.Raycast(plr_camera.transform.position, plr_camera.transform.forward, out RaycastHit hit, RAYCAST_RANGE)) //check if ray cast hit something
            {
                if (hit.collider == null)
                {
                    return;
                }
                if (hit.collider.gameObject.layer == 6) //the ray hit a control (Layer 6 = Control)
                {
                    IControllable target_control = hit.collider.gameObject.GetComponent<IControllable>(); //get corresponding class

                    List<KeyCode> current_inputs = new List<KeyCode>(); //gets all inputted keys
                    if (UnityEngine.Input.GetKey(KeyCode.Q))
                    {
                        current_inputs.Add(KeyCode.Q);
                    }
                    if (UnityEngine.Input.GetKey(KeyCode.E))
                    {
                        current_inputs.Add(KeyCode.E);
                    }
                    if (UnityEngine.Input.GetKey(KeyCode.F))
                    {
                        current_inputs.Add(KeyCode.F);
                    }
                    if (UnityEngine.Input.GetKey(KeyCode.Mouse0))
                    {
                        current_inputs.Add(KeyCode.Mouse0);
                    }
                    control_info.transform.GetChild(2).GetComponent<TMP_Text>().SetText("DECREASE - Q");
                    control_info.transform.GetChild(3).GetComponent<TMP_Text>().SetText("INCREASE - E");
                    if (hit.collider.gameObject.name == "ITCollider")
                    {
                        control_info.transform.GetChild(1).GetComponent<TMP_Text>().SetText("IMPULSE THROTTLE");
                    }
                    else if (hit.collider.gameObject.name == "CHCollider")
                    {
                        control_info.transform.GetChild(1).GetComponent<TMP_Text>().SetText("COURSE HEADING");
                    }
                    else
                    {
                        control_info.transform.GetChild(1).GetComponent<TMP_Text>().SetText("TORPEDO");
                        control_info.transform.GetChild(2).GetComponent<TMP_Text>().SetText("FIRE - LMB");
                        control_info.transform.GetChild(3).GetComponent<TMP_Text>().SetText("ARM - F");
                    }
                    control_info.SetActive(true); //show UI indicator
                    target_control.handleInputs(current_inputs); //call when all inputs have been checked
                    return;
                }
            }
        }
    }
}
