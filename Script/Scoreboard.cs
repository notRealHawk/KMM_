using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Scoreboard : MonoBehaviour {

	public Text PlayerNameText;
	public Text ScoreText;
	public string PlayerName;
	public string Score;
	public void SetScore(){
		PlayerNameText.text =PlayerName;
		ScoreText.text =Score;
	}
}
