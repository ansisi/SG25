using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerCtrl : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private List<Vector3> patrolPoints; // 순찰 지점 목록

    void Start()
    {
        // NavMeshAgent 컴포넌트 가져오기
        navMeshAgent = GetComponent<NavMeshAgent>();

        // 순찰 지점 설정
        patrolPoints = new List<Vector3>();
        patrolPoints.Add(new Vector3(100, 0, 200)); // 예시 순찰 지점
        patrolPoints.Add(new Vector3(300, 0, 400)); // 예시 순찰 지점
        patrolPoints.Add(new Vector3(500, 0, 100)); // 예시 순찰 지점

        // **순찰 코루틴 실행 (필요하지 않은 경우 제거 가능)**
        //StartCoroutine(Patrol()); // 순찰 코루틴 실행 (필요하지 않은 경우 제거 가능)
    }

    // **반복 가능한 코드 사용 (필요하지 않은 경우 제거 가능)**
    IEnumerator Patrol()
    {
        while (true)
        {
            // 현재 순찰 지점에 도착했는지 확인
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                // 다음 순찰 지점으로 이동
                int nextPatrolPointIndex = (patrolPoints.IndexOf(navMeshAgent.destination) + 1) % patrolPoints.Count;
                navMeshAgent.SetDestination(patrolPoints[nextPatrolPointIndex]);
            }

            // 다음 프레임까지 기다림
            yield return null;
        }
    }
}
