using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public float limitLeft;
    public float limitRight;
    public float limitUp;
    public float limitDown;
    public Camera m_OrthographicCamera;
    // Start is called before the first frame update
    GameObject heroObject;
    private Vector3 velocity = Vector3.zero;
    public float smoothTime = 0.3f;
    void Start()
    {
        heroObject = GameObject.FindGameObjectWithTag("Player");
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 goalPos = heroObject.transform.position;
        //goalPos.y = transform.position.y;
        goalPos.z = -10;

        if (goalPos.x > limitRight)
            goalPos.x = limitRight;
        if (goalPos.x < limitLeft)
            goalPos.x = limitLeft;
        if (goalPos.y > limitUp)
            goalPos.y = limitUp;
        if (goalPos.y < limitDown)
            goalPos.y = limitDown;

        transform.position = Vector3.SmoothDamp(transform.position, goalPos, ref velocity, smoothTime);
    }
}
