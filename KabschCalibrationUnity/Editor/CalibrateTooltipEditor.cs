using UnityEngine;
using System.Collections;

using UnityEditor;

#if UNITY_EDITOR

[CustomEditor(typeof(CalibrateTooltip))]
public class CalibrateTooltipEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CalibrateTooltip myScript = (CalibrateTooltip)target;
        if (myScript.GetActive())
        {
            if (GUILayout.Button("Stop Calibration"))
            {
                myScript.SetActive(false);
            }
        }
        else
        {
            if (GUILayout.Button("Start Calibration"))
            {
                myScript.SetActive(true);
            }
        }

        if (GUILayout.Button("Reset"))
        {
            myScript.Clear();
        }
    }
}
#endif