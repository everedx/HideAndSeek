using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour
{
    public Canvas cAlert;
    public float angleOfVision = 110.0f;
    public float distanceOfVision = 10.0f;
    private float angleOfDetection;
    private bool seen;
    EnemyChaser eChaser;
    CameraShader mainCamShader;
    // Start is called before the first frame update
    void Start()
    {
        eChaser = GetComponent<EnemyChaser>();
        mainCamShader = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShader>();
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector3 enemyPos = transform.position;
        Vector3 heroPostion = GameObject.FindGameObjectWithTag("Player").transform.position;

        if (Vector2.Distance(enemyPos, heroPostion) < distanceOfVision)
        {
            angleOfDetection = getAngle(GameObject.FindGameObjectWithTag("Player"));
            //Debug.Log(angleOfDetection);
            if (Mathf.Abs(angleOfDetection) < angleOfVision / 2)
            {
                //Debug.Log("Visible");
                RaycastHit2D hit = Physics2D.Raycast(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position - transform.position, distanceOfVision);
                if (hit.collider != null && hit.collider.tag == "Player")
                {
                    Debug.Log("DETECTED");
                    //eChaser.State = 1;
                    cAlert.enabled = true;
                    if (seen == false)
                        mainCamShader.changeEnemiesChasing(1);
                    seen = true;
                }
            }
            else
            {
                cAlert.enabled = false;
                // Debug.Log("No visible");
                if(seen)
                    mainCamShader.changeEnemiesChasing(-1);
                seen = false;
            }
        }
        else
        {
            cAlert.enabled = false;
            //Debug.Log("No visible");
            if (seen)
                mainCamShader.changeEnemiesChasing(-1);
            seen = false;
        }

    }

    float getAngle(GameObject hero)
    {
        Vector2 dirVector = Vector2.zero;
        switch (eChaser.DirToGo)
        {
            case 0:
                dirVector = transform.up;
                break;
            case 1:
                dirVector = transform.right;
                break;
            case 2:
                dirVector = -transform.right;
                break;
            case 3:
                dirVector = -transform.up;
                break;
        }
        Vector2 vectorToCompare = hero.transform.position-transform.position ;
        return Vector2.SignedAngle(vectorToCompare,dirVector);
    }




}
