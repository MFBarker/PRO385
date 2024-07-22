using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.FilePathAttribute;

public class UIButtons : MonoBehaviour
{
    enum RButtons
    { 
        Kitchen,
        Bar,
        Drinks
    }

    enum ButtonLocation
    { 
        Left,
        Right
    }

    [SerializeField] ButtonLocation buttonLocation;
    [SerializeField] Button buttonObjectLeft;
    [SerializeField] Button buttonObjectRight;
    Camera camera = null;

    Vector3 kitchen = new Vector3(-25, 0, -10);
    Vector3 bar = new Vector3(0, 0, -10);
    Vector3 drinks = new Vector3(25, 0, -10);

    /*
             Restaurant Layout 

     |  Kitchen --- Bar --- Drinks  |
     
     */

    // Update is called once per frame
    private void Awake()
    {
        /* Gets Secondary Camera to Move Around */
        camera = GameObject.FindGameObjectWithTag("GameCamera").GetComponent<Camera>();
    }

    private void Update()
    {
        if (camera.transform.position == kitchen && buttonObjectLeft.IsActive()) HideButton("Left");
        if (camera.transform.position == drinks && buttonObjectRight.IsActive()) HideButton("Right");
    }

    /// <summary>
    /// When button is clicked, the game will move left or right, depending on what button is pressed.
    /// </summary>
    public void OnButtonClick()
    {
        switch (buttonLocation)
        {
            case ButtonLocation.Left:
                //Kitchen Hides Buttons (Update)
                if (camera.transform.position == bar)
                {
                    LocationChange(RButtons.Kitchen); 
                }
                else //drinks
                {
                    //Unhide Right, Go to bar
                    UnHideButton("Right");
                    LocationChange(RButtons.Bar);
                }
                break;
            case ButtonLocation.Right:
                if (camera.transform.position == kitchen) 
                {
                    //unhide left, go to bar
                    UnHideButton("Left");
                    LocationChange(RButtons.Bar);
                }
                else if (camera.transform.position == bar)
                {
                    LocationChange(RButtons.Drinks);
                }
                //Drinks Hides Button (Update)
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Hides Button based on Location
    /// </summary>
    /// <param name="location">the location of the button to be hidden</param>
    private void HideButton(string location)
    { 
        if (location == "Left") buttonObjectLeft.gameObject.SetActive(false);
        if (location == "Right") buttonObjectRight.gameObject.SetActive(false);
    }

    /// <summary>
    /// Unhides Button based on Location
    /// </summary>
    /// <param name="location">the location of the button to be unhidden</param>
    private void UnHideButton(string location)
    {
        if (location == "Left") buttonObjectLeft.gameObject.SetActive(true);
        if (location == "Right") buttonObjectRight.gameObject.SetActive(true);
    }

    /// <summary>
    /// Moves the camera based on where the player needs to go.
    /// </summary>
    /// <param name="buttonType">Where the player is moving to.</param>
    private void LocationChange(RButtons buttonType)
    {
        switch (buttonType)
        {
            case RButtons.Kitchen:
                //-25,0
                Debug.Log("To Kitchen");
                camera.transform.position = kitchen;
                break;
            case RButtons.Bar:
                //0,0
                Debug.Log("To Bar");
                camera.transform.position = bar;
                break;
            case RButtons.Drinks:
                //25,0
                Debug.Log("To Drinks");
                camera.transform.position = drinks;
                break;
            default:
                break;
        }
    }
}
