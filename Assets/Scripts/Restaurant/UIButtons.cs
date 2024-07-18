using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtons : MonoBehaviour
{
    enum RButtons
    { 
        Kitchen,
        Bar,
        Drinks
    }

    [SerializeField] RButtons buttonType;
    [SerializeField] Camera camera;

    Vector3 kitchen = new Vector3(-25, 0, 0);
    Vector3 bar = new Vector3(0, 0, 0);
    Vector3 drinks = new Vector3(25, 0, 0);
    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //test
    private void OnMouseDown()
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
