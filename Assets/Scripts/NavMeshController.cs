/*
    Code created by Oliver Fiedot-Davies, 2024
    For Maynooth University Computer Science and Software Engineering Final Year Project.
*/
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshController : MonoBehaviour
{
    public Grid grid;
    public NavMeshAgent agent;
    public List<Node> path;
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
        MoveNavMeshAgent();
    }

    //Move function for NavMesh agent, waits until grid & path !null
    void MoveNavMeshAgent()
    {
        if (grid != null)
        {
            path = grid.getPath();
        }
        if (path != null && path.Count > 0 && transform.position != null)
        {
            if(path.Count != 1)
            {
                //Sets destination as final node rather than next node in list
                agent.SetDestination(path[path.Count - 1].worldPos);
                targetReached = false;
            }
            if (path.Count == 1 && !targetReached)
            {
                stopwatch.Stop();
                long elapsedTime = stopwatch.ElapsedMilliseconds;
                UnityEngine.Debug.Log("NavMesh NPC reached target, time: "+elapsedTime+"ms");
                targetReached=true;
            }
        }
    }
}
