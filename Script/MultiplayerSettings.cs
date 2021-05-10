using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerSettings : MonoBehaviour {

	public static MultiplayerSettings multiplayersetting;
	public bool delayStart;
	public int maxPlayers;
	public float GameTime;
	public int menuScene;
	public int SelectedGameMode;
	public int SelectedLevel;
	public string PlayerName{ get ; private set;}
	void Awake(){
		//PlayerPrefs.DeleteAll();
		if (MultiplayerSettings.multiplayersetting == null) {
			MultiplayerSettings.multiplayersetting = this;
		} else {
			if (MultiplayerSettings.multiplayersetting != null) {
				Destroy (this.gameObject);
			}
		}
	}

	void Start(){
		SetGameMode ();
		SetLevel ();
	}
	void SetGameMode(){
		//		Debug.Log ("Object"+gameObject.name);
		if (PlayerPrefs.HasKey ("MyGameMode")) {
			SelectedGameMode = PlayerPrefs.GetInt ("MyGameMode");
		} else {
			SelectedGameMode = 0;
			PlayerPrefs.SetInt("MyGameMode",SelectedGameMode);
		}
	}
	void SetLevel(){
//				Debug.Log ("Object"+gameObject.name);
		if (PlayerPrefs.HasKey ("MyLevel")) {
			Debug.Log ("Has Level Key");
			SelectedLevel = PlayerPrefs.GetInt ("MyLevel");
		} else {
			Debug.Log ("Doesn't Have Level Key");
			SelectedLevel =3;
			PlayerPrefs.SetInt("MyLevel",SelectedLevel);
		}
	}
}
