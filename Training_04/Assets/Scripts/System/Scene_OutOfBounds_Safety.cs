using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Scene_OutOfBounds_Safety : MonoBehaviour
{
    Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Update()
    {
        button.interactable = UiManager.um.currentScene + 1 < SceneManager.sceneCountInBuildSettings ? true : false;
    }
}
