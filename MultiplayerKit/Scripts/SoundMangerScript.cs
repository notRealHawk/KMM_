using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(AudioSource))]
public class SoundMangerScript : MonoBehaviour {





    private AudioSource audioSource_Component;
    public static bool AudioOn_OFF;


    public AudioSource GamePlayAudioSource_Component;
    public AudioSource gamePlay_AudioSource_Component;

    public AudioSource GameoverAudioSource_Component;
    public AudioClip levelComplete_Clip;
    public AudioClip levelFailed_Clip;

    public AudioClip buttonClick_Clip;
    public AudioClip coinCollect;
    public AudioClip selection_Clip;
    public AudioClip mainmenu_Clip;
    public AudioClip seatBelt_Clip;
    public AudioClip parkingComplete_Clip;
    public AudioClip xpIncreament_Clip;
    public AudioClip xpDecreament_Clip;
    public AudioClip carPurchased_Clip;
    public AudioClip timerWarning_Clip;


    void Start()
    {
        try
        {
            audioSource_Component = GetComponent<AudioSource>();
        }
        catch
        {
            Debug.LogError("Please Attach AudioSource to SoundManager Object");
        }
    }



    public void PlayAudioClip(string currentClipName)
    {
        switch (currentClipName)
        {

            case "buttonClick":
                audioSource_Component.PlayOneShot(buttonClick_Clip);
                break;

            case "selection":
                audioSource_Component.clip = selection_Clip;
                audioSource_Component.Play();
                break;

            case "mainmenu":
                audioSource_Component.clip = mainmenu_Clip;
                audioSource_Component.Play();
                break;


            case "levelComplete":
                audioSource_Component.Stop();
                gamePlay_AudioSource_Component.volume = .4f;
                GameoverAudioSource_Component.clip = levelComplete_Clip;
                GameoverAudioSource_Component.Play();

                break;

            case "levelFailed":
                audioSource_Component.Stop();
                gamePlay_AudioSource_Component.volume = .4f;
                GameoverAudioSource_Component.clip = levelFailed_Clip;
                GameoverAudioSource_Component.Play();

                break;
            case "xpIncreament":
                audioSource_Component.PlayOneShot(xpIncreament_Clip);

                break;
            case "xpDecreament":
                audioSource_Component.PlayOneShot(xpDecreament_Clip);

                break;

            case "coincollect":
                audioSource_Component.PlayOneShot(coinCollect);
                break;

            case "parkingComplete":
                audioSource_Component.PlayOneShot(parkingComplete_Clip);
                break;

            case "successfullyPurchased":
                audioSource_Component.PlayOneShot(carPurchased_Clip);
                break;

            case "timerWarning":
                audioSource_Component.PlayOneShot(timerWarning_Clip);
                break;


            default:
                Debug.Log("There is no audio Clip to play");
                break;
        }

    }


    public static void MuteCompleteAudio()
    {
        AudioOn_OFF = !AudioOn_OFF;


        if (AudioOn_OFF)
        {
            AudioListener.volume = 0;
        }
        else
        {
            AudioListener.volume = 1;
        }
    }
}
