using System.Collections;
using System.Collections.Generic;
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
