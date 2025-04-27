using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace VehiclePhysics { 

public class Wheels : MonoBehaviour
{
    public WheelComponent[] wheels;
    public Rigidbody carRigidbody;

    WheelComponent rearLeftWheel;
    WheelComponent rearRightWheel;

    WheelComponent frontLeftWheel;
    WheelComponent frontRightWheel;

    [Header("Drift Settings")]
    public float driftForce = 2;
    public float frictionBase = 2;
        public bool enableDrift;

    [Header("Differential Settings")]
    public float brakeIndex = 5;
    public float reduceFator = 0.003f;
    public float reduceFatorVelocity = 3.5f;
    public float brakeReduce = 20;

    [Header("Frontal Differential Settings")]
    public bool frontal;
    public float f_weight = 1;
    public float frontalForce = 0.52f;

    private float currentGear;
    private float numberOfGears;
    private float clutch;
    private float brakeIntensify;


   
    
    /*
    private WheelFrictionCurve rearLeftOriginalSidewaysFriction; 
    private WheelFrictionCurve rearLeftOriginalForwardFriction; 

    private WheelFrictionCurve rearRightOriginalSidewaysFriction; 
    private WheelFrictionCurve rearRightOriginalForwardFriction; 
    */


   

    public void transmissionInfo(float currentGear, float numberOfGears, float clutch)
    {
        this.currentGear = currentGear;
        this.numberOfGears = numberOfGears;
        this.clutch = clutch;
    }

    private void Start()
    {
        rearLeftWheel = wheels[2];
        rearRightWheel = wheels[3];
       
        frontLeftWheel = wheels[0];
        frontRightWheel = wheels[1];

/*
        //Left
        rearLeftOriginalSidewaysFriction = rearLeftWheel.sidewaysFriction;
        rearLeftOriginalForwardFriction = rearLeftWheel.forwardFriction;
        //Right
        rearRightOriginalSidewaysFriction = rearRightWheel.sidewaysFriction;
        rearRightOriginalForwardFriction = rearRightWheel.forwardFriction;
*/
    }

    public void forceReceiver(float bruteForce) 
    {
        Differential(bruteForce);
        ReduceValue(bruteForce, clutch);
    }

    public void steerReceiver(float steer)
    {
        wheels[0].steeringForce = steer;
        wheels[1].steeringForce = steer;
        steerValue = steer;
    }

    [HideInInspector] public float outerWheelTorque;
    [HideInInspector] public float innerWheelTorque;
    [HideInInspector] public float directionOfDifferential;
    [HideInInspector] public float leftForceOfDifferential;
    [HideInInspector] public float rightForceOfDifferential;
    [HideInInspector] public float carSpeed;
    [HideInInspector] public float indexOfReduce;
    [HideInInspector] public float brakeValue;
    [HideInInspector] public float steerValue;
    [HideInInspector] public float forceToLeftWheel;
    [HideInInspector] public float forceToRightWheel;
    [HideInInspector] public float splitFator;
    [HideInInspector] public float differentialBrake = 5;


    private void Update()
    {
        rearDifferential();

        if (frontal)
        {
            frontDifferential();
        }
        else
        {
            frontLeftWheel.motorTorque = 0;
            frontRightWheel.motorTorque = 0;
            
        }

        if(splitFator < frictionBase && enableDrift)
        {
            for(int i = 0; i < wheels.Length; i++)
            {
                wheels[i].activeParticles = true;
            }
        }
        else
        {
            for (int i = 0; i < wheels.Length; i++)
            {
                wheels[i].activeParticles = false;
            }
        }
    }

    private void rearDifferential()
    {
        rearLeftWheel.motorTorque = forceToLeftWheel;
        rearRightWheel.motorTorque = forceToRightWheel;

        if (Input.GetAxis("Vertical") < 0 && brakeValue != 0)
        {
            if (currentGear > 2)
            {
                rearLeftWheel.collider.brakeTorque = (brakeValue / 50) * currentGear;
                rearRightWheel.collider.brakeTorque = (brakeValue / 50) * currentGear;
            }
            else
            {
                rearLeftWheel.collider.brakeTorque = (brakeValue / 50);
                rearRightWheel.collider.brakeTorque = (brakeValue / 50);

            }
        }
        else
        {
            rearLeftWheel.collider.brakeTorque = brakeValue;
            rearRightWheel.collider.brakeTorque = brakeValue;
        }
    }

    private void frontDifferential()
    {
        frontLeftWheel.motorTorque = forceToLeftWheel * frontalForce * f_weight;
        frontRightWheel.motorTorque = forceToRightWheel * frontalForce * f_weight;

       
       
        
    }

    public void ReduceValue(float inputForce, float cl)
    {
        differentialBrake = brakeIndex * currentGear + (10 * (carSpeed /3));
        if(Input.GetAxis("Vertical") < 0 && carSpeed > 2)
        {
            brakeIntensify = brakeReduce;
        }
        else
        {
            brakeIntensify = 1;
        }
        if (inputForce == 0 || cl <= 0.1f && (carSpeed > 3 || Input.GetAxis("Vertical") < 0))
        {
            if (Input.GetAxis("Vertical") > 0)
            {
                brakeValue = differentialBrake * reduceFator;
            }
            else
            {
                brakeValue = differentialBrake * reduceFatorVelocity * brakeIntensify;
            }

        }
        else
        {
            brakeValue = 0;
        }
    }

    public void Differential(float inputForce)
    {
        // Valida��o do Rigidbody
        if (carRigidbody == null)
        {
            carRigidbody = GetComponent<Rigidbody>();
        }

        // Velocidade do carro em m/s
        carSpeed = carRigidbody.linearVelocity.magnitude * 1;

        float factorCurve;

        if (steerValue == 0)
        {
            forceToLeftWheel = inputForce;
            forceToRightWheel = inputForce;


            directionOfDifferential = Mathf.Lerp(directionOfDifferential, 0, 5.5f * Time.deltaTime);
            leftForceOfDifferential = Mathf.Lerp(directionOfDifferential, 0, 7.5f * Time.deltaTime);
            rightForceOfDifferential = Mathf.Lerp(directionOfDifferential, 0, 7.5f * Time.deltaTime);
        }
        else
        {
            factorCurve = (1 / (1 + currentGear)) * Mathf.Lerp(0, 1, 3.5f * Time.deltaTime);

            innerWheelTorque = inputForce * (factorCurve / 10);
            outerWheelTorque = inputForce * (factorCurve * 10);

            directionOfDifferential = Mathf.Lerp(directionOfDifferential, 1 * Input.GetAxis("Horizontal"), 3.5f * Time.deltaTime);

            if (Input.GetAxis("Horizontal") < 0)
            {
                forceToLeftWheel = innerWheelTorque;
                forceToRightWheel = outerWheelTorque;
                rightForceOfDifferential = Mathf.Lerp(directionOfDifferential, 1, 3.5f * Time.deltaTime);
                leftForceOfDifferential = Mathf.Lerp(directionOfDifferential, 0, 5.5f * Time.deltaTime);
            }
            else if (Input.GetAxis("Horizontal") > 0)
            {
                forceToLeftWheel = outerWheelTorque;
                forceToRightWheel = innerWheelTorque;
                rightForceOfDifferential = Mathf.Lerp(directionOfDifferential, 0, 5.5f * Time.deltaTime);
                leftForceOfDifferential = Mathf.Lerp(directionOfDifferential, 1, 3.5f * Time.deltaTime);
            }

        }

        if (carSpeed >= 7)
        {
            rearRightWheel.collider.brakeTorque = differentialBrake * rightForceOfDifferential * 2;
            rearLeftWheel.collider.brakeTorque = differentialBrake * leftForceOfDifferential * 2;
            indexOfReduce = Mathf.Lerp(indexOfReduce, 1, 1.2f * Time.deltaTime);
        }
        else
        {
            indexOfReduce = Mathf.Lerp(indexOfReduce, 0, 5.2f * Time.deltaTime);
            rearRightWheel.collider.brakeTorque = differentialBrake * 0;
            rearLeftWheel.collider.brakeTorque = differentialBrake * 0;
        }

        sidewayFriction(rearLeftWheel.collider);
        sidewayFriction(rearRightWheel.collider);

        if (Input.GetAxis("Vertical") < 0 && brakeValue != 0)
        {

            if (currentGear > 2)
            {
                frontLeftWheel.collider.brakeTorque = (brakeValue / 50) * currentGear;
                frontRightWheel.collider.brakeTorque = (brakeValue / 50) * currentGear;
            }
            else
            {
                frontLeftWheel.collider.brakeTorque = (brakeValue / 50);
                frontRightWheel.collider.brakeTorque = (brakeValue / 50);
            }
        }
        else
        {
            frontLeftWheel.collider.brakeTorque = brakeValue;
            frontRightWheel.collider.brakeTorque = brakeValue;
        }
    }

    private void sidewayFriction(WheelCollider wheel)
    {
        WheelFrictionCurve rearLeftSidewaysFriction = wheel.sidewaysFriction;
        splitFator = frictionBase - (driftForce * Mathf.Abs(directionOfDifferential) - ((driftForce - 0.1f) * indexOfReduce));
        rearLeftSidewaysFriction.extremumValue = frictionBase - (driftForce * Mathf.Abs(directionOfDifferential) - ((driftForce - 0.1f) * indexOfReduce));
        rearLeftSidewaysFriction.asymptoteValue = frictionBase - (driftForce * Mathf.Abs(directionOfDifferential) - (driftForce - 0.1f) * indexOfReduce);
        wheel.sidewaysFriction = rearLeftSidewaysFriction;
    }

    
}
    }