using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class CardMaking : MonoBehaviour
{
    private PassCard PassObj;
    private bool playerturn;
    public InputField userInputField;
    private string userInput;
    public Button submit;
    public Button shuffle;
    public Transform cards;
    private List<string> cardsData;
    private GameObject[] allPlayerCards;
    // Update is called once per frame
    void Start()
    {
        allPlayerCards = GameObject.FindGameObjectsWithTag("Cards");
        PassObj = GetComponent<PassCard>();
        cardsData = new List<string>();
    }
    public void SubmitLetter()
    {
        userInput = userInputField.text;
        if (!Regex.IsMatch(userInput, @"[a-bA-Z]", RegexOptions.IgnoreCase))
        {
            Debug.Log("Enter alphabets only");
            return;
        }
        //userInput = userInputField.text;
        for (int i = 0; i < allPlayerCards.Length; i++)
        {
            foreach (GameObject card in allPlayerCards)
            {
                Debug.Log(card.name);
                if (card.name == "TempCard")
                {
                    continue;
                }
                card.GetComponentInChildren<Text>().text = userInput;
                //Debug.Log(userInput);
                //Debug.Log(card.GetComponentInChildren<Text>().text);
            }
        }

        userInputField.gameObject.SetActive(false);
        submit.gameObject.SetActive(false);
        shuffle.gameObject.SetActive(true);
    }
    public void ShuffleCards()
    {
        foreach (GameObject card in allPlayerCards)
        {
            if (card.name == "TempCard")
            {
                continue;
            }
            cardsData.Add(card.GetComponentInChildren<Text>().text);
        }

        for (int i = 0; i < allPlayerCards.Length; i++)
        {
            foreach (Transform card in allPlayerCards[i].transform)
            {
                if (card.name == "TempCard")
                {
                    continue;
                }
                int rand = Random.Range(0, cardsData.Count);
                card.GetComponentInChildren<Text>().text = cardsData[rand];
                cardsData.Remove(cardsData[rand]);
            }
        }
        shuffle.gameObject.SetActive(false);
    }
}
