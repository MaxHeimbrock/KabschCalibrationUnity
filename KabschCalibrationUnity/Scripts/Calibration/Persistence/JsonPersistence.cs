using System;
using System.IO;
using UnityEngine;

public static class JsonPersistence
{
	public static void SaveToFile (string filePath, string json, bool overwrite = true)
	{
		string folderPath = GetFolderName(filePath);
		if (!Directory.Exists(folderPath))
		{
			Directory.CreateDirectory(folderPath); 
		}
		
		if (!overwrite)
		{
			if (new FileInfo(filePath).Exists)
			{
				return;
			}
		}

		StreamWriter writer = File.CreateText(filePath);
		writer.WriteLine (json);
		writer.Close ();
	}

	public static string LoadFromFile (string filePath)
	{
		StreamReader reader = new StreamReader(filePath); 
		string fileContent = reader.ReadToEnd ();
		reader.Close ();
		return fileContent;
	}
	
	private static string GetFolderName(string completePath)
	{
		string[] subPaths = completePath.Split('/');

		string result = "";

		for (int i = 0; i < subPaths.Length - 1; i++)
		{
			if (i != 0)
			{
				result = result + "/";
			}
			result = result + subPaths[i];
		}
		
		return result;
	}
}