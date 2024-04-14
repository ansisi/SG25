using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiCtrl : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform[] waypoints;
    private int currentWaypointIndex = 0;
    public float movementSpeed = 50f; // AI �մ��� �̵� �ӵ�

    public Transform itemHoldPoint; // �������� ��� �ִ� ��ġ
    public Transform itemDropPoint; // �������� ���� ��ġ
    private GameObject heldItem; // �մ��� ��� �ִ� ������

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
            agent.isStopped = true; // ���뿡 �����ϸ� �̵��� ����

            // ���뿡 �����ϸ� �������� ��� ����� �̵�
            PickUpItem();
        }
    }

    void PickUpItem()
    {
        // �������� ���ø�
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
        // �������� ���뿡 ����
        if (heldItem != null)
        {
            heldItem.transform.SetParent(null);
            heldItem.transform.position = itemDropPoint.position;
            heldItem.transform.rotation = itemDropPoint.rotation;

            // �������� �±׸� "Product"�� ����
            heldItem.tag = "Product";
        }
    }

    GameObject FindNearestItemWithTag(string tag)
    {
        // ���� ���� ������ �±װ� �ɸ� �������� �˻��Ͽ� ��ȯ
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
        // �����۰� Ʈ���ŵǸ� �ش� �������� �մ��� �ȷ� �̵�
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
