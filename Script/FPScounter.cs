using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPScounter : MonoBehaviour
{
    
    // int fps;
    // void Update()
    // {
    //     fps = (int)(1/Time.unscaledDeltaTime);
    //     fpsDisplay.text = fps.ToString();
    // }
    public float timer, refresh, avgFramerate;
    string display = "{0} FPS";
    public Text fpsDisplay;

    private void Update()
    {
        //Change smoothDeltaTime to deltaTime or fixedDeltaTime to see the difference
        float timelapse = Time.smoothDeltaTime;
        timer = timer <= 0 ? refresh : timer -= timelapse;
 
        if(timer <= 0) avgFramerate = (int) (1f / timelapse);
        fpsDisplay.text = string.Format(display,avgFramerate.ToString());
    }
}
