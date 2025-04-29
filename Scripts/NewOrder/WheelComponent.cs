using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VehicleCore
{
    public class WheelComponent : MonoBehaviour
    {
        public float motorTorque;
        public float steeringForce;


        [HideInInspector] public WheelCollider collider;
        public GameObject obj;
        public ParticleSystem particles;
        public TrailRenderer trail;

        public float initialLifetimeOfParticles = 3;
        public float finalLifetimeOfParticles = 15;
        public bool activeParticles;


        private void Awake()
        {

            collider = GetComponent<WheelCollider>();
        }


        private void Update()
        {
            collider.motorTorque = motorTorque;
            collider.steerAngle = steeringForce;

            Vector3 pos;
            Quaternion quaternion;

            collider.GetWorldPose(out pos, out quaternion);

            obj.transform.position = pos;
            obj.transform.rotation = quaternion;

            if (activeParticles)
            {
                particles.startLifetime = initialLifetimeOfParticles;
                particles.Play();
                trail.emitting = true;
            }
            else
            {
                particles.startLifetime = finalLifetimeOfParticles;
                particles.Stop();
                trail.emitting = false;
            }
        }

    }
}