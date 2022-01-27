using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

[System.Serializable]
public class MenuPanel
{
    public CanvasGroup canvasGroup;
    public GameObject firstSelected;
}


public class UiManager : MonoBehaviour
{
    public static UiManager um;

    public CanvasGroup fliesDisplay;
    public float transitionTime;
    public List<MenuPanel> menuPanels;

    public int currentScene;

    #region Unity Loop
    private void Awake()
    {
        um = this;
        DontDestroyOnLoad(this.gameObject);
        
    }

    private void Start()
    {
        StartCoroutine(Init());
    }
    #endregion



    #region Button Calls

    public void DisplayFlies(bool _b)
    {
        fliesDisplay.alpha = _b ? 1f : 0f;
    }

    public void DisplayMenuPanel(int id)
    {
        StartCoroutine(MenuTransition(id));
    }

    public void CloseMenu()
    {
        foreach (MenuPanel panel in menuPanels)
        {
            panel.canvasGroup.alpha = 0f;
            panel.canvasGroup.interactable = false;
            panel.canvasGroup.blocksRaycasts = false;
        }
    }

    public void LoadScene(int index)
    {
        StartCoroutine(SceneTransition(index));
    }

    public void Pause(bool pause)
    {
        Time.timeScale = pause ? 0f : 1f;
        StartCoroutine(MenuTransition(2));
    }

    public void Retry()
    {
        StartCoroutine(SceneTransition(currentScene));
    }

    public void NextLevel()
    {
        if (currentScene + 1 < SceneManager.sceneCountInBuildSettings)
            StartCoroutine(SceneTransition(currentScene + 1));
    }

    public void Quit()
    {
        Application.Quit();
    }

    #endregion

    #region Coroutines

    IEnumerator MenuTransition (int _id)
    {
        AudioManager.am.ui[0].mute = true;
        foreach (MenuPanel panel in menuPanels)
        {
            panel.canvasGroup.alpha = 0f;
            panel.canvasGroup.interactable = false;
            panel.canvasGroup.blocksRaycasts = false;
        }

        //Transition Start Animation
        menuPanels[_id].canvasGroup.alpha = 1f;
        menuPanels[_id].canvasGroup.interactable = true;
        menuPanels[_id].canvasGroup.blocksRaycasts = true;
        EventSystem.current.SetSelectedGameObject(menuPanels[_id].firstSelected);


        yield return new WaitForSecondsRealtime(transitionTime);
        AudioManager.am.ui[0].mute = false;
        // Transition End Animation

    }

    IEnumerator SceneTransition (int _i)
    {
        //Trigger Transition Animation

        yield return new WaitForSecondsRealtime(transitionTime);

        currentScene = _i;
        SceneManager.LoadScene(_i);

        
    }

    IEnumerator Init()
    {
        AudioManager.am.ui[0].mute = true;
        yield return new WaitForSecondsRealtime(0.4f);
        AudioManager.am.ui[0].mute = false;
    }



    #endregion

}
