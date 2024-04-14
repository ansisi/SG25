using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiCtrl : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform[] waypoints;
    private int currentWaypointIndex = 0;
    public float movementSpeed = 50f; // AI 손님의 이동 속도

    public Transform itemHoldPoint; // 아이템을 들고 있는 위치
    public Transform itemDropPoint; // 아이템을 놓을 위치
    private GameObject heldItem; // 손님이 들고 있는 아이템

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = movementSpeed; // 속도 설정
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
            agent.isStopped = true; // 계산대에 도착하면 이동을 멈춤

            // 계산대에 도착하면 아이템을 들고 계산대로 이동
            PickUpItem();
        }
    }

    void PickUpItem()
    {
        // 아이템을 들어올림
        heldItem = FindNearestItemWithTag("Item");
        if (heldItem != null)
        {
            heldItem.transform.SetParent(itemHoldPoint);
            heldItem.transform.localPosition = Vector3.zero;
            heldItem.transform.localRotation = Quaternion.identity;
        }
    }

    void DropItem()
    {
        // 아이템을 계산대에 놓음
        if (heldItem != null)
        {
            heldItem.transform.SetParent(null);
            heldItem.transform.position = itemDropPoint.position;
            heldItem.transform.rotation = itemDropPoint.rotation;

            // 아이템의 태그를 "Product"로 변경
            heldItem.tag = "Product";
        }
    }

    GameObject FindNearestItemWithTag(string tag)
    {
        // 일정 범위 내에서 태그가 걸린 아이템을 검색하여 반환
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2f);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag(tag))
            {
                return collider.gameObject;
            }
        }
        return null;
    }

    void OnTriggerEnter(Collider other)
    {
        // 아이템과 트리거되면 해당 아이템을 손님의 팔로 이동
        if (other.CompareTag("Item"))
        {
            heldItem = other.gameObject;
            heldItem.transform.SetParent(itemHoldPoint);
            heldItem.transform.localPosition = Vector3.zero;
            heldItem.transform.localRotation = Quaternion.identity;
        }
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            DropItem();
            MoveToNextWaypoint();
        }
    }
}
