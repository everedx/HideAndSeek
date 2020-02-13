using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{

    [SerializeField] Text textFinishLevel;
    [SerializeField] ScorePanel scorePanel;
    public int[] timeThresholdsScores;

    public void stopScene()
    {
        //Stop and update timer
        GetComponent<TimerScene>().TimerEnabled = false;
        textFinishLevel.text=GameObject.Find("Timer").GetComponent<Text>().text;
        setScore();
        //disable ingame GUI
        GetComponent<MenuController>().disableButtons();

        //STOP GAME
        GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>().MovementEnabled = false;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            EnemyChaser eChaser = enemy.GetComponent<EnemyChaser>();
            if (eChaser != null)
            {
                eChaser.MovementEnabled = false;
            }
        }
    }

    private void setScore()
    {
        int score = calculateScore();
        scorePanel.SetStars(score);
        
    }

    private int calculateScore()
    {
        string[] times = GameObject.Find("Timer").GetComponent<Text>().text.Split(':');
        int secondsScore = int.Parse(times[0]) * 60 + int.Parse(times[1]);
        for (int i = 0; i < timeThresholdsScores.Length; i++)
        {
            if (secondsScore < timeThresholdsScores[i])
                return timeThresholdsScores.Length+1-i;
        }
        if(secondsScore > timeThresholdsScores[timeThresholdsScores.Length-1])
            return timeThresholdsScores.Length;
        return 0;
    }
}
