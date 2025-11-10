/* 
PilotingSystem.cs
By: Jake Schott
*/

using UnityEngine;

public class PilotingSystem : MonoBehaviour
{
    public Transform worldRoot;
    public ImpulseThrottle impulseThrottle;
    public CourseHeading courseHeading;
    public Transform endBeacon;
    public GameObject scenarioComplete;

    [Header("Speed Settings")]
    private float maxImpulseForwardSpeed = 50f;

    [Header("Rotation Settings")]
    private float rotationPower = 10f;
    private float steeringResponsiveness = 2.5f;
    private float maxRotationSpeed = 15f;

    // Input values
    private float currentImpulse;
    private float steeringInput;

    // Movement state
    private float smoothedSteeringInput = 0f;
    public float currentRotationSpeed;
    public float forwardSpeed;
    public Vector3 currentVelocity;

    public float currentImpulseSpeed = 0f;

    public void UpdateInput()
    {
        currentImpulse = impulseThrottle.getCurrentImpulse();
        steeringInput = courseHeading.getSteeringValue();
    }

    public void UpdateMovement()
    {
        float dt = Time.deltaTime;

        Vector3 forward = transform.forward;

        currentImpulseSpeed = currentImpulse * maxImpulseForwardSpeed;

        Vector3 impulseVelocity = forward * currentImpulseSpeed;

        currentVelocity = impulseVelocity;

        if (currentVelocity.magnitude > maxImpulseForwardSpeed)
        {
            currentVelocity = currentVelocity.normalized * maxImpulseForwardSpeed;
        }

        if (worldRoot != null)
        {
            worldRoot.position -= currentVelocity * dt;
        }

        forwardSpeed = currentVelocity.magnitude;

        HandleRotation(dt);
    }

    private void HandleRotation(float dt)
    {
        forwardSpeed = currentVelocity.magnitude;
        float speedFactor = Mathf.Clamp01(forwardSpeed / maxImpulseForwardSpeed);

        smoothedSteeringInput = Mathf.Lerp(
            smoothedSteeringInput,
            steeringInput,
            steeringResponsiveness * dt
        );

        float targetRotationSpeed = smoothedSteeringInput * maxRotationSpeed * speedFactor;

        if (Mathf.Abs(forwardSpeed) < 0.01f)
            return;

        currentRotationSpeed = Mathf.Lerp(
            currentRotationSpeed,
            targetRotationSpeed,
            rotationPower * dt
        );

        if (Mathf.Abs(steeringInput) < 0.1f && Mathf.Abs(smoothedSteeringInput) < 0.1f)
        {
            currentRotationSpeed = Mathf.Lerp(currentRotationSpeed, 0f, rotationPower * dt);
        }

        transform.Rotate(0f, currentRotationSpeed * dt, 0f);
    }

    private void Update()
    {
        UpdateInput();
        UpdateMovement();
        if (Vector3.Distance(endBeacon.position, transform.position) < 25f)
        {
            scenarioComplete.gameObject.SetActive(true);
        }
    }
}