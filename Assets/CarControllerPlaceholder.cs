using UnityEngine;

public class CarControllerPlaceholder : MonoBehaviour
{
    public WheelCollider frontLeftWheel, frontRightWheel;
    public WheelCollider rearLeftWheel, rearRightWheel;

    public float maxSteerAngle = 45f;
    public float motorTorque = 100f;
    public float brakeTorque = 100f;

    private float currentSteerAngle;
    private float currentMotorTorque;
    private float currentBrakeTorque;

    private Transform frontLeftWheelModel, frontRightWheelModel, rearLeftWheelModel, rearRightWheelModel;

    private float rotationValue;
    private Vector3 wheelModelRotation = new Vector3(1, 0, 0);

    void Update()
    {
        // Get input values for steering and motor/brake torque
        currentSteerAngle = maxSteerAngle * Input.GetAxis("MoveHorizontalP1");
        currentMotorTorque = motorTorque * Input.GetAxis("MoveVerticalP1");
        currentBrakeTorque = brakeTorque * Input.GetAxis("AbilityP1");

        frontLeftWheelModel = frontLeftWheel.transform.GetChild(0);
        frontRightWheelModel = frontRightWheel.transform.GetChild(0);

        rearLeftWheelModel = rearLeftWheel.transform.GetChild(0);
        rearRightWheelModel = rearRightWheel.transform.GetChild(0);
    }

    void FixedUpdate()
    {
        // Apply steering angle to front wheels
        frontLeftWheel.steerAngle = currentSteerAngle;
        frontRightWheel.steerAngle = currentSteerAngle;

        // Apply motor torque to rear wheels
        rearLeftWheel.motorTorque = currentMotorTorque;
        rearRightWheel.motorTorque = currentMotorTorque;

        // Apply brake torque to all wheels
        frontLeftWheel.brakeTorque = currentBrakeTorque;
        frontRightWheel.brakeTorque = currentBrakeTorque;
        rearLeftWheel.brakeTorque = currentBrakeTorque;
        rearRightWheel.brakeTorque = currentBrakeTorque;

        rotationValue = frontLeftWheel.rpm * (360 / 60) * Time.deltaTime;
        frontLeftWheelModel.transform.Rotate(wheelModelRotation * rotationValue);
        rotationValue = frontRightWheel.rpm * (360 / 60) * Time.deltaTime;
        frontRightWheelModel.transform.Rotate(wheelModelRotation * rotationValue);
        rotationValue = rearLeftWheel.rpm * (360 / 60) * Time.deltaTime;
        rearLeftWheelModel.transform.Rotate(wheelModelRotation * rotationValue);
        rotationValue = rearRightWheel.rpm * (360 / 60) * Time.deltaTime;
        rearRightWheelModel.transform.Rotate(wheelModelRotation * rotationValue);
    }
}