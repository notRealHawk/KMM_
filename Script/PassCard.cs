using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassCard : MonoBehaviour
{
    public bool playerTurn;
    //public Transform cards;
    public List<GameObject> playerCards;
    public int playerNumber;
    private GameObject newCard = null;
    private Transform emptyCard = null;
    //public GameManager _gameManager;
    private Vector3 tempPos;

    [SerializeField] private Transform MainCanvas;

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.name != "player1")
        {
            playerTurn = false;
        }
        else
        {
            playerTurn = true;
        }
        foreach (GameObject card in playerCards)
        {
            if (card.name == "TempCard")
            {
                continue;
            }
            card.GetComponent<Button>().onClick.AddListener(() => passCardToNextPlayer(card));
        }
    }
    void passCardToNextPlayer(GameObject activeCard)
    {
        if(playerTurn == true)
        {
            Debug.Log("hello" + gameObject.name);
            int nextPlayerNum = playerNumber + 1;
            if (nextPlayerNum == 5)
            {
                nextPlayerNum = 1;
            }
            
            Transform nextPlayer = GameObject.Find("player" + nextPlayerNum).transform;
            nextPlayer.GetComponent<PassCard>().playerCards.Add(activeCard);
            gameObject.GetComponent<PassCard>().playerCards.Remove(activeCard);
            Transform nextPlayerCards = GameObject.Find("Player" + nextPlayerNum + "Panel").transform.Find("Cards");
            foreach (Transform child in nextPlayerCards)
            {
                Destroy(child);
            }
            tempPos = activeCard.transform.position;
            //activeCard.transform.parent = playerCards.Find("TempCard");
            //activeCard.position = playerCards.Find("TempCard").position;
            
            //_gameManager.NextPlayerTurn(gameObject);
            
            // foreach (Transform localCard in playerCards)
            // {
            //     if (localCard.name == "TempCard" && localCard.childCount != 0)
            //     {
            //         newCard = localCard.GetChild(0).gameObject;
            //         newCard.transform.parent = localCard.parent;
            //         newCard.transform.position = tempPos;
            //     }
            // }
        }
    }
}