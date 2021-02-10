using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class TransformPersistence : MonoBehaviour
{
	public string persistenceFilePath = "Calibration/SavedTransforms.txt";
	
	public bool loadCalibrationAtStartup;
	
	List<TransformInfo> tranformToSave;

	private static TransformPersistence instance;
	public static TransformPersistence GetInstance ()
	{
		return instance;
	}

	void Awake ()
	{
		if (instance == null) {
			instance = this;
			InitializeVariables ();
		} else if (instance != this) {
			Destroy (gameObject);
			Debug.Log("Duplicate singleton");
		} 
	}

	void Start() 
	{
		if (loadCalibrationAtStartup)
		{
			LoadAndApplyTransformationFromFile();
		}
	}

	private void InitializeVariables ()
	{
		if (tranformToSave == null) {
			tranformToSave = new List<TransformInfo> ();
		}
	}

	public void AddTransform (Transform transform)
	{
		TransformInfo exists = null;
		TransformInfo transInfo = new TransformInfo (transform);

		foreach(TransformInfo transformToSaveCurr in tranformToSave) {
			if (transformToSaveCurr.name == transInfo.name) {
				exists = transformToSaveCurr;
			}
		}

		if (exists == null) {
			tranformToSave.Add (transInfo);
		} else {
			exists.PasteTransformValues (transInfo);
		}
	}

	private void AddAllObjects() 
	{
		CalibrateObject[] objects = FindObjectsOfType<CalibrateObject>();

		foreach (CalibrateObject obj in objects) 
		{
			AddTransform(obj.transform);
		}
	}

	//Save Transform
	public void SaveToFile ()
	{
		InitializeVariables();
		AddAllObjects ();

		string jsonTransform = JsonHelper.ToJson (tranformToSave.ToArray (), true);
		JsonPersistence.SaveToFile (persistenceFilePath, jsonTransform);
		Debug.Log ("Saved " + tranformToSave.Count + " Transformations to file");
	}

	public TransformInfo[] LoadFromFile ()
	{
		InitializeVariables();

		if (!File.Exists(persistenceFilePath))
		{
			Debug.Log(persistenceFilePath + " not found.");
			return null;
		}

		string fileContent = JsonPersistence.LoadFromFile(persistenceFilePath);
		TransformInfo[] savedTransforms = JsonHelper.FromJson<TransformInfo> (fileContent);
		Debug.Log ("Loaded " + savedTransforms.Length + " transformations from file");
		
		return savedTransforms;
	}

	public void LoadAndApplyTransformationFromFile() {
		TransformInfo[] savedTransforms = LoadFromFile ();
		ApplyTransformation (savedTransforms);
	}

	public void ApplyTransformation (TransformInfo[] transformInfo)
	{
		foreach (TransformInfo currentTransformInfo in transformInfo)
		{
			GameObject transInScene = GameObject.Find(currentTransformInfo.name);

			if (transInScene == null) {
				Debug.Log ("GameObject with name " + currentTransformInfo.name + " not found.");
				continue;
			} 
			if (transInScene.GetComponent<CalibrateObject>() == null)
			{
                if (transInScene.name.Equals("[SteamVRComponents]") == true)
                {
					Debug.Log("Set transformation for GameObject " + transInScene.name + " (" + currentTransformInfo.name + ")");
					transInScene.transform.position = currentTransformInfo.position;
					transInScene.transform.rotation = currentTransformInfo.rotation;
				}
                else
                {
					Debug.Log ("GameObject with name " + currentTransformInfo.name + " has no AlignObject script. Maybe the name is not unique.");
                }

				continue;
			}
			
			Debug.Log ("Set transformation for GameObject " + transInScene.name + " (" +  currentTransformInfo.name + ")");
			transInScene.transform.position = currentTransformInfo.position;
			transInScene.transform.rotation = currentTransformInfo.rotation;
			transInScene.GetComponent<CalibrateObject>().SaveOriginalPosition();
		}
	}
}
