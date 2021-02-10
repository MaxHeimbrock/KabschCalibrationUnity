using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CalibrationManager))]
public class CalibrationManagerEditor : Editor
{
    // OnInspector GUI
    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();
        CalibrationManager calibrationManagerTarget = (CalibrationManager)target;

        if (Application.isPlaying)
        {              
            calibrationManagerTarget.ChoiceIndex = EditorGUILayout.Popup(calibrationManagerTarget.ChoiceIndex, calibrationManagerTarget.AlignObjectChoices);
            calibrationManagerTarget.ObjectToCalibrate = calibrationManagerTarget.AlignObjectsInScene[calibrationManagerTarget.ChoiceIndex];
        }

        if (GUILayout.Button("Reset last target points")) 
        {
            calibrationManagerTarget.ResetLastTargetPoint();
            calibrationManagerTarget.ChangeColorOfPointer();
        }
        
        if (GUILayout.Button("Reset all target points")) 
        {
            calibrationManagerTarget.ResetTargetPoints();
            calibrationManagerTarget.ChangeColorOfPointer();
        }

        if (GUILayout.Button("Load")) 
        {
            calibrationManagerTarget.LoadCalibrationFromFile();
        }

        if (GUILayout.Button("Save")) 
        {
            calibrationManagerTarget.SaveCalibrationToFile();
        }
    }
}
