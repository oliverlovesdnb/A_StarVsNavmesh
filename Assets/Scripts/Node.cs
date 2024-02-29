using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool traversable;

    public Vector3 worldPos;

    //Values for A* pathfinding algorithm
    public float g; //cost of getting to current node
    public float h; //additional cost of getting to solution
    public float f //approximation of current node's worth
    {
        get { return g + h; }
    }

    public int xPos;
    public int yPos;

    public Node parent;


    public Node(bool _traversable,  Vector3 _worldPos, int _xPos, int _yPos)
    {
        traversable = _traversable; 
        worldPos = _worldPos;
        xPos = _xPos;
        yPos = _yPos;
    }

}
