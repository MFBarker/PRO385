using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class TitleManager : MonoBehaviour
{
    #region Variables
    [SerializeField] GameObject TitleUI;
    [SerializeField] GameObject SaveSelectUI;

    Camera gameCamera = null;
    GameObject settingsButton = null;

    bool goTitle = false;
    bool goSaveSelect = false;

    int transitionSpeedScaler = 20;
    float transitionDuration = 1.5f;

    Vector3 titleXYZ = new Vector3 ( 0.0f, 0.0f, -10.0f );
    Vector3 saveXYZ = new Vector3 ( 20.15f, 0.0f, -10.0f );
    #endregion

    void Awake()
    {
        //Activates Title UI if Disabled, Deactivates the Save Select UI if Enables
        if (!TitleUI.activeSelf) TitleUI.SetActive(true);
        if (SaveSelectUI.activeSelf) SaveSelectUI.SetActive(false);
        //Gets Secondary Camera to Move Around
        gameCamera = GameObject.FindGameObjectWithTag("GameCamera").GetComponent<Camera>();
        //gameCamera.transform.position = titleXYZ;
        settingsButton = GameObject.FindGameObjectWithTag("Settings");

        goTitle = false;
        goSaveSelect = false;

        Debug.Log("TitleAwake");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (goSaveSelect) 
        {
            //move to save select
            StartCoroutine(MoveCamera(titleXYZ, saveXYZ, transitionDuration));
            goSaveSelect = false; 
            //have to set bool here so it doesn't keep trying to start the couroutine over and over again
        }

        if (goTitle)
        {
            //move to title
            StartCoroutine(MoveCamera(saveXYZ, titleXYZ, transitionDuration));
            goTitle = false;
        }
    }

    public void OnClickPlay()
    {
        GameManager.Instance.OnToGame();
    }

    public void OnClickSettings()
    { 
        GameManager.Instance.OnClickSettings();
    }

    /// <summary>
    /// Disables the Title UI and Enables the Transition Over to the Save Select Screen.
    /// </summary>
    public void OnClickStart()
    {
        //move to save select
        TitleUI.SetActive(false);
        settingsButton.SetActive(false);
        goSaveSelect = true;
        Debug.Log("Start");
    }
    /// <summary>
    /// Disables the Save Select UI and Enables the Transition Over to the Title Screen.
    /// </summary>
    public void OnClickBack() 
    {
        //move to title
        SaveSelectUI.SetActive(false);
        settingsButton.SetActive(false);
        goTitle = true;
    }

    /*
     Code references:
     * Reply from StarManta: https://discussions.unity.com/t/moving-the-camera-smoothly/660515
     * /\ Adjusted with suggestion/rant from JoeStrout : https://discussions.unity.com/t/how-do-i-smoothly-move-my-camera-to-a-set-position/139207/5
     */
    /// <summary>
    /// Moves the Camera in a Direction Based on a Start and End Vector3.
    /// </summary>
    /// <param name="start">Vector3 of the Start Position</param>
    /// <param name="end">Vector3 of the End Position</param>
    /// <param name="duration">Length of Time to Transition (in Seconds)</param>
    /// <returns></returns>
    IEnumerator MoveCamera(Vector3 start, Vector3 end, float duration)
    {
        Debug.Log("Couroutine Start");

        for (float t = 0.0f; t < duration; t += Time.deltaTime)
        {
            gameCamera.transform.position = Vector3.MoveTowards(start, end, transitionSpeedScaler * (t / duration));
            //Debug.Log("Moving");
            yield return 0;
        }

        gameCamera.transform.position = end;
        //Debug.Log("Done Moving");
        //re enable ui
        if (end == titleXYZ) yield return EndTransition(TitleUI);
        else if (end == saveXYZ) yield return EndTransition(SaveSelectUI);

        Debug.Log("Couroutine Complete");

        yield break;
    }

    /// <summary>
    /// Enables the UI After the MoveCamera Couroutine is Finished.
    /// </summary>
    /// <param name="ui">The UI to Enable</param>
    /// <returns></returns>
    IEnumerator EndTransition(GameObject ui)
    {
        ui.SetActive(true);
        settingsButton.SetActive(true);
        yield return 0;

        //Debug.Log("Transition Complete");
    }
}
