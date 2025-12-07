/* 
TorpedoTrigger.cs
By: Jake Schott
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorpedoTrigger : MonoBehaviour, IControllable
{
    //CLASS CONSTANTS
    private static float ARM_TIME = 1.0f;
    private static float COOLDOWN_TIME = 1.0f;
    private static float RED_BUTTON_PUSH_TIME = 0.25f;

    public Material lit_red;
    public Material lit_green;
    public Material unlit;

    public GameObject trigger_base;
    public GameObject trigger_green_light;
    public GameObject trigger_red_light;
    public GameObject torpedo;

    private float trigger_percentage = 0.0f;
    private Vector3 trigger_base_initial_pos;
    private Vector3 trigger_base_final_pos = new Vector3(-3.3307f, 8.8934f, 4.6722f);
    private Coroutine trigger_arm_coroutine = null;
    private Coroutine torpedo_fire_coroutine = null;
    private Coroutine red_button_coroutine = null;

    private List<KeyCode> keys_down = new List<KeyCode>();

    private void Start()
    {
        trigger_base_initial_pos = trigger_base.transform.localPosition;
    }

    private void displayAdjustment()
    {
        float trigger_base_distance_percentage = Mathf.Min(1.0f, trigger_percentage / 0.8f);
        trigger_base.transform.localPosition =
            new Vector3(Mathf.Lerp(trigger_base_initial_pos.x, trigger_base_final_pos.x, trigger_base_distance_percentage),
                        Mathf.Lerp(trigger_base_initial_pos.y, trigger_base_final_pos.y, trigger_base_distance_percentage),
                        Mathf.Lerp(trigger_base_initial_pos.z, trigger_base_final_pos.z, trigger_base_distance_percentage));

        float trigger_lever_rotation = Mathf.Max(0.0f, (trigger_percentage - 0.5f) / 0.5f);
        trigger_base.transform.GetChild(0).localRotation = Quaternion.Euler(25f + (trigger_lever_rotation * 15f), 0f, -90f);

        if (trigger_percentage >= 1.0f)
        {
            trigger_green_light.GetComponent<Renderer>().material = lit_green;
            trigger_red_light.GetComponent<Renderer>().material = unlit;
        }
        else
        {
            trigger_green_light.GetComponent<Renderer>().material = unlit;
            trigger_red_light.GetComponent<Renderer>().material = lit_red;
        }
    }

    IEnumerator pushRedButton()
    {
        for (int i = 0; i <= 1; i++)
        {
            float half_time = RED_BUTTON_PUSH_TIME * 0.5f;
            float push_time = half_time;

            while (push_time > 0.0f)
            {
                float dt = Mathf.Min(Time.deltaTime, 1.0f / 30.0f);
                push_time = Mathf.Max(0.0f, push_time - dt);

                float push_percentage = 1.0f - (push_time / half_time);
                if (i == 1)
                {
                    push_percentage = (push_time / half_time);
                }

                trigger_base.transform.GetChild(0).GetChild(0).localPosition =
                    new Vector3(0, 0, Mathf.Lerp(0.0f, -0.004f, push_percentage));

                yield return null;
            }
        }

        red_button_coroutine = null;
    }

    IEnumerator torpedoFire()
    {
        GameObject fired_torpedo = GameObject.Instantiate(torpedo);
        fired_torpedo.transform.position = GameObject.FindGameObjectWithTag("Ship").transform.position + (GameObject.FindGameObjectWithTag("Ship").transform.forward * 9.0f) + new Vector3(0.0f, -2.75f, 0.0f);
        fired_torpedo.transform.rotation = GameObject.FindGameObjectWithTag("Ship").transform.rotation;

        trigger_percentage = 1.0f;

        if (red_button_coroutine != null)
        {
            StopCoroutine(red_button_coroutine);
        }
        red_button_coroutine = StartCoroutine(pushRedButton());

        float cooldown_time = COOLDOWN_TIME;
        while (cooldown_time > 0.0f)
        {
            float dt = Mathf.Min(Time.deltaTime, 1.0f / 30.0f);

            cooldown_time = Mathf.Max(0.0f, cooldown_time - dt);

            float before_trigger_percentage = trigger_percentage;

            trigger_percentage = Mathf.Max(0.0f, ((trigger_percentage * COOLDOWN_TIME) - dt) / COOLDOWN_TIME);

            displayAdjustment();

            keys_down.Clear();
            yield return null;
        }

        trigger_percentage = 0.0f;

        torpedo_fire_coroutine = null;
    }

    IEnumerator triggerArming()
    {
        while (keys_down.Count > 0 || trigger_percentage > 0.0f)
        {
            float dt = Mathf.Min(Time.deltaTime, 1.0f / 30.0f);

            float before_trigger_percentage = trigger_percentage;

            bool arming = keys_down.Contains(KeyCode.F);
            if (arming == true)
            {
                trigger_percentage = Mathf.Min(1.0f, ((trigger_percentage * ARM_TIME) + dt) / ARM_TIME);
            }
            else
            {
                trigger_percentage = Mathf.Max(0.0f, ((trigger_percentage * ARM_TIME) - dt) / ARM_TIME);
            }

            if (trigger_percentage != before_trigger_percentage)
            {
                displayAdjustment();
            }

            keys_down.Clear();
            yield return null;
        }

        trigger_arm_coroutine = null;
    }

    public void handleInputs(List<KeyCode> inputs)
    {
        keys_down = inputs;
        if (trigger_arm_coroutine == null && torpedo_fire_coroutine == null)
        {
            if (keys_down.Contains(KeyCode.F))
            {
                trigger_arm_coroutine = StartCoroutine(triggerArming());
            }
        }
        else
        {
            if (trigger_percentage >= 1.0f && torpedo_fire_coroutine == null)
            {
                if (inputs.Contains(KeyCode.Mouse0))
                {
                    if (torpedo_fire_coroutine != null)
                    {
                        StopCoroutine(torpedo_fire_coroutine);
                    }
                    if (trigger_arm_coroutine != null)
                    {
                        StopCoroutine(trigger_arm_coroutine);
                        trigger_arm_coroutine = null;
                    }
                    torpedo_fire_coroutine = StartCoroutine(torpedoFire());
                }
            }
        }
    }
}
