using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{

    [SerializeField] private string[] tutorialSteps;
    [SerializeField] private string[] tutorialInstructions;
    [SerializeField] private Vector3[] tutorialCameraSequences;
    [SerializeField] private GameObject guard1;
    [SerializeField] private GameObject guard2;
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject key1;
    [SerializeField] private GameObject keyExit;
    [SerializeField] private GameObject spot1;
    [SerializeField] private GameObject spot2;
    [SerializeField] private GameObject vJoystickTuto1;
    [SerializeField] private GameObject vJoystickTuto2;
    [SerializeField] private GameObject cameraTutorial;
    [SerializeField] private Text textComponentInstructions;

    private int indexTuto;
    private float timerTextDisplay;
    private bool showText;
    private char[] stringToDisplay;
    private int indexDisplayText;
    private bool tutoActive;
    private SceneController sceneController;
    void Start()
    {
        indexTuto = 0;
        timerTextDisplay = 0;
        indexDisplayText = 0;
        showText = false;
        tutoActive = false;
        sceneController = GameObject.Find("GameController").GetComponent<SceneController>();
    }

    void Update()
    {
        if (tutoActive)
        {
            //Start with index tuto



            if (showText)
                displayTutoText();
        }
        
    }

    private void setTextToDisplayTuto(int tutoIndex)
    {
        string stringToDisplayTemp = tutorialInstructions[tutoIndex];
        indexDisplayText = 0;
        showText = true;
        stringToDisplay = stringToDisplayTemp.ToCharArray();
    }

    private void displayTutoText()
    {

    }

    public void startTutorialSequence()
    {
        tutoActive = true;
    }


}
