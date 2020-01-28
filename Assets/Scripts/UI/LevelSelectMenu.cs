using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectMenu : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    public void showLevelSelect()
    {
        canvas.enabled = true;
    }
    public void hideShowLevelSelect()
    {
        canvas.enabled = false;
    }
}
