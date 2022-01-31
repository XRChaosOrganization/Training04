using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AudioManager : MonoBehaviour
{
    public static AudioManager am;

    public enum SFX { UI_Confirm, UI_Navigate, Player_Tongue, Player_Death, Gameplay_Fly, Gameplay_Win}

    public List<AudioSource> bgm;
    public List<AudioSource> ui;
    public List<AudioSource> player;
    public List<AudioSource> gameplay;

    private void Awake()
    {
        am = this;

        DontDestroyOnLoad(this.gameObject);
        
    }

    public void PlaySFX(SFX sfx)
    {
        switch (sfx)
        {
            case SFX.Gameplay_Fly:
                gameplay[0].Play();
                break;
            case SFX.Gameplay_Win:
                gameplay[1].Play();
                break;
            case SFX.Player_Death:
                player[1].Play();
                bgm[0].Stop();
                bgm[1].volume = 1;
                break;
            case SFX.Player_Tongue:
                player[0].Play();
                break;
            default:
                break;
        }
    }



}
