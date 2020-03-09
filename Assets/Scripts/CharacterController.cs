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
    [SerializeField] private GameObject inventoryBar;
    [SerializeField] private GameObject actionAnimatorObject;
    

    private CharacterInventory charInv;
    private Animator anim;
    private bool movementEnabled;
    private Animator actionAnimator; 

    public bool MovementEnabled { get => movementEnabled; set => movementEnabled = value; }

    // private GameObject touch2
    //private GameObject threshold;

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
        charInv = new CharacterInventory(inventoryBar);
        anim = GetComponent<Animator>();
        actionAnimator = actionAnimatorObject.GetComponent<Animator>();
        movementEnabled = true;
        
    }


    private void Update()
    {
        transform.GetChild(2).transform.rotation = Quaternion.Euler(0.0f, 0.0f, gameObject.transform.rotation.z * -1.0f);
        transform.GetChild(2).transform.position = transform.position + new Vector3(0,3.29f,0);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        rbody.velocity = Vector2.zero;
        if (movementEnabled)
        {
            movement();
        }
        fingersPreviousFrame = Input.touchCount;
        if (actionAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            actionAnimator.enabled = false;
        }

    }

    private void movement()
    {
        Vector2 move = new Vector2(0,0);
        if (Input.touchCount == 0)
        {
            touch.transform.position = defaultJoysTickPos;
            threshold.transform.position = defaultJoysTickPos;
            anim.SetFloat("speed",0);
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
            anim.SetFloat("speed", speed);
            float angle = Vector2.SignedAngle(Vector2.up,move);
            transform.rotation = Quaternion.Euler(0,0,angle);
            rbody.MovePosition(origin);
            
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        

        if (collision.gameObject.tag == "Door")
        {
            if (charInv.keyExistInInventory(collision.gameObject.name))
            {
                //Debug.Log("Open");
                //collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                collision.gameObject.GetComponent<Animator>().SetTrigger("OpenDoor");
                charInv.removeKey(collision.gameObject.name);
                if (actionAnimator.enabled == false)
                {
                    actionAnimator.enabled = true;
                    actionAnimator.Play("OpenDoor");
                }
            }
            else
            {
                if (actionAnimator.enabled == false)
                {
                    actionAnimator.enabled = true;
                    actionAnimator.Play("LockedDoor");
                }
                
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Key")
        {
            //Add Key to inventory (Reference to the door that it opens)
            string doorToOpen = collision.gameObject.GetComponent<keyController>().getDoorThatThisKeyCanOpen();
            if(doorToOpen!= null)
                charInv.addKey(doorToOpen,collision.gameObject.GetComponent<SpriteRenderer>().sprite);
            else
                charInv.addKey(collision.gameObject.name, collision.gameObject.GetComponent<SpriteRenderer>().sprite);

            //Destroy Object
            Destroy(collision.gameObject);



        }
        if (collision.gameObject.tag == "Enemy")
        {
            
            

            GameObject levelChanger = GameObject.Find("LevelChanger");
            if (levelChanger != null)
            {
                m_OrthographicCamera.GetComponent<CameraShader>().changeEnemiesChasing(-50);
                Scene scene = SceneManager.GetActiveScene();
               
                GameObject.Find("GameController").GetComponent<SceneController>().stopScene(false);
                GameObject.Find("LostMenu").GetComponent<LostMenu>().showLostMenu();
            }

          
        }
        if (collision.gameObject.tag == "Finish")
        {
            GameObject levelChanger = GameObject.Find("LevelChanger");
            if (levelChanger != null)
            {
                m_OrthographicCamera.GetComponent<CameraShader>().changeEnemiesChasing(-50);
                GameObject.Find("GameController").GetComponent<SceneController>().stopScene(true);
                GameObject.Find("FinishMenu").GetComponent<FinishMenu>().showFinishMenu();
                
            }

     
        }

        if (collision.gameObject.tag == "TutorialSpot")
        {
            GameObject tutoController = GameObject.Find("TutorialController");
            if (tutoController != null)
            {
                tutoController.GetComponent<TutorialController>().spotTouched();

            }
            //Destroy Object
            Destroy(collision.gameObject);


        }

    }


}
