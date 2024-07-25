using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestaurantManager : MonoBehaviour
{
    [SerializeField] GameObject pauseUI;
    [SerializeField] GameObject gameUI;
    [SerializeField] GameObject quitConfirmUI;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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

    //Pause Menu
    /*
                 QUIT:
         open confirmation window
         wait for response
         act on response
    */
    public void OnClickQuit()
    {
        quitConfirmUI.SetActive(true);
        /*https://stackoverflow.com/questions/67934167/disable-user-from-interacting-in-input-field-dropdown-and-toggle-in-unity*/
        var uiElements = pauseUI.GetComponentsInChildren<Selectable>();
        foreach (var uiElement in uiElements)
        {
            uiElement.interactable = false;
        }
    }

    //button responses
    public void Quit_Yes()
    {
        Quit_No();
        OnUnPause();
        GameManager.Instance.OnToTitle();
    }
    public void Quit_No()
    {
        quitConfirmUI.SetActive(false);
        var uiElements = pauseUI.GetComponentsInChildren<Selectable>();
        foreach (var uiElement in uiElements)
        {
            uiElement.interactable = true;
        }
    }

    public void OnClickSettings()
    {
        GameManager.Instance.OnClickSettings();
    }


    public void tempCheck()
    {
        GameManager.Instance.OnToTitle();
    }
}
