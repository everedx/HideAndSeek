using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyChaser : MonoBehaviour
{

    Rigidbody2D rBody;
    Vector2 iniPosition;
    CameraShader mainCamShader;
    private float adjRotationParam;

    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float speedWhileChasing = 6.5f;
    private int minTimePatrol, maxTimePatrol, timePatrol;
    private float countChangeDirection;
    private bool timePatrolSet;
    float angle;
    private bool staticEnemy;
    [SerializeField] private Directions staticSightDirection = Directions.Up;
    Vector2 staticViewVector;
    bool setStaticSight;
    bool initialState = true;

    //Chasing mode variables
    GameObject pathFinderObject;
    Grid grd;
    public bool playerDetected;
    BreadCrumb bc;
    bool needToChangePoint;
    bool firstStep;
    Vector2 nextPoint;
    private bool forceNext;
    Point PlayerPrevPos;
    private Vector2 worldPosPlayer;
    private States prevState;

    //Patrol mode variables
    [SerializeField] private float patrolXDistance;
    [SerializeField] private float patrolYDistance;
    [SerializeField] private bool menuMode;
    private Vector2 patrolDestination;
    private Vector2 patrolDepartingPosition;
    private bool patrolGoingToDestination;
    Vector2 prevVector;
    Vector2 currentVector;
    private GameObject fovObject;


    //ComeBack mode
    [SerializeField] private int secondsToGoBackToPatrol;
    private float secondsToGoBackToPatrolCounter;



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
        ComingBack = 2,
        LookingForPlayer = 3
    }

    System.Random rnd;



    public int DirToGo { get => dirToGo; }
    private int dirToGo;
    public States State { get => state; set => state = value; }
    private bool movementEnabled;
    public bool MovementEnabled { get => movementEnabled; set => movementEnabled = value; }

    private States state;
    // Start is called before the first frame update
    void Start()
    {
        movementEnabled = true;
        initialState = true;
        //countChasePath = 0;
        secondsToGoBackToPatrolCounter = 0;
        forceNext = false;
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
        if (!menuMode)
            PlayerPrevPos = grd.GetClosestGoodNode(grd.Nodes, GameObject.FindGameObjectWithTag("Player").transform.position);
        patrolDepartingPosition = transform.position;
        patrolDestination = new Vector2(transform.position.x + patrolXDistance, transform.position.y + patrolYDistance);
        patrolGoingToDestination = true;
        mainCamShader = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShader>();
        prevState = States.Patrolling;
        if (patrolXDistance == 0 && patrolYDistance == 0)
            staticEnemy = true;
        else
            staticEnemy = false;
        switch (staticSightDirection)
        {
            case Directions.Up:
                staticViewVector = Vector2.up;
                break;
            case Directions.Down:
                staticViewVector = Vector2.down;
                break;
            case Directions.Right:
                staticViewVector = Vector2.right;
                break;
            case Directions.Left:
                staticViewVector = Vector2.left;
                break;
        }
        setStaticSight = true;
        adjRotationParam = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        if (movementEnabled)
        {
            switch (state)
            {
                case States.Patrolling:
                    if (prevState != States.Patrolling || initialState)
                    {
                        gameObject.GetComponent<EnemySight>().setAngle(staticViewVector, true);
                    }
                    if (staticEnemy)
                    {
                        gameObject.GetComponent<EnemySight>().setAngle(staticViewVector, false);
                        adjustRotation(staticViewVector);
                    }
                    patrol();
                    prevState = state;
                    break;
                case States.Chasing:
                    if (prevState == States.Patrolling || prevState == States.ComingBack)
                        mainCamShader.changeEnemiesChasing(1);
                    prevState = state;
                    chase();
                    break;
                case States.ComingBack:
                    if (prevState == States.LookingForPlayer)
                        mainCamShader.changeEnemiesChasing(-1);
                    prevState = state;
                    comeBack();
                    break;
                case States.LookingForPlayer:
                    prevState = state;
                    lookForPlayerRandomly();
                    break;
            }
        }



        initialState = false;
    }

    private void patrol()
    {


        Vector2 origin = transform.position;
        Point gridDestination;
        Point gridPosThisEnemy = grd.GetClosestGoodNode(grd.Nodes, transform.position);

        if (patrolGoingToDestination)
        {
            gridDestination = grd.GetClosestGoodNode(grd.Nodes, patrolDestination);
        }
        else
        {
            gridDestination = grd.GetClosestGoodNode(grd.Nodes, patrolDepartingPosition);
        }




        if (bc == null)
        {
            patrolGoingToDestination = !patrolGoingToDestination;
            bc = PathFinder.FindPath(grd, gridPosThisEnemy, gridDestination);
            needToChangePoint = true;
            firstStep = true;
        }
        else
        {
            firstStep = false;
            needToChangePoint = false;
        }


        if (bc != null)
        {
            if (needToChangePoint || forceNext)
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


            //LineRenderer lr = gameObject.GetComponent<LineRenderer>();
            //lr.positionCount = 2;  //Need a higher number than 2, or crashes out
            //lr.startWidth = 0.1f;
            //lr.endWidth = 0.1f;
            //lr.startColor = Color.yellow;
            //lr.endColor = Color.yellow;
            //lr.SetPosition(0, transform.position);
            //lr.SetPosition(1, new Vector2(nextPoint.x, nextPoint.y));

            float xDistance = transform.position.x - nextPoint.x; //If positive, we need to go left
            float yDistance = transform.position.y - nextPoint.y; //If positive we need to go down

            if (Mathf.Abs(xDistance) <= 0.5f && Mathf.Abs(yDistance) <= 0.5f)
            {
                xDistance = (xDistance > 0) ? 1 : -1;
                yDistance = (yDistance > 0) ? 1 : -1;
            }
            else
            {
                if (Mathf.Abs(xDistance) > 0.5f)
                    xDistance = (xDistance > 0) ? 1 : -1;
                else
                    xDistance = 0;
                if (Mathf.Abs(yDistance) > 0.5f)
                    yDistance = (yDistance > 0) ? 1 : -1;
                else
                    yDistance = 0;
            }



            Vector2 move = new Vector2(-xDistance, -yDistance);
            currentVector = move;


            origin = origin + move * speed * Time.deltaTime;
            adjustRotation(move);
            rBody.MovePosition(origin);
            if (!staticEnemy)
            {
                if (firstStep)
                {
                    gameObject.GetComponent<EnemySight>().setAngle(move, true);
                }
                else
                {
                    gameObject.GetComponent<EnemySight>().setAngle(move, false);
                }
            }
            Point enemylocation = grd.GetClosestGoodNode(grd.Nodes, transform.position);
            Point nextPointLocation = grd.GetClosestGoodNode(grd.Nodes, new Vector2(nextPoint.x, nextPoint.y));


            if ((transform.position.x >= nextPoint.x - 0.5f && transform.position.x <= nextPoint.x + 0.5f) && (transform.position.y >= nextPoint.y - 0.5f && transform.position.y <= nextPoint.y + 0.5f))
            {
                forceNext = true;
            }
            else
                forceNext = false;
            prevVector = currentVector;
        }
        else
        {
            forceNext = false;
        }



    }

    private void lookForPlayerRandomly()
    {
        secondsToGoBackToPatrolCounter += Time.deltaTime;
        if (secondsToGoBackToPatrolCounter > secondsToGoBackToPatrol)
        {
            //change MODE
            state = States.ComingBack;
            return;
        }
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
        origin = origin + move * speed / 2 * Time.deltaTime;
        adjustRotation(move);
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
        PlayerPrevPos = grd.GetClosestGoodNode(grd.Nodes, GameObject.FindGameObjectWithTag("Player").transform.position);
        Point gridPosPlayer = grd.GetClosestGoodNode(grd.Nodes, worldPosPlayer);
        Point gridPosThisEnemy = grd.GetClosestGoodNode(grd.Nodes, gameObject.transform.position);
        if (gridPosPlayer.X == gridPosThisEnemy.X && gridPosPlayer.Y == gridPosThisEnemy.Y)
        {
            state = States.LookingForPlayer;
            secondsToGoBackToPatrolCounter = 0;
            return;
        }
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


        if (bc != null)
        {
            if (needToChangePoint || forceNext)
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


            //LineRenderer lr = gameObject.GetComponent<LineRenderer>();
            //lr.positionCount = 2;  //Need a higher number than 2, or crashes out
            //lr.startWidth = 0.1f;
            //lr.endWidth = 0.1f;
            //lr.startColor = Color.yellow;
            //lr.endColor = Color.yellow;
            //lr.SetPosition(0, transform.position);
            //lr.SetPosition(1, new Vector2(nextPoint.x,nextPoint.y));

            float xDistance = transform.position.x - nextPoint.x; //If positive, we need to go left
            float yDistance = transform.position.y - nextPoint.y; //If positive we need to go down

            if (Mathf.Abs(xDistance) <= 0.5f && Mathf.Abs(yDistance) <= 0.5f)
            {
                xDistance = (xDistance > 0) ? 1 : -1;
                yDistance = (yDistance > 0) ? 1 : -1;
            }
            else
            {
                if (Mathf.Abs(xDistance) > 0.5f)
                    xDistance = (xDistance > 0) ? 1 : -1;
                else
                    xDistance = 0;
                if (Mathf.Abs(yDistance) > 0.5f)
                    yDistance = (yDistance > 0) ? 1 : -1;
                else
                    yDistance = 0;
            }



            Vector2 move = new Vector2(-xDistance, -yDistance);
            adjustRotation(move);
            origin = origin + move * speedWhileChasing * Time.deltaTime;
            rBody.MovePosition(origin);
            if ((transform.position.x >= nextPoint.x - 0.01f || transform.position.x <= nextPoint.x + 0.01f) && (transform.position.y >= nextPoint.y - 0.01f || transform.position.y <= nextPoint.y + 0.01f))
            {
                forceNext = true;
            }
            else
                forceNext = false;
        }
        else
        {

            Vector2 move = new Vector2(0, -1);
            adjustRotation(move);
            origin = origin + move * speedWhileChasing * Time.deltaTime;
            rBody.MovePosition(origin);
            forceNext = false;
        }


    }

    private void comeBack()
    {
        Vector2 origin = transform.position;
        Point gridDestination = grd.GetClosestGoodNode(grd.Nodes, patrolDepartingPosition);
        Point gridPosThisEnemy = grd.GetClosestGoodNode(grd.Nodes, transform.position);





        if (bc == null)
        {

            bc = PathFinder.FindPath(grd, gridPosThisEnemy, gridDestination);
            needToChangePoint = true;
            firstStep = true;
        }
        else
        {
            firstStep = false;
            needToChangePoint = false;
        }


        if (bc != null)
        {
            if (needToChangePoint || forceNext)
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

            if (bc == null)
            {
                state = States.Patrolling;
                return;
            }


            float xDistance = transform.position.x - nextPoint.x; //If positive, we need to go left
            float yDistance = transform.position.y - nextPoint.y; //If positive we need to go down

            if (Mathf.Abs(xDistance) <= 0.5f && Mathf.Abs(yDistance) <= 0.5f)
            {
                xDistance = (xDistance > 0) ? 1 : -1;
                yDistance = (yDistance > 0) ? 1 : -1;
            }
            else
            {
                if (Mathf.Abs(xDistance) > 0.5f)
                    xDistance = (xDistance > 0) ? 1 : -1;
                else
                    xDistance = 0;
                if (Mathf.Abs(yDistance) > 0.5f)
                    yDistance = (yDistance > 0) ? 1 : -1;
                else
                    yDistance = 0;
            }


            Vector2 move = new Vector2(-xDistance, -yDistance);
            currentVector = move;

            adjustRotation(move);
            origin = origin + move * speed * Time.deltaTime;
            rBody.MovePosition(origin);
            if (firstStep)
            {
                gameObject.GetComponent<EnemySight>().setAngle(move, true);
            }
            else
            {
                gameObject.GetComponent<EnemySight>().setAngle(move, false);
            }

            Point enemylocation = grd.GetClosestGoodNode(grd.Nodes, transform.position);
            Point nextPointLocation = grd.GetClosestGoodNode(grd.Nodes, new Vector2(nextPoint.x, nextPoint.y));


            if ((transform.position.x >= nextPoint.x - 0.5f && transform.position.x <= nextPoint.x + 0.5f) && (transform.position.y >= nextPoint.y - 0.5f && transform.position.y <= nextPoint.y + 0.5f))
            {
                forceNext = true;
            }
            else
                forceNext = false;
            prevVector = currentVector;

        }
        else
        {
            forceNext = false;
        }
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

    private int GiveMeANumber(int exclusion, int minim, int maxim)
    {
        var exclude = new HashSet<int>() { exclusion };
        var range = Enumerable.Range(minim, maxim).Where(i => !exclude.Contains(i));

        var rand = new System.Random();
        int index = rand.Next(minim, maxim - exclude.Count);
        return range.ElementAt(index);
    }

    //Find the shortest path to reach the target angle
    private void adjustRotation(Vector2 move)
    {
        float angleRot = Vector2.SignedAngle(Vector2.up, move);
        float currentAngle = transform.eulerAngles.z;
        //Debug.Log("Angulo:" + currentAngle + "  Angulo2:" + angleRot );
        float newAngle;
        newAngle = currentAngle + 360;
        float alpha = angleRot - currentAngle;
        float betta = angleRot - currentAngle + 360;
        float gamma = angleRot - currentAngle - 360;
        float smallestAbsolute;
        if (Mathf.Abs(alpha) < Mathf.Abs(betta))
        {
            smallestAbsolute = alpha;
            if (Mathf.Abs(gamma) < Mathf.Abs(alpha))
                smallestAbsolute = gamma;
        }
        else
        {
            smallestAbsolute = betta;
            if (Mathf.Abs(gamma) < Mathf.Abs(betta))
                smallestAbsolute = gamma;
        }


        if (angleRot < 0)
            angleRot += 360;
        if (Mathf.Abs(currentAngle - angleRot) >5 )
        {
            if (smallestAbsolute > 0)
                //clockwise
                newAngle = currentAngle + adjRotationParam;
            else
                //counterclockwise
                newAngle = currentAngle - adjRotationParam;

            Quaternion qua = Quaternion.Euler(0, 0, newAngle);
            //Debug.Log(qua);
            transform.rotation = qua;
        }

        
    }


    public void setState(int state)
    {
        prevState = (States)state;
        this.state = (States)state;
    }

    public void setFOVObject(GameObject fov)
    {
        fovObject = fov;
    }

    public void disableFOV()
    {
        fovObject.SetActive(false);
    }
    public void enableFOV()
    {
        fovObject.SetActive(true);
    }
}
