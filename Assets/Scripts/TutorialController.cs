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
    [SerializeField] private Button tutoButton;

    [SerializeField] private Camera cam;
    private AudioSource audioSource;
    private Vector3 iniCamPos;
    private Vector3 iniGuard1pos;
    private Vector3 iniGuard2pos;
    private int indexTuto;
    private float timerTextDisplay;
    private bool showText;
    private char[] stringToDisplay;
    private int indexDisplayText;
    private bool tutoActive;
    private SceneController sceneController;
    private bool startNextStep;
    private bool continueNextStep;
    private float timerToWait;
    
    void Start()
    {
        indexTuto = 0;
        timerTextDisplay = 0;
        indexDisplayText = 0;
        showText = false;
        tutoActive = false;
        sceneController = GameObject.Find("GameController").GetComponent<SceneController>();
        startNextStep = true;
        continueNextStep = false;
        timerToWait = 0.1f;
        audioSource = GetComponent<AudioSource>();
        iniCamPos = cam.transform.position;
        iniGuard1pos = guard1.transform.position;
        iniGuard2pos = guard2.transform.position;
    }

    void LateUpdate()
    {
        if (tutoActive)
        {
            
            if (startNextStep)
            {
                continueNextStep = true;
                setTextToDisplayTuto(indexTuto);
            }
            if (continueNextStep)
                prepareStep(indexTuto);



            if (showText)
                displayTutoText();
        }
        
    }

    private void setTextToDisplayTuto(int tutoIndex)
    {
        sceneController.stopScene(false);
        startNextStep = false;
        string stringToDisplayTemp = tutorialInstructions[tutoIndex];
        indexDisplayText = 0;
        showText = true;
        //tutoButton.enabled = true;
        stringToDisplay = stringToDisplayTemp.ToCharArray();
        if (tutoIndex == 0)
        {
            vJoystickTuto1.SetActive(true);
            vJoystickTuto2.SetActive(true);
            cam.transform.position = iniCamPos;
        }
        else
        {
            cam.transform.position = tutorialCameraSequences[tutoIndex];
        }
    }

    private void displayTutoText()
    {
        
        textComponentInstructions.enabled = true;
        timerTextDisplay += Time.deltaTime;
        if (timerTextDisplay > timerToWait && stringToDisplay.Length > indexDisplayText)
        {
            timerTextDisplay = 0;
            textComponentInstructions.text = textComponentInstructions.text + stringToDisplay[indexDisplayText];
            audioSource.Play();
            if (stringToDisplay[indexDisplayText].Equals(',') || stringToDisplay[indexDisplayText].Equals('.') || stringToDisplay[indexDisplayText].Equals('!'))
            {
                timerToWait = 1f;
            }
            else
            {
                timerToWait = 0.1f;
            }
            indexDisplayText++;
        }
        else if(stringToDisplay.Length >= indexDisplayText)
        {
            tutoButton.gameObject.SetActive(true);
        }
    }

    private void prepareStep(int indexStep)
    {
        continueNextStep = false;
        switch (tutorialSteps[indexStep])
        {
            case "Move1":
                spot1.SetActive(true);
                break;
            case "Move2":
                spot2.SetActive(true);
                break;
            case "DoorFail":
                door.SetActive(true);
            break;
            case "DoorKey":
                key1.SetActive(true);
                break;
            case "Guard1":
                guard1.SetActive(true);
                break;
            case "Guard2":
                guard2.SetActive(true);
                break;
            case "Escape":
                guard1.GetComponent<EnemyChaser>().enableFOV();
                //guard2.GetComponent<EnemyChaser>().enableFOV();
                guard1.SetActive(true);
                //guard2.SetActive(true);
                guard1.GetComponent<EnemyChaser>().setState(0);
                //guard2.GetComponent<EnemyChaser>().setState(0);
                cam.GetComponent<CameraShader>().setEnemiesChasing(0);
                keyExit.SetActive(true);
                break;
            default:
                Debug.Log("Tutorial Not Implemented");
                break;
        }
    }

    private void finishStep()
    {

        switch (tutorialSteps[indexTuto])
        {
            case "Move1":
                spot1.SetActive(false);
                indexTuto++;
                startNextStep = true;
                continueNextStep = false;
                break;
            case "Move2":
                spot2.SetActive(false);
                indexTuto++;
                startNextStep = true;
                continueNextStep = false;
                break;
            case "DoorFail":
                indexTuto++;
                startNextStep = true;
                continueNextStep = false;
                break;
            case "DoorKey":
                indexTuto++;
                startNextStep = true;
                continueNextStep = false;
                break;
            case "Guard1":
                guard1.GetComponent<EnemyChaser>().setState(0);
                guard1.SetActive(false);
                guard1.transform.position = iniGuard1pos;
                guard1.GetComponent<EnemyChaser>().disableFOV();
                cam.GetComponent<CameraShader>().setEnemiesChasing(0);
                
                indexTuto++;
                startNextStep = true;
                continueNextStep = false;
                break;
            case "Guard2":
                guard2.GetComponent<EnemyChaser>().setState(0);
                guard2.SetActive(false);
                guard2.transform.position = iniGuard2pos;
                guard2.GetComponent<EnemyChaser>().disableFOV();
                cam.GetComponent<CameraShader>().setEnemiesChasing(0);
                
                indexTuto++;
                startNextStep = true;
                continueNextStep = false;
                break;
            default:
                Debug.Log("Tutorial Not Implemented");
                break;
        }
    }

    public void startTutorialSequence()
    {
        tutoActive = true;
        Debug.Log("Tutorial Active");
    }

    public void startStep()
    {
        GameObject.Find("GameController").GetComponent<SceneController>().resumeScene();
        tutoButton.gameObject.SetActive(false);
        vJoystickTuto1.SetActive(false);
        vJoystickTuto2.SetActive(false);
        textComponentInstructions.enabled = false;
        textComponentInstructions.text = "";
        showText = false;
    }

    public void finishStepInterface()
    {
        finishStep();
    }

    public string currentStep()
    {
        return tutorialSteps[indexTuto];
    }



}
