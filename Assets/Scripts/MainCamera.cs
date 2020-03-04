using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCamera : MonoBehaviour
{
    [SerializeField] Button buttonSkipAnimation;
    [SerializeField] Vector3[] startSequencePositions;
    private bool stopSequence;
    private Vector3 velocity = Vector3.zero;
    private int sequenceIndex= 0;
    private bool mustExecuteSequence;
    private bool nextRequired;


    public void onAnimationFinish()
    {
        GameObject.Find("GameController").GetComponent<SceneController>().resumeScene();
        gameObject.GetComponent<Animator>().enabled = false;
        buttonSkipAnimation.gameObject.SetActive(false);
        stopSequence = true;
        mustExecuteSequence = false;
    }

    private void Start()
    {
        stopSequence = false;
        mustExecuteSequence = false;
        nextRequired = false;
    }

    private void Update()
    {
       
        if (mustExecuteSequence)
        {
            executeStartSequence();
            //nextRequired = false;
            //if (transform.position.x == startSequencePositions[sequenceIndex].x && transform.position.y == startSequencePositions[sequenceIndex].y)
            if (Math.Abs(transform.position.x - startSequencePositions[sequenceIndex].x) < 0.1 && Math.Abs(transform.position.y - startSequencePositions[sequenceIndex].y) < 0.1)
            {
                sequenceIndex++;
                nextRequired = true;
                if (sequenceIndex >= startSequencePositions.Length)
                {
                    mustExecuteSequence = false;
                    onAnimationFinish();
                }
            }
        }

        
        
    }

    public void executeStartSequence()
    {
        transform.position = Vector3.SmoothDamp(transform.position, startSequencePositions[sequenceIndex], ref velocity, 1,10);
    }

    public void startCameraSequence()
    {
        mustExecuteSequence = true;
        nextRequired = true;
    }

}
