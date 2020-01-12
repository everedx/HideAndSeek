﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyChaser : MonoBehaviour
{

    Rigidbody2D rBody;
    Vector2 iniPosition;
    public float distanceToPatrol = 20.0f;
    public float speed = 5.0f;
    public float speedWhileChasing = 6.5f;
    private int minTimePatrol, maxTimePatrol, timePatrol;
    private float countChangeDirection;
    private bool timePatrolSet;
    float angle;

    //Chasing mode variables
    GameObject pathFinderObject; 
    Grid grd; 
    public bool playerDetected;
    BreadCrumb bc;
    bool needToChangePoint;
    bool firstStep;
    Vector2 nextPoint;
    private float countChasePath;
    Point PlayerPrevPos;
    private Vector2 worldPosPlayer;

    public Vector2 WorldPosPlayer { get => worldPosPlayer; set => worldPosPlayer = value; }
    private enum Directions
    {
        Up = 0,
        Right = 1,
        Left = 2,
        Down = 3
    };
    public enum States
    {
        Patrolling = 0,
        Chasing = 1,
        ComingBack=2,
        LookingForPlayer=3
    }

    System.Random rnd;



    public int DirToGo { get => dirToGo; }
    private int dirToGo;
    public States State { get => state; set => state = value; }
   

    private States state;
    // Start is called before the first frame update
    void Start()
    {
        countChasePath = 0;
        rBody = GetComponent<Rigidbody2D>();
        iniPosition = rBody.position;
        rnd = new System.Random();
        dirToGo = rnd.Next(0, 4);
        State = 0;
        countChangeDirection = 0f;
        minTimePatrol = 2;
        maxTimePatrol = 5;
        timePatrolSet = false;
        playerDetected = false;
        pathFinderObject = GameObject.FindGameObjectWithTag("Pathfinder");
        grd = pathFinderObject.GetComponent<Grid>().Instance;
        needToChangePoint = false;
        //Debug.Log(dirToGo);
        PlayerPrevPos = grd.GetClosestNode(grd.Nodes, GameObject.FindGameObjectWithTag("Player").transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case States.Patrolling:
                patrol();
                break;
            case States.Chasing:
                chase();
                break;
            case States.ComingBack:
                break;
            case States.LookingForPlayer:
                break;
        }
        
        

    }

    private void patrol()
    {
        countChangeDirection += Time.deltaTime;
        rBody.velocity = Vector2.zero;
        Vector2 lookDir = new Vector2(0, 0);
        Vector2 origin = transform.position;
        Vector2 move = new Vector2(0, 0);
        Vector2 leftOffset = new Vector2(0, 0);
        Vector2 rightOffset = new Vector2(0, 0);
        // Vector2 lookDirDrw = new Vector2(0, 0);
        switch (dirToGo)
        {
            case (int)Directions.Up:
                move.y = move.y + 1;
                lookDir.y = 1;
                leftOffset.x = -1;
                rightOffset.x = 1;
                angle = 0;
                break;
            case (int)Directions.Right:
                move.x = move.x + 1;
                lookDir.x = 1;
                leftOffset.y = 1;
                rightOffset.y = -1;
                angle = 90;
                break;
            case (int)Directions.Left:
                move.x = move.x - 1;
                lookDir.x = -1;
                leftOffset.y = -1;
                rightOffset.y = 1;
                angle = 270;
                break;
            case (int)Directions.Down:
                move.y = move.y - 1;
                lookDir.y = -1;
                leftOffset.x = 1;
                rightOffset.x = -1;
                angle = 180;
                break;
        }
        origin = origin + move * speed * Time.deltaTime;
        rBody.MovePosition(origin);
        //transform.rotation = Quaternion.Euler(0, 0, angle); 
        //Debug.DrawRay(rBody.position, lookDir, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(rBody.position, lookDir, 2f);
        RaycastHit2D hitLeft = Physics2D.Raycast(rBody.position + leftOffset, lookDir, 2f);
        RaycastHit2D hitRight = Physics2D.Raycast(rBody.position + rightOffset, lookDir, 2f);
        if (hit.collider != null || hitLeft.collider != null || hitRight.collider != null)
        {
            //Change dir
            dirToGo = GiveMeANumber(dirToGo, 0, 4);
        }
        if (!timePatrolSet)
        {
            timePatrol = GiveMeANumber(maxTimePatrol + 2, minTimePatrol, maxTimePatrol + 1);
            timePatrolSet = true;
        }
        if (timePatrolSet && timePatrol < countChangeDirection)
        {
            dirToGo = GiveMeANumber(dirToGo, 0, 4);
            timePatrolSet = false;
            countChangeDirection = 0;
        }

    }

    private void chase()
    {
        Vector2 origin = transform.position;
        //// countChasePath += Time.deltaTime;
        // if (playerDetected ) //If the enemy sees the player, follows him updating the place where he is
        // {

        //     playerDetected = true;
        //     Point gridPosPlayer = grd.GetClosestNode(grd.Nodes, GameObject.FindGameObjectWithTag("Player").transform.position);
        //     Point gridPosThisEnemy = grd.GetClosestNode(grd.Nodes, gameObject.transform.position);
        //     worldPosPlayer = grd.GridToWorld(gridPosPlayer);
        //     if (!(PlayerPrevPos.X == gridPosPlayer.X && PlayerPrevPos.Y == gridPosPlayer.Y) || bc == null)
        //     {
        //         bc = PathFinder.FindPath(grd, gridPosThisEnemy, gridPosPlayer);
        //         needToChangePoint = true;
        //     }
        //     else
        //         needToChangePoint = false;

        //     //countChasePath = 0;
        // }
        // else
        //     needToChangePoint = false;


        // //Check if we are toching next point, if we do, we need to get the next point (Done in the OnCollisionEnter2D)
        // if(bc!=null)
        //     if (Vector2.Distance(transform.position, grd.GridToWorld(bc.position)) < Grid.UnitSize)
        //     {
        //         needToChangePoint = true;
        //     }

        // //Get next point if we already got there
        // if (bc != null && needToChangePoint)
        // {
        //     nextPoint = grd.GridToWorld(bc.position);
        //     bc = bc.next;

        // }

        // //move in the direction of the nextPoint
        // Vector2 origin = transform.position;
        // if (transform.position.x == nextPoint.x && transform.position.y == nextPoint.y)
        // {
        //     nextPoint = grd.GridToWorld(bc.position);
        //     bc = bc.next;
        // }
        // float xDistance = transform.position.x - nextPoint.x; //If positive, we need to go left
        // float yDistance = transform.position.y - nextPoint.y; //If positive we need to go down


        // xDistance = (xDistance > 0) ? 1 : -1;
        // yDistance = (yDistance > 0) ? 1 : -1;


        // Vector2 move = new Vector2(-xDistance, -yDistance);
        // origin = origin + move *speedWhileChasing* Time.deltaTime;
        // rBody.MovePosition(origin);
        // PlayerPrevPos = grd.GetClosestNode(grd.Nodes, GameObject.FindGameObjectWithTag("Player").transform.position);

        Point gridPosPlayer = grd.GetClosestNode(grd.Nodes, worldPosPlayer);
        Point gridPosThisEnemy = grd.GetClosestNode(grd.Nodes, gameObject.transform.position);
        if (!(PlayerPrevPos.X == gridPosPlayer.X && PlayerPrevPos.Y == gridPosPlayer.Y) || bc == null)
        {
            bc = PathFinder.FindPath(grd, gridPosThisEnemy, gridPosPlayer);
            needToChangePoint = true;
            firstStep = true;
        }
        else
        {
            firstStep = false;
            needToChangePoint = false;
        }
        if (needToChangePoint)
        {
            if (bc != null)
            {
                nextPoint = grd.GridToWorld(bc.position);
                bc = bc.next;
                if (firstStep)
                {
                    nextPoint = grd.GridToWorld(bc.position);
                    bc = bc.next;
                }
            }
        }

        Debug.Log(nextPoint.x + " " + nextPoint.y);


        float xDistance = transform.position.x - nextPoint.x; //If positive, we need to go left
        float yDistance = transform.position.y - nextPoint.y; //If positive we need to go down


        xDistance = (xDistance > 0) ? 1 : -1;
        yDistance = (yDistance > 0) ? 1 : -1;


        Vector2 move = new Vector2(-xDistance, -yDistance);
        origin = origin + move * speedWhileChasing * Time.deltaTime;
        rBody.MovePosition(origin);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.otherCollider.tag == "Node")
        {
            if (collision.transform.position.x == nextPoint.x && collision.transform.position.y == nextPoint.y)
            {
                needToChangePoint = true;
            }
        }
    }

    private int GiveMeANumber(int exclusion,int minim, int maxim)
    {
        var exclude = new HashSet<int>() { exclusion };
        var range = Enumerable.Range(minim, maxim).Where(i => !exclude.Contains(i));

        var rand = new System.Random();
        int index = rand.Next(minim, maxim - exclude.Count);
        return range.ElementAt(index);
    }
}
