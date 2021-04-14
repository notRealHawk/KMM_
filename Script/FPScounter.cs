using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPScounter : MonoBehaviour
{
    public Text fpsDisplay;
    int fps;
    void Update()
    {
        fps = (int)(1/Time.unscaledDeltaTime);
        fpsDisplay.text = fps.ToString();
    }
}
