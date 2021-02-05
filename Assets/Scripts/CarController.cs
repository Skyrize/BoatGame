using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum TransmissionType
{
    TRACTION,
    PROPULSION,
    FOUR_WHEELS_DRIVE
}

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] protected TransmissionType transmission = TransmissionType.FOUR_WHEELS_DRIVE;
    [SerializeField] protected float motorForce = 1000;
    [SerializeField] protected float brakeForce = 3000;
    [SerializeField] protected float maxSteerAngle = 30;
    [SerializeField] protected float maxSteerSpeed = 5;

    [Header("References")]
    [SerializeField] protected Transform centerOfMass = null;
    [SerializeField] protected Rigidbody rb = null;

    [SerializeField] protected WheelCollider frontLeftWheelCollider = null;
    [SerializeField] protected WheelCollider frontRightWheelCollider = null;
    [SerializeField] protected WheelCollider backLeftWheelCollider = null;
    [SerializeField] protected WheelCollider backRightWheelCollider = null;
    
    [SerializeField] protected Transform frontLeftWheelTransform = null;
    [SerializeField] protected Transform frontRightWheelTransform = null;
    [SerializeField] protected Transform backLeftWheelTransform = null;
    [SerializeField] protected Transform backRightWheelTransform = null;

    [Header("Runtime")]
    [SerializeField] protected float steerInput = 0;
    [SerializeField] protected float accelerationInput = 0;
    [SerializeField] protected float currentBrakeForce = 0;
    [SerializeField] protected float currentSteerAngle = 0;
    [SerializeField] protected float targetSteerAngle = 0;
    [SerializeField] protected bool isBraking = false;

    protected virtual void Start() {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass.localPosition;
    }

    protected virtual void Update() {
        GetInput();
    }

    protected virtual void FixedUpdate() {
        HandleMotor();
        HandleSteering();
        SmoothSteering();
        UpdateWheels();
    }


    protected virtual void GetInput()
    {
        steerInput = Input.GetAxis("Horizontal");
        accelerationInput = Input.GetAxis("Vertical");
        isBraking = Input.GetKey(KeyCode.Space);
    }

    protected virtual void HandleTraction()
    {
        frontLeftWheelCollider.motorTorque = accelerationInput * motorForce / 2.0f;
        frontRightWheelCollider.motorTorque = accelerationInput * motorForce / 2.0f;
    }
    protected virtual void HandlePropulsion()
    {
        backLeftWheelCollider.motorTorque = accelerationInput * motorForce / 2.0f;
        backRightWheelCollider.motorTorque = accelerationInput * motorForce / 2.0f;
    }
    protected virtual void HandleFourWheelsDrive()
    {
        frontLeftWheelCollider.motorTorque = accelerationInput * motorForce / 4.0f;
        frontRightWheelCollider.motorTorque = accelerationInput * motorForce / 4.0f;
        backLeftWheelCollider.motorTorque = accelerationInput * motorForce / 4.0f;
        backRightWheelCollider.motorTorque = accelerationInput * motorForce / 4.0f;
    }

    protected virtual void HandleMotor()
    {
        switch (transmission)
        {
            case TransmissionType.TRACTION:
            HandleTraction();
            break;
            case TransmissionType.PROPULSION:
            HandlePropulsion();
            break;
            case TransmissionType.FOUR_WHEELS_DRIVE:
            HandleFourWheelsDrive();
            break;
        }
        currentBrakeForce = isBraking || Mathf.Sign(accelerationInput) != Mathf.Sign(transform.InverseTransformDirection(rb.velocity).z) ? brakeForce : 0;
        ApplyBraking();
    }

    protected virtual void ApplyBraking()
    {
        frontLeftWheelCollider.brakeTorque = currentBrakeForce / 4.0f;
        frontRightWheelCollider.brakeTorque = currentBrakeForce / 4.0f;
        backLeftWheelCollider.brakeTorque = currentBrakeForce / 4.0f;
        backRightWheelCollider.brakeTorque = currentBrakeForce / 4.0f;
    }

    protected virtual void HandleSteering()
    {
        targetSteerAngle = maxSteerAngle * steerInput;
    }

    protected virtual void SmoothSteering()
    {
        currentSteerAngle = Mathf.Lerp(currentSteerAngle, targetSteerAngle, Time.deltaTime * maxSteerSpeed);
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    protected virtual void UpdateWheel(WheelCollider wheelCollider, Transform wheelTransform) {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }

    protected virtual void UpdateWheels()
    {
        UpdateWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateWheel(backLeftWheelCollider, backLeftWheelTransform);
        UpdateWheel(backRightWheelCollider, backRightWheelTransform);
    }

}
