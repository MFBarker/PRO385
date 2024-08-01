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
    [Header("Settings")]
    [SerializeField] GameObject SettingsUIPrefab;
    
    //Settings
    bool m_mutedBGM = false;
    bool m_mutedSFX = false;

    GameObject m_settingsUI = null;
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

    #region Settings
    public void OnClickSettings()
    {
        if (SettingsUIPrefab != null)
        {
            Debug.Log("test");
            if (m_settingsUI == null)
            {
                //instantiate object
                m_settingsUI = GameObject.Instantiate(SettingsUIPrefab);
                m_settingsUI.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
                //Enable UI
                //Debug.Log("Null Game Object");
            }
            else
            {
                //Enable UI
                m_settingsUI.SetActive(true);
            }
        }
        else 
        {
            Debug.Log("Settings (Not Implemented)");
        }
    }

    public void SetBGMMuted(bool bgm)
    {
        m_mutedBGM = bgm;
    }

    public void SetSFXMuted(bool sfx)
    {
        m_mutedSFX = sfx;
    }

    public bool GetBGMMuted() {  return m_mutedBGM; }
    public bool GetSFXMuted() {  return m_mutedBGM; }

    public float GetSFXVolume() 
    {
        if (m_mutedSFX == false) return 1.0f;  //not muted
        else return 0.0f; //muted
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

        m_settingsUI = null;
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
