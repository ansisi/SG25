using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiCtrl : MonoBehaviour
{
    public Transform itemHoldPoint;
    public Transform itemDropPoint;

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
    private List<Item> heldItems = new List<Item>();
    private List<Transform> dropItems = new List<Transform>();

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
        transform.rotation = Quaternion.Euler(0, 90, 0);
        //animator.SetBool("isWalking", true);
        GetItems();

        yield return new WaitForSeconds(2f);

        agentCollider.enabled = false;
        isColliderEnabled = false;
        transform.rotation = Quaternion.identity;
        //animator.SetBool("isWalking", false);

        currentWaypointIndex++;
        MoveToNextWaypoint();
    }

    private List<Item> SearchItems(Transform shelf, int itemCount)
    {
        List<Item> targetItems = new List<Item>();
        Transform[] slots = shelf.GetComponentsInChildren<Transform>();

        List<Transform> itemSlots = new List<Transform>();
        foreach (Transform slot in slots)
        {
            if (slot.CompareTag("Slot") && slot.childCount > 0)
            {
                Debug.Log("슬롯 찾앗어요~");
                itemSlots.Add(slot);
            }
        }

        Debug.Log("Slots found: " + itemSlots.Count); // Debugging the number of slots found

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

                Debug.Log("Slot with item: " + slotWithItem.name); // Debugging the slot being processed

                // Checking children of the slot
                foreach (Transform child in slotWithItem)
                {
                    Debug.Log("Child found: " + child.name); // Debugging the children of the slot
                    Consumable consumable = child.GetComponent<Consumable>();
                    if (consumable != null)
                    {
                        Debug.Log("Item found: " + consumable.item.itemName); // Debugging the item found
                        targetItems.Add(consumable.item);
                        break; // Exit the loop once an item is found
                    }
                    else
                    {
                        Debug.Log("No Consumable component found on child: " + child.name);
                    }
                }
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
            List<Item> targetItems = SearchItems(currentShelfInRange, itemCount);
            if (targetItems.Count > 0)
            {
                foreach (Item item in targetItems)
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

    private IEnumerator MoveItemToHoldPoint(Item item)
    {
        GameObject itemobj = Instantiate(item.itemPrefab, itemHoldPoint);
        itemobj.transform.localPosition = Vector3.zero;
        itemobj.transform.localRotation = Quaternion.identity;

        heldItems.Add(item);

        yield return new WaitForSeconds(2f);
    }

    void DropItem(Item item)
    {
        GameObject itemobj = Instantiate(item.itemPrefab, itemDropPoint);
        itemobj.transform.localPosition = Vector3.zero;
        itemobj.transform.localRotation = Quaternion.identity;

        dropItems.Add(itemobj.transform);
        itemobj.tag = "Product";
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);

        if (other.CompareTag("Waypoint"))
        {
            if (agent.remainingDistance < agent.stoppingDistance + 0.5f && !agent.pathPending)
            {
                Debug.Log("코루틴 시작");
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
