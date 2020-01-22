using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterController : MonoBehaviour
{
    public Camera m_OrthographicCamera;
    Rigidbody2D rbody;
    public float speed = 5.0f;
    private float reductionFactor = 5.0f;
    private int fingersPreviousFrame;
    private Vector3 positionJoysTick;
    private Vector2 defaultJoysTickPos;
    private float maxOffset;
    [SerializeField] private GameObject touch;
    [SerializeField] private GameObject threshold;
    private CharacterInventory charInv;
   // private GameObject touch2
    //private GameObject threshold;
    private
    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        reductionFactor = m_OrthographicCamera.orthographicSize / 2;
        fingersPreviousFrame = 0;
        defaultJoysTickPos = new Vector2(-1000,-1000);
        maxOffset = 5;
        touch= Instantiate(touch, defaultJoysTickPos, new Quaternion(0,0,0,0),m_OrthographicCamera.transform);
        threshold= Instantiate(threshold, defaultJoysTickPos, new Quaternion(0, 0, 0, 0),m_OrthographicCamera.transform);
        positionJoysTick = new Vector3(0,0,0);
        charInv = new CharacterInventory();
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        rbody.velocity = Vector2.zero;
      
        movement();
        fingersPreviousFrame = Input.touchCount;
    }

    private void movement()
    {
        Vector2 move = new Vector2(0,0);
        if (Input.touchCount == 0)
        {
            touch.transform.position = defaultJoysTickPos;
            threshold.transform.position = defaultJoysTickPos;
        }

        if (Input.touchCount > 0 && fingersPreviousFrame == 0)
        {
            Touch fingerTouch = Input.GetTouch(0);
            positionJoysTick.x = m_OrthographicCamera.ScreenToWorldPoint(fingerTouch.position).x;
            positionJoysTick.y= m_OrthographicCamera.ScreenToWorldPoint(fingerTouch.position).y;
            touch.transform.position = positionJoysTick;
            threshold.transform.position = positionJoysTick;
        }

        if (Input.touchCount > 0 && fingersPreviousFrame != 0)
        {
 

            Vector2 origin = transform.position;
            Vector3 posTouch= new Vector3(0,0,0);
            Touch fingerTouch = Input.GetTouch(0);
            Vector2 fingerTouchWorld = m_OrthographicCamera.ScreenToWorldPoint(fingerTouch.position);
            positionJoysTick = threshold.transform.position;
            if (Vector2.Distance(fingerTouchWorld, positionJoysTick) > maxOffset)
            {
                // Debug.Log("FAR");
                move.Set(fingerTouchWorld.x - positionJoysTick.x, fingerTouchWorld.y - positionJoysTick.y);
                move = Vector2.ClampMagnitude(move,maxOffset);
                posTouch.x = positionJoysTick.x+ move.x;
                posTouch.y = positionJoysTick.y+move.y;
                touch.transform.position = posTouch;
            }
            else
            {
                posTouch.x = fingerTouchWorld.x;
                posTouch.y = fingerTouchWorld.y;
                touch.transform.position = posTouch;
                move.Set(fingerTouchWorld.x-positionJoysTick.x, fingerTouchWorld.y - positionJoysTick.y);
            }
            move = move / maxOffset;
            origin = origin + move * speed * Time.deltaTime;

            rbody.MovePosition(origin);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Scene scene = SceneManager.GetActiveScene(); 
            SceneManager.LoadScene(scene.name);
        }

        if (collision.gameObject.tag == "Door")
        {
            if (charInv.keyExistInInventory(collision.gameObject.name))
            {
                //Debug.Log("Open");
                collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                charInv.removeKey(collision.gameObject.name);
            }
            else
            {
                //Debug.Log("Closed");
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Key")
        {
            //Add Key to inventory (Reference to the door that it opens)
            charInv.addKey(collision.gameObject.GetComponent<keyController>().getDoorThatThisKeyCanOpen());

            //Destroy Object
            Destroy(collision.gameObject);

        }
    }


}
