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
    public Transform player;

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
                //Sets centre of each node as currentPos
                Vector3 currentPos = gridOrigin + Vector3.right * (nodeDiameter * x + nodeRadius) + Vector3.forward * (nodeDiameter * y + nodeRadius);

                //Checks if node overlaps with obstacleMask based on current position and radius
                bool traversable = !Physics.CheckSphere(currentPos, nodeRadius, obstacleMask);

                //Adds node to grid array
                gridArray[x,y] = new Node(traversable, currentPos);
            }
        }
    }

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

    //Using Wire Cube Gizmo for visualisation
    void OnDrawGizmos()
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
                else
                Gizmos.DrawWireCube(node.worldPos, Vector3.one*0.90f); 
            }
        }
    }

}
