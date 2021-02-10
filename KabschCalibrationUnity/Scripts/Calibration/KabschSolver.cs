using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Taken from https://github.com/zalo/MathUtilities/blob/master/Assets/Kabsch/Kabsch.cs

// works only with solveScale = false
// but not a problem, because one unity length unit equals one meter in real world so no scaling necessary
public static class KabschSolver
{
	public static Matrix4x4 SolveKabsch (Vector3[] sourcePoints, Vector4[] targetPoints, bool solveRotation = true, bool solveScale = false)
	{
		Vector3[] quatBasis = new Vector3[3];
		Vector3[] dataCovariance = new Vector3[3];
		Quaternion optimalRotation = Quaternion.identity;
		float scaleRatio = 1f;
		
		if (sourcePoints.Length != targetPoints.Length) {
			return Matrix4x4.identity;
		}

		//Calculate the centroid offset and construct the centroid-shifted point matrices
		Vector3 sourceCentroid = Vector3.zero;
		Vector3 targetCentroid = Vector3.zero;
		for (int i = 0; i < sourcePoints.Length; i++) {
			sourceCentroid += new Vector3 (sourcePoints [i].x, sourcePoints [i].y, sourcePoints [i].z);
			targetCentroid += new Vector3 (targetPoints [i].x, targetPoints [i].y, targetPoints [i].z);
		}
		sourceCentroid /= sourcePoints.Length;
		targetCentroid /= sourcePoints.Length;

		//Calculate the scale ratio
		if (solveScale) {
			float inScale = 0f, refScale = 0f;
			for (int i = 0; i < sourcePoints.Length; i++) {
				inScale += (new Vector3 (sourcePoints [i].x, sourcePoints [i].y, sourcePoints [i].z) - sourceCentroid).magnitude;
				refScale += (new Vector3 (targetPoints [i].x, targetPoints [i].y, targetPoints [i].z) - targetCentroid).magnitude;
			}
			scaleRatio = (refScale / inScale);
		}

		//Calculate the 3x3 covariance matrix, and the optimal rotation
		if (solveRotation) {
			extractRotation (TransposeMultSubtract (sourcePoints, targetPoints, sourceCentroid, targetCentroid, dataCovariance), ref optimalRotation, quatBasis);
		}

		return Matrix4x4.TRS (targetCentroid, Quaternion.identity, Vector3.one * scaleRatio) *
			Matrix4x4.TRS (Vector3.zero, optimalRotation, Vector3.one) *
			Matrix4x4.TRS (-sourceCentroid, Quaternion.identity, Vector3.one);
	}

	//https://animation.rwth-aachen.de/media/papers/2016-MIG-StableRotation.pdf
	//Iteratively apply torque to the basis using Cross products (in place of SVD)
	static void extractRotation (Vector3[] A, ref Quaternion q, Vector3[] QuatBasis)
	{
		for (int iter = 0; iter < 9; iter++) {
			q.FillMatrixFromQuaternion (ref QuatBasis);
			Vector3 omega = (Vector3.Cross (QuatBasis [0], A [0]) +
				Vector3.Cross (QuatBasis [1], A [1]) +
				Vector3.Cross (QuatBasis [2], A [2])) *
				(1f / Mathf.Abs (Vector3.Dot (QuatBasis [0], A [0]) +
					Vector3.Dot (QuatBasis [1], A [1]) +
					Vector3.Dot (QuatBasis [2], A [2]) + 0.000000001f));

			float w = omega.magnitude;
			if (w < 0.000000001f)
				break;
			q = Quaternion.AngleAxis (w * Mathf.Rad2Deg, omega / w) * q;
			q = Quaternion.Lerp (q, q, 0f); //Normalizes the Quaternion; critical for error suppression
		}
	}

	//Calculate Covariance Matrices --------------------------------------------------
	public static Vector3[] TransposeMultSubtract (Vector3[] vec1, Vector4[] vec2, Vector3 vec1Centroid, Vector3 vec2Centroid, Vector3[] covariance)
	{
		for (int i = 0; i < 3; i++) { //i is the row in this matrix
			covariance [i] = Vector3.zero;
		}

		for (int k = 0; k < vec1.Length; k++) {//k is the column in this matrix
			Vector3 left = (vec1 [k] - vec1Centroid) * vec2 [k].w;
			Vector3 right = (new Vector3 (vec2 [k].x, vec2 [k].y, vec2 [k].z) - vec2Centroid) * Mathf.Abs (vec2 [k].w);

			covariance [0] [0] += left [0] * right [0];
			covariance [1] [0] += left [1] * right [0];
			covariance [2] [0] += left [2] * right [0];
			covariance [0] [1] += left [0] * right [1];
			covariance [1] [1] += left [1] * right [1];
			covariance [2] [1] += left [2] * right [1];
			covariance [0] [2] += left [0] * right [2];
			covariance [1] [2] += left [1] * right [2];
			covariance [2] [2] += left [2] * right [2];
		}

		return covariance;
	}
	
	public static void FillMatrixFromQuaternion (this Quaternion q, ref Vector3[] covariance)
	{
		covariance [0] = q * Vector3.right;
		covariance [1] = q * Vector3.up;
		covariance [2] = q * Vector3.forward;
	}
}