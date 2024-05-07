using UnityEngine;
using UnityEngine.AI;
using UnityEngine;
using System.Collections;

public class AIScript : MonoBehaviour
{
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        // AI가 돌아다닐 경로를 설정합니다.
        // 예를 들어, 매장 내부의 모든 네비게이션 포인트를 순서대로 방문하도록 설정할 수 있습니다.
        if (agent.destination == null)
        {
            // 새로운 목적지를 설정합니다.
            NavMeshWaypoint[] waypoints = FindObjectsOfType<NavMeshWaypoint>();
            if (waypoints.Length > 0)
            {
                agent.destination = waypoints[Random.Range(0, waypoints.Length)].transform.position;
            }
        }
    }
}
