using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiEnemy : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float range = 2.0f;

    private int currentWaypointIndex = 0;

    public Transform itemHoldPoint;
    public Transform itemDropPoint;

    public Transform[] waypoints;

    private NavMeshAgent agent;

    public MoneyConsumable[] moneyPrefabs;

    private List<Consumable> holdItem = new List<Consumable>();

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        MoveToWaypoint();
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            MoveToWaypoint();
        }

        GetItems();
    }

    void MoveToWaypoint()
    {
        if (currentWaypointIndex < waypoints.Length)
        {
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }

    void GetItems()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, range);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Shelf"))
            {
                Shelf shelf = collider.GetComponent<Shelf>();
                if (shelf != null)
                {
                    List<Consumable> items = null;
                    int itemCount = 0;
                    if (Vector3.Distance(transform.position, shelf.wayPoint1.position) < range)
                    {
                        items = shelf.AddRandomItems1(out itemCount);
                    }
                    else if (Vector3.Distance(transform.position, shelf.wayPoint2.position) < range)
                    {
                        items = shelf.AddRandomItems2(out itemCount);
                    }

                    if (items != null && items.Count > 0)
                    {
                        shelf.GotoHand(itemHoldPoint, items);
                        holdItem.AddRange(items);

                        foreach (Consumable item in items)
                        {
                            Debug.Log("가져온 아이템 : " + item.name + "개수 : " +itemCount);
                            break;
                        }
                    }
                }
            }
        }
    }
}
