using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class AiEnemy : MonoBehaviour
{
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        // AI�� ���ƴٴ� ��θ� �����մϴ�.
        // ���� ���, ���� ������ ��� �׺���̼� ����Ʈ�� ������� �湮�ϵ��� ������ �� �ֽ��ϴ�.
        if (agent.destination == null)
        {
            // ���ο� �������� �����մϴ�.
            NavMeshWaypoint[] waypoints = FindObjectsOfType<NavMeshWaypoint>();
            if (waypoints.Length > 0)
            {
                agent.destination = waypoints[Random.Range(0, waypoints.Length)].transform.position;
            }
        }
    }
}