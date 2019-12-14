using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyChaser : MonoBehaviour
{

    Rigidbody2D rBody;
    Vector2 iniPosition;
    public float distanceToPatrol = 20.0f;
    public float speed = 5.0f;
    public float speedWhileChasing = 10.0f;
    float angle;
    private enum Directions
    {
        Up = 0,
        Right = 1,
        Left = 2,
        Down = 3
    };
    System.Random rnd;

    private int dirToGo;
    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
        iniPosition = rBody.position;
        rnd = new System.Random();
        dirToGo = rnd.Next(0, 4);
        //Debug.Log(dirToGo);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 lookDir = new Vector2(0, 0);
        Vector2 origin = transform.position;
        Vector2 move = new Vector2(0, 0);
        Vector2 leftOffset = new Vector2(0,0);
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
        RaycastHit2D hit = Physics2D.Raycast(rBody.position, lookDir, 2f );
        RaycastHit2D hitLeft = Physics2D.Raycast(rBody.position +leftOffset , lookDir, 2f);
        RaycastHit2D hitRight = Physics2D.Raycast(rBody.position + rightOffset , lookDir, 2f);
        if (hit.collider != null || hitLeft.collider != null || hitRight.collider != null)
        {
            //Change dir
            dirToGo = GiveMeANumber(dirToGo,0,4);
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
