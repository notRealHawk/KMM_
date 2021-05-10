using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Card : MonoBehaviour
{
    public int Iteration;
    public bool isBlocked = false;
    public string cardValue;
    public Sprite cardSprite;
    //public Button button;
    public TMP_Text DisplayText;
    public Image DisplaySprite;
    public PlayerController_ Owner;
    public int CardCounter;
    Vector3 cardScale;
    private void Start()
    {
        //button.onClick.AddListener(() => OnClickCard());
    }

    public void OnClickCard()
    {
        if(Input.touchCount > 1){
            return;
        }
        if(isBlocked){
            Debug.Log("This card is blocked");
            //Owner.SelectCard();
            GameSetup.GS.CheckTurn();
            return;
        }
        Debug.Log("Passing Card from Player " + Owner.np._photonPlayer.MyNumber);
        int temp=Owner.np.MyNoinRoom-1;
        int tempIndex = Owner.playerCards.IndexOf(this);
        //Debug.Log("Passing Card from Player " + temp);
            if (temp == GameSetup.GS.currentTurn && temp+1 < GameSetup.GS.Players.Count)
            {
                var nextPlayer = GameSetup.GS.Players[temp + 1].GetComponent<NetworkPlayer>().PC;
                Owner.PassCard(nextPlayer, tempIndex, cardValue);
            }
            else
            {
                var nextPlayer = GameSetup.GS.Players[0].GetComponent<NetworkPlayer>().PC;
                Owner.PassCard(nextPlayer, 0, cardValue);
            }
            GameSetup.GS.TurnDelay();
    }

    /*void OnMouseOver()
    {
        cardScale.x = 4F;
        cardScale.y = 0.04F;
        cardScale.z = 6.65F;
        transform.localScale = cardScale;
    }
    void OnMouseExit()
    {
        cardScale.x = 3F;
        cardScale.y = 0.03F;
        cardScale.z = 5F;
        transform.localScale = cardScale;
    }*/
}
