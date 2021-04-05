using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Profile : MonoBehaviour {



    void Start()
    {
        InitialValues();
    }
    public void InitialValues()
    {
//        Name = "";
		if (!PlayerPrefs.HasKey ("Souls")) {
			Souls = 800;
			MainMenu.mainMenu.PlayerGoldText.text = Souls.ToString();
		}
    }
    public static string Name
     {
        get {return PlayerPrefs.GetString("Name"); }
        set{ PlayerPrefs.SetString("Name",value); }
    }

	public static int Souls
	{
		get { return PlayerPrefs.GetInt("Souls"); }
		set { PlayerPrefs.SetInt("Souls", value); }
	}

}
