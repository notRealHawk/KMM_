using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using System.Linq;
public class PhotonPlayer : MonoBehaviour {
 
	public PhotonView PV;
	int SpawnPicker;
	public GameObject player=null;
	public int MyTeam;
	public int MyNumber;
	public int MyKills;
	public bool IsBot;
	public int MyBotCharacter;
	public string BotName;
	void Start () {
		if (!PV.IsMine)
			return;
		PV = GetComponent<PhotonView> ();
		if (!IsBot) {
			SetNumberinRoom ();
			SetColor ();
		} else {
			MyBotCharacter = Random.Range (0, Playerinfo.PI.Bots.Length);
			int temp = Random.Range (0, GameSetup.GS.BotNames.Length);
			BotName = GameSetup.GS.BotNames [temp];
			GameSetup.GS.BotNames = GameSetup.GS.BotNames.Where(val => val != BotName).ToArray();
		}
		PV.RPC ("RPC_GetTeam", RpcTarget.MasterClient);

	}
	void LateUpdate(){
		//Spawning PlayerPrefabs on Network w.r.t to their teams and setting their color
		int SpawnPos = 0;
		if (player == null&&GameSetup.GS.IsGameStarted) 
		{
			Debug.Log("Player is equal to Null");
			if (MyTeam == 1) {
				//SpawnPicker = Random.Range (0, GameSetup.GS.SpawnpointsTeam1.Length);
				if (PV.IsMine && !IsBot) {
//					Debug.Log ("Spawning Player Avatar in Team 1");
					player =	PhotonNetwork.Instantiate (Path.Combine ("Players", Playerinfo.PI.Characters [Playerinfo.PI.mySelectedChar].name), 
						GameSetup.GS.SpawnpointsTeam1 [SpawnPos].position, GameSetup.GS.SpawnpointsTeam1 [SpawnPos].rotation, 0);
					SpawnPos++;
					player.GetComponent<NetworkPlayer> ()._photonPlayer = gameObject.GetComponent<PhotonPlayer> ();
					player.GetComponent<NetworkPlayer> ().Kills = MyKills;
					//Taking Player Prefab Name from the Playerinfo and Spawning it from the Resources folder 
					GameSetup.GS.PlayerCustomProperties ["Team"] = MyTeam;
					PhotonNetwork.LocalPlayer.SetCustomProperties (GameSetup.GS.PlayerCustomProperties);// Local Properties provided by Photon can be used Generally 
				} else {
//					Debug.Log ("Team 1 Bot Spawning");
					SpawnBot ();
				}
			} 
			else {
				SpawnPicker = Random.Range (0, GameSetup.GS.SpawnpointsTeam2.Length);
				Debug.Log("Player is equal to Null");
				if (PV.IsMine && !IsBot) {
					Debug.Log ("Spawning Player Avatar in Team 2");
					player =	PhotonNetwork.Instantiate (Path.Combine ("Players", Playerinfo.PI.Characters [Playerinfo.PI.mySelectedChar].name), 
					GameSetup.GS.SpawnpointsTeam2 [SpawnPos].position, GameSetup.GS.SpawnpointsTeam2 [SpawnPos].rotation, 0);
					SpawnPos++;
					player.GetComponent<NetworkPlayer> ()._photonPlayer = gameObject.GetComponent<PhotonPlayer> ();
					player.GetComponent<NetworkPlayer> ().Kills = MyKills;
					GameSetup.GS.PlayerCustomProperties ["Team"] = MyTeam;
					PhotonNetwork.LocalPlayer.SetCustomProperties (GameSetup.GS.PlayerCustomProperties); 
				} else {
//					Debug.Log ("Team 2 Bot Spawning");
					if (PhotonNetwork.IsMasterClient)
						SpawnBot ();
				}
			}
		}
	}
	void SpawnBot(){
		if (!PV.IsMine)
			return;
        Debug.Log("Bot Number" + MyNumber);
        //		Debug.Log ("Spawning Bot"+GameSetup.GS.BotsSpawnPoints [MyNumber-1].gameObject.name);
        player =	PhotonNetwork.InstantiateRoomObject (Path.Combine ("Bots", Playerinfo.PI.Bots [MyBotCharacter].name), 
			GameSetup.GS.BotsSpawnPoints [MyNumber-2].position, GameSetup.GS.BotsSpawnPoints [MyNumber-2].rotation, 0);
			print(player);
			player.GetComponent<NetworkPlayer> ()._photonPlayer = gameObject.GetComponent<PhotonPlayer> ();
			player.GetComponent<NetworkPlayer> ().Kills = MyKills;
		player.GetComponent<NetworkPlayer>().isBot = true;
		player.GetComponent<NetworkPlayer> ().MyName = BotName;
	}
	void SetColor(){
		int RandomColor = MyNumber-1;
		if (MultiplayerSettings.multiplayersetting.SelectedGameMode==0) {
//				Debug.Log ("Random Color for Mode ="+MultiplayerSettings.multiplayersetting.SelectedGameMode);
			GameSetup.GS.PlayerCustomProperties ["MyColor"] = RandomColor;//In Case of FREE For All
			PhotonNetwork.LocalPlayer.SetCustomProperties (GameSetup.GS.PlayerCustomProperties);
		}
		else if (MultiplayerSettings.multiplayersetting.SelectedGameMode == 1 && MyTeam == 0) {
			Debug.Log ("Team 1 Color");
			GameSetup.GS.PlayerCustomProperties ["MyColor"] = 0; // In Team Death Match case and team 1
			PhotonNetwork.LocalPlayer.SetCustomProperties (GameSetup.GS.PlayerCustomProperties);
		} else {
			Debug.Log ("Team 2 Color");
			GameSetup.GS.PlayerCustomProperties ["MyColor"] = 1; // In Team Death Match case and team 2
			PhotonNetwork.LocalPlayer.SetCustomProperties (GameSetup.GS.PlayerCustomProperties);
		}
	}
	void SetNumberinRoom(){
		//Number according to the players Entry
	
		var PR=PhotonRoom.photonRoom;
		
		if (PhotonNetwork.IsMasterClient) {
			MyNumber = PR.myNumberinRoom;
		} else {
		
			MyNumber = PR.myNumberinRoom+PR.BotsInGame;
		}
	
			GameSetup.GS.PlayerCustomProperties ["MyNumber"] = MyNumber;
			GameSetup.GS.PlayerCustomProperties ["MyKills"] = MyKills;
			PhotonNetwork.LocalPlayer.SetCustomProperties (GameSetup.GS.PlayerCustomProperties);
		PV.RPC ("SetMyNumber", RpcTarget.AllBuffered,PV.ViewID, MyNumber);

	}
	[PunRPC]
	public void SetMyNumber(int _viewID,int mynumber){
		var PP = PhotonView.Find (_viewID).gameObject.GetComponent<PhotonPlayer> ();
		PP.MyNumber = mynumber;
	}
	[PunRPC]
	void RPC_GetTeam(){
		//Getting Team w.r.t to previous player
		MyTeam = GameSetup.GS.NextPlayerTeam;
		PlayerPrefs.SetInt ("MyGameMode", MultiplayerSettings.multiplayersetting.SelectedGameMode);
		int GameMode = PlayerPrefs.GetInt ("MyGameMode");
		GameSetup.GS.UpdateTeam ();
		PV.RPC ("RPC_SetTeam", RpcTarget.OthersBuffered,MyTeam,GameMode);

	}

	[PunRPC]
	void RPC_SetTeam(int WhichTeam,int WhichMode){
		//Changing Player Team for next player that joins
		Debug.Log ("Setting Team");
		PlayerPrefs.SetInt ("MyGameMode", WhichMode);
		MyTeam = WhichTeam;
	}
}
