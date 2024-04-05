using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiCtrl : MonoBehaviour
{

    private NavMeshAgent agent;
    public Transform[] waypoints;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        MoveToRandomWaypoint();
    }

    void MoveToRandomWaypoint()
    {
        if (waypoints.Length == 0)
        {
            Debug.LogError("No waypoints assigned to the AI controller.");
            return;
        }

        int randomIndex = Random.Range(0, waypoints.Length);
        agent.SetDestination(waypoints[randomIndex].position);
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            MoveToRandomWaypoint();
        }
    }
}
