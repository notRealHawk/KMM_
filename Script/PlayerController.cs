using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class PlayerController : MonoBehaviour
{
    public bool myTurn = false;
    //public bool pressedSubmit = false;
    public InputField userInput;
    public Button submitButton;
    public string enteredText;
    public GameManager _gameManager;
    public GameObject PlayerHand;
    public List<GameObject> playerCards = new List<GameObject>();

    private void Start()
    {
        submitButton.GetComponent<Button>().onClick.AddListener(() => SubmitText());
    }
    public void SubmitText()
    {
        if (!Regex.IsMatch(userInput.text, @"[a-bA-Z]", RegexOptions.IgnoreCase))
        {
            Debug.Log("Enter alphabets only");
            return;
        }
        else if(_gameManager.GetComponent<GameManager>().Alphabets.Contains(userInput.text)){
	        enteredText = userInput.text;
	        //pressedSubmit = true;
	        _gameManager.CardInstantiate(gameObject);
	        userInput.gameObject.SetActive(false);
	        submitButton.gameObject.SetActive(false);
	        _gameManager.AlertMessageText.text = "";
        }
        else{
        	_gameManager.AlertMessageText.text = "Already Taken";
        	userInput.text = "";
        	return;
        }
    }
}
