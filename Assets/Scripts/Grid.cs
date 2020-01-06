using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Direction
{
	Right,
	Left,
	Top,
	Bottom,
	BottomLeft,
	BottomRight,
	TopLeft,
	TopRight,  
}

public class Grid : MonoBehaviour {

    public Vector2 fromPosition;

    [SerializeField] private int Width;
    [SerializeField] private int Height;
	
	public Node[,] Nodes;
	
	public int Left { get { return 0; } }
	public int Right { get { return Width; } }
	public int Bottom { get { return 0; } }
	public int Top { get { return Height; } }

	public const float UnitSize = 3f;

	private LineRenderer LineRenderer;
	GameObject Player;

	void Awake () 
	{	
		Player = GameObject.Find ("Player");
		LineRenderer = transform.GetComponent<LineRenderer>();

		//Get grid dimensions
		fromPosition = this.transform.position;


        //Vertexes
        //Width = serializableField
        //Height = serializableField

        Nodes = new Node[2 * Height + 1+2, 2 * Width+2];

        //Initialize the grid nodes - 1 grid unit between each node
        //We render the grid in a diamond pattern
        //      int x = 0, y = 0;
        //for (int xi=(int)Offset.x ; xi < Width/2; xi=xi+(int)UnitSize)
        //{
        //	for (int yi =(int)Offset.y; yi < Height; yi=yi+(int)UnitSize)
        //	{
        //		float ptx = xi;
        //              float pty = -(yi/ 2) + (UnitSize/2f);
        //		int offsetx = 0;

        //		if (yi % 2 == 0)
        //		{
        //			ptx = xi + (UnitSize/2f);
        //			offsetx = 1;
        //		}	
        //		else
        //		{
        //			pty = -(yi/2);
        //		}

        //		Vector2 pos = new Vector2(ptx, pty);
        //		Node node = new Node(xi*2 + offsetx, yi, pos, this);
        //		Nodes[x*2 + offsetx, y] = node;
        //              y++;
        //	}
        //          x++;
        //}
        int matrixIndexY = 0, matrixIndexX = 0;
        for (int x =0; x < Height; x = x + 1)
        {
            matrixIndexX = 0;
            for (int y = 0; y < Width/2; y = y + 1)
            {

                float ptx, pty;
                int offsetx = 0;
                if (y % 2 == 0)
                {
                    pty = (int)fromPosition.y - (y) * (UnitSize / 2f);
                    ptx = (int)fromPosition.x+ x*UnitSize + (UnitSize / 2f);
                    offsetx = 1;
                }
                else
                {
                    ptx = (int)fromPosition.x + x * UnitSize;
                    pty = (int)fromPosition.y - (y) * (UnitSize / 2f); //- (UnitSize*2 );
                }
                Vector2 pos = new Vector2(ptx, pty);
                Node node = new Node(x*2 + offsetx, y, pos, this);
                Nodes[x * 2 + offsetx, y] = node;
                matrixIndexX++;
            }
            matrixIndexY++;
        }

        //Create connections between each node
        for (int xi = 0; xi < Height*2+1; xi++)
        {
            for (int yi = 0; yi < Width/2; yi++)
            {
                if (Nodes[xi, yi] == null) continue;
                Nodes[xi, yi].InitializeConnections(this);
            }
        }

        //Pass 1, we removed the bad nodes, based on valid connections
        for (int xi = 0; xi < Height * 2 + 1; xi++)
        {
            for (int yi = 0; yi < Width / 2; yi++)
            {
                if (Nodes[xi, yi] == null)
                    continue;

                Nodes[xi, yi].CheckConnectionsPass1(this);
            }
        }

        //Pass 2, remove bad connections based on bad nodes
        for (int xi = 0; xi < Height * 2 + 1; xi++)
        {
            for (int yi = 0; yi < Width / 2; yi++)
            {
                if (Nodes[xi, yi] == null)
                    continue;

                Nodes[xi, yi].CheckConnectionsPass2();
                //Nodes[xi, yi].DrawConnections ();	//debug
            }
        }
    }
    public Point GetClosestNode(Node[,] nodes, Vector2 pos)
    {
        Node tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = pos;
        //foreach (Node[] tArray in nodes)
        //{
            foreach (Node t in nodes)
            {

                if (t == null)
                    continue;
                float dist = Vector3.Distance(t.Position, currentPos);
                if (dist < minDist)
                {
                    tMin = t;
                    minDist = dist;
                }
            }
       //}
        return new Point(tMin.X,tMin.Y);
    }

    public Point WorldToGrid(Vector2 worldPosition)
	{
		Vector2 gridPosition = new Vector2((worldPosition.x * 2f), -(worldPosition.y * 2f) + 1);

		//adjust to our nearest integer
		float rx = gridPosition.x % 1;
		if (rx < 0.5f)
			gridPosition.x = gridPosition.x - rx;
		else
			gridPosition.x = gridPosition.x + (1 - rx);
		
		float ry = gridPosition.y % 1;
		if (ry < 0.5f)
			gridPosition.y = gridPosition.y - ry;
		else
			gridPosition.y = gridPosition.y + (1 - ry);
				
		int x = (int)gridPosition.x;
		int y = (int)gridPosition.y;

		if (x < 0 || y < 0 || x > Width || y > Height)
			return null;

		Node node = Nodes [x, y];
		//We calculated a spot between nodes'
		//Find nearest neighbor
		if((node == null) ||  (x % 2 == 0 && y % 2 == 0) || (gridPosition.y % 2 == 1 && gridPosition.x % 2 == 1))
		{   
			float mag = 100;


			if (x < Width && !Nodes[x + 1, y].BadNode)
			{
				float mag1 = (Nodes[x + 1, y].Position - worldPosition).magnitude;
				if (mag1 < mag)
				{
					mag = mag1;
					node = Nodes[x + 1, y];
				}
			}
			if (y < Height - 1 && !Nodes[x, y + 1].BadNode)
			{
				float mag1 = (Nodes[x, y+ 1].Position - worldPosition).magnitude;
				if (mag1 < mag)
				{
					mag = mag1;
					node = Nodes[x, y + 1];
				}
			}
			if (x > 0 && !Nodes[x- 1, y].BadNode)
			{
				float mag1 = (Nodes[x - 1, y].Position - worldPosition).magnitude;
				if (mag1 < mag)
				{
					mag = mag1;
					node = Nodes[x-1, y];
				}
			}
			if (y > 0 && !Nodes[x, y - 1].BadNode)
			{
				float mag1 = (Nodes[x, y - 1].Position - worldPosition).magnitude;
				if (mag1 < mag)
				{
					mag = mag1;
					node = Nodes[x, y -1+ 1];
				}
			}
		}
		return new Point(node.X , node.Y);
	}

	public Vector2 GridToWorld(Point gridPosition)
	{
        

       Node  world = Nodes[gridPosition.X, gridPosition.Y];

		return world.Position;
	}
	
	public bool ConnectionIsValid(Point point1, Point point2)
	{
		//comparing same point, return false
		if (point1.X == point2.X && point1.Y == point2.Y)
			return false;
		
		if (Nodes [point1.X, point1.Y] == null)
			return false;
		
		//determine direction from point1 to point2
		Direction direction = Direction.Bottom;

		if (point1.X == point2.X)
		{
			if (point1.Y < point2.Y)
				direction = Direction.Bottom;
			else if (point1.Y > point2.Y)
				direction = Direction.Top;
		}
		else if (point1.Y == point2.Y)
		{
			if (point1.X < point2.X)
				direction = Direction.Right;
			else if (point1.X > point2.X)
				direction = Direction.Left;
		}
		else if (point1.X < point2.X)
		{
			if (point1.Y > point2.Y)
				direction = Direction.TopRight;
			else if (point1.Y < point2.Y)
				direction = Direction.BottomRight;
		}
		else if (point1.X > point2.X)
		{
			if (point1.Y > point2.Y)
				direction = Direction.TopLeft;
			else if (point1.Y < point2.Y)
				direction = Direction.BottomLeft;
		}

		//check connection
		switch (direction)
		{
			case Direction.Bottom:
			if (Nodes[point1.X, point1.Y].Bottom != null)
				return Nodes[point1.X, point1.Y].Bottom.Valid;
			else
				return false;

			case Direction.Top:
			if (Nodes[point1.X, point1.Y].Top != null)
				return Nodes[point1.X, point1.Y].Top.Valid;
			else
				return false;
		
			case Direction.Right:
			if (Nodes[point1.X, point1.Y].Right != null)
				return Nodes[point1.X, point1.Y].Right.Valid;
			else
				return false;

			case Direction.Left:
			if (Nodes[point1.X, point1.Y].Left != null)
				return Nodes[point1.X, point1.Y].Left.Valid;
			else
				return false;
		
			case Direction.BottomLeft:
			if (Nodes[point1.X, point1.Y].BottomLeft != null)
				return Nodes[point1.X, point1.Y].BottomLeft.Valid;
			else
				return false;

			case Direction.BottomRight:
			if (Nodes[point1.X, point1.Y].BottomRight != null)
				return Nodes[point1.X, point1.Y].BottomRight.Valid;
			else
				return false;
		
			case Direction.TopLeft:
			if (Nodes[point1.X, point1.Y].TopLeft != null)
				return Nodes[point1.X, point1.Y].TopLeft.Valid;
			else
				return false;
		
			case Direction.TopRight:
			if (Nodes[point1.X, point1.Y].TopRight != null)
				return Nodes[point1.X, point1.Y].TopRight.Valid;
			else
				return false;
		
			default:
				return false;
		}		
	}


	void Update()
	{
		//Pathfinding demo
		if(Input.GetMouseButtonDown(0))
		{
			//Convert mouse click point to grid coordinates
			Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Point gridPos = GetClosestNode(Nodes,new Vector2(worldPos.x,worldPos.y) );

			if (gridPos != null) {						
				
				//if (gridPos.X > 0 && gridPos.Y > 0 && gridPos.X < Width && gridPos.Y < Height) {

					//Convert player point to grid coordinates
					Point playerPos = GetClosestNode (Nodes, GameObject.FindGameObjectWithTag("Player").transform.position);					
					Nodes[playerPos.X, playerPos.Y].SetColor(Color.blue);

					//Find path from player to clicked position
					BreadCrumb bc = PathFinder.FindPath (this, playerPos, gridPos);

					int count = 0;		
					LineRenderer lr = GameObject.FindGameObjectWithTag("Player").GetComponent<LineRenderer> ();
					lr.positionCount =100;  //Need a higher number than 2, or crashes out
                    lr.startWidth = 0.1f;
                    lr.endWidth = 0.1f;
                    lr.startColor = Color.yellow;
                    lr.endColor = Color.yellow;

                    //Draw out our path
                    while (bc != null) {					
						lr.SetPosition(count, GridToWorld(bc.position));
						bc = bc.next;
						count += 1;
					}
					lr.positionCount = count;					
				//}				
			}
		}
	}

}



