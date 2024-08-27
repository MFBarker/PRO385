using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class EndScreenManager : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;

    // Start is called before the first frame update
    private void Start()
    {
        GameManager.Instance.GetBGM("End").Play();
        //set score
        if (scoreText != null)
        { 
            int score = GameManager.Instance.GetScore();
            scoreText.text = "Score: " + score;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //should run first
        

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
