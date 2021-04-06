using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public int Iteration;
    public bool isBlocked = false;
    public string cardValue;
    public Button button;
    public Text DisplayText;
    public PlayerController_ Owner;
    private void Start()
    {
        button.onClick.AddListener(() => OnClickCard());
    }

    public void OnClickCard()
    {
        Debug.Log("Passing Card from Player " + Owner.np._photonPlayer.MyNumber);
        int temp=Owner.np.MyNoinRoom-1;
        int tempIndex = Owner.playerCards.IndexOf(this);
        //Debug.Log("Passing Card from Player " + temp);
        var nextPlayer = GameSetup.GS.Players[temp+1].GetComponent<NetworkPlayer>().PC;
        if (temp == GameSetup.GS.currentTurn && nextPlayer.np.MyNoinRoom < GameSetup.GS.Players.Count)
        {
            Owner.PassCard(nextPlayer,tempIndex,cardValue);
        }
        else
        {
            Owner.PassCard(GameSetup.GS.Players[0].GetComponent<NetworkPlayer>().PC,0,cardValue);
        }
        GameSetup.GS.StartTurnTimer();
        GameSetup.GS.CheckTurn();
    }
}
