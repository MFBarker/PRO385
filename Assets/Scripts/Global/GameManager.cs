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

    //Settings
    float m_masterVolumeBGM = 1.0f;
    bool m_mutedBGM = false;
    #endregion

    #region SceneTransitions
    public void OnToGame()
    {
        StopAll();
        SceneManager.LoadSceneAsync("Restaurant", LoadSceneMode.Single);
    }
    public void OnToEnd()
    {
        StopAll();
        SceneManager.LoadSceneAsync("End", LoadSceneMode.Single);
    }

    public void OnToTitle()
    {
        StopAll();
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

    #region Audio
    private void StopAll()
    { 
        Title.Stop();
        InGame.Stop();
        End.Stop();
    }

    public AudioSource GetBGM(string scene)
    {
        if (scene.ToLower() == "title")
        {
            return Title;
        }
        else if (scene.ToLower() == "restaurant")
        {
            return InGame;
        }
        else if (scene.ToLower() == "end")
        { 
            return End;
        }
        else 
        {
            return null;
        }
    }
    #endregion
}
