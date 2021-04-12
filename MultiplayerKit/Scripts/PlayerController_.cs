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
            //GameSetup.GS.Alphabets.Remove(userInput.text);
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
        if(GameSetup.GS.gameOver != true){
            //Debug.Log("")
            playerCards[cardIndex].Owner = newOwner;
            //playerCards[cardIndex].cardValue = cardNewValue;
            //playerCards[cardIndex].DisplayText.text = playerCards[cardIndex].cardValue;
            playerCards[cardIndex].transform.SetParent(newOwner.PlayerHand.transform);
            playerCards[cardIndex].transform.rotation = Quaternion.identity;
            playerCards[cardIndex].transform.localScale = Vector3.one;
            newOwner.playerCards.Add(playerCards[cardIndex]);
            GameSetup.GS.BlockCard(playerCards[cardIndex]);
            playerCards.Remove(playerCards[cardIndex]);
        }
    }
    public void SelectCard()
    {
        /*List<int> SelectedCard = new List<int>();
        for(int i=0; i < playerCards.Count; i++)
        {
            for(int j=i+1; j < playerCards.Count; j++){
                if (playerCards[i].DisplayText.text == playerCards[j].DisplayText.text)
                {
                    // if (!CompoundCards.Contains(playerCards[i]))
                    // {
                    //     CompoundCards.Add(playerCards[i]);
                    // }
                    playerCards[i].CardCounter++;
                    playerCards[j].CardCounter++;
                }
                else if (playerCards[i].DisplayText.text != playerCards[j].DisplayText.text){
                    if(playerCards[j].CardCounter <= playerCards[i].CardCounter && !SelectedCard.Contains(j))
                    {
                        SelectedCard.Add(j);
                    }
                }
            }
        }
        int randCard = Random.Range(0,SelectedCard.Count);
        print(randCard);
        if(SelectedCard != null)
        {
            playerCards[SelectedCard[randCard]].OnClickCard();
        }
        else{
            int rCard = Random.Range(0,playerCards.Count);
            playerCards[randCard].OnClickCard();
        }*/
        int randCard = Random.Range(0,playerCards.Count);
        playerCards[randCard].OnClickCard();
    }
}
