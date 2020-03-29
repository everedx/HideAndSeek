using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Level10Animator : MonoBehaviour
{

    [SerializeField] Camera mainCam;
    [SerializeField] private Light2D light;
    [SerializeField] int cameraSizeMax;
    [SerializeField] int cameraSizeMin;
    [SerializeField] float lightMax;
    [SerializeField] float lightMin;
    [SerializeField] float sizeInterval;
    [SerializeField] float lightInterval;
    private bool growing;
    private bool animEnded;
    private bool animStart;
    private bool finishAll;
    // Start is called before the first frame update
    void Start()
    {
        growing = true;
        animEnded = false;
        animStart = false;
        finishAll = false;
        //10 to 70
        //0 to 0.5
    }

    // Update is called once per frame
    void Update()
    {
        if (!animEnded && animStart)
        {
            if (growing)
            {
                if (mainCam.orthographicSize < cameraSizeMax)
                {
                    mainCam.orthographicSize += sizeInterval;
                }
                else
                {
                    growing = false;
                }
                if (light.intensity < lightMax)
                {
                    light.intensity += lightInterval;
                }
                else
                {
                    growing = false;
                }

            }
            else
            {
                if (mainCam.orthographicSize > cameraSizeMin)
                {
                    mainCam.orthographicSize -= sizeInterval;
                }
                else
                {
                    animEnded = true;
                }
                if (light.intensity > lightMin)
                {
                    light.intensity -= lightInterval;
                }
                else
                {
                    animEnded = true;
                }
            }
        }
        else if(animEnded)
        {

            if (!finishAll)
            {
                finishAll = true;
                light.intensity = lightMin;
                mainCam.orthographicSize = cameraSizeMin;
                GameObject.Find("GameController").GetComponent<SceneController>().resumeScene();
            }
           
        }

    }

    public void startLastLevelAnim()
    {
        animStart = true;
    }
}
