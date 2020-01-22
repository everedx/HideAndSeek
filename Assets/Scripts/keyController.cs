using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keyController : MonoBehaviour
{

    [SerializeField] GameObject doorThatThisKeyCanOpen;

    public string getDoorThatThisKeyCanOpen()
    {
        return doorThatThisKeyCanOpen.name;
    }
}
