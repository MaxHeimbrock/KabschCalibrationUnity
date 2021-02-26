using System;
using UnityEngine;

public class CalibrateObject : MonoBehaviour
{
	public Transform[] sourcePoints = new Transform[0];
	public Transform[] targetPoints;
	public int calibrationPointIndex;
	
	private Vector3 origPosition;
	private Quaternion origRotation;
    public float calibrationDistanceError = 0;

    // Use this for initialization
    public void Start () 
	{
		if (sourcePoints != null)
		{
			targetPoints = new Transform[sourcePoints.Length];	
		}
		calibrationPointIndex = 0;
		
		SaveOriginalPosition ();
	}
	
	public void ResetAllTargetPoints() {
		foreach (Transform currRefPoint in targetPoints)
		{
			if (currRefPoint != null)
			{
				Destroy (currRefPoint.gameObject);	
			}
		}

		targetPoints = new Transform[sourcePoints.Length];
		calibrationPointIndex = 0;
	}

	public void ResetLastTargetPoint()
	{
		int indexToDelete;
		
		if (calibrationPointIndex == 0)
		{
			indexToDelete = sourcePoints.Length - 1;
		}
		else
		{
			indexToDelete = calibrationPointIndex - 1;
		}

		if (targetPoints[indexToDelete] != null)
		{
			Destroy(targetPoints[indexToDelete].gameObject);	
			calibrationPointIndex = indexToDelete;
		}
	}

	public void AddTargetPoint(Vector3 position, Transform targetPointParent)
	{
		// TODO: Update GUI in Editor everytime this gets called
		
		GameObject newRefPoint;

		if (targetPoints[calibrationPointIndex] == null)
		{
			newRefPoint = Instantiate(Resources.Load("SourcePointPrefab", typeof(GameObject)), position, Quaternion.identity, targetPointParent) as GameObject;
			newRefPoint.name = "TargetPoint";
			Renderer renderer = newRefPoint.GetComponent<Renderer>();
			renderer.material = new Material(Shader.Find("UI/Unlit/Detail"));
			renderer.sharedMaterial.color = ColorOrder.GetColor(calibrationPointIndex);
		}
		else
		{
			newRefPoint = targetPoints[calibrationPointIndex].gameObject;
		}

		newRefPoint.transform.SetPositionAndRotation(new Vector3(position.x, position.y, position.z), Quaternion.identity);
		targetPoints[calibrationPointIndex] = newRefPoint.transform;
		calibrationPointIndex += 1;
		
		if (calibrationPointIndex >= sourcePoints.Length)
		{
			calibrationPointIndex = 0;
			Matrix4x4 alignmentTransform = CalculateAlignmentTransform();
			ApplyAlignment(alignmentTransform);
			calibrationDistanceError = CalculateCalibrationDistance();
			SaveOriginalPosition();
		}
	}

    private float CalculateCalibrationDistance()
    {
		float result = 0;

		for(int i = 0; i < sourcePoints.Length; i++)
        {
			result += Vector3.Distance(sourcePoints[i].position, targetPoints[i].position);
        }

		result = result / sourcePoints.Length;
		return result;
    }

    public void ApplyAlignment(Matrix4x4 alignmentTransform)
	{
		transform.position = alignmentTransform.MultiplyPoint3x4(origPosition);
		transform.rotation = alignmentTransform.rotation * origRotation;

		Debug.Log("calibrate Object");
	}

	public Matrix4x4 CalculateAlignmentTransform()
	{
		Vector3[] sourcePointsPosition = new Vector3[sourcePoints.Length];
		Vector4[] targetPointsPosition = new Vector4[sourcePoints.Length];;

		for (int i = 0; i < sourcePoints.Length; i++)
		{
			sourcePointsPosition[i] = sourcePoints[i].position;
			targetPointsPosition[i] = new Vector4(targetPoints[i].position.x, targetPoints[i].position.y, targetPoints[i].position.z, 1);
		}

		return KabschSolver.SolveKabsch(sourcePointsPosition, targetPointsPosition);
	}

	public void SaveOriginalPosition ()
	{
		origPosition = transform.position;
		origRotation = transform.rotation;
	}
	
	public void AddSourcePoint(Transform point)
	{
		Transform[] newArray = new Transform[sourcePoints.Length + 1];

		for (int i = 0; i < sourcePoints.Length; i++)
		{
			newArray[i] = sourcePoints[i];
		}

		newArray[sourcePoints.Length] = point;
		sourcePoints = newArray;
		
		targetPoints = new Transform[sourcePoints.Length];
	}

	public void DeleteLastSourcePoint()
	{
		if(sourcePoints.Length == 0)
        {
			return;
        }

		DestroyImmediate(sourcePoints[sourcePoints.Length - 1].gameObject);
		
		Transform[] newArray = new Transform[sourcePoints.Length - 1];

		for (int i = 0; i < sourcePoints.Length - 1; i++)
		{
			newArray[i] = sourcePoints[i];
		}
		
		sourcePoints = newArray;
		
		targetPoints = new Transform[sourcePoints.Length];
	}
}