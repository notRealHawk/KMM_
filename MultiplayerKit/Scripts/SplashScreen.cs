﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using Facebook.Unity.Example;//
public class SplashScreen : MonoBehaviour {
	public float DelayTime;
	public AudioSource _soundManager;
	public AudioClip Sound;
	public string SplashScreenSound;
	void Start () {
		// FB.Init(this.OnInitComplete, this.OnHideUnity);
		StartCoroutine (SplashSound ());
		StartCoroutine (StartGame ());	
	}
	IEnumerator SplashSound(){
		yield return new WaitForSeconds (.5f);
		_soundManager.PlayOneShot(Sound);
	}


	IEnumerator StartGame(){
		yield return new WaitForSeconds (DelayTime);

		SceneManager.LoadScene ("MainMenu");
	}
}
