using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager am;

    private void Awake()
    {
        am = this;



        DontDestroyOnLoad(this.gameObject);
    }
}
