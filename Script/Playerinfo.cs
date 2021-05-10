using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playerinfo : MonoBehaviour {
	public int mySelectedChar;
	public GameObject[] Characters;
	public GameObject[] Bots;
	public static Playerinfo PI;

	private void OnEnable(){
//			Debug.Log ("Enabled");
		if (Playerinfo.PI == null) {

			Playerinfo.PI = this;
		}else {
			if (Playerinfo.PI != null) {
				Destroy (Playerinfo.PI.gameObject);
				Playerinfo.PI = this;
			}
		}
	}
	void Start(){
//		Debug.Log ("Object"+gameObject.name);
		if (PlayerPrefs.HasKey ("MyCharacter")) {
			mySelectedChar = PlayerPrefs.GetInt ("My Character");
		} else {
			mySelectedChar = 0;
			PlayerPrefs.SetInt("MyCharacter",mySelectedChar);
		}
	}
}
