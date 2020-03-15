using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    private BoxCollider2D collider;

    [SerializeField] AudioClip openDoorClip;
    [SerializeField] AudioClip lockedDoorClip;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
    }

    public void disableCollider()
    {
        collider.enabled = false;
    }

    public void soundLocked()
    {
        audioSource.PlayOneShot(lockedDoorClip);
    }
    public void soundOpen()
    {
        audioSource.PlayOneShot(openDoorClip);
    }
}
