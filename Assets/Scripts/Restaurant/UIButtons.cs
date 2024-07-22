using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIButtons : MonoBehaviour
{
    public enum RButtons
    { 
        Kitchen,
        Bar,
        Drinks
    }

    [SerializeField] RButtons buttonType { get; set; }
    Camera camera = Camera.main;

    public Vector3 kitchen = new Vector3(-25, 0, 0);
    Vector3 bar = new Vector3(0, 0, 0);
    Vector3 drinks = new Vector3(25, 0, 0);

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnButtonClick()
    {
        switch (buttonType)
        {
            case RButtons.Kitchen:
                //-25,0
                camera.transform.transform.localPosition = kitchen;
                break;
            case RButtons.Bar:
                //0,0
                camera.transform.transform.localPosition = bar;
                break;
            case RButtons.Drinks:
                //25,0
                camera.transform.transform.localPosition = drinks;
                break;
            default:
                break;
        }
    }
}
