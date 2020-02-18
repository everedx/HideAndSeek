using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{

    public void onAnimationFinish()
    {
        GameObject.Find("GameController").GetComponent<SceneController>().resumeScene();
        gameObject.GetComponent<Animator>().enabled = false;
    }
}
