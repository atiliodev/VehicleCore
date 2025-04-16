using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CarControllerSystem))]
public class CarControllerSystemEditor : Editor
{
    private bool showMotor = false;
    private bool showSuspension = false;
    private bool showTransmission = false;
    private bool showBrakes = false;
    public override void OnInspectorGUI()
    {
        CarControllerSystem carController = (CarControllerSystem)target;

        // Tipo de Ve�culo
        EditorGUILayout.LabelField("Tipo de Ve�culo", EditorStyles.boldLabel); // Simula o [Header]
        carController.vehicleType = (CarControllerSystem.VehicleType)EditorGUILayout.EnumPopup("Tipo de Ve�culo", carController.vehicleType);

        EditorGUILayout.Space(); // Adiciona espa�amento para organiza��o

        // Se��o: Motor
        showMotor = EditorGUILayout.Foldout(showMotor, "Motor");
        if (showMotor)
        {
            EditorGUI.indentLevel++;
            carController.motorSettings.enginePower = EditorGUILayout.FloatField("Pot�ncia do Motor", carController.motorSettings.enginePower);
            carController.motorSettings.engineTorque = EditorGUILayout.FloatField("Torque do Motor", carController.motorSettings.engineTorque);
            carController.motorSettings.maxSpeed = EditorGUILayout.FloatField("Velocidade M�xima", carController.motorSettings.maxSpeed);
            EditorGUI.indentLevel--;
        }

        // Se��o: Suspens�o
        showSuspension = EditorGUILayout.Foldout(showSuspension, "Suspens�o");
        if (showSuspension)
        {
            EditorGUI.indentLevel++;
            carController.suspensionSettings.suspensionSpring = EditorGUILayout.FloatField("Rigidez da Mola", carController.suspensionSettings.suspensionSpring);
            carController.suspensionSettings.suspensionDamper = EditorGUILayout.FloatField("Amortecimento", carController.suspensionSettings.suspensionDamper);
            carController.suspensionSettings.suspensionHeight = EditorGUILayout.FloatField("Altura da Suspens�o", carController.suspensionSettings.suspensionHeight);
            EditorGUI.indentLevel--;
        }

        // Se��o: Transmiss�o
        showTransmission = EditorGUILayout.Foldout(showTransmission, "Transmiss�o");
        if (showTransmission)
        {
            EditorGUI.indentLevel++;
            carController.transmissionSettings.gearCount = EditorGUILayout.IntField("N�mero de Marchas", carController.transmissionSettings.gearCount);
            carController.transmissionSettings.gearRatio = EditorGUILayout.FloatField("Rela��o de Transmiss�o", carController.transmissionSettings.gearRatio);
            carController.transmissionSettings.gearShiftSmoothness = EditorGUILayout.FloatField("Suavidade da Troca", carController.transmissionSettings.gearShiftSmoothness);
            EditorGUI.indentLevel--;
        }

        // Se��o: Freios
        showBrakes = EditorGUILayout.Foldout(showBrakes, "Freios");
        if (showBrakes)
        {
            EditorGUI.indentLevel++;
            carController.brakeSettings.brakeForce = EditorGUILayout.FloatField("For�a de Frenagem", carController.brakeSettings.brakeForce);
            carController.brakeSettings.brakeBias = EditorGUILayout.FloatField("Distribui��o de Frenagem", carController.brakeSettings.brakeBias);
            carController.brakeSettings.handbrakeForce = EditorGUILayout.FloatField("For�a do Freio de M�o", carController.brakeSettings.handbrakeForce);
            EditorGUI.indentLevel--;
        }

        // Salvar mudan�as
        if (GUI.changed)
        {
            EditorUtility.SetDirty(carController);
        }
    }
}

