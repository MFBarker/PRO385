using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{ 
    #region _Variables
    [Header("BGM")]
    [SerializeField] AudioSource Title;
    [SerializeField] AudioSource InGame;
    [SerializeField] AudioSource End;
    

    [SerializeField] eState currentScene;

    //Settings
    float m_masterVolumeBGM = 1.0f;
    bool m_mutedBGM = false;
    #endregion

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
    

    //Settings
    public void OnClickSettings()
    {
        Debug.Log("Settings (Not Implemented)");
    }

    #endregion

    #region End
    //Put stuff here
    #endregion
}
