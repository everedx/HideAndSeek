using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    private bool gamePaused;
    [SerializeField] Button playPauseButton;
    [SerializeField] Sprite pauseImg;
    [SerializeField] Sprite playImg;
    [SerializeField] Button stopButton;
    [SerializeField] Text pauseText;
    // Start is called before the first frame update
    void Start()
    {
        gamePaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void pauseGame()
    {
        Time.timeScale = 0;
        playPauseButton.GetComponent<Image>().sprite = playImg;
        stopButton.enabled = true;
        stopButton.GetComponent<Image>().enabled = true;
        pauseText.enabled = true;
        gamePaused = true;
    }

    public void unPauseGame()
    {
        Time.timeScale = 1;
        playPauseButton.GetComponent<Image>().sprite = pauseImg;
        stopButton.enabled = false;
        stopButton.GetComponent<Image>().enabled = false;
        pauseText.enabled = false;
        gamePaused = false;
    }

    public void stopPressed()
    {
        Time.timeScale = 1;
    }

    public void playPausePressed()
    {
        if (gamePaused)
            unPauseGame();
        else
            pauseGame();
    }

    public void disableButtons()
    {
        playPauseButton.enabled = false;
        stopButton.enabled = false;
    }
    public void enableButtons()
    {
        playPauseButton.enabled = true;
        stopButton.enabled = true;
    }


}
