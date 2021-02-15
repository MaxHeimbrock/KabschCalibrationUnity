using System;
using UnityEngine;
using System.Text;
//using Valve.VR;
using Matrix = MathNet.Numerics.LinearAlgebra.Matrix<float>;


//Code taken from https://github.com/anthonysteed/CalibrateTooltip 

public class CalibrateTooltip : MonoBehaviour
{

    public GameObject controller;     // The controller to add a tooltip
    //public Mesh tooltipMesh;          // The mesh to use
    public float tooltipSize = 0.01f;   // Local scale of the mesh

    [Space(10)]
    public int index;

    // For export to python test script
    private StringBuilder rotBuilder;
    private StringBuilder posBuilder;

    // For creation of the tooltip object
    private GameObject tooltip;
    private Renderer meshRenderer;

    // For the inverse calculation
    private Matrix m;
    private Matrix v;

    private bool calibrationActive = false;
    private CalibrationManager calibrationManager;

    void Start()
    {
        calibrationManager = GetComponent<CalibrationManager>();
        tooltip = calibrationManager.tooltip.gameObject;
    }

    void Update()
    {
        if (calibrationActive)
        {
            // input from Keyboard
            if (Input.GetKeyDown(KeyCode.T))
            {
                AddOne();
            }
            
            /*
            Example to add custom input here, depending on VR system used - example is for SteamVR 1.0 
            if (SteamVR_Input._default.inActions.InteractUI.GetStateDown(SteamVR_Input_Sources.LeftHand ))
            {
                AddOne();
            }
            */

        }
    }

    private void AddOne()
    {
        print("set tippoint");
        index++;
        Matrix4x4 mat = controller.transform.localToWorldMatrix;

        { // For debugging & verification in Python tool
            Debug.Log("Add point (" + index + ")");
            posBuilder.Append(controller.transform.localPosition.x + "," + controller.transform.localPosition.y + "," + controller.transform.localPosition.z + ",");
            rotBuilder.Append(mat.m00 + "," + mat.m01 + "," + mat.m02 + "," +
                mat.m10 + "," + mat.m11 + "," + mat.m12 + "," +
                mat.m20 + "," + mat.m21 + "," + mat.m22 + ",");
            Debug.Log(posBuilder.ToString());
            Debug.Log(rotBuilder.ToString());
        }

        Matrix row = Matrix.Build.Dense(3, 3);
        row[0, 0] = -mat.m00;
        row[0, 1] = -mat.m01;
        row[0, 2] = -mat.m02;
        row[1, 0] = -mat.m10;
        row[1, 1] = -mat.m11;
        row[1, 2] = -mat.m12;
        row[2, 0] = -mat.m20;
        row[2, 1] = -mat.m21;
        row[2, 2] = -mat.m22;
        row = Matrix.Build.DenseIdentity(3).Append(row);

        Matrix col = Matrix.Build.Dense(3, 1);
        col[0, 0] = controller.transform.localPosition.x;
        col[1, 0] = controller.transform.localPosition.y;
        col[2, 0] = controller.transform.localPosition.z;

        if (index == 1)
        {
            m = row;
            v = col;
        }
        else
        {
            m = m.Stack(row);
            v = v.Stack(col);
        }

        if (index >= 8)
        {
            Matrix inv = m.PseudoInverse();
            Matrix res = inv.Multiply(v);
            tooltip.transform.localPosition = new Vector3(res[3, 0], res[4, 0], res[5, 0]);
            Debug.Log("Offset: " + tooltip.transform.localPosition.x + " " + tooltip.transform.localPosition.y + " " + tooltip.transform.localPosition.z);

            calibrationManager.tooltip = tooltip.transform;
            SetActive(false);
        }
    }

    public void Clear()
    {
        rotBuilder = new StringBuilder();
        posBuilder = new StringBuilder();
        index = 0;
        tooltip.transform.localPosition = new Vector3(0f, 0f, 0f);
        Debug.Log("Reset");
    }

    public void SetActive(bool active)
    {
        calibrationActive = active;
    }

    public bool GetActive()
    {
        return calibrationActive;
    }
}
