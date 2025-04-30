using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InputManager : MonoBehaviour
{
    public bool fullScreen = true;
    public bool showFrameRate = true;
    public bool showStats = false;
    public bool activSelect = true;
    public TMP_Text frameRateText;
    public TMP_Text numberOfAsteroids;
    public TMP_Text numberOfMajorBodies;
    public Efficiency efficiency;
    public GameObject select;
    IEnumerator FramesPerSecond()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            frameRateText.text = Mathf.Ceil(Time.frameCount / Time.time).ToString();
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            fullScreen = !fullScreen;
            Screen.fullScreen = fullScreen;
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            showFrameRate = !showFrameRate;
            frameRateText.gameObject.SetActive(showFrameRate);
            StartCoroutine(FramesPerSecond());
        }
        if(Input.GetKeyDown(KeyCode.F3))
        {
            showStats = !showStats;
            numberOfAsteroids.gameObject.SetActive(showStats);
            numberOfMajorBodies.gameObject.SetActive(showStats);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit(0);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            activSelect = !activSelect;
            select.gameObject.SetActive(activSelect);
        }
        numberOfAsteroids.text = efficiency.asteroidNumber.ToString();
        numberOfMajorBodies.text = efficiency.majorPlanets.ToString();
    }
}
