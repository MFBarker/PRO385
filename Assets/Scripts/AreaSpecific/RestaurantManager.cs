using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
 Game Loop: 
* Choose 2-4 customers at random
* Have one or two come in at a time (depending on seat availiability)
* Take their order
* Once order is taken, have timer count down (customer patience)
* serve order as fast as possible, else customer gets mad and leaves
* Have waiting timer to displace all customers (so all customers don't come all at once)
* Loop Until all Customers have been gone through
 
* WIN: Serve all customers (ranking based on number of customers that left [S = 0, A = 1, C = 2, D = 3])
* LOSE: Fail All Customers (all customers leave)
 */
public class RestaurantManager : MonoBehaviour
{
    [Header("General UI")]
    [SerializeField] GameObject pauseUI;
    [SerializeField] GameObject gameUI;
    [SerializeField] GameObject quitConfirmUI;
    [SerializeField] GameObject customerInfoUI;
    [SerializeField] GameObject customerUI;
    [Header("Area Specific UI")]
    [SerializeField] GameObject kitchenUI;
    [SerializeField] GameObject barUI;
    [SerializeField] GameObject drinksUI;

    Camera gameCamera = null;
    int[] x_Location = { -25, 0, 25 };
    bool paused = false;


    void Awake()
    {
        gameCamera = GameObject.FindGameObjectWithTag("GameCamera").GetComponent<Camera>();
    }

    private void Start()
    {
        GameManager.Instance.GetBGM("Restaurant").Play();
    }

    // Update is called once per frame
    void Update()
    {
        //UI
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
        //BGM Volume
        if (GameManager.Instance.GetBGMMuted() == false && paused == false) GameManager.Instance.GetBGM("Restaurant").volume = 1.0f; //not muted
        else if (GameManager.Instance.GetBGMMuted() == true) GameManager.Instance.GetBGM("Restaurant").volume = 0.0f; //muted
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
        paused = true;
        Debug.Log("pause");
        Time.timeScale = 0.0f;
        pauseUI.SetActive(true);
        gameUI.SetActive(false);
    }
    public void OnUnPause()
    {
        paused = false;
        Time.timeScale = 1.0f;
        pauseUI.SetActive(false);
        gameUI.SetActive(true);
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
        /*https://stackoverflow.com/questions/67934167/disable-user-from-interacting-in-input-field-dropdown-and-toggle-in-unity */
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

    public void tempEnd()
    { 
        GameManager.Instance.OnToEnd();
    }

    #region CustomerInfo
    public void Click_Character() 
    {
        Debug.Log("test");
        customerUI.SetActive(true);
    }
    public void Click_OutCharacter()
    {
        customerUI.SetActive(false);
    }
    public void Click_OutInfo()
    {
        customerInfoUI.SetActive(false);
        //Unpause
        gameUI.SetActive(true);
        Time.timeScale = 1.0f;
    }
    #endregion

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
        customerInfoUI.SetActive(true);
        //Pause Game
        gameUI.SetActive(false);
        Time.timeScale = 0.0f;
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
