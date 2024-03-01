/*
    Code created by Oliver Fiedot-Davies, 2024
    For Maynooth University Computer Science and Software Engineering Final Year Project.

    Vector3 docs https://docs.unity3d.com/ScriptReference/Vector3.html
    CheckSphere code taken from https://docs.unity3d.com/ScriptReference/Physics.CheckSphere.html
    Gizmos code taken from https://docs.unity3d.com/ScriptReference/Gizmos.DrawWireCube.html
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    //Public values set via Unity Editor
    public Vector2 gridSize; //(40,30)
    public float nodeRadius; //0.5
    public LayerMask obstacleMask;
    //public Transform player;

    //Internal script values
    int gridX;
    int gridY;
    float nodeDiameter;

    //2D node array
    Node [,] gridArray;

    Vector3 gridOrigin;


    //Calculating node count for grid, spawns grid
    void Start()
    {
        gridX = (int)(gridSize.x);
        gridY =  (int)(gridSize.y);
        nodeDiameter = nodeRadius * 2;

        SpawnGrid();
    }

    //Creates grid (node array) while checking if traversable
    void SpawnGrid()
    {
        //Calculates position of grid's bottom left corner (0,0)
        Vector3 halfOfGrid = new Vector3(gridSize.x / 2, 0, gridSize.y / 2);
        gridOrigin = transform.position - halfOfGrid;

        //Declares array for grid nodes
        gridArray = new Node[gridX, gridY];

        //Loops through each node
        for (int x = 0; x < gridX; x++)
        {
            for (int y = 0; y < gridY; y++)
            {
                //Sets centre of each node as currentPos --------------GRAVITY GLITCH IS IN HERE SOMEWHERE--------------
                Vector3 currentPos = gridOrigin + Vector3.right * (nodeDiameter * x + nodeRadius) + Vector3.forward * (nodeDiameter * y - nodeRadius);
                
                //Checks if node overlaps with obstacleMask based on current position and radius
                bool traversable = !Physics.CheckSphere(currentPos, nodeRadius, obstacleMask);

                //Adds node to grid array
                gridArray[x,y] = new Node(traversable, currentPos);
            }
        }
    }

    //Gets nearest node to player or target
    public Node PlayerNearestNode(Vector3 currentPos)
    {
        //Rounds current position to nearest int
        int x = Mathf.RoundToInt((currentPos.x - gridOrigin.x) / nodeDiameter - nodeRadius);
        int y = Mathf.RoundToInt((currentPos.z - gridOrigin.z) / nodeDiameter - nodeRadius);

        //Clamps to avoid IndexOutOfBounds
        x = Mathf.Clamp(x, 0, gridX-1);
        y = Mathf.Clamp(y, 0, gridY-1);
            
        return gridArray[x, y];
    }

    //Gets list of neighbouring nodes
    public List<Node> GetNbr(Node node)
    {
        List<Node> nbr = new List<Node>();

        //Offset for World Origin
        Vector3 gridOffset = (Vector3.forward* gridOrigin.z)+(Vector3.right * gridOrigin.x);
        Vector3 nodePos = node.worldPos-gridOffset;

        //Debug.Log((nodePos).ToString()+"worldPos");
        //Debug.Log(gridOrigin + "gridOrigin");


        //Checking for traversable neighbours loop
        for (int x = -1; x <= 1; x++)
        {
            for(int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }
                int _x = Mathf.Clamp((int)nodePos.x + x, 0, gridX - 1);
                int _y = Mathf.Clamp((int)nodePos.z + y, 0, gridY - 1);


                //Prevents adding nodes outside of the grid
                if (_x < gridX && _x >= 0 && _y < gridY && _y >= 0) 
                {
                    //Debug.Log("X: " + _x + " Y: " + _y);
                    nbr.Add(gridArray[_x,_y]);
                }
            }
        }

        return nbr;

/*
        Debug.Log(nodePos.ToString());
        Node test = gridArray[1,1];
        neighbours.Add(test);
        float testPos = node.worldPos.x;
        return neighbours;*/
    }



    public List<Node> path;

    //Using Wire Cube Gizmo for visualisation
    void OnDrawGizmos()
    {
        //Defense against NullReferenceException
        if (gridArray != null)
        {
            foreach (Node node in gridArray)
            {
                Gizmos.color = (node.traversable) ? Color.white : Color.red;
                if (path != null)
                {
                    if (path.Contains(node))
                    {
                        //Debug.Log("attempt to path");
                        Gizmos.color = Color.black;
                    }

                }
                Gizmos.DrawCube(node.worldPos, Vector3.one * (nodeDiameter - .1f));
            }
        }
    }

    /*void OnDrawGizmos()
    {
        //Defense against NullReferenceException
        if (gridArray != null)
        {
            Node currentNode = PlayerNearestNode(player.position);
            foreach (Node node in gridArray)
            {
                //Paints nodes green if traversable, otherwise red
                Gizmos.color = Color.green;
                if (!node.traversable)
                {
                    Gizmos.color = Color.red;
                }
                if (currentNode == node)
                {
                    Gizmos.color = Color.magenta;
                    Gizmos.DrawCube(node.worldPos, Vector3.one * 0.90f);
                }
                if (path != null)
                {
                    if (path.Contains(node))
                    {
                        Gizmos.color = Color.black;
                    }
                }
                else
                    Gizmos.DrawWireCube(node.worldPos, Vector3.one * 0.90f);
            }
        }
    }*/

}
