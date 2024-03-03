/*
    Code created by Oliver Fiedot-Davies, 2024
    For Maynooth University Computer Science and Software Engineering Final Year Project.
*/
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
public class MovementController : MonoBehaviour
{
    public Transform target;
    public Grid grid;
    public List<Node> path;

    private int currentPathIndex = 0;
    public float movementSpeed = 5f;

    bool targetReached = false;

    //Timer for measuring travel time
    Stopwatch stopwatch = new Stopwatch();  

    void Awake()
    {
        //Find Grid class
        grid = FindObjectOfType<Grid>();
        stopwatch.Start();
    }

    void Update()
    {
        MoveNPC();
    }
    public void MoveNPC()
    {
        //Get path from grid class
        if (grid != null)
        {
            path = grid.getPath();
        }
        //Workaround for Debug.Log reached target spam
        if (path != null)
        {
            if (path.Count > 1)
            {
                targetReached = false;
            }
        }
        if (path != null && path.Count > 0 && transform.position != null)
        {

            //targetNode is always next node in list, unlike NavMesh
            Node targetNode = path[currentPathIndex];

            //Move towards target node
            transform.position = Vector3.MoveTowards(transform.position, targetNode.worldPos, movementSpeed * Time.deltaTime);
        }
        if (Vector3.Distance(transform.position, target.position) < 1f && !targetReached)
        {
            stopwatch.Stop();
            long elapsedTime = stopwatch.ElapsedMilliseconds;
            UnityEngine.Debug.Log("A_Star NPC reached target, time: "+ elapsedTime + "ms");
            targetReached = true;

        }
    }
}