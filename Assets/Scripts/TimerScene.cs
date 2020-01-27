using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerScene : MonoBehaviour
{
    GameObject timer;
    float timerInSecond;
    // Start is called before the first frame update
    void Start()
    {
        timer = GameObject.Find("Timer");
        timerInSecond = 0;
    }

    void Update()
    {
        timerInSecond += Time.deltaTime;
        int minutes = Mathf.FloorToInt(timerInSecond / 60f);
        int seconds = Mathf.FloorToInt(timerInSecond % 60f);
        int decSeconds = Mathf.FloorToInt((timerInSecond-60*minutes-seconds)*10f );
        string sMin=minutes.ToString();
        string sSec=seconds.ToString();
        if (minutes < 10)
            sMin = "0" + sMin;
        if (seconds < 10)
            sSec = "0" + sSec;
        timer.GetComponent<UnityEngine.UI.Text>().text = sMin + ":"+ sSec + ":" + decSeconds ;
    }
}
