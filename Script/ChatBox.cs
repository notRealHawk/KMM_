using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class ChatBox : MonoBehaviour {

	public GameObject MessageGO;
	public InputField inputField;
	public Transform MessageBox;
	PhotonView PV;
	void Start () {
		PV = GetComponent<PhotonView> ();
	}
	public void Message(){
		GameObject tempListing = Instantiate(MessageGO, MessageBox);
		InLobbyMessage tempMessage= tempListing.GetComponent<InLobbyMessage> ();
		tempMessage.PlayerName = PhotonNetwork.NickName;
		tempMessage.Message = inputField.text;
		tempMessage.SetMessage ();
		PV.RPC ("RPC_SendMessage", RpcTarget.Others, PhotonNetwork.NickName, inputField.text);
		inputField.text = null;
	}
	[PunRPC]
	public void RPC_SendMessage(string playerName,string message){
		GameObject tempListing = Instantiate(MessageGO, MessageBox);
		InLobbyMessage tempMessage= tempListing.GetComponent<InLobbyMessage> ();
		tempMessage.PlayerName = playerName;
		tempMessage.Message = message;
		tempMessage.SetMessage ();
	}
}
