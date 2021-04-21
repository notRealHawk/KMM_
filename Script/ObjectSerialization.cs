using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectSerialization : MonoBehaviour
{
	public List<Feature> features;
	public ObjectSerialization myFeatures;
	public Data data ;
	public Feature playerFeatures;
	public int currFeature;
	
	void OnEnable()
	{
		data = new Data();
		LoadDataFromFile();
	}
	public void SaveDataInFile()
	{
		Debug.Log("Name : " + name);
		if(data != null)
		{
			data.indexes.Clear();
		}
		for (int i = 0; i < features.Count; i++)
		{
			//Debug.Log();
			data.indexes.Add(features[i].currIndex);
		}
		string path = Path.Combine(Application.persistentDataPath + "/data.json");
		string newdata = JsonUtility.ToJson(data);
		File.WriteAllText(path, newdata);
		//SaveSystem.Save("PlayerData", newdata);
		Debug.Log(newdata);
		//LoadData();
	}
	public void LoadDataFromFile()
	{
		//string mydata = SaveSystem.Load("PlayerData");
		if(File.Exists(Application.persistentDataPath + "/data.json")){
			data = JsonUtility.FromJson<Data>(File.ReadAllText(Application.persistentDataPath + "/data.json"));
			Debug.Log("Data Loaded From File");
			if (data != null)
			{
				for (int i = 0; i < data.indexes.Count; i++)
				{
					Transform CurrentFeature = gameObject.transform.GetChild(i);
					CurrentFeature.GetChild(features[i].currIndex).gameObject.SetActive(false);
					features[i].currIndex = data.indexes[i];
					//features[i].MeshObj.mesh = features[i].choices[features[i].currIndex].GetComponent<MeshFilter>().sharedMesh;
					//features[i].ObjRenderer.material = features[i].choices[features[i].currIndex].GetComponent<MeshRenderer>().sharedMaterial;
					features[currFeature].UpdateObject();
					CurrentFeature.GetChild(features[i].currIndex).gameObject.SetActive(true);
				}
			}
		}
	}
	public void SetCurr(int index){
		if(features == null){
			return;
		}
		currFeature = index;
	}
	public void NextChoice()
	{
		if(features == null){
			return;
		}
		Transform CurrentFeature = gameObject.transform.GetChild(currFeature);
		CurrentFeature.GetChild(features[currFeature].currIndex).gameObject.SetActive(false);
		features[currFeature].currIndex++;
		features[currFeature].UpdateObject();
		CurrentFeature.GetChild(features[currFeature].currIndex).gameObject.SetActive(true);
	}
	public void PrevChoice(){
		if(features == null){
			return;
		}
		Transform CurrentFeature = gameObject.transform.GetChild(currFeature);
		CurrentFeature.GetChild(features[currFeature].currIndex).gameObject.SetActive(false);
		features[currFeature].currIndex--;
		features[currFeature].UpdateObject();
		CurrentFeature.GetChild(features[currFeature].currIndex).gameObject.SetActive(true);
	}

	public void randGenerator()
	{
		
		for (int i = 0; i < features.Count;i++)
		{
			int randomNum = Random.Range(0, features[i].choices.Count);
			currFeature = i;
			Transform CurrentFeature = gameObject.transform.GetChild(currFeature);
			CurrentFeature.GetChild(features[currFeature].currIndex).gameObject.SetActive(false);
			features[currFeature].currIndex = randomNum;
			features[currFeature].UpdateObject();
			CurrentFeature.GetChild(features[currFeature].currIndex).gameObject.SetActive(true);
		}
	}
}

[System.Serializable]
public class Data
{
	[SerializeField] public string name;
	[SerializeField] public List<int> indexes;

	public  Data()
	{
		name = "";
		indexes = new List<int>();
	}
}
[System.Serializable]
public class Feature
{
    //public GameObject playerhat;
    //public GameObject playershirt;
    
    public string ID;
    public int currIndex;
    public List<GameObject> choices;
    public MeshFilter MeshObj;
    public MeshRenderer ObjRenderer;
    
    public Feature(string id, MeshFilter meshObject, MeshRenderer meshRender){
    	ID = id;
	    MeshObj = meshObject;
	    ObjRenderer = meshRender;
    	UpdateObject();
    }
    public void UpdateObject(){
	    /*choices = new List<GameObject>();
	    foreach(GameObject g in Resources.LoadAll(("Prefabs/" + ID), typeof(GameObject)))
	    {
		    choices.Add(g);
	    }*/
    	//choices = Resources.LoadAll("Prefabs/" + ID) as typeof(GameObject);
    	if(choices == null /*|| MeshObj == null*/){
    		return;
    	}
    	if(currIndex < 0){
    		currIndex = choices.Count -1;
    	}
	    if(currIndex >= choices.Count){
		    currIndex = 0;
	    }
    	
    	//MeshObj.mesh = choices[currIndex].GetComponent<MeshFilter>().sharedMesh;
    	//ObjRenderer.material = choices[currIndex].GetComponent<MeshRenderer>().sharedMaterial;
    }
}
[System.Serializable]
public class Playerhat
{
	public GameObject Hat;
	public Material HatMaterial;
}
[System.Serializable]
public class Playershirt
{
	public GameObject Shirt;
	public Material ShirtMaterial;
}
[System.Serializable]
public class Playerpant
{
	public GameObject Pant;
	public Material PantMaterial;
}