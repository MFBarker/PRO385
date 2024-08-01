using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class EndScreenManager : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        GameManager.Instance.GetBGM("End").Play();
    }

    // Update is called once per frame
    void Update()
    {
        //BGM Volume
        if (GameManager.Instance.GetBGMMuted() == false) GameManager.Instance.GetBGM("End").volume = 1.0f; //not muted
        else if (GameManager.Instance.GetBGMMuted() == true) GameManager.Instance.GetBGM("End").volume = 0.0f; //muted
    }

    public void QuitToTitle()
    { 
        GameManager.Instance.OnToTitle();
    }

    public void NextDay()
    {
        GameManager.Instance.OnToGame();
    }

    public void Settings()
    { 
        GameManager.Instance.OnClickSettings();
    }

}
