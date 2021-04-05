using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour {

	public GameObject UnlockButton;
	public GameObject ChooseButton;
	public GameObject LockedDialog;
	public GameObject LockImage;
	public Text LockedText;
	private int UnlockedLevel=5;

	public void OnClickLevelSelection(int WhichLevel){
		MainMenu.mainMenu.UpdateLevelImages (WhichLevel-2);
		if (WhichLevel <= UnlockedLevel) {
			LockImage.SetActive(false);
			UnlockButton.SetActive (false);
			ChooseButton.SetActive (true);
			MultiplayerSettings.multiplayersetting.SelectedLevel = WhichLevel;
			PlayerPrefs.SetInt ("MyLevel", WhichLevel);
		} else {
			LockImage.SetActive(true);
			UnlockButton.SetActive (true);
			ChooseButton.SetActive (false);
		}
	}
	public void OnChooseLevelBtn(){
		MainMenu.mainMenu.UpdateLevelImages (PlayerPrefs.GetInt("MyLevel")-2);
	}
	public void BuyLevel(){
		if (Profile.Souls >= 1500) {
			//MainMenu.mainMenu.SoundManger.PlayAudioClip ("coincollect");
			UnlockedLevel++;
			LockedDialog.SetActive (false);
			OnClickLevelSelection (UnlockedLevel);
			Profile.Souls -= 1500;
		} else {
			//			LockedText.color = Color.red;
			LockedDialog.GetComponent<Animation> ().Play ();
			//MainMenu.mainMenu.SoundManger.PlayAudioClip ("xpDecreament");
		}
	}



}
