/*
    Code created by Oliver Fiedot-Davies, 2024
    For Maynooth University Computer Science and Software Engineering Final Year Project.

    Code adapted from A* psuedocode presented in CS422: Robotics & Automation, Topic 4 - Robot Planning (lecture)
    & CS404: Artificial Intelligence & Language Processing, GOFAI - Search (lecture)
*/

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;
using UnityEngine.Rendering;

public class Pathfinding : MonoBehaviour
{
    //Initialize grid
    Grid grid;

    //
    void GeneratePath(Vector3 _startNode, Vector3 _endNode)
    {
        //Nodes to be used in path calculation
        Node startNode = grid.PlayerNearestNode(_startNode);
        Node endNode = grid.PlayerNearestNode(_endNode);

        //{OPEN} and {CLOSED} node sets
        HashSet<Node> openSet = new HashSet<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        //Add the starting node to the open set
        openSet.Add(startNode);
        

        //A* Loop
        while (openSet.Count != 0)
        {
            //Gets LowestCostNode from openSet as it will be explored next
            Node currentNode = getLowestCostNode(openSet);
            
            //Removes currentNode as it value has been evaluated
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            //Ends process if A* arrives at the endNode
            if (currentNode == endNode)
            {
                break;
            }
        }
        return;
        

        Node getLowestCostNode(HashSet<Node> openNodes)
        {
            //Declare variables
            Node lowestCostNode = null;
            int lowestCost = int.MaxValue;
            
            //Goes through entire node set to check for lowest cost
            foreach (Node node in openNodes)
            {
                if (node.g < lowestCost)
                {
                    lowestCost = node.g;
                    lowestCostNode = node;
                }
            }
            return lowestCostNode;

        }
    }

    
    void Awake()
    {
        grid = GetComponent<Grid>();
    }
}