using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VehiclePhysics
{
    public class Steering : MonoBehaviour
    {
        public float maxSteer = 45f;


        float outputSteer;
        Rigidbody vehicleRigidbody;

        float steerForce;
        float steerInput;

        public float steerAssistForce = 3f;
        public Transform centerOfMass;
        void Start()
        {
            vehicleRigidbody = GetComponent<Rigidbody>();
        }

        public void Steer(float steer)
        {
            outputSteer = steer * steerForce;
            steerInput = steer;
        }

        private void FixedUpdate()
        {
            steerControl();
            assitSteer();
        }

        private void assitSteer()
        {
            if (Mathf.Abs(steerInput) > 0.01f)
            {

                Vector3 lateralForce = transform.right * steerInput * steerAssistForce;


                float forwardSpeed = Vector3.Dot(vehicleRigidbody.linearVelocity, transform.forward);
                lateralForce *= Mathf.Clamp(forwardSpeed / 10f, 0, 1);


                vehicleRigidbody.AddForce(lateralForce, ForceMode.Acceleration);
            }
        }

        private void steerControl()
        {
            float vehicleSpeed = vehicleRigidbody.linearVelocity.magnitude;

            float halfSpeed = vehicleSpeed / 100;

            if (vehicleSpeed >= 7)
            {
                if (Input.GetAxis("Horizontal") != 0)
                {
                    steerForce = Mathf.Lerp(steerForce, maxSteer, (5) * Time.deltaTime);
                }
                else
                {
                    steerForce = Mathf.Lerp(steerForce, 0, 5 * Time.deltaTime);
                }
            }
            else
            {
                if (Input.GetAxis("Horizontal") != 0)
                {
                    steerForce = Mathf.Lerp(steerForce, maxSteer, 5 * Time.deltaTime);
                }
                else
                {
                    steerForce = Mathf.Lerp(steerForce, maxSteer, 7 * Time.deltaTime);
                }
            }
        }

        public float wheelSteering()
        {
            return outputSteer;
        }
    }
}