using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class PlayerController_ : MonoBehaviour
{
    
    public NetworkPlayer np;
    public bool myTurn = false;
    //public bool pressedSubmit = false;
    public InputField userInput;
    public Button submitButton;
    public string enteredText;
    //public GameManager _gameManager;
    public GameObject PlayerHand;
    public List<Card> playerCards = new List<Card>();
    public bool readyToPlay;

    private void Start()
    {
        //if (np.PV.IsMine&&!np.isBot)
        //{
        //    PlayerHand.SetActive(true);
        //}
        BotValues();
    }
    void BotValues()
    {
        if (np.isBot)
        {
            userInput.text = GameSetup.GS.Alphabets[UnityEngine.Random.Range(0, GameSetup.GS.Alphabets.Count)];
            SubmitText();
        }
    }
    public void SubmitText()
    {
        if (!Regex.IsMatch(userInput.text, @"[a-bA-Z]", RegexOptions.IgnoreCase))
        {
            Debug.Log("Enter alphabets only");
            return;
        }
        else if(GameSetup.GS.Alphabets.Contains(userInput.text)){
	        enteredText = userInput.text;
	        //pressedSubmit = true;
	        for(int i = 0; i < playerCards.Count; i++)
            {
                playerCards[i].cardValue = userInput.text;
                playerCards[i].DisplayText.text = userInput.text;
            }
	        userInput.gameObject.SetActive(false);
	        submitButton.gameObject.SetActive(false);
	        GameSetup.GS.AlertMessageText.text = "";
            np.readyToPlay = true;
            GameSetup.GS.CheckPlayersReady();
        }
        else{
            GameSetup.GS.AlertMessageText.text = "Already Taken";
        	userInput.text = "";
            BotValues();
        	return;
        }
    }
    public void PassCard(PlayerController_ newOwner,int cardIndex,string cardNewValue)
    {

        playerCards[cardIndex].Owner = newOwner;
        playerCards[cardIndex].DisplayText.text = cardNewValue;
        playerCards[cardIndex].transform.SetParent(newOwner.PlayerHand.transform);
        playerCards[cardIndex].transform.rotation = Quaternion.identity;
        playerCards[cardIndex].transform.localScale = Vector3.one;
    }

    public void SelectCard()
    {
        int randCard = Random.Range(0,playerCards.Count);
        playerCards[randCard].OnClickCard();
    }
}
