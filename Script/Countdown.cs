using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Countdown : MonoBehaviour {
//	public SoundManagerScript soundManager_Script;
	public static Countdown Clock;
	public float timeLeft = 300.0f;
	public static bool stop = true;
	public static bool StartTime=false;
	private float minutes;
	private float seconds;
	public Text time;

	private bool timerWarning=false;

	private void OnEnable(){
//		Debug.Log ("Enabled");
		if (Countdown.Clock == null) {

			Countdown.Clock = this;
		}else {
			if (Countdown.Clock != null) {
				Destroy (Countdown.Clock.gameObject);
				Countdown.Clock = this;
			}
		}
	}



	public void startTimer(float from){
		StartTime = true;
		stop = false;
		timeLeft = from;
		Update();
		StartCoroutine(updateCoroutine());
	}

	void Update() 
	{
		
		if(StartTime)
		{
			if (stop)
				return;
			timeLeft -= Time.deltaTime;
			minutes = Mathf.Floor (timeLeft / 60);
			seconds = timeLeft % 60;
			if (seconds > 59)
				seconds = 59;
			if (minutes < 0) 
			{
				stop = true;
				minutes = 0;
				seconds = 0;
				PhotonRoom.photonRoom.isGameLoaded = false;
				GameSetup.GS.IsGameStarted = false;
				GameSetup.GS.GameOver ();

			}
			if (minutes == 0 && seconds < 11 && !timerWarning) {

               
				time.color = new Color32 (255, 111, 0, 255);
				timerWarning = true;
//				StartCoroutine (TimerWarningSound ());

			} else if(seconds>11){
                
				time.color = new Color32(255,255,255,255);
				timerWarning = false;
			}
		}

	}

    public IEnumerator updateCoroutine(){
		while(!stop){
			time.text = string.Format("{0:0}:{1:00}", minutes, seconds);
			yield return new WaitForSeconds(0.2f);
		}
	}

//	private IEnumerator TimerWarningSound()
//	{
//		if (!GameConstants.isGameOver && !GameConstants.isGamePause && timerWarning) {
//			soundManager_Script.PlayAudioClip ("timerWarning");
//			yield return new WaitForSeconds (1);
//			StartCoroutine (TimerWarningSound ());
//		}
//
//	}
}