using UnityEngine;
using TMPro;

namespace VehicleCore
{
    public class Speedometer : MonoBehaviour
    {
        public TextMeshProUGUI speedText;
        public TextMeshProUGUI gearText;


        [HideInInspector] public Transmission transmission;


        float velocity;
        string gear;
        private void Start()
        {
            transmission = GameObject.FindGameObjectWithTag("VehicleCore").GetComponent<Transmission>();
        }

        void Update()
        {
            velocity = transmission.vehicleRigidbody.linearVelocity.magnitude * 3;

            int intValue = (int)velocity;
            speedText.text = " " + intValue.ToString("D3");

            gearText.text = gear;

            if(transmission.onRear)
            {
                gear = "R";
            }
            else if(!transmission.onRear && transmission.currentGear == 0)
            {
                gear = "N";
            }
            else if(!transmission.onRear && transmission.currentGear >= 1)
            {
                gear = " " + transmission.currentGear;
            }

        }
    }
}
