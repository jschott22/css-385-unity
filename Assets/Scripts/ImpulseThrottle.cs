/* 
ImpulseThrottle.cs
By: Jake Schott
*/

using System.Collections.Generic;
using UnityEngine;

public class ImpulseThrottle : MonoBehaviour, IControllable
{
    //CLASS CONSTANTS
    private static float MOVE_SPEED = 35.0f;

    public GameObject handle;
    public AudioSource ship_sound;

    private float impulse = 0.0f;
    private Vector3 initial_pos; //handle starting position (0% impulse)
    private Vector3 final_pos = new Vector3(1.1831f, -1.0405f, 20.2876f);

    private void Start()
    {
        initial_pos = handle.transform.localPosition; //sets the initial position
    }
    private void displayAdjustment()
    {
        //update lever position
        handle.transform.localPosition =
            new Vector3(Mathf.Lerp(initial_pos.x, final_pos.x, impulse),
                        Mathf.Lerp(initial_pos.y, final_pos.y, impulse),
                        Mathf.Lerp(initial_pos.z, final_pos.z, impulse));

        //update sound
        ship_sound.volume = impulse;
    }

    public float getCurrentImpulse()
    {
        return impulse;
    }

    public void handleInputs(List<KeyCode> inputs)
    {
        int impulse_direction = 0;
        if (inputs.Contains(KeyCode.E)) //E to increment
        {
            impulse_direction += 1;
        }
        if (inputs.Contains(KeyCode.Q))  //Q to decrement
        {
            impulse_direction -= 1;
        }
        if (impulse_direction != 0)
        {
            if (impulse_direction > 0)
            {
                impulse = Mathf.Min(1.0f, impulse + (0.002f * (impulse / 0.5f) + 0.001f) * Time.deltaTime * MOVE_SPEED);
            }
            else
            {
                impulse = Mathf.Max(0.0f, impulse - (0.002f * (impulse / 0.5f) + 0.001f) * Time.deltaTime * MOVE_SPEED);
            }
        }
        displayAdjustment();
    }
}
