using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] GameObject pauseUI;
    [SerializeField] GameObject gameUI;

    enum eState
    { 
        Title,
        InGame,
        Paused,
        End
    }

    eState currentScene;

    // Start is called before the first frame update
    void Start()
    {
        currentScene = eState.Title;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentScene)
        {
            case eState.Title:
                //starts on title
                break;
            case eState.InGame:
                break;
            case eState.End:
                break;
            case eState.Paused:
                
                break;
            default:
                break;
        }
    }

    //functions for title screen
    public void OnToGame()
    {
        SceneManager.LoadSceneAsync("Restaurant", LoadSceneMode.Single);
    }
    public void ToEnd()
    {
        SceneManager.LoadSceneAsync("End", LoadSceneMode.Single);
    }

    public void ToTitle()
    {
        SceneManager.LoadSceneAsync("Title", LoadSceneMode.Single);
    }

    /* https://gamedevbeginner.com/the-right-way-to-pause-the-game-in-unity/ */
    public void OnPause() 
    {
        Time.timeScale = 0.0f;
        pauseUI.SetActive(true);
        gameUI.SetActive(false);
    }
    public void OnUnPause()
    {
        Time.timeScale = 1.0f;
        pauseUI.SetActive(false);
        gameUI.SetActive(true);
    }
}
