/*
    Code created by Oliver Fiedot-Davies, 2024
    For Maynooth University Computer Science and Software Engineering Final Year Project.

    Vector3 docs https://docs.unity3d.com/ScriptReference/Vector3.html
    CheckSphere code taken from https://docs.unity3d.com/ScriptReference/Physics.CheckSphere.html
    Gizmos code taken from https://docs.unity3d.com/ScriptReference/Gizmos.DrawWireCube.html
*/

using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    //Public values set via Unity Editor
    public Vector2 gridSize; //(40,30)
    public float nodeRadius; //0.5
    public LayerMask obstacleMask;

    //Internal script values
    int gridX;
    int gridY;
    float nodeDiameter;

    //2D node array
    Node[,] gridArray;

    Vector3 gridOrigin;


    //Calculating node count for grid, spawns grid
    void Start()
    {
        gridX = (int)(gridSize.x);
        gridY = (int)(gridSize.y);
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
                //Sets centre of each node as currentPos
                Vector3 currentPos = gridOrigin + Vector3.right * (nodeDiameter * x + nodeRadius) + Vector3.forward * (nodeDiameter * y + nodeRadius);

                //Checks if node overlaps with obstacleMask based on current position and radius
                bool traversable = !Physics.CheckSphere(currentPos, nodeRadius, obstacleMask);

                //Adds node to grid array
                gridArray[x, y] = new Node(traversable, currentPos);
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
        x = Mathf.Clamp(x, 0, gridX - 1);
        y = Mathf.Clamp(y, 0, gridY - 1);

        return gridArray[x, y];
    }

    //Gets list of neighbouring nodes
    public List<Node> getNbr(Node node)
    {
        List<Node> nbr = new List<Node>();

        //Offset for World Origin
        Vector3 nodePosOffset = (-Vector3.forward * gridOrigin.z) - (Vector3.right * gridOrigin.x);
        Vector3 nodePos = node.worldPos + nodePosOffset;

        //Checking for traversable neighbours loop
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                //Check to avoid adding itself to neighbours list
                if (x == 0 && y == 0)
                {
                    continue;
                }
                int _x = (int)nodePos.x + x;
                int _y = (int)nodePos.z + y;


                //Prevents adding nodes outside of the grid
                if (_x < gridX && _x >= 0 && _y < gridY && _y >= 0)
                {
                    nbr.Add(gridArray[_x, _y]);
                }
            }
        }

        return nbr;
    }


    public List<Node> path;

    //Using Cube Gizmo for visualisation
    void OnDrawGizmos()
    {
        //Defense against NullReferenceException
        if (gridArray != null)
        {
            foreach (Node node in gridArray)
            {
                //Set colour based on if node is traversable or not
                if (node.traversable)
                { Gizmos.color = Color.white; }
                else
                { Gizmos.color = Color.red; }

                //Colour path black
                if (path != null)
                {
                    if (path.Contains(node))
                    {
                        Gizmos.color = Color.black;
                    }
                }

                //Render gizmo cubes
                Gizmos.DrawCube(node.worldPos, Vector3.one * (nodeDiameter - .1f));
            }
        }
    }
    public List<Node> getPath()
    {
        return path;
    }
}