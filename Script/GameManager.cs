using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject card;
    public List<string> Alphabets = new List<string>();
    public GameObject[] players;
    public Button ShuffleButton;
    public Button BackButton;
    public Text WinnerTextDisplay;
    public Text AlertMessageText;
    private int playerReady;
    public int playerNumber;
    public List<GameObject> Cards = new List<GameObject>();
    public List<string> cardsData = new List<string>();
    private float delay;
    private GameObject BlockedCard;
    void Awake()
    {
        for (int i = 0; i < 26; i++)
        {
            Alphabets.Add((char)('A' + i) + "");
            //Alphabets.Add((char)('a' + i) + "");
        }
        //players = GameObject.FindGameObjectsWithTag("Player");
        playerNumber = 0;
        players[playerNumber].GetComponent<PlayerController>().myTurn = true;
    }
    public void CardInstantiate(GameObject currentPlayer)
    {
        for (int j = 1; j <= 4; j++)
        {
            GameObject myCard = Instantiate(card);
            currentPlayer.GetComponent<PlayerController>().playerCards.Add(myCard);
            myCard.AddComponent<CardData>();
            myCard.GetComponent<CardData>().cardValue = currentPlayer.GetComponent<PlayerController>().enteredText;
            myCard.GetComponentInChildren<Text>().text = myCard.GetComponent<CardData>().cardValue;
            myCard.transform.SetParent(currentPlayer.GetComponent<PlayerController>().PlayerHand.transform);
            myCard.GetComponent<Button>().onClick.AddListener(() => Passcard(myCard));
            Cards.Add(myCard);
        }
        playerReady++;
        if (playerReady == 4)
        {
            ShuffleButton.gameObject.SetActive(true);
        }
    }

    private void BotUpdate()
    {
    	List<Transform> LowestAvailable = new List<Transform>();
    	int counter;
        if (playerNumber == 0)
        {
            return;
        }
        if(players[playerNumber].GetComponent<AIManager>().BotTurn)
        {
        	Transform CardsParent = players[playerNumber].GetComponent<AIManager>().BotHand.transform;
        	/*for(int i=0; i<CardsParent.childCount; i++){
        		counter = 0;
        		//Finder = CardsParent.GetChild(i);
            	for(int j=i+1; j<CardsParent.childCount; j++){
        			if(CardsParent.GetChild(i).GetComponentInChildren<Text>().text == CardsParent.GetChild(j).GetComponentInChildren<Text>().text)
        			{
        				counter++;
        			}
        			if(CardsParent.GetChild(i).GetComponentInChildren<Text>().text != CardsParent.GetChild(j).GetComponentInChildren<Text>().text){
        				if(LowestAvailable.Contains(CardsParent.GetChild(i))){
        				   	continue;
        				}
        				else{
        					LowestAvailable.Add(CardsParent.GetChild(i));
        				}
        			}
        			else if(CardsParent.GetChild(i).GetComponentInChildren<Text>().text == CardsParent.GetChild(j).GetComponentInChildren<Text>().text){
        				if(LowestAvailable.Contains(CardsParent.GetChild(i))){
        					LowestAvailable.Remove(CardsParent.GetChild(i));
        				}
        			}
        		}
        		if(counter <= 0){
        			LowestAvailable.Add(CardsParent.GetChild(i));
        		}
            }
        	if(LowestAvailable != null){
        		int card = Random.Range(0, LowestAvailable.Count);
            	//GameObject BotSelectedCard = players[playerNumber].GetComponent<AIManager>().BotHand.transform.GetChild(card).gameObject;
            	GameObject BotSelectedCard = LowestAvailable[card].gameObject;
            	Passcard(BotSelectedCard);
        	}*/
            	int card = Random.Range(0, CardsParent.childCount);
            	GameObject BotSelectedCard = players[playerNumber].GetComponent<AIManager>().BotHand.transform.GetChild(card).gameObject;
            	//GameObject BotSelectedCard = LowestAvailable[card].gameObject;
            	Passcard(BotSelectedCard);
        }
    }

    public void BotCardInstantiate(GameObject BotPlayer)
    {
        for (int j = 1; j <= 4; j++)
        {
            GameObject myCard = Instantiate(card);
            BotPlayer.GetComponent<AIManager>().BotCards.Add(myCard);
            myCard.AddComponent<CardData>();
            myCard.GetComponent<CardData>().cardValue = BotPlayer.GetComponent<AIManager>().SelectedCharacter;
            myCard.GetComponentInChildren<Text>().text = myCard.GetComponent<CardData>().cardValue;
            Alphabets.Remove(BotPlayer.GetComponent<AIManager>().SelectedCharacter);
            myCard.transform.SetParent(BotPlayer.GetComponent<AIManager>().BotHand.transform);
            myCard.GetComponent<Button>().onClick.AddListener(() => Passcard(myCard));
            Cards.Add(myCard);
        }
        playerReady++;
    }
    public void ShuffleCards()
    {
        foreach (GameObject card in Cards)
        {
            cardsData.Add(card.GetComponentInChildren<Text>().text);
        }
        foreach (GameObject card in Cards)
        {
            int rand = Random.Range(0, cardsData.Count);
            card.GetComponentInChildren<Text>().text = cardsData[rand];
            cardsData.Remove(cardsData[rand]);
        }
        ShuffleButton.gameObject.SetActive(false);
        players[playerNumber].transform.GetChild(0).Find("Locker").gameObject.SetActive(false);
    }
    
    void Passcard(GameObject mycard)
    {
    	//print("Player "+playerNumber+"'s Turn");
        if (mycard.GetComponent<CardData>().isBlocked == true)
        {
            AlertMessageText.text = "This Card has been blocked";
            if (playerNumber == 0)
            {
                return;
            }
            else
            {
                BotUpdate();
                return;
            }
        }
        TurnCheck(players[playerNumber]);
        if (players[playerNumber] == players[0])
        {
        	mycard.transform.SetParent(players[playerNumber].GetComponent<PlayerController>().PlayerHand.transform);
            players[playerNumber].GetComponent<PlayerController>().myTurn = true;
            players[playerNumber].transform.GetChild(0).Find("Locker").gameObject.SetActive(false);
        }
        else
        {
        	mycard.transform.SetParent(players[playerNumber].GetComponent<AIManager>().BotHand.transform);
            players[playerNumber].GetComponent<AIManager>().BotTurn = true;
            //players[playerNumber].transform.GetChild(0).Find("Locker").gameObject.SetActive(false);
        }
        int num = playerNumber;
	    if(num == 0){
        	num = 4;
        }
        foreach(Transform c in players[num-1].transform.GetChild(0).GetChild(0)){
        	c.GetComponent<Button>().onClick.AddListener(() => Passcard(c.gameObject));
            c.GetComponent<CardData>().isBlocked = false;
        }
        DeckCompleteCheck(players[playerNumber]);
        BlockCard(mycard);
        StartCoroutine(TimeDelay());
    }
    // public void BotPassCard(GameObject botcard)
    // {
    //     if (botcard.GetComponent<CardData>().isBlocked == true)
    //     {
    //         Debug.Log("This Card has been blocked");
    //         return;
    //     }
    // }
    public void TurnCheck(GameObject currentPlayer)
    {
        if (players[playerNumber] == players[0])
        {
            if (currentPlayer.GetComponent<PlayerController>().myTurn == true)
            {
                playerNumber = playerNumber + 1;
                if (playerNumber == players.Length)
                {
                    playerNumber = 0;
                }
                currentPlayer.GetComponent<PlayerController>().myTurn = false;
                currentPlayer.transform.GetChild(0).Find("Locker").gameObject.SetActive(true);
            }
        }
        else
        {
            if (currentPlayer.GetComponent<AIManager>().BotTurn)
            {
                playerNumber = playerNumber + 1;
                if (playerNumber == players.Length)
                {
                    playerNumber = 0;
                }
            }
            currentPlayer.GetComponent<AIManager>().BotTurn = false;
            currentPlayer.transform.GetChild(0).Find("Locker").gameObject.SetActive(true);
        }
    }
    public void DeckCompleteCheck(GameObject currentPlayer)
    {
        int counterCheck = 1;
        Transform playerCards;
        if(playerNumber == 0){
        	playerCards = currentPlayer.GetComponent<PlayerController>().PlayerHand.transform;
        }
        else
        {
        	playerCards = currentPlayer.GetComponent<AIManager>().BotHand.transform;
        }
        for(int i = 0; i < playerCards.childCount; i++)
        {
            for (int j = 0; j < playerCards.childCount; j++)
            {
                if (i == j)
                {
                    continue;
                }
                if (playerCards.GetChild(i).GetComponentInChildren<Text>().text ==
                    playerCards.GetChild(j).GetComponentInChildren<Text>().text)
                {
                    counterCheck++;
                }
            }
            if (counterCheck == 4)
            {
                goto Recheck;
            }
            else
            {
                counterCheck = 1;
            }
        }
        Recheck:
        if (counterCheck >= 4)
        {
            WinnerTextDisplay.gameObject.SetActive(true);
            WinnerTextDisplay.text = currentPlayer.name + " has Won";
            currentPlayer.transform.GetChild(0).Find("Locker").gameObject.SetActive(true);
            Finish();
        }
    }
    public void BlockCard(GameObject passedCard)
    {
    	/*int num = playerNumber;
	    if(num-1 < 0){
        	num = 4;
        }
        Transform PC = players[num-1].transform.GetChild(0).GetChild(0);
        foreach (Transform c in PC)
        {
            if (c.GetComponent<CardData>().isBlocked == true)
            {
                c.GetComponent<Button>().onClick.AddListener(() => Passcard(c.gameObject));
                c.GetComponent<CardData>().isBlocked = false;
            }
        }*/
    	
        passedCard.GetComponent<CardData>().Iteration++;
        if (passedCard.GetComponent<CardData>().Iteration == 4)
        {
            /*foreach (Transform c in passedCard.transform.parent)
            {
                if (c == passedCard)
                {
                    continue;
                }
                if (c.GetComponent<CardData>().isBlocked == true)
                {
                    c.GetComponent<Button>().onClick.AddListener(() => Passcard(c.gameObject));
                    c.GetComponent<CardData>().isBlocked = false;
                }
            }*/
            passedCard.GetComponent<Button>().onClick.RemoveListener(() => Passcard(passedCard));
            passedCard.GetComponent<CardData>().isBlocked = true;
            //BlockedCard = passedCard;
        }
    }
    IEnumerator TimeDelay()
    {
        delay = Random.Range(1.5f,3f);
        yield return new WaitForSeconds(delay);
        AlertMessageText.text = "";
        BotUpdate();
    }
    void Finish()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[playerNumber] == players[0])
            {
                players[playerNumber].GetComponent<PlayerController>().myTurn = false;
            }
            else
            {
                players[playerNumber].GetComponent<AIManager>().BotTurn = false;
            }
        }
        BackButton.gameObject.SetActive(true);
    }
    public void BackToMenu(){
    	SceneManager.LoadScene("MainMenu");
    }
}