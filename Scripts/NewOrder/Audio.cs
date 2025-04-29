using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VehicleCore
{
    public class Audio : MonoBehaviour
    {
        public AudioSource start;
        public AudioSource idle;
        public AudioSource run;
        public AudioSource shot;

        public Transmission transmission;

        public float rpmUnit;
        public float rpmLimit = 1;

        public float b_pitch = 0.3f;
        public float maxReduceFator = 3;

        [HideInInspector] public float indexReduce;

        void Start()
        {

        }


        void FixedUpdate()
        {
            RPMUnit();


            run.pitch = b_pitch + pitchFromRPM;

            if (maxReduceFator - transmission.vehicleRigidbody.linearVelocity.magnitude > 0.1f)
            {
                indexReduce = maxReduceFator - transmission.vehicleRigidbody.linearVelocity.magnitude;
            }
            else
            {
                indexReduce = 0;
            }
            if (Input.GetAxis("Vertical") > 0 && !transmission.onShift && (indexReduce < maxReduceFator - 0.01f || transmission.currentGear == 0))
            {
                if (transmission.currentGear <= 1)
                {
                    pitchFromRPM = Mathf.Lerp(pitchFromRPM, 1, rpmUnit * Time.deltaTime);
                }
                else
                {
                    pitchFromRPM = Mathf.Lerp(pitchFromRPM, 1, (rpmUnit / transmission.currentGear) * Time.deltaTime);
                }
            }
            else
            {
                if (transmission.currentGear <= 1)
                {
                    pitchFromRPM = Mathf.Lerp(pitchFromRPM, 0, (indexReduce + rpmUnit) * Time.deltaTime);
                }
                else
                {
                    pitchFromRPM = Mathf.Lerp(pitchFromRPM, 0, (indexReduce + (rpmUnit / transmission.currentGear)) * Time.deltaTime);
                }
            }
            if (transmission.shot)
            {
                shot.Play();
            }
        }
        float value;
        float pitchFromRPM;
        private void RPMUnit()
        {
            value = transmission.currentRPM / 2000;
            if (rpmUnit > rpmLimit)
            {
                rpmUnit = rpmLimit;
            }
            else
            {
                rpmUnit = Mathf.Lerp(rpmUnit, value, 7 * Time.deltaTime);
            }
        }
    }
}