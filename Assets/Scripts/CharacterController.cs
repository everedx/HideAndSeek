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
    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        reductionFactor = m_OrthographicCamera.orthographicSize / 2;
    }



    // Update is called once per frame
    void Update()
    {
        rbody.velocity = Vector2.zero;
        // Handle screen touches.
        if (Input.touchCount > 0)
        {
            Vector2 origin = transform.position;
            Touch touch = Input.GetTouch(0);
            Vector2 destination = m_OrthographicCamera.ScreenToWorldPoint(touch.position);
            
            //IF DISTANCE > reductionFactor, THEN SPEED = MAX, else the speed varies
            float xDistance = transform.position.x - destination.x; //If positive, we need to go left
            float yDistance = transform.position.y - destination.y; //If positive we need to go down

            xDistance = -xDistance / reductionFactor;
            yDistance = -yDistance / reductionFactor;
            //Debug.Log("X:"+ xDistance + " Y:"+ yDistance);
            if (Mathf.Abs(xDistance) > 1)
            {
                xDistance = (xDistance > 0) ?  1 : -1;
            }
            if (Mathf.Abs(yDistance) > 1)
            {
                yDistance = (yDistance > 0) ? 1 : -1;
            }
           // Debug.Log("X:" + xDistance + " Y:" + yDistance);
            Vector2 move = new Vector2(xDistance,yDistance);

            origin = origin + move * speed * Time.deltaTime;

            rbody.MovePosition(origin);
            //m_OrthographicCamera.ScreenToWorldPoint
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Scene scene = SceneManager.GetActiveScene(); 
            SceneManager.LoadScene(scene.name);
        }
    }
}
