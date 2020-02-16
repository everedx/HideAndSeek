using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    Mesh mesh;
    float fov;
    Vector3 origin;
    int rayCount;
    float angle;
    float angleIncrease;
    float viewDistance;
    private float startingAngle;
    [SerializeField] private float adjustingSpeedInc;
    [SerializeField] private float adjustingSpeedMax;
    private float adjustingSpeed;
    [SerializeField] private float adjustingOffset;
    [SerializeField] private GameObject lightFOV;
    private bool increasing;


    GameObject spawner;
    EnemyChaser eChaser;
   // CameraShader mainCamShader;

    private bool seenFinal;
    private bool seenTemp;

    // Start is called before the first frame update
    void Awake()
    {
        adjustingSpeed = 0;
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        origin = Vector3.zero;

        seenFinal = false;
        //mainCamShader = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShader>();
        // adjustingSpeedMax = 3;
        // adjustingSpeedInc = 0.1f;
        increasing = true;

    }
    void Update(){

        fov = 90f;
        rayCount = 50;
        angle = startingAngle;
        angleIncrease = fov / rayCount;
        viewDistance = 20f;


        Vector3[] vertices = new Vector3[rayCount+2];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount*3];

        vertices[0] = origin;

        int vertexIndex = 1;
        int trianglesIndex = 0;
        seenTemp = false;
        for (int i = 0; i<= rayCount; i++)
        {
            Vector3 vertex;

            RaycastHit2D rayCastHit2D = Physics2D.Raycast(origin, getVectorFromAngle(angle), viewDistance,LayerMask.GetMask("Walls","Player","Doors"));
            if (rayCastHit2D.collider == null)
            {
                vertex = origin + getVectorFromAngle(angle) * viewDistance;
            }
            else
            {
                vertex = rayCastHit2D.point;
                if (rayCastHit2D.transform.tag == "Player")
                {
                    seenTemp = true;
                }
            }
            //vertex = origin + getVectorFromAngle(angle) * viewDistance;
            vertices[vertexIndex] = vertex;

            if (i > 0)
            {
                triangles[trianglesIndex] = 0;
                triangles[trianglesIndex+1] = vertexIndex -1;
                triangles[trianglesIndex+2] = vertexIndex;

                trianglesIndex += 3;
            }

            vertexIndex++;
            angle -= angleIncrease;
        }
 
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;



        mesh.bounds = new Bounds(origin,Vector3.one*1000);

        if (seenTemp == true)
        {
            eChaser.State = EnemyChaser.States.Chasing;
            eChaser.playerDetected = true;
            //if (seenFinal == false)
            //     mainCamShader.changeEnemiesChasing(1);
            seenFinal = true;
            eChaser.WorldPosPlayer = GameObject.FindGameObjectWithTag("Player").transform.position;
        }
        else
        {
            //eChaser.State = 0;
            eChaser.playerDetected = false;
            //if (seenFinal == true)
            //    mainCamShader.changeEnemiesChasing(-1);
            seenFinal = false;
        }
    }


    public void setOrigin(Vector3 origin)
    {
        this.origin = origin;
        lightFOV.transform.position = this.origin;
    }

    public void setAimDirection(Vector3 aimDir)
    {
        startingAngle = getAngleFromVector(aimDir) - fov/2f;
        adjustingSpeed = 0;
        //Debug.Log(startingAngle);
        lightFOV.transform.eulerAngles = new Vector3(0, 0, startingAngle + fov / 2f+180);
    }

    public float getAimDirection()
    {
        return startingAngle;
        //Debug.Log(startingAngle);
    }

    public void waveAimDirection(Vector3 aimDir)
    {
        float ang = (getAngleFromVector(aimDir) - fov / 2f);
        if (ang > startingAngle + adjustingOffset)
        {
            if (adjustingSpeed < adjustingSpeedMax)
                adjustingSpeed += adjustingSpeedInc;
            startingAngle = startingAngle + adjustingSpeed * Time.deltaTime * 100;
            increasing = true;
        }
        else if (ang <= startingAngle - adjustingOffset)
        {
            if (adjustingSpeed > -adjustingSpeedMax)
                adjustingSpeed -= adjustingSpeedInc;
            startingAngle = startingAngle + adjustingSpeed * Time.deltaTime * 100;
            increasing = false;
        }
        else
        {
            if (adjustingSpeed <= 0)
                adjustingSpeed = -adjustingSpeedMax;
            else
                adjustingSpeed = adjustingSpeedMax;
            startingAngle = startingAngle + adjustingSpeed * Time.deltaTime*100;
        }
        //startingAngle = startingAngle + adjustingSpeed;
        //startingAngle = getAngleFromVector(aimDir) - fov / 2f;
        //Debug.Log(startingAngle);
        lightFOV.transform.eulerAngles = new Vector3(0, 0, startingAngle + fov / 2f + 180);
    }

    public void setSpawner(GameObject spawner)
    {
        this.spawner = spawner;
        eChaser = spawner.GetComponent<EnemyChaser>();
    }


    public Vector3 getVectorFromAngle(float angle) 
    {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad),Mathf.Sin(angleRad));
    }


    private float getAngleFromVector(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }
}
