using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.Text;
using System.IO;

public class NetworkPlayer : MonoBehaviour {		
	
	public PhotonView PV;
	public int myteam;
	public PhotonPlayer _photonPlayer;
	public string MyName;
	public int Kills;
	public GameObject Controls;
	public Image Staminafill;
	public Transform vUI;
	public Text PlayerNameDisplay;
	public int MyColor;
	public int MyNoinRoom;
	public SpriteRenderer Indicator;
	public Image Unagi;
	public GameObject UnagiKey;
	public Image Gola;
	public GameObject GolaKey;
	public Transform GolaSpawnPoint;
	[HideInInspector]
	public GameObject Gauntlet;
	public bool UnagiActivated=false;
	public bool GolaActivated=false;
	public int GolaLeft;
	public bool isBot;
	public bool readyToPlay;
	public PlayerController_ PC;
	int MyCharacter;
	void Start(){
        //PV.RPC("AddPlayertoCam", RpcTarget.AllBuffered, PV.ViewID);

        if (!PV.IsMine) 
			return;

		InitialValues();
	}
	void InitialValues(){
		if (gameObject.tag == "Player") {
//			Instantiate (Controls, vUI);		
			//Staminafill.enabled=true;
			Indicator.enabled = true;
            Controls.SetActive(true);
            MyNoinRoom = (int)PhotonNetwork.LocalPlayer.CustomProperties ["MyNumber"];
			MyColor=(int)PhotonNetwork.LocalPlayer.CustomProperties ["MyColor"];
			Kills=(int)PhotonNetwork.LocalPlayer.CustomProperties ["MyKills"];
			MyName = PhotonNetwork.NickName;
			MyCharacter = PlayerPrefs.GetInt ("My Character");
			if (PhotonNetwork.IsMasterClient)
				SyncStatsOnNetwork (PhotonRoom.photonRoom.myNumberinRoom);
			else {
				SyncStatsOnNetwork (_photonPlayer.MyNumber);
			}
		}
		if (gameObject.tag == "Enemy"&&PhotonNetwork.IsMasterClient) {
			MyNoinRoom = _photonPlayer.MyNumber;
			Kills = _photonPlayer.MyKills;
			MyName = _photonPlayer.BotName;
			MyCharacter = _photonPlayer.MyBotCharacter;
			SyncStatsOnNetwork (_photonPlayer.MyNumber);
		}

		myteam = GameSetup.GS.NextPlayerTeam-1;
		//PhotonRoom.photonRoom.SortLists ();
//		AddPlayerScoreCount ();
	}
	void SyncStatsOnNetwork(int NumberinRoom){
		PV.RPC ("AddPlayerToGS", RpcTarget.MasterClient, PV.ViewID, MyName, NumberinRoom, MyColor, MyCharacter);	
	}
	[PunRPC]
	public void AddPlayerToGS(int ViewId,string myname,int myNoInRoom,int Color,int myCharacter){	
//		Debug.Log ("MyNumber in ROom" + myNoInRoom);
		GameObject player = PhotonView.Find (ViewId).gameObject;
		Transform plyr = player.transform;
		GameSetup.GS.playerNames [myNoInRoom-1] = myname;
		GameSetup.GS.playerNameTexts [myNoInRoom-1].text = myname;
		GameSetup.GS.playerNameTexts [myNoInRoom - 1].color = GameSetup.GS.Colors [myNoInRoom-1]; 
		GameSetup.GS.PlayerImages [myNoInRoom-1].sprite = GameSetup.GS.CharacterImages[myCharacter];
//		GameSetup.GS.scoreOrder [myNoInRoom - 1].gameObject.SetActive (true);
		var slottemp= GameSetup.GS.playerNameTexts [myNoInRoom - 1].gameObject.GetComponent<SlotOwner> ();
		slottemp.PlyrviewID = ViewId;
		var AS = player.GetComponent<NetworkPlayer> ();
		AS.PlayerNameDisplay.text = myname;
//		Debug.Log ("Color =" + Color);
		AS.PlayerNameDisplay.color = GameSetup.GS.Colors[myNoInRoom-1];
		AS.MyName = myname;
//		AS.Indicator.color = GameSetup.GS.Colors[Color]; //to assign indicator player color
		AS.MyNoinRoom = myNoInRoom;


//		Debug.Log ("ADDPlayer to GS" + myname + "number" + myNoInRoom);

		if (!GameSetup.GS.Players.Contains (player)) {
			GameSetup.GS.Players.Add (player);
//			Debug.Log ("Adding PLayer"+MyNoInRoom);

		}
		PV.RPC ("SyncGSWithClients", RpcTarget.OthersBuffered, PV.ViewID,myname,myNoInRoom,Color,myCharacter);
	}
	[PunRPC]
	public void SyncGSWithClients(int ViewId,string NameIn,int MyNumber,int Colour,int character){
		GameObject player = PhotonView.Find (ViewId).gameObject;
		//		Transform plyr = player.transform;
		GameSetup.GS.playerNames [MyNumber-1] = NameIn;
		GameSetup.GS.playerNameTexts [MyNumber-1].text = NameIn;
		GameSetup.GS.playerNameTexts [MyNumber - 1].color = GameSetup.GS.Colors [MyNumber-1];
		GameSetup.GS.PlayerImages [MyNumber-1].sprite = GameSetup.GS.CharacterImages[character];
//		GameSetup.GS.scoreOrder [MyNumber - 1].gameObject.SetActive (true);
				var slottemp= GameSetup.GS.playerNameTexts [MyNumber - 1].gameObject.GetComponent<SlotOwner> ();
				slottemp.PlyrviewID = ViewId;
		var AS = player.GetComponent<NetworkPlayer> ();
		AS.PlayerNameDisplay.text = NameIn;
		AS.PlayerNameDisplay.color = GameSetup.GS.Colors[MyNumber-1];
		AS.MyName = NameIn;
		//		AS.Indicator.color = GameSetup.GS.Colors [Colour]; //To Sync Indicator color over network 
		AS.MyNoinRoom = MyNumber;
		//adding Transforms to GameSetup for camera bounds calculation
		//		if (!GameSetup.GS.camera.targets.Contains (plyr)) {
		//			GameSetup.GS.camera.targets.Add (plyr);
		//		}//adding All spawned players to GS list
		if (!GameSetup.GS.Players.Contains (player)) {
			GameSetup.GS.Players.Add (player);
			Debug.Log ("Sync Adding PLayer"+MyNumber);
		}
	}

	[PunRPC]
	public void SyncScoreBoard(int viewid,string PlyrName,int kills){
		GameObject PlayerStat = PhotonView.Find (viewid).gameObject;
		Scoreboard tempScoreboard = PlayerStat.GetComponent<Scoreboard> ();
		tempScoreboard.PlayerName = PlyrName;
		tempScoreboard.Score = kills.ToString();
		tempScoreboard.SetScore ();
	}


	/*public void HealthSync(){
		if (PV.IsMine && gameObject.tag == "Player") {
			PV.RPC ("SyncHealth", RpcTarget.AllBuffered, PV.ViewID, controller.currentHealth);
		}
		if (PV.IsMine && gameObject.tag == "Enemy") {
			var controller = GetComponent<v_AIController> ();
			PV.RPC ("SyncHealth", RpcTarget.AllBuffered, PV.ViewID, controller.currentHealth);
		}
	}
	[PunRPC]
	public void SyncHealth(int ViewID,float _newhealth)
	{
		//		Debug.Log ("Syncing Health");
		GameObject player = PhotonView.Find (ViewID).gameObject;
		if (player.tag == "Player") {
			var CC = player.GetComponent<vThirdPersonController> ();
			CC.currentHealth = _newhealth;
		} else {
			var CC = player.GetComponent<v_AIController> ();
			CC.currentHealth = _newhealth;
		}
	}*/
	public void OnDeathPlayer(){
		if (PV.IsMine&&gameObject.tag=="Player") {
			Debug.Log ("Before Setting Custom Properties" + (int)PhotonNetwork.LocalPlayer.CustomProperties ["MyKills"]);
			GameSetup.GS.PlayerCustomProperties ["MyKills"] = Kills;
			PhotonNetwork.LocalPlayer.SetCustomProperties (GameSetup.GS.PlayerCustomProperties);
			Debug.Log ("After Setting Custom Properties" + (int)PhotonNetwork.LocalPlayer.CustomProperties ["MyKills"]);
		}
		if (PV.IsMine&&gameObject.tag=="Enemy") {
			_photonPlayer.MyKills = Kills;
			PV.RPC ("SyncKillBot", RpcTarget.OthersBuffered, PV.ViewID);
		}
	}
	[PunRPC]
	public void SyncKillBot(int viewid){
		GameObject bot= PhotonView.Find (viewid).gameObject;
		var np = bot.GetComponent<NetworkPlayer> ()._photonPlayer;
		if(np!=null)
		np.MyKills = Kills;
	}
	public void EnableUnagi(){
		UnagiActivated = true;
		Unagi.color = Color.white;
		UnagiKey.SetActive (true);
		Debug.Log ("Unlocked Unagi");
	}
	public void EnableGola(string WhichGola){
		GolaActivated = true;
		GolaLeft = 3;
		Gola.color=Color.white;
		GolaKey.SetActive (true);
		Gauntlet= PhotonNetwork.Instantiate (Path.Combine ("Props",WhichGola+"G"), 
			GolaSpawnPoint.position,GolaSpawnPoint.rotation, 0);
		Gauntlet.transform.SetParent (GolaSpawnPoint);
		int Gauntletid = Gauntlet.GetComponent<PhotonView> ().ViewID;
		PV.RPC ("GauntletSync", RpcTarget.OthersBuffered,PV.ViewID,Gauntletid);
		Debug.Log ("Unlocked Unagi");
	}

	[PunRPC]
	public void GauntletSync(int id,int gauntletid){
		NetworkPlayer NP = PhotonView.Find (id).GetComponent<NetworkPlayer> ();
		Gauntlet = PhotonView.Find (gauntletid).gameObject;
		Gauntlet.transform.SetParent (NP.GolaSpawnPoint);
		Gauntlet.transform.localPosition = Vector3.zero;
		Gauntlet.transform.localEulerAngles = Vector3.zero; 
	}
	[PunRPC]
	public void AddKill(int KillerNoInRoom,string KillerName,string KilledName,int WeaponUsed,int Killercolor,int KillerViewID){

		if (!PhotonNetwork.IsMasterClient)
			return;
	
		var KillerNP = PhotonView.Find (KillerViewID).gameObject;
		KillerNP.GetComponent<NetworkPlayer> ().Kills++;
		int kills =	KillerNP.GetComponent<NetworkPlayer> ().Kills;
		GameSetup.GS.playerScores [KillerNoInRoom-1]=kills;
		//		GameSetup.GS.playerScores [KillerNoInRoom-1]=KillerNP.GetComponent<NetworkPlayer> ().Kills;
		GameSetup.GS.DisplayDeathStats (KillerName,KilledName,WeaponUsed,Killercolor);
		Debug.Log ("Added Kill"+KillerNoInRoom+"NewKills"+kills);

		PV.RPC ("SyncKill", RpcTarget.OthersBuffered,KillerNoInRoom,kills,KillerName,KilledName,WeaponUsed,Killercolor,KillerViewID);
	}
	[PunRPC]
	public void DeductKill(int killer,string KillerName,string KilledName,int WeaponUsed,int Killercolor, int KillerviewID){
		Debug.Log ("Deducted Kill" + killer);
		var KillerNP = PhotonView.Find (KillerviewID).gameObject;
		var KillerKills= KillerNP.GetComponent<NetworkPlayer> ()._photonPlayer;
		if (KillerKills.MyKills> 0) {
			KillerKills.MyKills--;
			GameSetup.GS.DisplayDeathStats (KillerName, KilledName, WeaponUsed, Killercolor);
			PV.RPC ("SyncKill", RpcTarget.OthersBuffered, killer, KillerKills, KillerName, KilledName, WeaponUsed, Killercolor,KillerviewID);
		}
	}

	[PunRPC]
	public void SyncKill(int Killer,int score,string KillerName_,string KilledName_,int WeaponUsed_,int Killercolor_,int plyrID){
		GameObject plyr= PhotonView.Find (plyrID).gameObject;
		NetworkPlayer np = plyr.GetComponent<NetworkPlayer> ();
		np.Kills = score;
		GameSetup.GS.playerScores [Killer - 1] = score;
		GameSetup.GS.DisplayDeathStats (KillerName_,KilledName_,WeaponUsed_,Killercolor_);
	}
	[PunRPC]
	public void SyncKills(int plyrID,int score,int MyNo){
		GameObject plyr= PhotonView.Find (plyrID).gameObject;
		NetworkPlayer np = plyr.GetComponent<NetworkPlayer> ();
		np.Kills = score;
		Debug.Log ("Sync Kills =" + MyNo);
		GameSetup.GS.playerScores [MyNo] = score;
	}
	public void RecieveAward(int _souls){
		if (!PV.IsMine||gameObject.tag!="Player")
			return;

		Profile.Souls += _souls;
	}
	[PunRPC]
	public void AddPlayertoCam(int viewid){
		GameObject player = PhotonView.Find (viewid).gameObject;
		Transform plyr = player.transform;
		if (!GameSetup.GS.camera.targets.Contains (plyr)) {
			GameSetup.GS.camera.targets.Add (plyr);
		}
	}
	[PunRPC]
	public void RemoveFromCam(int ViewID){
		Transform plyr = PhotonView.Find (ViewID).gameObject.transform;
		GameSetup.GS.camera.targets.Remove (plyr);
	}
	
}
