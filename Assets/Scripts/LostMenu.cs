﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostMenu : MonoBehaviour
{
    // Start is called before the first frame update
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void showLostMenu()
    {
        animator.SetTrigger("ShowLostMenu");
    }

}
