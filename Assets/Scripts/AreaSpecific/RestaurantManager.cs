using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.Intrinsics;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
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
    [SerializeField] Image[] slots = new Image[3];
    //General
    Camera gameCamera = null;
    int[] x_Location = { -25, 0, 25 };
    bool paused = false;
    //Customer Stuff
    bool canSeat = true;
    float coolDown = 30.0f;
    bool done;

    Customer[] seats = { null, null, null };
    float[] timers = { 0.0f, 0.0f, 0.0f };
    CustomerAI[] customerAIs =
    {
        new CustomerAI("Akio Tanaka","Assets/Art/Characters/SpriteTemp.png","Assets/Art/Characters/SpriteTemp_Mad.png",""),
        new CustomerAI("Haruto Nakamura","Assets/Art/Characters/SpriteTemp.png","Assets/Art/Characters/SpriteTemp_Mad.png",""),
        new CustomerAI("Hayato Kami","Assets/Art/Characters/SpriteTemp.png","Assets/Art/Characters/SpriteTemp_Mad.png",""),
        new CustomerAI("Logan Smith","Assets/Art/Characters/SpriteTemp.png","Assets/Art/Characters/SpriteTemp_Mad.png","")
    };
    List<Customer> hasServed = new List<Customer>(); 

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
        //Game Loop
        if (IsSeatOpen() && canSeat)
        {
            SeatCustomers();
        }
        
        if (canSeat == false && coolDown > 0) coolDown -= Time.deltaTime;
        if (coolDown <= 0 && hasServed.Count < 4)
        {
            //cooldown reset
            canSeat = true;
            coolDown = 60;
            Debug.Log("ALERT: Cool Down Reset!!");
        }
        //if (hasServed.Count == 4)
        //{
        //    done = true;
        //}

        //end conditions
        if (done && IsEmpty())
        {
            //done
            tempEnd();
        }
    }

    private void DisableAllUI()
    {
        kitchenUI.SetActive(false);
        barUI.SetActive(false);
        drinksUI.SetActive(false);
    }

    #region Game Loop
    bool IsSeatOpen()
    {
        foreach (Customer c in seats)
        {
            if (c == null) return true;
        }
        return false;
    }

    bool IsEmpty()
    {
        foreach (Customer c in seats)
        {
            if (c != null) return false;
        }
        return true;
    }

    //seat customer
    //wait five-ish seconds
    //add popup for order
    //take order
    //start countdown (personality based)
    //serve customer or fail
    private void SeatCustomers()
    {
        if (hasServed.Count == 4) return;
        //get slot
        if (seats[0] == null && canSeat == true)
        {
            //set customer to slot
            seats[0] = GetRandomCustomer();
            if (seats[0] == null) { return; }//null check
            //put customer sprite there
            slots[0].gameObject.SetActive(true);
            slots[0].sprite = GetCustomerAI(seats[0]).spriteNormal;
            //start timer
            timers[0] = GetCustomerAI(seats[0]).SetTimer();
            
            hasServed.Add(seats[0]);
            canSeat = false;
        }
        else if (seats[1] == null && canSeat == true)
        {
            //set customer to slot
            seats[1] = GetRandomCustomer();
            if (seats[1] == null) { return; }//null check
            //put customer sprite there
            slots[1].gameObject.SetActive(true);
            slots[1].sprite = GetCustomerAI(seats[0]).spriteNormal;
            //start timer
            timers[1] = GetCustomerAI(seats[0]).SetTimer();
            hasServed.Add(seats[1]);
            canSeat = false;
        }
        else if (seats[2] == null && canSeat == true)
        {
            //set customer to slot
            seats[2] = GetRandomCustomer();
            if (seats[2] == null) { return; }//null check
            //put customer sprite there
            slots[2].gameObject.SetActive(true);
            slots[2].sprite = GetCustomerAI(seats[0]).spriteNormal;
            //start timer
            timers[2] = GetCustomerAI(seats[0]).SetTimer();
            hasServed.Add(seats[2]);
            canSeat = false;
        }
    }

    private Customer GetRandomCustomer()
    {
        Customer c = null;
        bool valid = false;
        while (valid == false)
        {
            c = Customers.Instance.GetCustomerByIndex(Random.Range(0, 3));
            if (!seats.Contains(c) && !hasServed.Contains(c)) valid = true;
        }

        return c;
    }

    private CustomerAI GetCustomerAI(Customer c)
    {
        foreach (CustomerAI ai in customerAIs)
        {
            if (ai.customer == c)
            { 
                return ai;
            }
        }
        return null;
    }

    private void RemoveCustomer(bool isAngry, int slot)
    {
        if (isAngry) 
        {
            //is Angry

            //change to mad sprite?
            slots[slot].sprite = GetCustomerAI(seats[0]).spriteMad;
            //WaitForSeconds(3);
        }   
        else
        {
            //not Angry
        }
        //remove character
        slots[slot].gameObject.SetActive(false);
        //clear seat spot
        seats[slot] = null;
        timers[slot] = 0.0f;
    }
    //ServeCustomer
    //Take Order
    //Start Customer Timer
    //Fail Customer
    void FailCustomer()
    { 

    }
    #endregion

    #region Settings/PauseMenu
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
    #endregion

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
