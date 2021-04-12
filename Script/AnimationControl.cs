using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControl : MonoBehaviour
{
    public Animator anim;
    float valueChange;
    public float increament;
    bool increase, decrease;
    void Awake(){
        valueChange = Random.Range(0f,1f);
        anim.SetFloat("AnimationBlend", valueChange);
        increase = true;
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
