using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InLobbyMessage : MonoBehaviour {

	public Text PlayerNameText;
	public Text MessageText;
	public string PlayerName;
	public string Message;
	public void SetMessage(){
		PlayerNameText.text =PlayerName+":";
		MessageText.text =Message;
	}
}
