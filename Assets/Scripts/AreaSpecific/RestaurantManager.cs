using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
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
    [SerializeField] TMP_Text orderDisplay;
    [SerializeField] GameObject hintsUI;
    [Header("Area Specific UI")]
    [SerializeField] GameObject kitchenUI;
    [SerializeField] GameObject barUI;
    [SerializeField] GameObject drinksUI;
    [Header("Customers")]
    [SerializeField] Image[] slots = new Image[3];
    [SerializeField] Button[] takeOrderButtons = new Button[3];
    [SerializeField] Button[] serveOrderButtons = new Button[3];
    //General
    Camera gameCamera = null;
    int[] x_Location = { -25, 0, 25 };
    bool paused = false;
    //Customer Stuff
    bool canSeat = true;
    float coolDown = 10.0f;
    bool done;

    Customer[] seats = { null, null, null };
    float[] timers = { 0.0f, 0.0f, 0.0f };
    List<CustomerAI> customerAIs = new List<CustomerAI>();
    
    List<Customer> hasServed = new List<Customer>(4); 

    //Serve Customers
    List<string> items = new List<string>();
    List<string> orders = new List<string>(3);
    bool[] served = { false, false, false };

    void Awake()
    {
        gameCamera = GameObject.FindGameObjectWithTag("GameCamera").GetComponent<Camera>();

        customerAIs.Add(new CustomerAI("Akio Tanaka", "Assets/Art/Characters/frog-sprite.png", "Assets/Art/Characters/SpriteTemp_Mad.png", null)); //sake
        customerAIs.Add(new CustomerAI("Haruto Nakamura", "Assets/Art/Characters/frog-sprite.png", "Assets/Art/Characters/SpriteTemp_Mad.png", null));
        customerAIs.Add(new CustomerAI("Hayato Kami", "Assets/Art/Characters/frog-sprite.png", "Assets/Art/Characters/SpriteTemp_Mad.png", null));
        customerAIs.Add(new CustomerAI("Logan Smith", "Assets/Art/Characters/frog-sprite.png", "Assets/Art/Characters/SpriteTemp_Mad.png", null)); //whiskey
    }

    private void Start()
    {
        GameManager.Instance.GetBGM("Restaurant").Play();
    }

    // Update is called once per frame
    void Update()
    {
        //end conditions
        if (IsDone())
        {
            //done
            GameManager.Instance.OnToEnd();
        }

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
                ReactivateSlots();
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
            coolDown = 10;
        }

        if (GameManager.Instance.GetHintsEnabled() == true) hintsUI.SetActive(true);
        else hintsUI.SetActive(false);

        if (orders != null) 
        {
            CanServeCustomer();
            DecrementTimers();
        }
    }

    #region Utility
    ////
    /// <summary>
    /// Disables All UI In-Game
    /// </summary>
    private void DisableAllUI()
    {
        kitchenUI.SetActive(false);
        barUI.SetActive(false);
        drinksUI.SetActive(false);

        foreach (var slot in slots)
        {
            slot.gameObject.SetActive(false);
        }
    }

    private void ReactivateSlots()
    {
        if (seats[0] != null) slots[0].gameObject.SetActive(true);
        if (seats[1] != null) slots[1].gameObject.SetActive(true);
        if (seats[2] != null) slots[2].gameObject.SetActive(true);
    }
    private void UpdateOrdersList()
    {
        string temp = "";
        foreach (var item in orders)
        {
            temp += item.ToString() + " ";
        }

        orderDisplay.text = temp;
    }
    #endregion

    #region Game Loop
    bool IsSeatOpen()
    {
        foreach (Customer c in seats)
        {
            if (c == null) return true;
        }
        return false;
    }

    bool IsDone()
    {
        //check if all customers gone
        foreach (Customer c in seats)
        {
            if (c != null) return false;
        }
        if (hasServed.Count == 4)
        {
            //no customers in store and all customers served
            return true;
        }
        return false;
    }

    //Loop methods
    private void SeatCustomers()
    {
        if (hasServed != null)
        {
            if (hasServed.Count == 4) done = true;
        }
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
            ShowTakeOrder(0);
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
            ShowTakeOrder(1);
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
            ShowTakeOrder(2);
        }
        //make sure everything is always invisible
        if (gameCamera.transform.position.x == -25 || gameCamera.transform.position.x == 25) 
        {
            foreach (var slot in slots)
            {
                slot.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Gets Random Customer. Verifies that the character has not already been seated.
    /// </summary>
    /// <returns>Returns customer if there is one still avaliable, else null</returns>
    private Customer GetRandomCustomer()
    {
        Customer c = null;
        bool valid = false;
        while (valid == false)
        {
            c = Customers.Instance.GetCustomerByIndex(Random.Range(0, 3));
            //don't validate anything unless they are not null
            if (seats != null || hasServed != null)
            {
                if (!seats.Contains(c) && (!hasServed.Contains(c) && hasServed != null)) valid = true;
                if (valid == false && hasServed.Count == 4) break;
                if (hasServed.Count == 3)
                {
                    //get last customer (prevent potentially infinite looping)
                    foreach (Customer cu in Customers.Instance.customers)
                    {
                        if (!hasServed.Contains(cu))
                        { 
                            c = cu;
                            valid = true;
                            break;
                        }
                    }
                }
            }
            else valid = true;
        }
        return c;
    }

    /// <summary>
    /// Gets Customer AI
    /// </summary>
    /// <param name="c">The Customer to Retrieve</param>
    /// <returns>The CustomerAI if found, else false</returns>
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

    /// <summary>
    /// Remove the Customer and open the Seat back up
    /// </summary>
    /// <param name="isAngry">Is the Customer Leaving out of Anger? (Lost Patience/Timer Ran Out)</param>
    /// <param name="slot">Which seat is the customer in?</param>
    private void RemoveCustomer(bool isAngry, int slot)
    {
        if (seats[slot] == null) return;
        if (isAngry) 
        {
            //is Angry

            //change to mad sprite?
            StartCoroutine(AngryAnim(slot));
            GameManager.Instance.SetScore(GameManager.Instance.GetScore() - 1);

            string ord = GetCustomerAI(seats[slot]).order;
            //remove order from list and remove item from availiable
            orders.RemoveAt(orders.IndexOf(ord));
            UpdateOrdersList();
        }   
        else
        {
            //not Angry
            GameManager.Instance.SetScore(GameManager.Instance.GetScore() + 1);
        }
        //remove character
        slots[slot].gameObject.SetActive(false);
        //clear seat spot
        seats[slot] = null;
        timers[slot] = 0.0f;
    }

    IEnumerator AngryAnim(int slot)
    {
        if (slots[slot] == null) yield break;
        //change sprite
        slots[slot].sprite = GetCustomerAI(seats[slot]).spriteMad;
        //wait
        yield return new WaitForSeconds(10);
        
    }

    //decrement patience timers
    private void DecrementTimers()
    { 
        for(int t = 0; t < timers.Length; t++)
        {
            //null check
            if (seats[t] == null) continue;
            //check if timer is up (customer is angry and leaves)
            if (timers[t] <= 0) RemoveCustomer(true, 0);
            //else decrement timer
            else
            {
                timers[t] -= Time.deltaTime;
                //Debug.Log($"Seat{t}: {timers[t]}");
            }
        }
    }

    //Take Order
    private void ShowTakeOrder(int slot) { takeOrderButtons[slot].gameObject.SetActive(true); }
    public void Click_TakeOrder(GameObject button) 
    { 
        //set ui not active
        button.SetActive(false);
        //Add Order to List
        if (button == takeOrderButtons[0].gameObject) orders.Add(GetCustomerAI(seats[0]).order);
        else if (button == takeOrderButtons[1].gameObject) orders.Add(GetCustomerAI(seats[1]).order);
        else if (button == takeOrderButtons[2].gameObject) orders.Add(GetCustomerAI(seats[2]).order);

        UpdateOrdersList();
    }
    //ServeCustomer
    private void CanServeCustomer()
    {
        //don't try anything if there are no orders or drinks poured
        if (orders.Count == 0) return;
        if (items.Count == 0) return;
        //items list contains order
        foreach (var item in seats) 
        {
            //don't try if there is no customer in the seat
            if (item == null) continue;
            //yes
            string order = GetCustomerAI(item).order;
            if (items.Contains(order))
            {
                //show serve button
                if (item == seats[0] && !takeOrderButtons[0].gameObject.activeSelf) 
                { 
                    serveOrderButtons[0].gameObject.SetActive(true);
                }
                else if (item == seats[1] && !takeOrderButtons[1].gameObject.activeSelf) 
                {
                    serveOrderButtons[1].gameObject.SetActive(true);
                }
                else if (item == seats[2] && !takeOrderButtons[2].gameObject.activeSelf) 
                { 
                    serveOrderButtons[2].gameObject.SetActive(true); 
                }

            }
            //no
            else 
            {
                //hide serve button if active
                if (item == seats[0] && serveOrderButtons[0].gameObject.activeSelf) 
                    serveOrderButtons[0].gameObject.SetActive(false);
                else if (item == seats[1] && serveOrderButtons[1].gameObject.activeSelf) 
                    serveOrderButtons[1].gameObject.SetActive(false);
                else if (item == seats[2] && serveOrderButtons[2].gameObject.activeSelf) 
                    serveOrderButtons[2].gameObject.SetActive(false);
            }
        }
    }

    public void Click_ServeOrder(GameObject button)
    {
        int slot = -1;
        //hide button
        if (button == serveOrderButtons[0].gameObject) 
        {
            serveOrderButtons[0].gameObject.SetActive(false);
            slot = 0;
        }
        else if (button == serveOrderButtons[1].gameObject) 
        {
            serveOrderButtons[1].gameObject.SetActive(false);
            slot = 1;
        }
        else if (button == serveOrderButtons[2].gameObject) 
        { 
            serveOrderButtons[2].gameObject.SetActive(false);
            slot = 2;
        }
        //get customer and ai, get order
        string ord = GetCustomerAI(seats[slot]).order;
        //remove order from list and remove item from availiable
        items.RemoveAt(items.IndexOf(ord));
        orders.RemoveAt(orders.IndexOf(ord));
        UpdateOrdersList();

        //temp
        RemoveCustomer(false, slot);
    }

    
    #endregion

    #region Settings/PauseMenu
    /* https://gamedevbeginner.com/the-right-way-to-pause-the-game-in-unity/ */
    public void OnPause()
    {
        paused = true;
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

    //Customer info
    public void Bar_CustomerInfo()
    {
        customerInfoUI.SetActive(true);
        //Pause Game
        gameUI.SetActive(false);
        Time.timeScale = 0.0f;
    }
    #endregion

    #region CustomerInfo
    public void Click_Character() 
    {
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

    //not implemented
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

    #region drinks
    public void Drinks_Beer()
    {
        items.Add("beer");
    }
    public void Drinks_Sake()
    {
        items.Add("sake");
    }
    public void Drinks_Shochu()
    {
        items.Add("shochu");
    }
    public void Drinks_Whiskey()
    {
        items.Add("whiskey");
    }
    #endregion
}
