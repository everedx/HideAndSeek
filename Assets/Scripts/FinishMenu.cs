using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinishMenu : MonoBehaviour
{
    // Start is called before the first frame update
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void showFinishMenu()
    {
        animator.SetTrigger("showFinishMenu");
     
    }
    public void hideNextIfLast()
    {
        string path = SceneUtility.GetScenePathByBuildIndex(SceneManager.GetActiveScene().buildIndex + 1);
        if (string.IsNullOrEmpty(path))
            GameObject.Find("ButtonNext").GetComponent<Button>().enabled = false;
    }

}
