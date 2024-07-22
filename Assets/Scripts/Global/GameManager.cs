using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UIButtons;

public class GameManager : MonoBehaviour
{
    [Header("BGM")]
    [SerializeField] AudioSource Title;
    [SerializeField] AudioSource InGame;
    [SerializeField] AudioSource End;
    [Header("Restaurant")]
    [SerializeField] GameObject pauseUI;
    [SerializeField] GameObject gameUI;

    [SerializeField] eState currentScene;

    //Settings
    float m_masterVolumeBGM = 1.0f;
    bool m_mutedBGM = false;

    enum eState
    { 
        Title,
        InGame,
        End
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentScene)
        {
            case eState.Title:
                //starts on title
                //if (!Title.isPlaying) Title.Play();
                break;
            case eState.InGame:
                //if (!InGame.isPlaying) InGame.Play();
                break;
            case eState.End:
                //if (!End.isPlaying) End.Play();
                break;
            default:
                break;
        }
    }

    #region SceneTransitions
    public void OnToGame()
    {
        /*if (Title.isPlaying) Title.Stop();
        if (End.isPlaying) End.Stop();*/
        SceneManager.LoadSceneAsync("Restaurant", LoadSceneMode.Single);
    }
    public void OnToEnd()
    {
        SceneManager.LoadSceneAsync("End", LoadSceneMode.Single);
    }

    public void OnToTitle()
    {
        SceneManager.LoadSceneAsync("Title", LoadSceneMode.Single);
    }
    #endregion

    #region In-Game
    /* https://gamedevbeginner.com/the-right-way-to-pause-the-game-in-unity/ */
    public void OnPause() 
    {
        Time.timeScale = 0.0f;
        pauseUI.SetActive(true);
        gameUI.SetActive(false);
        /*InGame.volume = MasterVolume;*/
    }
    public void OnUnPause()
    {
        Time.timeScale = 1.0f;
        pauseUI.SetActive(false);
        gameUI.SetActive(true);
        /*InGame.volume = 1.0f;*/
    }

    //Restaurant Buttons in UIButtons

    #endregion

    #region End
    //Put stuff here
    #endregion
}
