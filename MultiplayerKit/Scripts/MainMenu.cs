using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour {
	public static MainMenu mainMenu;
	//public SoundMangerScript SoundManger;
	public Server _server;
	public GameObject[] Characters;
	public GameObject[] LevelImages;
	public GameObject[] CurrentLevelImage;
	public string[] LevelNames;
	public Dropdown Dd;
	public Text MaxPlayersText;
	public GameObject Camera;
	public GameObject InternetDialouge;
	public Text PlayerNameText;
	public Text PlayerCashText;
	public Text PlayerGoldText;
	public GameObject UnlockButton;
	public GameObject ChooseButton;
	public GameObject LockedDialog;
	public GameObject LoadingScreen;
	public GameObject LockImage;
	public GameObject RewardedVideoBtn;
	public Text LockedText;
	[HideInInspector]
	//public	CameraViewGamepLay cam;
	private int UnlockedCharacters=5;

	void Awake(){
		mainMenu = this;
	}
	void Start(){

		InitialValues();
		//cam = Camera.GetComponent<CameraViewGamepLay> ();
	}
	void InitialValues(){
		Debug.Log ("Selected level =" + MultiplayerSettings.multiplayersetting.SelectedLevel);
//		Debug.Log ("Name of PLayer is="+PlayerPrefs.GetString("Name"));
//		PlayerNameText.text	=PlayerPrefs.GetString ("Name");
		//Debug.Log ("Souls ="+Profile.Souls.ToString());
		//PlayerGoldText.text = Profile.Souls.ToString ();
		PhotonRoom.photonRoom.isGameLoaded = false;
		Characters [PlayerPrefs.GetInt ("My Character")].SetActive (true);
		LevelImages [MultiplayerSettings.multiplayersetting.SelectedLevel-2 ].SetActive (true);
		CurrentLevelImage [MultiplayerSettings.multiplayersetting.SelectedLevel -2].SetActive (true);
		SetCharacterToggle (PlayerPrefs.GetInt("My Character"));
	}
	//public void ChangeCamPosition(int position){
	//	cam.index = position;
	//}
	public void OnClickCharSelection(int WhichCharacter){
		if (WhichCharacter <= UnlockedCharacters) {
			//Playerinfo.PI.mySelectedChar = WhichCharacter;
			PlayerPrefs.SetInt ("My Character", WhichCharacter);
			LockImage.SetActive(false);
			UnlockButton.SetActive (false);
			ChooseButton.SetActive (true);
		} else {
			LockImage.SetActive(true);
			UnlockButton.SetActive (true);
			ChooseButton.SetActive (false);
		}
		SetCharacterToggle (WhichCharacter);
	}
	public void UnlockCharacters(int WhichCharacter){
		if (WhichCharacter == UnlockedCharacters + 1) {
			LockedDialog.SetActive (true);
			LockedText.text="Heal This Character for 2000 Souls?";	
		} else {
			LockedDialog.SetActive (true);
			LockedText.text="Free Previous Character to Unlock This One!";
		}
	}
//	public void BuyCharacter(){
//		if (Profile.Souls >= 2000) {
//			SoundManger.PlayAudioClip ("coincollect");
//			UnlockedCharacters++;
//			LockedDialog.SetActive (false);
//			OnClickCharSelection (UnlockedCharacters);
//			Profile.Souls -= 2000;
//		} else {
////			LockedText.color = Color.red;
//			LockedDialog.GetComponent<Animation> ().Play ();
//			SoundManger.PlayAudioClip ("xpDecreament");
//		}
//	}

	public void OnClickGameModeSelection(int WhichMode){
		MultiplayerSettings.multiplayersetting.SelectedGameMode = WhichMode;
		PlayerPrefs.SetInt ("MyGameMode", WhichMode);
	}
	public void OnClickLevelSelection(){
		MultiplayerSettings.multiplayersetting.SelectedLevel = Dd.value+2;
		PlayerPrefs.SetInt ("MyLevel", MultiplayerSettings.multiplayersetting.SelectedLevel);
	}
	public void SetActive(GameObject ToOn){
		ToOn.SetActive (true);
	}
	public void DeActive(GameObject ToOff){
		ToOff.SetActive (false);
	}
	public void SetCharacterToggle(int Player){
		for (int i=0; i < Characters.Length; i++) {
			if (i == Player) {
				Characters [i].SetActive (true);
			} else {
				Characters [i].SetActive (false);
			}
		}
		
	}
	public void UpdateLevelImages(int Level){
		Debug.Log("Updating The Current Level To= "+Level);
//		int Level = MultiplayerSettings.multiplayersetting.SelectedLevel - 2;
		for (int i=0; i < LevelImages.Length; i++) {
			if (i == Level) {
				LevelImages [i].SetActive (true);
				CurrentLevelImage [i].SetActive (true);
			} else {
				LevelImages [i].SetActive (false);
				CurrentLevelImage [i].SetActive (false);
			}
		}
	}
	public void MaxPlayersSelect(Slider slider){
		if (slider.value == 0) {
			MultiplayerSettings.multiplayersetting.maxPlayers= 4;
		}
		if (slider.value == 1 ) {
			MultiplayerSettings.multiplayersetting.maxPlayers=6;
		}
		if (slider.value == 2 ) {
			MultiplayerSettings.multiplayersetting.maxPlayers=8;
		}
	}
	public void ExitLobby(){
		Destroy (PhotonRoom.photonRoom.gameObject);
		StartCoroutine (LeaveLobby ());
	}
	IEnumerator LeaveLobby(){
		PhotonNetwork.LeaveRoom ();
		while (PhotonNetwork.InRoom) 
			yield return null;
		SceneManager.LoadScene (MultiplayerSettings.multiplayersetting.menuScene);
	}
	public void ExitBtnOnClick(){
	//	MoPubManager.Show_Interstitial (MoPubManager.interstitial_MM);
	}
	public void StayBtnClick(){
		//MoPubManager.Load_Interstitial (MoPubManager.interstitial_MM);
	}
	public void OnRewardVideoClick(){
		//MoPubManager.Show_RewardedVideo ();
	//	RewardedVideoBtn.SetActive (false);
	}
	public void Close(){
		Application.Quit ();
	}
}