using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class RoomButton : MonoBehaviour {

	public Text NameText;
	public Text SizeText;
	public Text MapText;
	public string RoomName;
	public int RoomSize;
	public int PlayersinRoom;
	public string MapName;
	public void SetRoom(){
		NameText.text = RoomName;
		SizeText.text =PlayersinRoom.ToString()+"/"+RoomSize.ToString();
//		MapText.text = MapName;
	}
	public void OnClickJoinRoom(){

		PhotonNetwork.JoinRoom (RoomName);
	}
}
