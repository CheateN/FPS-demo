using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider loadingBar;
    public void PlayGame()
    {
        StartCoroutine(LoadSceneAsync("Level"));
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    
    IEnumerator LoadSceneAsync(string sceneName)
    {
        loadingScreen.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            loadingBar.value = operation.progress;
            yield return null;
        }
    }
}