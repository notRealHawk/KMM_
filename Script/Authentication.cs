using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class Authentication : MonoBehaviour {
	public Text mainText;
	public InputField NameBar;
	public GameObject AuthenticationScreen;
	public GameObject LoginScreen;
	public GameObject Timeline;
	// Use this for initialization
	void Start () {
		//PlayerPrefs.DeleteAll();
		if (PlayerPrefs.HasKey("Name")&&PlayerPrefs.GetString("Name")!=""){
			Debug.Log ("Name of PLayer is="+PlayerPrefs.GetString("Name"));
			//MainMenu.mainMenu.Camera.GetComponent<CameraViewGamepLay>().enabled=true;
			MainMenu.mainMenu._server.enabled=true;
			gameObject.SetActive(false);
			//Timeline.GetComponent<PlayableDirector>().enabled = true;
			Timeline.GetComponent<PlayableDirector>().playableGraph.GetRootPlayable(0).SetSpeed(1);
			}
		else{
			AuthenticationScreen.SetActive (false);
			LoginScreen.SetActive (true);
			Debug.Log("Failed to authenticate");
			mainText.text = "Please Enter Your Name?";
			mainText.color = Color.green;
			Timeline.GetComponent<PlayableDirector>().playableGraph.GetRootPlayable(0).SetSpeed(0);
			}
		}
	public void Login(){
		if (NameBar.text.Length > 3) {
			PlayerPrefs.SetString ("Name", NameBar.text.ToString ());
			PlayerPrefs.Save ();
			//MainMenu.mainMenu.Camera.GetComponent<CameraViewGamepLay> ().enabled = true;
			MainMenu.mainMenu._server.enabled = true;
			gameObject.SetActive (false);
			//Timeline.GetComponent<PlayableDirector>().enabled = true;
			Timeline.GetComponent<PlayableDirector>().playableGraph.GetRootPlayable(0).SetSpeed(1);
		} else {
			Debug.Log("InCorrect Username");
			mainText.text = "UserName should atleast Contain 4 Letters!";
			mainText.color = Color.red;
		}
	}
}
