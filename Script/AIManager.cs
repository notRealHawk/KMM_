using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AIManager : MonoBehaviour
{
    private GameManager _gm;
    public string SelectedCharacter;
    public bool BotTurn;
    public GameObject BotHand;
    public List<GameObject> BotCards = new List<GameObject>();
    void Start()
    {
        _gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        AssignCards();
    }
    void AssignCards()
    {
        int character = Random.Range(0,_gm.Alphabets.Count);
        SelectedCharacter = _gm.Alphabets[character];
        _gm.BotCardInstantiate(gameObject);
    }
    
}
