using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiCtrl : MonoBehaviour
{

    private NavMeshAgent agent;
    public Transform[] waypoints;
    private int currentWaypointIndex = 0;
    public float movementSpeed = 3.5f; // AI �մ��� �̵� �ӵ�

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = movementSpeed; // �ӵ� ����
        MoveToNextWaypoint();
    }

    void MoveToNextWaypoint()
    {
        if (currentWaypointIndex < waypoints.Length)
        {
            agent.SetDestination(waypoints[currentWaypointIndex].position);
            currentWaypointIndex++;
        }
        else
        {
            Debug.Log("AI reached the checkout counter.");
            agent.isStopped = true; // ���뿡 �����ϸ� �̵��� ����
        }
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            MoveToNextWaypoint();
        }
    }
}
