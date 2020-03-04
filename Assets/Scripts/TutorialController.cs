using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{

    [SerializeField] private string[] tutorialSteps;
    private int indexTuto;

    void Start()
    {
        indexTuto = 0;
    }

    void Update()
    {
        
    }
}
