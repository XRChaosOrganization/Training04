using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelButtonDisable : MonoBehaviour
{
    Button button;
    public int levelIndex;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.interactable = levelIndex > SceneManager.sceneCountInBuildSettings - 1 ? false : true;
    }


}
