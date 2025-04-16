using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

namespace VehiclePhysics
{
    public class LightSystem : MonoBehaviour
    {
        public bool isAutomatic = false;
        public bool withTurnSigns;
        public float nightThreshold = 0f;




        private Rigidbody carRB;
        private Light directionalLight;
        private Transmission transmission;


        public bool isNight;
        public bool low, mid, high;
        public bool left, right, emergency;
        public bool brake;


        public GameObject frontLowLight;
        public GameObject frontMidLight;
        public GameObject frontHighLight;
        public GameObject rearHighLight;
        public GameObject rearMidLight;
        public GameObject emergencyLight;

        public GameObject leftTurnSign;
        public GameObject rightTurnSign;

        public bool waitToChange;
        public bool waitToChangeSign;
        void Start()
        {
            carRB = GetComponent<Rigidbody>();
            transmission = GetComponent<Transmission>();

            Light[] lights = FindObjectsOfType<Light>();

            foreach (Light light in lights)
            {

                if (light.type == UnityEngine.LightType.Directional && light.intensity > 0.5f)
                {
                    directionalLight = light;
                    Debug.Log("Luz direcional encontrada: " + directionalLight.name);
                    break;
                }
            }

            if (directionalLight == null)
            {
                Debug.LogError("Nenhuma luz direcional foi encontrada com os crit�rios especificados!");
            }
        }


        void Update()
        {
            SunLight();
            FrontLightControl();
            BrakeInfo();

            ApplyLogic();

            LightSignControl();

        }

        private void LightSignControl()
        {
            emergencyLight.SetActive(emergency);

            if (emergency)
            {
                leftTurnSign.SetActive(false);
                rightTurnSign.SetActive(false);
            }
            else
            {
                leftTurnSign.SetActive(left);
                rightTurnSign.SetActive(right);
            }

            if (Input.GetKey(KeyCode.LeftBracket) && !waitToChangeSign)
            {
                if (!left)
                {
                    left = true;
                    right = false;
                    StartCoroutine(applyWaitSign());
                    waitToChangeSign = true;
                }
                else
                {
                    left = false;
                    StartCoroutine(applyWaitSign());
                    waitToChangeSign = true;
                }
            }

            if (Input.GetKey(KeyCode.RightBracket) && !waitToChangeSign)
            {
                if (!right)
                {
                    right = true;
                    left = false;
                    StartCoroutine(applyWaitSign());
                    waitToChangeSign = true;
                }
                else
                {
                    right = false;
                    StartCoroutine(applyWaitSign());
                    waitToChangeSign = true;
                }
            }

            if (Input.GetKey(KeyCode.T) && !waitToChangeSign)
            {
                if (!emergency)
                {
                    emergency = true;
                    StartCoroutine(applyWaitSign());
                    waitToChangeSign = true;
                }
                else
                {
                    emergency = false;
                    StartCoroutine(applyWaitSign());
                    waitToChangeSign = true;
                }
            }
        }

        private void ApplyLogic()
        {
            frontLowLight.SetActive(low);
            frontMidLight.SetActive(mid);

            if (mid)
            {
                frontHighLight.SetActive(high);
            }
            else
            {
                frontHighLight.SetActive(false);
            }
            rearMidLight.SetActive(low);
            rearHighLight.SetActive(brake);
        }

        private void BrakeInfo()
        {
            if (transmission.onRear)
            {
                if (Input.GetKey(KeyCode.UpArrow) && carRB.linearVelocity.magnitude > 2)
                {
                    brake = true;
                }
                else
                {
                    brake = false;
                }
            }
            else
            {
                if (Input.GetKey(KeyCode.DownArrow) && carRB.linearVelocity.magnitude > 2)
                {
                    brake = true;
                }
                else
                {
                    brake = false;
                }
            }

        }

        private void FrontLightControl()
        {
            if (Input.GetKey(KeyCode.K) && !waitToChange)
            {
                if (high)
                {
                    high = false;
                    StartCoroutine(applyWait());
                    waitToChange = true;
                }
                else
                {
                    high = true;
                    StartCoroutine(applyWait());
                    waitToChange = true;
                }
            }

            if (Input.GetKey(KeyCode.J) && !waitToChange)
            {
                if (low)
                {
                    if (mid)
                    {
                        low = false;
                        mid = false;
                        StartCoroutine(applyWait());
                        waitToChange = true;
                    }
                    else
                    {
                        mid = true;
                        StartCoroutine(applyWait());
                        waitToChange = true;
                    }
                }
                else
                {
                    low = true;
                    StartCoroutine(applyWait());
                    waitToChange = true;
                }

            }
        }

        IEnumerator applyWait()
        {
            yield return new WaitForSeconds(0.3f);
            waitToChange = false;
        }

        IEnumerator applyWaitSign()
        {
            yield return new WaitForSeconds(0.3f);
            waitToChangeSign = false;
        }

        private void SunLight()
        {
            if (directionalLight != null)
            {
                float sunAngle = Vector3.Dot(directionalLight.transform.forward, Vector3.down);
                if (sunAngle > nightThreshold)
                {
                    isNight = true;
                }
                else
                {
                    isNight = false;
                }
            }
        }
    }

}