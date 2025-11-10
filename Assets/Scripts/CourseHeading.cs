/* 
CourseHeading.cs
By: Jake Schott
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourseHeading : MonoBehaviour, IControllable
{
    private const float maxAngularVelocity = 1.2f;
    private const float accelerationRate = 1.5f;
    private const float decelerationRate = 4.0f;
    private const float returnSpringForce = 6.0f;
    private const float wheelFriction = 0.95f;

    public GameObject wheel;

    // State variables
    private float angularVelocity = 0f;
    public float wheel_angle = 0.0f; // Normalized wheel angle (-1, 1), visual wheel position 
    public float steering_input; // True steering input (Does not register spring oscillations beyond neutral)

    private Coroutine wheel_spin_coroutine = null;
    private List<KeyCode> keys_down = new List<KeyCode>();

    public float getSteeringValue() => steering_input;

    private void displayAdjustment()
    {
        //point physical wheel in right direction
        wheel.transform.localRotation = Quaternion.Euler(-113.0f, 0.0f, 450.0f * wheel_angle);
    }

    IEnumerator wheelSpinning()
    {
        steering_input = 0f;
        int lastInputDirection = 0;
        bool hasCrossedZeroSinceLastInput = false;

        while (keys_down.Count > 0 || Mathf.Abs(wheel_angle) > 0f || Mathf.Abs(angularVelocity) > 0f)
        {
            float dt = Mathf.Min(Time.deltaTime, 1.0f / 30.0f);
            int inputDirection = 0;
            bool isPlayerInputActive = false;

            if (!(keys_down.Contains(KeyCode.E) && keys_down.Contains(KeyCode.Q)))
            {
                if (keys_down.Contains(KeyCode.E)) //E
                {
                    inputDirection = 1;
                    isPlayerInputActive = true;
                }
                else if (keys_down.Contains(KeyCode.Q)) //Q
                {
                    inputDirection = -1;
                    isPlayerInputActive = true;
                }

                if (isPlayerInputActive)
                {
                    lastInputDirection = inputDirection;
                    hasCrossedZeroSinceLastInput = false;
                }

                if (inputDirection != 0)
                {
                    if (Mathf.Sign(angularVelocity) != inputDirection && Mathf.Abs(angularVelocity) > 0.1f)
                    {
                        angularVelocity = Mathf.MoveTowards(angularVelocity, 0f, decelerationRate * dt);
                    }
                    else
                    {
                        angularVelocity += inputDirection * accelerationRate * dt;
                        angularVelocity = Mathf.Clamp(angularVelocity, -maxAngularVelocity, maxAngularVelocity);
                    }
                }
                else
                {
                    float springAccel = -wheel_angle * returnSpringForce;
                    angularVelocity += springAccel * dt;
                }
            }
            else
            {
                angularVelocity *= wheelFriction;
            }

            float previousAngle = wheel_angle;
            angularVelocity *= Mathf.Pow(wheelFriction, dt * 60f);
            wheel_angle += angularVelocity * dt;
            wheel_angle = Mathf.Clamp(wheel_angle, -1f, 1f);

            // Detect zero crossing
            if (Mathf.Sign(previousAngle) != Mathf.Sign(wheel_angle) && !isPlayerInputActive)
            {
                hasCrossedZeroSinceLastInput = true;
            }

            if (isPlayerInputActive)
            {
                steering_input = wheel_angle;
            }
            else
            {
                if (hasCrossedZeroSinceLastInput)
                {
                    // Crossed 0 - Ignore all values on the opposite side
                    steering_input = 0f;
                }
                else
                {
                    // Clamp the steering input to avoid registering oscillations past neutral
                    if (lastInputDirection == 1) // last input was right
                    {
                        steering_input = Mathf.Clamp(wheel_angle, 0f, 1f); // Clamp [0, 1]
                    }
                    else if (lastInputDirection == -1) // last input was left
                    {
                        steering_input = Mathf.Clamp(wheel_angle, -1f, 0f); // Clamp [-1, 0]
                    }
                    else
                    {
                        steering_input = 0f;
                    }
                }
            }

            // Reset the wheel to the neutral position
            if (Mathf.Abs(wheel_angle) < 0.001f && Mathf.Abs(angularVelocity) < 0.01f)
            {
                wheel_angle = Mathf.MoveTowards(wheel_angle, 0.0f, Time.deltaTime * 0.001f);
                angularVelocity = 0f;
                steering_input = 0f;
                hasCrossedZeroSinceLastInput = false;
            }

            keys_down.Clear();
            displayAdjustment();
            yield return null;
        }

        wheel_spin_coroutine = null;
    }

    public void handleInputs(List<KeyCode> inputs)
    {
        keys_down = inputs;
        if (wheel_spin_coroutine == null && inputs.Count > 0)
        {
            wheel_spin_coroutine = StartCoroutine(wheelSpinning());
        }
    }
}