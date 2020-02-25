using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keyController : MonoBehaviour
{

    [SerializeField] GameObject doorThatThisKeyCanOpen;

    public string getDoorThatThisKeyCanOpen()
    {
        if(doorThatThisKeyCanOpen !=null)
            return doorThatThisKeyCanOpen.name;
        else
            return null;
    }
}
