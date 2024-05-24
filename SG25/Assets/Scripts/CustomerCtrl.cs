using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerCtrl : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private List<Vector3> patrolPoints; // ���� ���� ���

    void Start()
    {
        // NavMeshAgent ������Ʈ ��������
        navMeshAgent = GetComponent<NavMeshAgent>();

        // ���� ���� ����
        patrolPoints = new List<Vector3>();
        patrolPoints.Add(new Vector3(100, 0, 200)); // ���� ���� ����
        patrolPoints.Add(new Vector3(300, 0, 400)); // ���� ���� ����
        patrolPoints.Add(new Vector3(500, 0, 100)); // ���� ���� ����

        // **���� �ڷ�ƾ ���� (�ʿ����� ���� ��� ���� ����)**
        //StartCoroutine(Patrol()); // ���� �ڷ�ƾ ���� (�ʿ����� ���� ��� ���� ����)
    }

    // **�ݺ� ������ �ڵ� ��� (�ʿ����� ���� ��� ���� ����)**
    IEnumerator Patrol()
    {
        while (true)
        {
            // ���� ���� ������ �����ߴ��� Ȯ��
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                // ���� ���� �������� �̵�
                int nextPatrolPointIndex = (patrolPoints.IndexOf(navMeshAgent.destination) + 1) % patrolPoints.Count;
                navMeshAgent.SetDestination(patrolPoints[nextPatrolPointIndex]);
            }

            // ���� �����ӱ��� ��ٸ�
            yield return null;
        }
    }
}
