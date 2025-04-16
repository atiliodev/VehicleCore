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

        // Tipo de Veículo
        EditorGUILayout.LabelField("Tipo de Veículo", EditorStyles.boldLabel); // Simula o [Header]
        carController.vehicleType = (CarControllerSystem.VehicleType)EditorGUILayout.EnumPopup("Tipo de Veículo", carController.vehicleType);

        EditorGUILayout.Space(); // Adiciona espaçamento para organização

        // Seção: Motor
        showMotor = EditorGUILayout.Foldout(showMotor, "Motor");
        if (showMotor)
        {
            EditorGUI.indentLevel++;
            carController.motorSettings.enginePower = EditorGUILayout.FloatField("Potência do Motor", carController.motorSettings.enginePower);
            carController.motorSettings.engineTorque = EditorGUILayout.FloatField("Torque do Motor", carController.motorSettings.engineTorque);
            carController.motorSettings.maxSpeed = EditorGUILayout.FloatField("Velocidade Máxima", carController.motorSettings.maxSpeed);
            EditorGUI.indentLevel--;
        }

        // Seção: Suspensão
        showSuspension = EditorGUILayout.Foldout(showSuspension, "Suspensão");
        if (showSuspension)
        {
            EditorGUI.indentLevel++;
            carController.suspensionSettings.suspensionSpring = EditorGUILayout.FloatField("Rigidez da Mola", carController.suspensionSettings.suspensionSpring);
            carController.suspensionSettings.suspensionDamper = EditorGUILayout.FloatField("Amortecimento", carController.suspensionSettings.suspensionDamper);
            carController.suspensionSettings.suspensionHeight = EditorGUILayout.FloatField("Altura da Suspensão", carController.suspensionSettings.suspensionHeight);
            EditorGUI.indentLevel--;
        }

        // Seção: Transmissão
        showTransmission = EditorGUILayout.Foldout(showTransmission, "Transmissão");
        if (showTransmission)
        {
            EditorGUI.indentLevel++;
            carController.transmissionSettings.gearCount = EditorGUILayout.IntField("Número de Marchas", carController.transmissionSettings.gearCount);
            carController.transmissionSettings.gearRatio = EditorGUILayout.FloatField("Relação de Transmissão", carController.transmissionSettings.gearRatio);
            carController.transmissionSettings.gearShiftSmoothness = EditorGUILayout.FloatField("Suavidade da Troca", carController.transmissionSettings.gearShiftSmoothness);
            EditorGUI.indentLevel--;
        }

        // Seção: Freios
        showBrakes = EditorGUILayout.Foldout(showBrakes, "Freios");
        if (showBrakes)
        {
            EditorGUI.indentLevel++;
            carController.brakeSettings.brakeForce = EditorGUILayout.FloatField("Força de Frenagem", carController.brakeSettings.brakeForce);
            carController.brakeSettings.brakeBias = EditorGUILayout.FloatField("Distribuição de Frenagem", carController.brakeSettings.brakeBias);
            carController.brakeSettings.handbrakeForce = EditorGUILayout.FloatField("Força do Freio de Mão", carController.brakeSettings.handbrakeForce);
            EditorGUI.indentLevel--;
        }

        // Salvar mudanças
        if (GUI.changed)
        {
            EditorUtility.SetDirty(carController);
        }
    }
}

