﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private ObjectSerialization mgr;
    private Text descTxt;
	public List<Button> btn;
	public RectTransform prefabBtn;
    
    public void Start(){
    	mgr = FindObjectOfType<ObjectSerialization>();
    	descTxt = transform.Find("Navigation").Find("Text").GetComponent<Text>();
    	transform.Find("Navigation").Find("prev").GetComponent<Button>().onClick.AddListener(() => mgr.PrevChoice());
    	transform.Find("Navigation").Find("next").GetComponent<Button>().onClick.AddListener(() => mgr.NextChoice());
	    InitializeFeatureButtons();
    }
    void Update()
    {
    	descTxt.text = mgr.features[mgr.currFeature].ID + " " + (mgr.features[mgr.currFeature].currIndex+1).ToString();
    }
	void InitializeFeatureButtons()
	{
		btn = new List<Button>();
		float height = prefabBtn.rect.height;
		float width = prefabBtn.rect.width;

		for (int i = 0; i < mgr.features.Count; i++)
		{
			RectTransform temp = Instantiate<RectTransform>(prefabBtn);
			temp.name = i.ToString();
			temp.SetParent(transform.Find("Features").GetComponent<RectTransform>());
			temp.localScale = new Vector3(1,1,1);
			temp.localPosition = new Vector3(0,0,0);
			temp.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left,0,width);
			temp.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top,i * height,height);
			Button b = temp.GetComponent<Button>();
			b.onClick.AddListener(() => mgr.SetCurr(int.Parse(temp.name)));
			btn.Add(b);
			b.GetComponentInChildren<Text>().text = mgr.features[i].ID.ToString();
		}
	}
}