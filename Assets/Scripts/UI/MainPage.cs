using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPage : MonoBehaviour
{

    [SerializeField] Canvas canvas;
    public void showTitleScreen()
    {
        canvas.enabled = true;
    }
    public void hideTitleScreen()
    {
        canvas.enabled = false;
    }
}
