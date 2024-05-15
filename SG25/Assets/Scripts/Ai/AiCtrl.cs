using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiCtrl : MonoBehaviour
{
    public Transform itemHoldPoint;
    public Transform itemDropPoint;

    public LayerMask itemLayer;
    public Collider agentCollider;

    public MoneyConsumable[] moneyPrefabs;
    public Transform[] waypoints;

    private NavMeshAgent agent;
    private Transform currentShelfInRange;
    //private Animator animator;

    public float movementSpeed = 20f;
    private int currentWaypointIndex = 0;

    private bool isColliderEnabled = false;

    private List<Transform> checkedShelves = new List<Transform>();
    private List<Transform> heldItems = new List<Transform>();
    private List<GameObject> holdItems = new List<GameObject>();

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //animator = GetComponent<Animator>();
        agent.speed = movementSpeed;
        currentWaypointIndex = 0;
        agentCollider.enabled = false;

        MoveToNextWaypoint();
    }

    void MoveToNextWaypoint()
    {
        if (currentWaypointIndex < waypoints.Length)
        {
            agent.SetDestination(waypoints[currentWaypointIndex].position);
            //animator.SetBool("isWalking", true);
            Debug.Log("다음 웨이포인트로 이동 중: " + waypoints[currentWaypointIndex].name);
            Debug.Log("현재 목적지: " + waypoints[currentWaypointIndex].position);
        }
    }

    IEnumerator WaitForOneSecond()
    {
        agentCollider.enabled = true;
        isColliderEnabled = true;
        //animator.SetBool("isWalking", true);
        GetItems();

        yield return new WaitForSeconds(0.1f);

        agentCollider.enabled = false;
        isColliderEnabled = false;
        //animator.SetBool("isWalking", false);

        currentWaypointIndex++;
        MoveToNextWaypoint();
    }



    private List<Transform> SearchItems(Transform shelf, int itemCount)
    {
        List<Transform> targetItems = new List<Transform>();
        Transform[] slots = shelf.GetComponentsInChildren<Transform>();

        List<Transform> itemSlots = new List<Transform>();
        foreach (Transform slot in slots)
        {
            if (slot.CompareTag("Slot") && slot.childCount > 0)
            {
                itemSlots.Add(slot);
            }
        }

        if (itemSlots.Count > 0)
        {
            List<int> indices = new List<int>();
            for (int i = 0; i < Mathf.Min(itemCount, itemSlots.Count); i++)
            {
                int randomIndex = Random.Range(0, itemSlots.Count);
                while (indices.Contains(randomIndex))
                {
                    randomIndex = Random.Range(0, itemSlots.Count);
                }
                indices.Add(randomIndex);
                Transform slotWithItem = itemSlots[randomIndex];
                Transform item = slotWithItem.GetChild(0);
                targetItems.Add(item);
            }
        }
        else
        {
            currentWaypointIndex++;
            Debug.Log("No items found, moving to the next waypoint.");
        }
        return targetItems;
    }

    private void GetItems()
    {
        int itemCount = 3;

        if (currentShelfInRange != null)
        {
            List<Transform> targetItems = SearchItems(currentShelfInRange, itemCount);
            if (targetItems.Count > 0)
            {
                foreach (Transform item in targetItems)
                {
                    StartCoroutine(MoveItemToHoldPoint(item));
                }
            }
            else
            {
                MoveToNextWaypoint();

            }
        }
    }

    private IEnumerator MoveItemToHoldPoint(Transform item)
    {
        //animator.SetBool("isPicking", true);

        item.SetParent(itemHoldPoint);
        item.localPosition = Vector3.zero;
        item.localRotation = Quaternion.identity;

        heldItems.Add(item);

        yield return new WaitForSeconds(0.1f);

        //animator.SetBool("isPicking", false);
    }

    void DropItem()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);

        if (other.CompareTag("Waypoint"))
        {
            if (agent.remainingDistance < agent.stoppingDistance && !agent.pathPending)
            {
                StartCoroutine(WaitForOneSecond());
            }
        }

        if (other.CompareTag("Shelf"))
        {
            if (!checkedShelves.Contains(other.transform))
            {
                currentShelfInRange = other.transform;
                checkedShelves.Add(other.transform);
            }
        }
    }

}