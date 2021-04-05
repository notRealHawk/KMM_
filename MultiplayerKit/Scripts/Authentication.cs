using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Authentication : MonoBehaviour {
	public Text mainText;
	public InputField NameBar;
	public GameObject AuthenticationScreen;
	public GameObject LoginScreen;
	// Use this for initialization
	void Start () {
		if (PlayerPrefs.HasKey("Name")&&PlayerPrefs.GetString("Name")!=""){
			Debug.Log ("Name of PLayer is="+PlayerPrefs.GetString("Name"));
			//MainMenu.mainMenu.Camera.GetComponent<CameraViewGamepLay>().enabled=true;
			MainMenu.mainMenu._server.enabled=true;
			gameObject.SetActive(false);
			}else{
			AuthenticationScreen.SetActive (false);
			LoginScreen.SetActive (true);
			Debug.Log("Failed to authenticate");
			mainText.text = "Please Enter Your Name?";
			mainText.color = Color.green;
			}
		}
	public void Login(){
		if (NameBar.text.Length > 3) {
			PlayerPrefs.SetString ("Name", NameBar.text.ToString ());
			PlayerPrefs.Save ();
			//MainMenu.mainMenu.Camera.GetComponent<CameraViewGamepLay> ().enabled = true;
			MainMenu.mainMenu._server.enabled = true;
			gameObject.SetActive (false);
		} else {
			Debug.Log("InCorrect Username");
			mainText.text = "UserName should atleast Contain 4 Letters!";
			mainText.color = Color.red;
		}
	}
}
