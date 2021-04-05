using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Server : MonoBehaviourPunCallbacks,ILobbyCallbacks {
	public static Server server;
	public GameObject RandomBattlebtn;
	public GameObject Cancelbtn;
	public Text Debugs;
	public GameObject updateScreen;
	[HideInInspector]
	public string RoomName;
	[HideInInspector]
	public int RoomSize;
	public GameObject RoomListingPrefab;
	public Transform RoomsPanel;
	[HideInInspector]
	public bool RandomJoin=false;
	public List <RoomInfo> roomListings;
	void Awake(){
		if (Server.server == null) {
			Server.server = this;
		} else {
			if (Server.server != null) {
				Destroy (Server.server.gameObject);
				Server.server = this;
			}
		}
	}
	void Start () {
		if (PhotonNetwork.IsConnected) {
			RandomBattlebtn.SetActive (true);
//			Debug.Log ("Name of PLayer is="+PlayerPrefs.GetString("Name"));
//			MainMenu.mainMenu.PlayerNameText.text	=PlayerPrefs.GetString ("Name");
			return;
		}
		else{
			if (PhotonNetwork.OfflineMode )
			{
				PhotonNetwork.Disconnect ();
			}
			PhotonNetwork.OfflineMode = false;
			PhotonNetwork.ConnectUsingSettings();
			//MainMenu.mainMenu.cam.index = 1;
			roomListings = new List<RoomInfo> ();
		}

	}
	public override void OnConnectedToMaster(){
		Debug.Log ("Player Connected");
		PhotonNetwork.AutomaticallySyncScene=true;
		if (PlayerPrefs.HasKey ("Name")) {
			PhotonNetwork.NickName = PlayerPrefs.GetString ("Name");

		} else {
			if (!Social.localUser.authenticated) {
				int temp = Random.Range (0, 10000);
				PhotonNetwork.NickName = "Guest" + temp.ToString ();
			} else {
				PhotonNetwork.NickName = Social.localUser.userName;
			}
		}
//		MainMenu.mainMenu.PlayerNameText.text = Social.localUser.userName;
		RandomBattlebtn.SetActive (true);
		MainMenu.mainMenu.PlayerNameText.text	=PlayerPrefs.GetString ("Name");
		Debugs.gameObject.SetActive (false);
	//	MoPubManager.Load_RewardedVideo();
	}
	public void OpenURL(string url){
		Application.OpenURL (url);
	}
	public void OnRandomBtnClick(){
		Debug.Log ("BattleButton Clicked. Now trying to connect RandomRoom");
		RandomJoin = true;
		Cancelbtn.SetActive (true);
		Debug.Log ("Rooms Length =" + PhotonNetwork.CountOfRooms);
		RandomBattlebtn.SetActive (false);
//		if (PhotonNetwork.CountOfRooms == 0) {
			PhotonNetwork.JoinRandomRoom (null, 0);
//		}
	}
	public override void OnJoinRandomFailed(short returnCode,string message){
		Debug.Log ("Player Tried to join random room but Failed coz= "+message);
		RandomCreateRoom();	
	}

	void RandomCreateRoom(){
		Debug.Log ("Creating a room");
		Debugs.gameObject.SetActive (true);
		Debugs.text = "Creating a room";
		int RoomName = MultiplayerSettings.multiplayersetting.SelectedLevel+Random.Range(0,9999);
		RoomOptions roomOps = new RoomOptions (){	
			IsVisible = true,
			IsOpen = true,
			PublishUserId=true,
			MaxPlayers= (byte)MultiplayerSettings.multiplayersetting.maxPlayers};
		Debug.Log ("Creating a room line 77");
		PhotonNetwork.CreateRoom("Room"+RoomName,roomOps);
	}
	public override void OnCreateRoomFailed(short returnCode,string message){
		Debug.Log ("Failed to create so trying again"+message );
		RandomCreateRoom();
	}
	public void OnCancelBtnClick(){
		Debug.Log ("Cancel Button Clicked");
		Cancelbtn.SetActive (false);
		RandomBattlebtn.SetActive (true);
		Debugs.gameObject.SetActive (false);
		PhotonNetwork.LeaveRoom ();
	}

	public override void OnRoomListUpdate(List<RoomInfo> roomlist){
		base.OnRoomListUpdate (roomlist);
		int tempIndex;
		RemoveRoomListing ();
		Debug.Log ("Removed Rooms");
		foreach (RoomInfo room in roomlist) {
			Debug.Log ("Adding new room");
			if(roomListings!=null ){
				tempIndex=roomListings.FindIndex(ByName(room.Name) );
			}
			else{
				tempIndex=-1;
			}
			if(tempIndex!=-1){
				roomListings.RemoveAt(tempIndex);
				Destroy(RoomsPanel.GetChild(tempIndex).gameObject);
			}
			else{
				roomListings.Add(room);
				ListRoom (room);
			}
	}
	}
	static System.Predicate<RoomInfo> ByName(string name){
		return delegate(RoomInfo room) {
			return room.Name == name;
		};
	}
	void RemoveRoomListing(){
		int i = 0;
		while (RoomsPanel.childCount != 0) {
			Destroy (RoomsPanel.GetChild (i).gameObject);
			i++;
		}
	}
	void ListRoom(RoomInfo room){
		if (room.IsOpen && room.IsVisible) {
			GameObject tempListing = Instantiate (RoomListingPrefab, RoomsPanel);
			RoomButton tempButton = tempListing.GetComponent<RoomButton>();
			tempButton.RoomName = room.Name;
			tempButton.RoomSize = room.MaxPlayers;
			tempButton.PlayersinRoom = room.PlayerCount;
//			tempButton.MapName=room
			tempButton.SetRoom ();
		}
	}

	public void CustomCreateRoom(){
//		MultiplayerSettings.multiplayersetting.delayStart = true;
		RandomJoin = false;
		Debug.Log ("Creating a room");
		Debugs.gameObject.SetActive (true);
		Debugs.text = "Creating a room";
		RoomOptions roomOps = new RoomOptions (){	
			IsVisible = true,
			IsOpen = true,
			PublishUserId=true,
			MaxPlayers= (byte)MultiplayerSettings.multiplayersetting.maxPlayers};
		if (RoomName != null) {
			PhotonNetwork.CreateRoom (RoomName, roomOps);
		} else {
			RoomName = MainMenu.mainMenu.LevelNames [MultiplayerSettings.multiplayersetting.SelectedLevel+2];
			PhotonNetwork.CreateRoom (RoomName, roomOps);
		}
	}
	public void OnRoomNameChanged(string NameIn){
		RoomName = NameIn;
	}
	public void OnRoomSizeChanged(string SizeIn){
		RoomSize = int.Parse(SizeIn);
	}
	public void JoinLobbyOnClick(){
		RandomJoin = false;


		if (!PhotonNetwork.InLobby) {
			PhotonNetwork.JoinLobby ();
		}

//		MoPubManager.Show_Interstitial (MoPubManager.interstitial_MM);
	}

}
