using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VehicleCore
{
    public class Transmission : MonoBehaviour
    {
        public enum typeOfTransmission { Manual, Automatic }

        [Header("Type of Transmission")]
        public typeOfTransmission TypeOfTransmission;

        [Header("Clutch Settings")]
        public float clutch = 1;
        public float clutchInterval = 0.5f;

        [Header("Gear Settings")]
        public float[] gearForce;
        public int currentGear;
        [HideInInspector] public int numberOfGears;
        public float finalDriveRatio = 4.1f;
        public float swapRatio = 0.85f;
        public float reduceRatio = 0.2f;
        public float velocityBase = 30;

        [Header("RPM Settings")]
        public float indexRPM = 7;
        public float baseRPM = 200;
        [HideInInspector] public float currentRPM;

        [Header("Rear Settings")]
        public float rearForce = 300f;
        public float rearFatorForce = 3;
        public float minValueToChange = 0.3f;
        public float maxReduceFator = 3;


        private float outputWheelForce;
        private float value;
        private float rpmUnit;

        [HideInInspector] public Rigidbody vehicleRigidbody;
        [HideInInspector] public bool onShift;
        [HideInInspector] public float currentVelocity;
        [HideInInspector] public bool shot;
        [HideInInspector] public float valueSet;
        [HideInInspector] public bool onRear;
        [HideInInspector] public float fatorChangeSense;
        [HideInInspector] public bool onChangeSense;
        [HideInInspector] public int burnTimes;
        [HideInInspector] public bool isAutoTransmission;
        [HideInInspector] public float RPM;
        [HideInInspector] public float indexReduce;

        private void Awake()
        {
            if (vehicleRigidbody == null)
            {
                vehicleRigidbody = GetComponent<Rigidbody>();
            }
        }

        private void Start()
        {
            setUpGearBox();


        }



        private void FixedUpdate()
        {
            MirrorRPM();
        }
        private void Update()
        {
            ShiftSystem();
            if (TypeOfTransmission == typeOfTransmission.Manual)
            {
                isAutoTransmission = false;
            }
            else
            {
                isAutoTransmission = true;
            }
        }

        private void MirrorRPM()
        {
            if (maxReduceFator - vehicleRigidbody.linearVelocity.magnitude > 0.1f)
            {
                indexReduce = maxReduceFator - vehicleRigidbody.linearVelocity.magnitude;
            }
            else
            {
                indexReduce = 0;
            }
            if (VehicleController.inputV > 0 && !onShift)
            {
                if (currentGear <= 1)
                {
                    RPM = Mathf.Lerp(RPM, 1, rpmUnit * Time.deltaTime);
                }
                else
                {
                    RPM = Mathf.Lerp(RPM, 1, (rpmUnit / currentGear) * Time.deltaTime);
                }
            }
            else
            {
                if (currentGear <= 1)
                {
                    RPM = Mathf.Lerp(RPM, 0, (indexReduce + 1 + rpmUnit) * Time.deltaTime);
                }
                else
                {
                    RPM = Mathf.Lerp(RPM, 0, (indexReduce + (rpmUnit / currentGear) + (0.1f * (currentGear / (vehicleRigidbody.linearVelocity.magnitude / 100)))) * Time.deltaTime);
                }
            }

            RPMUnit();
        }


        private void RPMUnit()
        {
            value = currentRPM / 2000;
            if (rpmUnit > 1)
            {
                rpmUnit = 1;
            }
            else
            {
                rpmUnit = Mathf.Lerp(rpmUnit, value, 7 * Time.deltaTime);
            }
        }

        private void setUpGearBox()
        {
            numberOfGears = gearForce.Length;

            /* 
             for (int i = 0; i < numberOfGears; i++)
             {
                 if (i == 0)
                 {
                     gearForce[i] = 0f;
                 }
                 else
                 {
                     gearForce[i] = i; 
                 }
             }*/
        }

        public void boxOfTransmission(float inputForce)
        {
            float vehicleSpeed = vehicleRigidbody.linearVelocity.magnitude;
            float forceSense;

            if (onRear)
            {
                forceSense = -1;
            }
            else
            {
                if (VehicleController.inputV > 0)
                {
                    forceSense = 1;
                }
                else
                {
                    forceSense = 0;
                }
            }

            float forceOfEngine = Mathf.Abs(inputForce) * forceSense;


            if (onRear)
            {
                if (VehicleController.inputV < 0)
                {
                    outputWheelForce = rearForce * rearFatorForce * VehicleController.inputV;
                }
                else
                {
                    outputWheelForce = 0;
                }
            }
            else
            {
                outputWheelForce = forceOfEngine * clutch * (gearForce[currentGear] / redutiveForce(vehicleSpeed));
            }



            if (RPM >= 0.899f)
            {
                clutch = 0;
            }
            else
            {
                clutch = 1;
            }




            if (currentGear == 0)
            {
                if (onRear)
                {
                    currentRPM = vehicleSpeed * 60;
                }
                else
                {
                    currentRPM = VehicleController.inputV * 20 * 500;
                }
            }
            else
            {
                if (onRear)
                {
                    currentRPM = vehicleSpeed * 60;
                }
                else
                {
                    currentRPM = CalculateRPM();
                }
            }
            valueSet = indexRPM * baseRPM + (fatorOfSmooth());

            float fatorOfSmooth()
            {
                return (indexRPM * baseRPM) / (baseRPM * 1.5f);
            }



            if (vehicleSpeed <= 0.01f && !onChangeSense)
            {
                if (VehicleController.inputV < 0)
                {
                    fatorChangeSense += 3 * Time.deltaTime;
                    if (fatorChangeSense >= minValueToChange)
                    {
                        StartCoroutine(ChangeFator());
                        onChangeSense = true;
                    }
                }

                if (VehicleController.inputV > 0 && onRear)
                {
                    fatorChangeSense += 3 * Time.deltaTime;
                    if (fatorChangeSense >= minValueToChange)
                    {
                        StartCoroutine(ChangeFator());
                        onChangeSense = true;
                    }
                }
            }
            else
            {
                fatorChangeSense = 0;
            }

            float redutiveForce(float vehicleSpeed)
            {
                if (vehicleSpeed < 7)
                {
                    return (1 + (((10 - vehicleSpeed) * (currentGear - 1))));
                }
                else
                {
                    return 1;
                }
            }
        }

        private void ShiftSystem()
        {
            currentVelocity = (vehicleRigidbody.linearVelocity.magnitude * 3.6f) / 2;
            if (!isAutoTransmission)
            {
                if (Input.GetKey(KeyCode.LeftShift) && !onShift && currentGear < numberOfGears - 1)
                {
                    StartCoroutine(shiftUp());
                    onShift = true;
                }
                if (Input.GetKey(KeyCode.LeftControl) && !onShift && currentGear > 0)
                {
                    StartCoroutine(shiftDown());
                    onShift = true;
                }
            }
            else
            {
                if (Input.GetKey(KeyCode.UpArrow) && RPM > swapRatio && !onShift && currentGear < numberOfGears - 1)
                {
                    StartCoroutine(shiftUp());
                    onShift = true;
                }
                else if (!Input.GetKey(KeyCode.UpArrow) && RPM < (reduceRatio + (currentGear / 10) + 0.2f) && !onShift && currentGear > 1 && currentVelocity <= velocityBase * currentGear)
                {
                    StartCoroutine(shiftDown());
                    onShift = true;
                }

            }
        }

        private float CalculateRPM()
        {

            float vehicleSpeed = vehicleRigidbody.linearVelocity.magnitude;


            if (vehicleSpeed <= 0 || currentGear <= 0)
            {
                return 0f;
            }

            return (vehicleSpeed * gearForce[currentGear]) * 60;
        }

        IEnumerator ChangeFator()
        {
            if (fatorChangeSense > 0)
            {
                onRear = !onRear;
                fatorChangeSense = 0;
            }
            yield return new WaitForSeconds(1);
            onChangeSense = false;
        }



        IEnumerator shiftUp()
        {
            if (burnTimes == 1)
            {
                burnTimes = Random.Range(1, 4);
            }
            else
            {
                burnTimes = 1;
            }
            clutch = Mathf.Lerp(clutch, 0, 7 * Time.deltaTime);
            yield return new WaitForSeconds(clutchInterval);
            if (burnTimes > 2)
            {
                burnTimes = 1;
            }
            clutch = Mathf.Lerp(clutch, 1, 5 * Time.deltaTime);
            if (currentGear > 0)
            {
                if (burnTimes == 1)
                {
                    shot = true;
                }

                if (burnTimes == 2)
                {
                    shot = true;
                    yield return new WaitForSeconds(0.015f);
                    shot = false;
                    yield return new WaitForSeconds(0.085f);
                    shot = true;
                }
            }
            yield return new WaitForSeconds(0.4f);
            if (onShift)
            {
                currentGear++;
                clutch = 1;
                shot = false;
                onShift = false;
            }
        }

        IEnumerator shiftDown()
        {
            clutch = Mathf.Lerp(clutch, 0, 7 * Time.deltaTime);
            yield return new WaitForSeconds(clutchInterval);
            clutch = Mathf.Lerp(clutch, 1, 5 * Time.deltaTime);
            yield return new WaitForSeconds(0.4f);
            if (onShift)
            {
                currentGear--;
                clutch = 1;
                onShift = false;
            }
        }

        public float wheelForce()
        {
            return outputWheelForce;
        }

        public float GetCurrentRPM()
        {
            return currentRPM;
        }
    }

}
    