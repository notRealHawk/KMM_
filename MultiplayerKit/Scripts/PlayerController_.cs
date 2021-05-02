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
    public Sprite userSelection;
    public Button submitButton;
    //public string enteredText;
    //public GameManager _gameManager;
    public GameObject PlayerHand;
    public Card LastBlockedCard;
    public GameObject Locker;
    public List<Card> playerCards = new List<Card>();
    public bool readyToPlay;
    public bool isTurnComplete = false;
    public Camera cam;
    public Animator camAnimator;
    public Quaternion cardRotation;
    private void Start()
    {
        cam = Camera.main;
        camAnimator = cam.GetComponent<Animator>();
        //if (np.PV.IsMine&&!np.isBot)
        //{ 
        //    PlayerHand.SetActive(true);
        //}
        BotValues();
    }
    void Update(){
        if(!np.isBot && !GameSetup.GS.gameOver){
            if(Input.GetMouseButtonDown(0)){
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit, Mathf.Infinity)){
                    if(hit.collider.tag == "Card"){
                        hit.collider.GetComponent<Card>().OnClickCard();
                    }
                }
            }
        }
    }
    void BotValues()
    {
        if (np.isBot)
        {
            //userInput.text = GameSetup.GS.Alphabets[UnityEngine.Random.Range(0, GameSetup.GS.Alphabets.Count)];
            //GameSetup.GS.Alphabets.Remove(userInput.text);
            int stickerIndex = UnityEngine.Random.Range(0, GameSetup.GS.CardSymbols.Count);
            print(stickerIndex);
            userSelection = GameSetup.GS.CardSymbols[stickerIndex];
            GameSetup.GS.CardSymbols.Remove(userSelection);
            SubmitText();
        }
    }
    public void SubmitText()
    {
        /*if (!Regex.IsMatch(userInput.text, @"[a-bA-Z]", RegexOptions.IgnoreCase))
        {
            Debug.Log("Enter alphabets only");
            return;
        }*/
        /*GameSetup.GS.Alphabets.Contains(userInput.text)*/
        if(GameSetup.GS.CardSymbols.Contains(userSelection)){
            PlayerHand.gameObject.SetActive(true); 
	        //enteredText = userInput.text;
	        //pressedSubmit = true;
	        for(int i = 0; i < playerCards.Count; i++)
            {
                //playerCards[i].cardValue = userInput.text;
                playerCards[i].cardSprite = userSelection;
                //playerCards[i].DisplayText.text = userInput.text;
                playerCards[i].DisplaySprite.sprite = userSelection;
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
        if(!GameSetup.GS.gameOver){
            //Debug.Log("")
            if (LastBlockedCard != null)
			{
				//LastBlockedCard.GetComponent<Button>().onClick.AddListener(() => LastBlockedCard.OnClickCard());
				LastBlockedCard.isBlocked = false;
			}
            //Locker.gameObject.SetActive(true);
            playerCards[cardIndex].Owner = newOwner;
            //playerCards[cardIndex].cardValue = cardNewValue;
            //playerCards[cardIndex].DisplayText.text = playerCards[cardIndex].cardValue;
            playerCards[cardIndex].transform.SetParent(newOwner.PlayerHand.transform);
            playerCards[cardIndex].transform.localRotation = cardRotation;
            //playerCards[cardIndex].transform.localScale = Vector3.one;
            newOwner.playerCards.Add(playerCards[cardIndex]);
            GameSetup.GS.BlockCard(playerCards[cardIndex]);
            playerCards.Remove(playerCards[cardIndex]);
            if (!np.isBot){
                gameObject.GetComponent<AnimationControl>().IdleToPass();
                camAnimator.SetBool("Forward", false);
                camAnimator.SetBool("Backward", true);
                np.gameObject.GetComponent<Collider>().enabled = true;
            }

        }
        else
        {
            return;
        }
    }
    public void SelectCard()
    {
        /*List<int> SelectedCard = new List<int>();
        List<int> OtherCards = new List<int>();
        for(int i=0; i < playerCards.Count; i++)
        {
            for(int j=i+1; j < playerCards.Count; j++){
                if (playerCards[i].DisplayText.text == playerCards[j].DisplayText.text)
                {
                    playerCards[i].CardCounter++;
                    playerCards[j].CardCounter++;
                    //if(!SelectedCard.Contains(i)){
                    //  SelectedCard.Add(i);
                    //}
                    continue;
                }
                else if (playerCards[i].DisplayText.text != playerCards[j].DisplayText.text){
                    if(playerCards[i].CardCounter <= playerCards[j].CardCounter && !SelectedCard.Contains(j) && !playerCards[j].isBlocked)
                    {
                        SelectedCard.Add(j);
                    }
                    // else{
                    //     goto RandomSelect;
                    // }
                }
                
            }
        }
        if(SelectedCard != null)
        {
            int randCard = Random.Range(0,SelectedCard.Count);
            playerCards[SelectedCard[randCard]].OnClickCard();
        }
        else{
            int rCard = Random.Range(0,playerCards.Count);
            playerCards[rCard].OnClickCard();
        }
        */
        // return;
        // RandomSelect:
        int randSelectCard = Random.Range(0,playerCards.Count);
        playerCards[randSelectCard].OnClickCard();
    }
}
