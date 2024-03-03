/*
    Code created by Oliver Fiedot-Davies, 2024
    For Maynooth University Computer Science and Software Engineering Final Year Project.

    Code adapted from A* psuedocode presented in CS422: Robotics & Automation, Topic 4 - Robot Planning (lecture)
    & CS404: Artificial Intelligence & Language Processing, GOFAI - Search (lecture).
*/

using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public Transform seeker, target; 

    //Initialize grid
    Grid grid;

    //Updates path every frame
    void Update()
    {
        GeneratePath(seeker.position, target.position);
    }

    //A* core, main code for path generation
    void GeneratePath(Vector3 _startNode, Vector3 _endNode)
    {
        //Nodes nearest player and target to be used in path calculation
        Node startNode = grid.PlayerNearestNode(_startNode);
        Node endNode = grid.PlayerNearestNode(_endNode);

        //{OPEN} and {CLOSED} node sets
        List<Node> openSet = new List<Node>();
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
            
            //Check heuristic value of each neighbour in the open set that is traversable
            foreach (Node nbr in grid.getNbr(currentNode))
            {
                //Avoid closed and untraversable nodes
                if (closedSet.Contains(nbr) || !nbr.traversable)
                { 
                    continue;
                }

                //Get cost of traveling to selected neigbour
                float nbrPathCost = currentNode.g + GetPathDistance(currentNode , nbr);

                //A* calculations
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

        //Gets next node with lowest cost from current node
        Node GetLowestCostNode(List<Node> openNodes)
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

        //Groups selected nodes once A* completes into path
        void ReturnPath(Node startNode, Node endNode)
        {
            List<Node> returnPath = new List<Node>();

            //Traces through parent of each node
            while (endNode != startNode)
            {
                returnPath.Add(endNode);
                endNode = endNode.parent;
            }
            //Reverses list as its built in reverse by default
            returnPath.Reverse();
            grid.path = returnPath;
        }

        //Gets distance between two nodes
        float GetPathDistance(Node originNode, Node endNode)
        {
            float distanceX = Mathf.Abs(originNode.worldPos.x - endNode.worldPos.x);
            float distanceY = Mathf.Abs(originNode.worldPos.z - endNode.worldPos.z);
            if (distanceX > distanceY)
            {
                return 1.414f*distanceY + 1f * (distanceX-distanceY); //1.414 is diagonal travel cost, from sqrt(2)
            }
            return 1.414f * distanceX + 1f * (distanceY-distanceX);
        }   
    }

    void Awake()
    {
        grid = GetComponent<Grid>();
    }
}