using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using Facebook.Unity.Example;//
public class SplashScreen : MonoBehaviour {
	public float DelayTime;
	void Start () {
		StartCoroutine (StartGame ());	
	}
	IEnumerator StartGame(){
		yield return new WaitForSeconds (DelayTime);

		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex + 1);
	}
	public void Skip(){
		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex + 1);
	}
}
