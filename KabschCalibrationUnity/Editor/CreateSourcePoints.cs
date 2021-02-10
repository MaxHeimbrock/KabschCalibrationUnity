using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CalibrateObject))]
public class CreateSourcePoints : Editor
{
    private enum Mode
    {
        none, edit, add
    }

    private Mode mode = Mode.none;
    CalibrateObject currentObject;
    private int currentIndex;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        currentObject = target as CalibrateObject;

        int numberOfSourcePoints = currentObject.sourcePoints.Length;
        Color baseColor = GUI.color;

        for (int i = 0; i < numberOfSourcePoints; i++)
        {
            GUILayout.BeginHorizontal();
            GUI.color = ColorOrder.GetColor(i);
            GUILayout.Box("             ");

            GUI.color = baseColor;

            if (mode == Mode.edit && currentIndex == i)
            {
                if (GUILayout.Button("Stop editing"))
                {
                    mode = Mode.none;
                    currentIndex = -1;
                }
            }
            else
            {
                if (GUILayout.Button("Edit source point"))
                {
                    mode = Mode.edit;
                    currentIndex = i;
                }
            }

            GUILayout.EndHorizontal();
        }

        if (mode == Mode.add)
        {
            if (GUILayout.Button("Click on object to add"))
            {

            }
        }
        else
        {
            if (GUILayout.Button("Add new source point"))
            {
                mode = Mode.add;
                currentIndex = numberOfSourcePoints;
            }
        }

        if (GUILayout.Button("Delete source point"))
        {
            currentObject.DeleteLastSourcePoint();
        }
    }

    void OnSceneGUI()
    {
        if (mode != Mode.none)
        {
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
            Tools.current = Tool.None;

            currentObject = target as CalibrateObject;
            Collider[] childrenCollider = currentObject.GetComponentsInChildren<Collider>();

            if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
            {
                Ray worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                RaycastHit hitInfo;
                if (Physics.Raycast(worldRay, out hitInfo, 10000, -1, QueryTriggerInteraction.Ignore))
                {
                    foreach (Collider currentCollider in childrenCollider)
                    {
                        if (hitInfo.collider.Equals(currentCollider))
                        {
                            if (mode == Mode.add)
                            {
                                GameObject sourcePoint = Instantiate(Resources.Load("SourcePointPrefab", typeof(GameObject)), hitInfo.point, Quaternion.identity, currentObject.transform) as GameObject;
                                if (sourcePoint == null)
                                {
                                    Event.current.Use();
                                    return;
                                }
                                Renderer renderer = sourcePoint.GetComponent<Renderer>();
                                renderer.material = new Material(Shader.Find("UI/Unlit/Detail"));
                                renderer.sharedMaterial.color = ColorOrder.GetColor(currentIndex);
                                currentObject.AddSourcePoint(sourcePoint.transform);
                            }
                            else if (mode == Mode.edit)
                            {
                                currentObject.sourcePoints[currentIndex].transform.position = hitInfo.point;
                            }
                            mode = Mode.none;
                        }
                    }
                }

                Event.current.Use();
            }
        }
    }
}
