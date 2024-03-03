/*
    Code created by Oliver Fiedot-Davies, 2024
    For Maynooth University Computer Science and Software Engineering Final Year Project.
*/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshController : MonoBehaviour
{
    public Grid grid;
    public NavMeshAgent agent;
    public List<Node> path;
    bool targetReached = false;
    void Awake()
    {
        //Find Grid class
        grid = FindObjectOfType<Grid>();
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
                Debug.Log("NavMesh NPC reached target");
                targetReached=true;
            }
        }
    }
}
