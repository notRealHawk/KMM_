using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControl : MonoBehaviour
{
    public Animator anim;
    float valueChange = 0;
    public float increament;
    bool increase, decrease;
    void Start(){
        anim.SetFloat("AnimationBlend", 0);
    }

    void Update()
    {
        if(valueChange <= 0){
            increase = true;
            decrease = false;
        }
        if(valueChange >= 1){
            decrease = true;
            increase = false;
        }
        if(increase)
        {
            valueChange += increament;
            anim.SetFloat("AnimationBlend", valueChange);
        }
        if(decrease)
        {
            valueChange -= increament;
            anim.SetFloat("AnimationBlend", valueChange);
        }
    }
}
