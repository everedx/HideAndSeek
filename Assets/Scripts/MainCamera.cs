using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCamera : MonoBehaviour
{
    [SerializeField] Button buttonSkipAnimation;
    public void onAnimationFinish()
    {
        GameObject.Find("GameController").GetComponent<SceneController>().resumeScene();
        gameObject.GetComponent<Animator>().enabled = false;
        buttonSkipAnimation.gameObject.SetActive(false);
    }


}
