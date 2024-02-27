using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool traversable;

    public Vector3 worldPos;

    //Values for A* pathfinding algorithm
    public int g; //cost of getting to current node
    public int h; //additional cost of getting to solution
    public int f //approximation of current node's worth
    {
        get { return g + h; }
    }

    public Node(bool _traversable,  Vector3 _worldPos)
    {
        traversable = _traversable; 
        worldPos = _worldPos;
    }

}
