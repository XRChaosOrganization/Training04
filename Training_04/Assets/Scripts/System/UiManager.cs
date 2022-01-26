using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UiManager : MonoBehaviour
{
    public static UiManager um;

    public CanvasGroup titleScreen;
    public CanvasGroup levelSelect;
    public CanvasGroup pause;
    public CanvasGroup gameOver;
    public CanvasGroup win;
    public CanvasGroup fliesDisplay;

    private void Awake()
    {
        um = this;



        DontDestroyOnLoad(this.gameObject);
    }

}
