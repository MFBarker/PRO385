using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestaurantManager : MonoBehaviour
{
    [SerializeField] GameObject pauseUI;
    [SerializeField] GameObject gameUI;
    [SerializeField] GameObject quitConfirmUI;
    [Header("Area Specific UI")]
    [SerializeField] GameObject kitchenUI;
    [SerializeField] GameObject barUI;
    [SerializeField] GameObject drinksUI;

    Camera gameCamera = null;
    int[] x_Location = { -25, 0, 25 }; 

    void Awake()
    {
        gameCamera = GameObject.FindGameObjectWithTag("GameCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameCamera.transform.position.x == x_Location[0]) //kitchen
        {
            if (!kitchenUI.activeSelf)
            { 
                DisableAllUI();
                kitchenUI.SetActive(true);
            }
        }
        else if (gameCamera.transform.position.x == x_Location[1]) //bar
        {
            if (!barUI.activeSelf)
            {
                DisableAllUI();
                barUI.SetActive(true);
            }
        }
        else if (gameCamera.transform.position.x == x_Location[2]) //drinks
        {
            if (!drinksUI.activeSelf)
            {
                DisableAllUI();
                drinksUI.SetActive(true);
            }
        }
    }

    private void DisableAllUI()
    { 
        kitchenUI.SetActive(false);
        barUI.SetActive(false);
        drinksUI.SetActive(false);
    }

    /* https://gamedevbeginner.com/the-right-way-to-pause-the-game-in-unity/ */
    public void OnPause()
    {
        Debug.Log("pause");
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

    public void tempEnd()
    { 
        GameManager.Instance.OnToEnd();
    }


    #region kitchen
    public void Kitchen_Fridge()
    {
        Debug.Log("fridge");
    }
    public void Kitchen_Fryer()
    {
        Debug.Log("fryer");
    }
    public void Kitchen_Pan()
    {
        Debug.Log("pan");
    }
    public void Kitchen_Plates()
    {
        Debug.Log("plates");
    }
    public void Kitchen_Bowls()
    {
        Debug.Log("bowls");
    }
    #endregion

    #region bar
    public void Bar_CustomerInfo()
    {
        Debug.Log("Customer Info");
    }
    public void Bar_Orders()
    {
        Debug.Log("Orders");
    }
    #endregion

    #region drinks
    public void Drinks_Beer()
    {
        Debug.Log("beer");
    }
    public void Drinks_Sake()
    {
        Debug.Log("sake");
    }
    public void Drinks_Shochu()
    {
        Debug.Log("shochu");
    }
    public void Drinks_Whiskey()
    {
        Debug.Log("whiskey");
    }

    public void Drinks_Fridge()
    {
        Debug.Log("fridge");
    }
    #endregion
}
