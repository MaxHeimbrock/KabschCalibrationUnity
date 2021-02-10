using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TransformInfo
{
	public string name;
	public Vector3 position;
	public Quaternion rotation;

	public TransformInfo(Transform transform) {
		name = transform.name;
		SetValues(transform.position, transform.rotation);
		
		/*
		if (transform.GetComponent<UUID> ().transformType == UUID.TransformType.World) {
			SetValues(transform.position, transform.rotation, transform.lossyScale);
		} else if (transform.GetComponent<UUID> ().transformType == UUID.TransformType.Local) {
			SetValues(transform.localPosition, transform.localRotation, transform.localScale);
		}
		*/
	}

	public void PasteTransformValues(TransformInfo transformInfo) {
		SetValues(transformInfo.position, transformInfo.rotation);
	}

	private void SetValues(Vector3 position, Quaternion rotation) {
		this.position = position;
		this.rotation = rotation;
	}
}
