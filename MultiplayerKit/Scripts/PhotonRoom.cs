using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;

public class PhotonRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
	public static PhotonRoom photonRoom;
	private PhotonView PV;
	public bool isGameLoaded;
	public int currentScene;
	public Player[] PhotonPLayers;
	public int PlayersinRoom;
	public int myNumberinRoom;
	public int PlayersInGame;
	public int TotalBots;
	public int BotsInGame;
	//Delayed Start
	public bool ReadyToCount;
	private bool ReadyToStart;
	public float StartingTime;
	public float startDelay;
	private float LessThanMaxPLayers;
	private float AtMaxPlayers;
	private float TimeToStart;

	public Text Debugs;
	public Text Waittext;
	public GameObject LobbyGO;
	public GameObject RoomGO;
	public GameObject playerListingPrefab;
	public GameObject StartButton;
	public GameObject WaitingImage;
	public Transform PlayersPanel;
	public GameObject RoomSelection;
	public GameObject Lobby;
	bool BotsSpawned;
	void Awake()
	{
		//set up Singleton
		if (PhotonRoom.photonRoom == null)
		{
			PhotonRoom.photonRoom = this;
		}
		else
		{
			if (PhotonRoom.photonRoom != this)
			{
				Destroy(PhotonRoom.photonRoom.gameObject);
				PhotonRoom.photonRoom = this;
			}
		}
	}
	public override void OnEnable()
	{
		base.OnEnable();
		PhotonNetwork.AddCallbackTarget(this);
		SceneManager.sceneLoaded += OnSceneFinishedLoading;
	}
	public override void OnDisable()
	{
		base.OnEnable();
		PhotonNetwork.RemoveCallbackTarget(this);
		SceneManager.sceneLoaded -= OnSceneFinishedLoading;
	}
	void Start()
	{

		PV = GetComponent<PhotonView>();
		ReadyToCount = false;
		ReadyToStart = false;
		LessThanMaxPLayers = StartingTime;
		AtMaxPlayers = 6;
		TimeToStart = StartingTime;
	}
	void Update()
	{
		if (MultiplayerSettings.multiplayersetting.delayStart)
		{
			if (PlayersinRoom == 1)
			{
				RestartTimer();
			}
			if (!isGameLoaded)
			{
				if (ReadyToStart)
				{
					AtMaxPlayers -= Time.deltaTime;
					LessThanMaxPLayers = AtMaxPlayers;
					TimeToStart = AtMaxPlayers;
				}
				if (ReadyToCount)
				{
					LessThanMaxPLayers -= Time.deltaTime;
					Debugs.text = "Starting Game in" + TimeToStart.ToString();
					TimeToStart = LessThanMaxPLayers;
				}
				if (TimeToStart <= 0)
				{
					StartCoroutine(StartGame(0f));
				}
			}
		}

	}

	public override void OnJoinedRoom()
	{
		Debug.Log("Successfully Joined a Room");
		base.OnJoinedRoom();
		PhotonPLayers = PhotonNetwork.PlayerList;
		PlayersinRoom = PhotonPLayers.Length;
		myNumberinRoom = PlayersinRoom;
		if (MultiplayerSettings.multiplayersetting.delayStart)
		{
			Debug.Log("Display PLayer Room in out of max Players possible(" + PlayersinRoom + ";" + MultiplayerSettings.multiplayersetting.maxPlayers + ")");
			if (PlayersinRoom > 1)
			{
				ReadyToCount = true;
				Debug.Log("Delayed start Ready to COunt =" + ReadyToCount);
			}
			if (PlayersinRoom == MultiplayerSettings.multiplayersetting.maxPlayers + BotsInGame)
			{
				ReadyToStart = true;
				if (!PhotonNetwork.IsMasterClient)
				{
					return;
				}
				PhotonNetwork.CurrentRoom.IsOpen = false;
			}
		}
		else
		{
			if (Server.server.RandomJoin && PhotonNetwork.IsMasterClient)
			{
				StartCoroutine(StartGame(startDelay));
				StartCoroutine(StartDelay(startDelay));
				RoomSelection.SetActive(false);
				Lobby.SetActive(true);
				LobbyGO.SetActive(false);
				RoomGO.SetActive(true);
				Debugs.text = "Waiting for other players";
				WaitingImage.SetActive(true);
				ClearPLayerListings();
				ListPlayers();
			}
			else
			{
				if (!isGameLoaded)
				{
					Debug.Log("Create Room Succesfull Now Waiting For Players");
					RoomSelection.SetActive(false);
					Lobby.SetActive(true);
					LobbyGO.SetActive(false);
					RoomGO.SetActive(true);
					Debugs.gameObject.SetActive(true);
					Debugs.text = "Waiting for Host to Start Party";
					if (PhotonNetwork.IsMasterClient)
					{
						StartButton.SetActive(true);
					}
					ClearPLayerListings();
					ListPlayers();
				}
			}
		}
	}
	void ClearPLayerListings()
	{
		if (!isGameLoaded)
		{
			//			Debug.Log ("PlayersPanel" + PlayersPanel.childCount);
			for (int i = PlayersPanel.childCount - 1; i >= 0; i--)
			{
				Destroy(PlayersPanel.GetChild(i).gameObject);
			}
			
		}
	}
	void ListPlayers()
	{
		if (PhotonNetwork.InRoom && !isGameLoaded)
		{
			foreach (Player player in PhotonNetwork.PlayerList)
			{
				GameObject templisting;

			
					templisting = Instantiate(playerListingPrefab, PlayersPanel);

				Text tempText = templisting.transform.GetChild(0).GetComponent<Text>();
				tempText.text = player.NickName;
			}
		}
	}
	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		base.OnPlayerEnteredRoom(newPlayer);
		Debug.Log("A new player has Joined the Room" + newPlayer.NickName);
		Debug.Log("New Player's User Id=" + newPlayer.UserId);
		if (!isGameLoaded)
		{
			ClearPLayerListings();
			ListPlayers();
			MainMenu.mainMenu.LoadingScreen.SetActive(false);
		}
		PhotonPLayers = PhotonNetwork.PlayerList;
		PlayersinRoom++;
		PV.RPC("SyncBotsNumber", RpcTarget.MasterClient);
		if (MultiplayerSettings.multiplayersetting.delayStart)
		{
			Debug.Log("Display PLayer Room in out of max Players possible(" + PlayersinRoom + ";" + MultiplayerSettings.multiplayersetting.maxPlayers + ")");
			if (PlayersinRoom > 1)
			{
				ReadyToCount = true;
			}
			if (PlayersinRoom == MultiplayerSettings.multiplayersetting.maxPlayers + BotsInGame)
			{
				ReadyToStart = true;
				if (!PhotonNetwork.IsMasterClient)
				{
					return;
				}
				//				PhotonNetwork.CurrentRoom.IsOpen= false;
				//				PhotonNetwork.CurrentRoom.IsVisible = false;
			}
		}
	}
	[PunRPC]
	public void SyncBotsNumber()
	{
		PV.RPC("SyncBotsClients", RpcTarget.AllBuffered, BotsInGame);
	}
	[PunRPC]
	public void SyncBotsClients(int _bots)
	{
		BotsInGame = _bots;
	}
	IEnumerator StartGame(float wait)
	{
		yield return new WaitForSeconds(wait);

		isGameLoaded = true;
		if (PhotonNetwork.IsMasterClient)
		{

			//if (PlayersinRoom < 4)
			//{
			//    SpawnBotsController();
			//    BotsSpawned = true;
			//}


			if (MultiplayerSettings.multiplayersetting.delayStart)
			{
				PhotonNetwork.CurrentRoom.IsOpen = false;
			}

			//		Debug.Log ("Selected Level ="+MultiplayerSettings.multiplayersetting.SelectedLevel);
			PhotonNetwork.LoadLevel(MultiplayerSettings.multiplayersetting.SelectedLevel);
			//PV.RPC ("ShowLoadingScreen", RpcTarget.OthersBuffered);
		}
	}
	IEnumerator StartDelay(float wait)
	{
		Waittext.gameObject.SetActive(true);
		while (wait > 0)
		{
			yield return new WaitForSeconds(1f);
			wait--;
			Waittext.text = wait.ToString();
		}
	}
	void SpawnBotsController()
	{
		Debug.Log("Players in Room before Spawning Bots= " + PlayersInGame);
		int bots = TotalBots - PlayersinRoom;
		//		GameSetup.GS.Bots = new GameObject[TotalBots];
		Debug.Log("TotalBots =" + TotalBots + "Player in Game" + PhotonRoom.photonRoom.PlayersinRoom);
		for (int i = 0; i < bots; i++)
		{
			//			Debug.Log ("TotalBots In Loop =" + bots+"the index is="+i);
			var obj = Resources.Load("PhotonPrefabs/PhotonBotPlayer", typeof(GameObject));
			Debug.Log("Object name= " + obj);
			var prefabPath = "PhotonPrefabs/" + obj.name;
			GameObject BotController = PhotonNetwork.InstantiateRoomObject(prefabPath, transform.position, Quaternion.identity, 0);
			BotsInGame++;
			GameSetup.GS.TotalPlayerinGame++;
			var _BotController = BotController.GetComponent<PhotonPlayer>();
			_BotController.MyNumber = PlayersinRoom + BotsInGame;
		}

	}
	//void SpawnBotsController()
	//{
	//	int bots = TotalBots - PhotonRoom.photonRoom.PlayersinRoom;
	//	//		GameSetup.GS.Bots = new GameObject[TotalBots];
	//	//		Debug.Log ("TotalBots =" + TotalBots+"Player in Game"+PhotonRoom.photonRoom.PlayersinRoom);
	//	for (int i = 0; i < bots; i++)
	//	{
	//		//			Debug.Log ("TotalBots In Loop =" + bots+"the index is="+i);
	//		GameObject BotController = PhotonNetwork.InstantiateSceneObject(Path.Combine("PhotonPrefabs", "PhotonBotPlayer"), transform.position, Quaternion.identity);
	//		BotsInGame++;
	//		GameSetup.GS.TotalPlayerinGame++;
	//		var _BotController = BotController.GetComponent<PhotonPlayer>();
	//		_BotController.MyNumber = PlayersinRoom + BotsInGame;
	//	}
	//}
	public void StartGameBtn()
	{
		StartCoroutine(StartGame(0f));
	}
	[PunRPC]
	public void ShowLoadingScreen()
	{
		//MainMenu.mainMenu.LoadingScreen.SetActive (true);
	}
	void RestartTimer()
	{
		LessThanMaxPLayers = StartingTime;
		TimeToStart = StartingTime;
		AtMaxPlayers = 8;
		ReadyToCount = false;
		ReadyToStart = false;
	}




	void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
	{
		//		Debug.Log("Current scene"+currentScene);
		currentScene = scene.buildIndex;
		//		Debug.Log("asdads"+currentScene);
		if (currentScene > MultiplayerSettings.multiplayersetting.menuScene)
		{
			isGameLoaded = true;
			if (MultiplayerSettings.multiplayersetting.delayStart)
			{
				PV.RPC("RPC_LoadedGameScene", RpcTarget.MasterClient);
			}
			else
			{
				//				Debug.Log ("Creating PLayer");
				RPC_CreatePlayer();
			}
		}
		if (PlayersinRoom < 4 && PhotonNetwork.IsMasterClient)
		{
			SpawnBotsController();
			BotsSpawned = true;
		}
	}
	[PunRPC]
	public void BotSpawned()
	{
		BotsSpawned = true;
	}
	[PunRPC]
	private void RPC_LoadedGameScene()
	{
		PlayersInGame++;
		if (PlayersInGame == PhotonNetwork.PlayerList.Length)
		{
			PV.RPC("RPC_CreatePlayer", RpcTarget.AllBuffered);
			Debug.Log("Players in Game In Rpc=" + PlayersInGame);
		}
	}
	[PunRPC]
	public void RPC_CreatePlayer()
	{
		//PhotonNetwork.Instantiate (Path.Combine ("PhotonPrefabs", "PhotonNetworkPlayer"), transform.position, Quaternion.identity,0,null);
		var obj = Resources.Load("PhotonPrefabs/PhotonNetworkPlayer", typeof(GameObject));
		Debug.Log("Object name= " + obj);
		var prefabPath = "PhotonPrefabs/" + obj.name;
		PlayersInGame++;
		//PlayersinRoom++;
		PhotonNetwork.Instantiate(prefabPath, transform.position, Quaternion.identity, 0, null);
	}


	public override void OnPlayerLeftRoom(Player OtherPlayer)
	{
		base.OnPlayerLeftRoom(OtherPlayer);
		if (PhotonRoom.photonRoom.isGameLoaded)
		{
			//			GameSetup.GS.IsGameStarted = false;
			PV.RPC("SortLists", RpcTarget.All);
		}
		else
		{
			if (PhotonNetwork.IsMasterClient)
			{
				StartButton.SetActive(true);
			}
		}
		PlayersInGame--;
		Debug.Log(OtherPlayer.NickName + "Has Left the Game");
	}
	/*	[PunRPC]
		public void SortLists(){
	//		yield return new WaitForEndOfFrame ();
	//		yield return new WaitForEndOfFrame ();
			for (int i=0; i < GameSetup.GS.GetComponent<Camera>().targets.Count; i++) {
	//			Debug.Log ("Indexxx" + i);
				if (GameSetup.GS.GetComponent<Camera>().targets [i] == null) {
	//				Debug.Log ("Indexxxxxxxx" + i);
					GameSetup.GS.GetComponent<Camera>().targets.Remove (GameSetup.GS.GetComponent<Camera>().targets [i] );
				}
			}
		}*/
}
