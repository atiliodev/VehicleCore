using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VehicleCore
{
    public class Engine : MonoBehaviour
    {
        public float engineForce = 2000;

        float outputForce;
        public void Move(float accel)
        {
            outputForce = accel * engineForce;
        }


        public float bruteForce()
        {
            return outputForce;
        }
    }
}