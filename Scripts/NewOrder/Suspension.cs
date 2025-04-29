using UnityEngine;
using System.Collections;

namespace VehicleCore
{
    public class Suspension : MonoBehaviour
    {
        public WheelCollider rearLeftWheel, rearRightWheel;
        public float force = 10000f, stabilityBySpeed = 800f;

        public Rigidbody rigidBody;
        private bool isGroundedLeft, isGroundedRight;

        void Start()
        {
            //rigidBody = GetComponent<Rigidbody>();
            // Ajuste do centro de massa para melhorar a estabilidade
            rigidBody.centerOfMass += new Vector3(0, -0.3f, -0.3f);
        }

        void Update()
        {
            float leftWheelForce = 1f;
            float rightWheelForce = 1f;

            // Verificar colis�o da roda esquerda com o ch�o
            WheelHit hit;
            isGroundedLeft = rearLeftWheel.GetGroundHit(out hit);
            if (isGroundedLeft)
            {
                leftWheelForce = (-rearLeftWheel.transform.InverseTransformPoint(hit.point).y - rearLeftWheel.radius) / rearLeftWheel.suspensionDistance;
            }

            // Verificar colis�o da roda direita com o ch�o
            isGroundedRight = rearRightWheel.GetGroundHit(out hit);
            if (isGroundedRight)
            {
                rightWheelForce = (-rearRightWheel.transform.InverseTransformPoint(hit.point).y - rearRightWheel.radius) / rearRightWheel.suspensionDistance;
            }

            // Aplicar for�a anti-rolagem
            float antiRollForce = (leftWheelForce - rightWheelForce) * force;

            if (isGroundedLeft)
            {
                rigidBody.AddForceAtPosition(rearLeftWheel.transform.up * -antiRollForce, rearLeftWheel.transform.position);
            }
            if (isGroundedRight)
            {
                rigidBody.AddForceAtPosition(rearRightWheel.transform.up * -antiRollForce, rearRightWheel.transform.position);
            }
        }

        void FixedUpdate()
        {
            // Se o ve�culo estiver no ch�o, aplicar for�a para mant�-lo est�vel
            if (isGroundedLeft || isGroundedRight)
            {
                rigidBody.AddForce(-transform.up * (5000f + stabilityBySpeed * Mathf.Abs(rigidBody.linearVelocity.magnitude * 3.6f)));
            }

            // Limitar a velocidade m�xima
            rigidBody.linearVelocity = Vector3.ClampMagnitude(rigidBody.linearVelocity, 300f);
        }
    }
}