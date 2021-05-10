using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathStats : MonoBehaviour {
	public Text DeathDisplayText;
	public void SetStats(string killer,string killed,int deathby,int KillerColor){
		if (deathby == 0) {
			DeathDisplayText.text = killer + "   " + "'Bombed'" + "   " + killed;
			DeathDisplayText.color = GameSetup.GS.Colors [0];
		} else if (deathby == 1) {
			DeathDisplayText.text = killer + "Killed" + killed;
			DeathDisplayText.color = GameSetup.GS.Colors [0];
		} else {
			DeathDisplayText.text = killer + "killed Himself";
			DeathDisplayText.color = GameSetup.GS.Colors [0];
		}
	}
}
