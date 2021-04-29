using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManger : MonoBehaviour
{
    public float distance = 3;
    float presentchild;
    float previouschild;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Update(){
        for (int i = 0; i < transform.childCount-1; i++)
        {
            int j = i+1;
            previouschild = transform.GetChild(i).position.y;
            presentchild = transform.GetChild(j).position.y;
            presentchild = previouschild + (previouschild * -distance);
        }
    }
}
