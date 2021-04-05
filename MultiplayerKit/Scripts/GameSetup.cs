using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;


public class GameSetup : MonoBehaviour {
	public static GameSetup GS;
	public ExitGames.Client.Photon.Hashtable  PlayerCustomProperties =new ExitGames.Client.Photon.Hashtable();
	public List<GameObject> Players;
	public MultiTargetCam camera;
	public PhotonView PV;
	public int NextPlayerTeam;
	public GameObject[] Bots;
	public Transform[] SpawnpointsTeam1;
	public Transform[] SpawnpointsTeam2;
	public Transform[] BotsSpawnPoints;
	public Transform[] ResultStagePoints;
	public Color32[] Colors;
	public string[] BotNames;
	#region CardMangerReigon

    public List<string> Alphabets = new List<string>();
    [SerializeField]
	List<Card> allCards = new List<Card>();
	public int currentTurn;
	public Text AlertMessageText;
	[SerializeField]
	int totalCardsPerPerson;

	#endregion

	#region UI
	public GameObject[] GameModeUI;
	public Transform Scorelist;
	public string[] playerNames;
	public Text[] playerNameTexts;
	public int[] playerScores;
	public Text[] playerScoreText;
	public Transform[] scoreOrder;
	public Sprite[] CharacterImages;
	public Image[] PlayerImages;
	public int scoreTotal;
	public Text Ping;
	public Text CountdownText;
	public GameObject GameOverDialogFOF;
	public Text[] WinnersNames;
	public Text[] WinnersSouls;
	public int[] SoulReward;
	public Button NextBtn;
	public Button RestartBtn;
	public Text NextTime;
	public GameObject DeathStatPrefab;
	public Transform DeathBoard;
	public Transform UnagiSpawnPoint;
	#endregion
	public int MaxScenes=7;
	public int TotalWinners=3;
	public int StartingTime=5;
	public float GameTime=600f;
	public bool IsGameStarted= false;
	float TimeLeft;
	int NextLevelTime=8;
	int currentUnagi=0;
	public int TotalPlayerinGame;
	public int GolaTime;
	int readyPlayersCount;
	[HideInInspector]
	public bool UnagiSpawned=false;

	private void OnEnable(){
//		Debug.Log ("Enabled");
		if (GameSetup.GS == null) {

			GameSetup.GS = this;
		}else {
			if (GameSetup.GS != null) {
				Destroy (GameSetup.GS.gameObject);
				GameSetup.GS = this;
			}
		}
	}
	void Start(){
		PhotonRoom.photonRoom.isGameLoaded = true;
		PV = GetComponent<PhotonView> ();
		Debug.Log ("Starting TIme = " + StartingTime);
		GameModeUI [PlayerPrefs.GetInt ("MyGameMode")].SetActive (true);
		GolaTime = Random.Range (30, 45);
		StartCoroutine (StartDelay ());
		for (int i = 0; i < 26; i++)
		{
			Alphabets.Add((char)('A' + i) + "");
			//Alphabets.Add((char)('a' + i) + "");
		}
	}
	public void StartTime(int startingTime,float TotalTime){

		if (startingTime <= 0) {
			IsGameStarted = true;
			CountdownText.gameObject.SetActive (false);
			Countdown.Clock.startTimer (TotalTime);
		}
	}
	public void SpawnUnagi(){
		Debug.Log ("SpawningUnagi");
		if (PhotonNetwork.IsMasterClient&&IsGameStarted) {
			PhotonNetwork.InstantiateSceneObject (Path.Combine ("Bombs", "Unagi"), UnagiSpawnPoint.position, UnagiSpawnPoint.rotation,0);
			Invoke ("SpawnUnagi", Random.Range(42,58));
		}
	}
	public void SpawnGolaPickUp(){
		Debug.Log ("SpawningGola");
		int randomspawn = Random.Range (0,4);
		if (PhotonNetwork.IsMasterClient&&IsGameStarted) {
			int temp = Random.Range (0, 2);
			if (temp == 0) {
				PhotonNetwork.InstantiateSceneObject (Path.Combine ("Bombs", "Gola"), SpawnpointsTeam1[randomspawn].position, Quaternion.identity, 0);
			} else {
				PhotonNetwork.InstantiateSceneObject (Path.Combine ("Bombs", "Gola"), SpawnpointsTeam2[randomspawn].position,Quaternion.identity ,0);
			}
			GolaTime = Random.Range (20, 35);
			Invoke ("SpawnGolaPickUp", GolaTime);
		}
	}
	IEnumerator StartDelay(){
		CountdownText.gameObject.SetActive (true);
		while (StartingTime > 0) {
			yield return new WaitForSeconds (1f);
			StartingTime--; 
			CountdownText.text = StartingTime.ToString();
			if (StartingTime<=0) {
				if (PhotonNetwork.IsMasterClient) {
					StartTime (StartingTime, GameTime);
				} else {
					PV.RPC ("SyncTimeWithMaster", RpcTarget.MasterClient);
				}
			}
		}
	}

	[PunRPC]
	public void SyncTimeWithMaster(){
		float timeleft = Countdown.Clock.timeLeft; 
			PV.RPC ("SyncClientsTime", RpcTarget.AllBuffered, timeleft);

	}
	[PunRPC]
	public void SyncClientsTime(float timeleft){
//		TimeLeft = timeleft;
		StartTime (StartingTime,timeleft);
	}
	public void OnLeaveRoom_Btn(){
		Destroy (PhotonRoom.photonRoom.gameObject);
		StartCoroutine (DisConnectandLoad ());
	}
	public void PauseBtnClick(){
//		MoPubManager.Show_Interstitial (MoPubManager.interstitial_MM);

	}
	public void ResumeBtnClick(){
//		MoPubManager.Load_Interstitial (MoPubManager.interstitial_MM);
	}
	public void GameOver(){

		if (Countdown.Clock.timeLeft < 0 && !IsGameStarted) {
				for (int i = 0; i < TotalWinners; i++) {
					Debug.Log (">=");
				WinnersNames [i].text = Scorelist.GetChild (i).GetChild(0).GetComponent<Text> ().text;
					WinnersSouls [i].text = SoulReward [i].ToString ();

				if (WinnersNames [i].text != "Player???") {
					WinnersNames [i].gameObject.SetActive (true);
					WinnersSouls [i].gameObject.SetActive (true);
					if (PhotonNetwork.IsMasterClient) {
						PV.RPC ("SendReward", RpcTarget.AllBuffered, i);
					}
				}
			}
			GameOverDialogFOF.SetActive (true);
		}
		PhotonRoom.photonRoom.BotsInGame = 0;
//		ResultStage ();
		NextLevel ();
	//	MoPubManager.Show_Interstitial (MoPubManager.interstitial_MM);
	}
	void NextLevel(){
		if (PV.IsMine && PhotonNetwork.IsMasterClient) {
//			NextBtn.gameObject.SetActive (true);
//			RestartBtn.gameObject.SetActive (true);
		}
		StartCoroutine (NextLevelDelay ());
	}
	public void CheckPlayersReady() 
    {
		readyPlayersCount = 0;
		for (int i = 0; i < Players.Count; i++)
        {
			var np=Players[i].GetComponent<NetworkPlayer>();
			if (np.readyToPlay)
				readyPlayersCount++;
        }
		//Debug.Log("readyPlayersCount= " + readyPlayersCount + " Total Players="+Players.Count);
		if (readyPlayersCount == Players.Count)
			ShuffleCards();
    }
	void ShuffleCards()
    {
		
		
		for (int i = 0; i < Players.Count; i++)
        {
			var pc = Players[i].GetComponent<NetworkPlayer>().PC;
			for (int j = 0; j < pc.playerCards.Count; j++)
            {
				allCards.Add(pc.playerCards[j]);
            }
			pc.playerCards.Clear();
        }
		Debug.Log("All Cards Recieved");
		for (int i = 0; i < Players.Count; i++)
		{
			var pc = Players[i].GetComponent<NetworkPlayer>().PC;
			for (int j = 0; j < totalCardsPerPerson; j++)
			{
				int rand = Random.Range(0, allCards.Count);
				pc.playerCards.Add(allCards[rand]);
				//pc.playerCards[pc.playerCards.Count - 1].Owner=pc;
				pc.PassCard(pc, pc.playerCards.Count - 1, allCards[rand].cardValue);
				allCards.Remove(allCards[rand]);
				//allCards.Sort();
				Debug.Log("Shuffled Cards Assigned");
			}
		}
		CheckTurn();
	}
	void CheckTurn()
    {
		for (int j = 0; j < Players.Count; j++)
		{
			var player = Players[j].GetComponent<NetworkPlayer>().PC;
			//Debug.Log("Checking Player"+j);
			for (int i = 0; i < player.playerCards.Count; i++)
			{
				//Debug.Log("Checking Player " + j+"'s Card "+i);
				if (j == currentTurn)
				{
					player.playerCards[i].GetComponent<Button>().enabled = true;
					
				}
				else
				{
					player.playerCards[i].GetComponent<Button>().enabled = false;
				}
			}
		}

	}
	void StartTurnTimer()
    {

    }
	void NextTurn()
    {

    }
	IEnumerator	NextLevelDelay (){
		while (NextLevelTime != 0) {
			yield return new WaitForSeconds (1f);
			NextTime.text ="("+ NextLevelTime.ToString()+")";
			NextLevelTime--;
		}
		if (NextLevelTime <= 0&&PhotonNetwork.IsMasterClient) {
			NextBtnClick ();
		}
	}
	[PunRPC]
	public void SendReward(int position){
			Debug.Log ("Position=" + position);
			var WinnerPlayerID = Scorelist.GetChild (position).GetChild (0).GetComponent<SlotOwner> ().PlyrviewID;
			GameObject Winner = PhotonView.Find (WinnerPlayerID).gameObject;
			var NP = Winner.GetComponent<NetworkPlayer> ();
			NP.RecieveAward (SoulReward [position]);
	}
	void ResultStage(){
		var WinnerPlayerID = scoreOrder [0].GetChild (0).GetComponent<SlotOwner> ().PlyrviewID;
		GameObject Winner = PhotonView.Find (WinnerPlayerID).gameObject;
		Winner.transform.position = ResultStagePoints [0].position;
		Winner.transform.rotation = ResultStagePoints [0].rotation;
	}
	public void NextBtnClick(){

		if (PhotonNetwork.IsMasterClient) {
			int temp = Random.Range (2, 6);
			PhotonNetwork.LoadLevel (temp);
			PhotonRoom.photonRoom.isGameLoaded = true;

		} else {
			Debug.Log ("Afra Tafri");
		}
	}
	public void Restart(){
		PhotonRoom.photonRoom.isGameLoaded = true;
		if (!PhotonNetwork.IsMasterClient) {
			Debug.Log ("Afra Tafri");
			return;
		} else {
			PhotonNetwork.LoadLevel (MultiplayerSettings.multiplayersetting.SelectedLevel);
		}
	}
	public	void DisplayDeathStats(string _KillerName,string _KilledName,int _WeaponUsed,int _Killercolor){
		Debug.Log ("_KillerName in GameSetup"+_KilledName);
		GameObject temp = Instantiate (DeathStatPrefab, DeathBoard);
//		Debug.Log ("Killer Color=" + _Killercolor);
		temp.GetComponent<DeathStats> ().SetStats (_KillerName,_KilledName,_WeaponUsed,_Killercolor);
	}
	void LateUpdate()
	{
			ScoreUpdate ();

	}

	void ScoreUpdate(){
		int tempTotal = 0;
		for (int i = 0; i < playerScores.Length; i++) {
			tempTotal += playerScores [i];
		}
		if (tempTotal != scoreTotal) {
			OrderUpdate ();
			scoreTotal = tempTotal;
			for (int i = 0; i < playerScores.Length; i++) {
				playerScoreText [i].text = playerScores [i].ToString ();
			}
		}
	}
	void OrderUpdate(){
		Transform[] order = scoreOrder;
		int[] scores = playerScores;
		int[] places = { 0, 0, 0,0,0,0,0,0};
		for (int i = 0; i < scores.Length; i++) {
			for (int j = 0; j < scores.Length; j++) {
				if (scores [i] < scores [j]) {
					places [i]++;
				}
			}
		}
		for (int i = 0; i < order.Length; i++) {
			order [i].SetSiblingIndex (places [i]);
		}
	}
	IEnumerator DisConnectandLoad(){
		PhotonNetwork.LeaveRoom ();
		while (PhotonNetwork.InRoom) 
			yield return null;
		SceneManager.LoadScene (MultiplayerSettings.multiplayersetting.menuScene);
	}
	public void UpdateTeam(){
		
		if (NextPlayerTeam == 1)
		{
			NextPlayerTeam = 2;
		}
		else 
		{
			Debug.Log ("NextPlayerTeam=1");
			NextPlayerTeam = 1;
		}
	}
	public void UpdateNxtPlyrNumber(){
		TotalPlayerinGame++;
	}
	public void SetActive(GameObject ToOn){
		ToOn.SetActive (true);
	}
	public void DeActive(GameObject ToOff){
		ToOff.SetActive (false);
	}
	IEnumerator SetPing(){
		while (PhotonNetwork.IsConnected) {
			PlayerCustomProperties["Ping"] = PhotonNetwork.GetPing ();
			PhotonNetwork.LocalPlayer.SetCustomProperties (GameSetup.GS.PlayerCustomProperties);
			yield return new WaitForSeconds (0.25f);
		}
		yield break;
	}
	IEnumerator DisplayPing(){
		while (PhotonNetwork.IsConnected) {
			int ping = (int)PhotonNetwork.LocalPlayer.CustomProperties ["Ping"];
			Ping.text = ping.ToString ();
			yield return new WaitForSeconds (0.25f);
		}
		yield break;
	}
}
