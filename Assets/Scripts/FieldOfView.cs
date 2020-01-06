﻿using System.Collections;
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

    GameObject spawner;
    EnemyChaser eChaser;
    CameraShader mainCamShader;

    private bool seenFinal;
    private bool seenTemp;

    // Start is called before the first frame update
    void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        origin = Vector3.zero;

        seenFinal = false;
        mainCamShader = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShader>();

    }
    void Update(){

        fov = 90f;
        rayCount = 100;
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

            RaycastHit2D rayCastHit2D = Physics2D.Raycast(origin, getVectorFromAngle(angle), viewDistance);
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

        if (seenTemp == true)
        {
            eChaser.State = 1;
            if (seenFinal == false)
                 mainCamShader.changeEnemiesChasing(1);
             seenFinal = true;
        }
        else
        {
            eChaser.State = 0;
            if (seenFinal == true)
                mainCamShader.changeEnemiesChasing(-1);
            seenFinal = false;
        }
    }


    public void setOrigin(Vector3 origin)
    {
        this.origin = origin;
    }

    public void setAimDirection(Vector3 aimDir)
    {
        startingAngle = getAngleFromVector(aimDir) - fov/2f;
        //Debug.Log(startingAngle);
    }


    public void setSpawner(GameObject spawner)
    {
        this.spawner = spawner;
        eChaser = spawner.GetComponent<EnemyChaser>();
    }


    private Vector3 getVectorFromAngle(float angle) 
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
