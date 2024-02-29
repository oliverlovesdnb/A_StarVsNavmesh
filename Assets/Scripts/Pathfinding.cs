/*
    Code created by Oliver Fiedot-Davies, 2024
    For Maynooth University Computer Science and Software Engineering Final Year Project.

    Code adapted from A* psuedocode presented in CS422: Robotics & Automation, Topic 4 - Robot Planning (lecture)
    & CS404: Artificial Intelligence & Language Processing, GOFAI - Search (lecture)
*/

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;
using UnityEngine.Rendering;

public class Pathfinding : MonoBehaviour
{
    public Transform seeker, target; 

    //Initialize grid
    Grid grid;

    void Update()
    {
        GeneratePath(seeker.position, target.position);
    }
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
            Node currentNode = GetLowestCostNode(openSet);
            
            //Removes currentNode as it value has been evaluated
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            //Ends process if A* arrives at the endNode
            if (currentNode == endNode)
            {
                ReturnPath(startNode, endNode);
                break;
            }

            foreach (Node nbr in grid.getNbr(currentNode))
            {
                if (closedSet.Contains(nbr) || !nbr.traversable)
                { 
                    continue;
                }

                float nbrPathCost = currentNode.g + GetPathDistance(currentNode , nbr);
                if (!openSet.Contains(nbr) || nbrPathCost < nbr.g)
                {
                    nbr.parent = currentNode;
                    nbr.g = nbrPathCost;
                    nbr.h = GetPathDistance(nbr, endNode);

                    if (!openSet.Contains(nbr))
                    {
                        openSet.Add(nbr);
                    }
                }
            }
        }
        return;


        Node GetLowestCostNode(HashSet<Node> openNodes)
        {
            //Declare variables
            Node lowestCostNode = null;
            float lowestCost = int.MaxValue;
            
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

        void ReturnPath(Node startNode, Node endNode)
        {
            HashSet<Node> returnPath = new HashSet<Node>();

            Node currentNode = endNode;

            while (currentNode != startNode)
            {
                returnPath.Add(currentNode);
                currentNode = currentNode.parent;
            }
            returnPath.Reverse();
            grid.path = returnPath;
        }

        float GetPathDistance(Node originNode, Node endNode)
        {
            float distanceX = Mathf.Abs(originNode.worldPos.x - endNode.worldPos.x);
            //Debug.Log(distanceX);
            float distanceY = Mathf.Abs(originNode.worldPos.y - endNode.worldPos.y);
            if (distanceX < distanceY)
            {
                return 1.414f*distanceY + 1f * (distanceX-distanceY);
            }
            return 1.414f * distanceX + 1f * (distanceY - distanceX);
        }   
    }

    
    void Awake()
    {
        grid = GetComponent<Grid>();
    }
}