using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keyController : MonoBehaviour
{

    [SerializeField] GameObject doorThatThisKeyCanOpen;
    [SerializeField] AudioClip pickUpSound;

 
    public string getDoorThatThisKeyCanOpen()
    {
        if(doorThatThisKeyCanOpen !=null)
            return doorThatThisKeyCanOpen.name;
        else
            return null;
    }

    public AudioClip soundPickUp()
    {
        return pickUpSound;
    }
}
