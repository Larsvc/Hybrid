using System;
using UnityEngine;

public class CarControllerPlaceholder : MonoBehaviour
{
    public WheelCollider frontLeftWheel, frontRightWheel;
    public WheelCollider rearLeftWheel, rearRightWheel;

    private Rigidbody rigidbody;

    public float maxSteerAngle = 45f;
    public float motorTorque = 100f;
    public float brakeTorque = 100f;

    private float currentSteerAngle;
    private float currentMotorTorque;
    private float currentBrakeTorque;

    [SerializeField] private GameObject stuckScreen;
    [SerializeField] private float stuckDelay = 13f;
    private float stuckTimer;

    private Transform frontLeftWheelModel, frontRightWheelModel, rearLeftWheelModel, rearRightWheelModel;

    private float rotationValue;
    private Vector3 wheelModelRotation = new Vector3(1, 0, 0);

    private PlayerCar player;


    private void Awake()
    {
        player = GetComponent<PlayerCar>();
        rigidbody = GetComponent<Rigidbody>();

        frontLeftWheelModel = frontLeftWheel.transform.GetChild(0);
        frontRightWheelModel = frontRightWheel.transform.GetChild(0);

        rearLeftWheelModel = rearLeftWheel.transform.GetChild(0);
        rearRightWheelModel = rearRightWheel.transform.GetChild(0);
    }

    void Update()
    {
        if (!player.IsDead)
            HandleInput();
    }

    void HandleInput()
    {
        currentSteerAngle = maxSteerAngle * Input.GetAxis("MoveHorizontalP" + player.playerNumber);
        currentMotorTorque = motorTorque * Input.GetAxis("MoveVerticalP" + player.playerNumber);
        //currentBrakeTorque = brakeTorque * Input.GetAxis("AbilityP");
    }

    void HandleHoles()
    {
        int wheelsGrounded = Convert.ToInt32(frontLeftWheel.isGrounded) 
            + Convert.ToInt32(frontRightWheel.isGrounded)
            + Convert.ToInt32(rearLeftWheel.isGrounded)
            + Convert.ToInt32(rearRightWheel.isGrounded);

        if (wheelsGrounded < 4 && rigidbody.velocity.magnitude < 0.5f)
            stuckTimer -= Time.deltaTime;
        else
            stuckTimer = stuckDelay;

        if (stuckTimer < 3)
            stuckScreen.SetActive(true);

        if (stuckTimer < 0)
        {
            stuckScreen.SetActive(false);
            stuckTimer = stuckDelay;
            player.DestroySelf();
        }
            
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
        //frontLeftWheelModel.transform.Rotate(new Vector3(1, 0, 0) * currentSteerAngle * 0.1f);
        rotationValue = frontRightWheel.rpm * (360 / 60) * Time.deltaTime;
        frontRightWheelModel.transform.Rotate(wheelModelRotation * rotationValue);
        //frontRightWheelModel.transform.Rotate(new Vector3(1, 0, 0) * currentSteerAngle * 0.1f);
        rotationValue = rearLeftWheel.rpm * (360 / 60) * Time.deltaTime;
        rearLeftWheelModel.transform.Rotate(wheelModelRotation * rotationValue);
        rotationValue = rearRightWheel.rpm * (360 / 60) * Time.deltaTime;
        rearRightWheelModel.transform.Rotate(wheelModelRotation * rotationValue);

        HandleHoles();
    }
}