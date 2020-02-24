using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class InterstitialAd : MonoBehaviour
{
    string gameId = "912c1774-5d66-4b20-840a-f5f47b69aa32";
    [SerializeField] bool testMode;
    private string levelToLoad;
    void Start()
    {
        Advertisement.Initialize(gameId, testMode);
        levelToLoad = null;
    }

    public void showAd(string ltl)
    {
        levelToLoad = ltl;
        if (!ltl.Equals("Menu"))
        {
            ShowOptions showOptions = new ShowOptions();
            showOptions.resultCallback = result =>
            {
                changeLevel();
            };
            if (Advertisement.IsReady())
                Advertisement.Show(showOptions);
        }
        else
        {
            changeLevel();
        }
   
    }

    private void changeLevel()
    {
        SceneManager.LoadScene(levelToLoad);
    }
}
