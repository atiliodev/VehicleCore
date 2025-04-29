using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


namespace VehicleCore
{
    public class VehicleController : MonoBehaviour
    {
        public enum VehicleCategory { Sedan, SUV, PickUp, Van, Hatch, Supercar, Truck, Bus }
        public VehicleCategory vehicleCategory;


        private Engine engine;
        private Wheels wheels;
        private Steering steering;
        private Transmission transmission;

        private Rigidbody rb;

        [HideInInspector] public float velocityOfCar;
        [HideInInspector] public float torqueForce;
        [HideInInspector] public bool shift;

        private VehicleControls controls;
        float throttleInput;
        float steerInput;

        private void Awake()
        {
            controls = new VehicleControls(); 
        }

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            transmission = GetComponent<Transmission>();
            engine = GetComponent<Engine>();
            steering = GetComponent<Steering>();
            wheels = GetComponentInChildren<Wheels>();
        }


        private void Update()
        {
            Calls();

            ReadInput();
        }

        private void Calls()
        {
            velocityOfCar = rb.linearVelocity.magnitude;
            torqueForce = transmission.wheelForce();
            shift = transmission.onShift;
            engine.Move(throttleInput);
            steering.Steer(steerInput);
            transmission.boxOfTransmission(engine.bruteForce());

            wheels.transmissionInfo(transmission.currentGear, transmission.numberOfGears, transmission.clutch);
            wheels.forceReceiver(transmission.wheelForce());
            wheels.steerReceiver(steering.wheelSteering());
        }
        void OnEnable()
        {
            controls.Enable();
        }

        void OnDisable()
        {
            controls.Disable();
        }

        private void ReadInput()
        {
            throttleInput = controls.Driving.Throttle.ReadValue<float>() + -controls.Driving.Brake.ReadValue<float>();
            steerInput = controls.Driving.Steering.ReadValue<float>();
        }


    }


}