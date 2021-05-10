using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{   
    public List<Transform> cardsPos;
    void Start(){
        ArrangeCards();
    }
    public void ArrangeCards() {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).localPosition = cardsPos[i].localPosition;
        }
    }
}
