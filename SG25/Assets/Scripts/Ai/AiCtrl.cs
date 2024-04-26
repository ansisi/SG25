using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiCtrl : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform[] waypoints;
    private int currentWaypointIndex = 0;
    public float movementSpeed = 50f;

    public Transform itemHoldPoint;
    public Transform itemDropPoint;

    public LayerMask itemLayer;
    public float itemSearchRange = 2f;
    public float itemPickRange = 2f;

    private List<GameObject> searchItems = new List<GameObject>();
    private List<GameObject> selectedObjects = new List<GameObject>();
    public MoneyConsumable[] moneyPrefabs;

    private List<GameObject> holdItems = new List<GameObject>();

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = movementSpeed;

        SearchItem();
    }

    void MoveToNextWaypoint()
    {
        if (currentWaypointIndex < waypoints.Length)
        {
            agent.SetDestination(waypoints[currentWaypointIndex].position);
            Debug.Log("다음 웨이포인트로 이동 중: " + waypoints[currentWaypointIndex].name);
            currentWaypointIndex++;

            foreach (GameObject obj in selectedObjects)
            {
                if ((obj != null))
                {
                    if (itemPickRange <= 4)
                    {
                        agent.SetDestination(obj.transform.position);
                    }
                }
            }
        }
        else
        {
            agent.isStopped = true;
            DropItem();
            return;
        }
    }
    void SearchItem()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, itemSearchRange, itemLayer);
        searchItems.Clear();

        foreach (Collider collider in colliders)
        {
            searchItems.Add(collider.gameObject);
        }

        int itemsCount = Mathf.Min(Random.Range(0, 30), searchItems.Count);

        for (int i = 0; i < itemsCount; i++)
        {
            int randomIndex = Random.Range(0, searchItems.Count);

            selectedObjects.Add(searchItems[randomIndex]);
            searchItems.RemoveAt(randomIndex);
        }
        Debug.Log(selectedObjects.Count);
        MoveToNextWaypoint();
    }

    void DropItem()
    {
        int totalValue = 0;

        foreach (GameObject itemObj in holdItems)
        {
            Consumable consumable = itemObj.GetComponent<Consumable>();
            if (consumable.item != null)
            {
                totalValue += consumable.item.price;
            }
        }

        Debug.Log("총 가치: " + totalValue);

        foreach (MoneyConsumable moneyConsumable in moneyPrefabs)
        {
            int moneyValue = moneyConsumable.money.value;

            if (totalValue >= moneyValue)
            {
                int moneyToGive = Random.Range(moneyValue, totalValue + 1);

                if (moneyToGive < moneyValue)
                {
                    moneyToGive = moneyValue;
                }

                GameObject moneyObject = Instantiate(moneyConsumable.money.moneyPrefab, itemHoldPoint.position, itemHoldPoint.rotation);
                MoneyConsumable moneyComponent = moneyObject.GetComponent<MoneyConsumable>();


                if (moneyComponent != null)
                {
                    moneyComponent.money.value = moneyValue;

                    moneyObject.transform.SetParent(itemHoldPoint);
                    moneyObject.transform.localPosition = Vector3.zero;
                    moneyObject.transform.localRotation = Quaternion.identity;
                }
                totalValue -= moneyValue;
                Debug.Log("주는 돈의 가치: " + moneyToGive);
            }
            foreach (GameObject itemObj in holdItems)
            {
                itemObj.transform.SetParent(itemDropPoint);
                itemObj.transform.position = itemDropPoint.position;
                itemObj.transform.rotation = itemDropPoint.rotation;
                itemObj.tag = "Product";
            }

            holdItems.Clear();
            agent.isStopped = true;
        }
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.1f)
        {
            MoveToNextWaypoint();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (selectedObjects.Contains(other.gameObject))
        {
            if (other.CompareTag("Item"))
            {
                GameObject obj = other.gameObject;
                float distance = Vector3.Distance(transform.position, obj.transform.position);
                if (distance <= itemPickRange)
                {
                    obj.transform.SetParent(itemHoldPoint);
                    obj.transform.localPosition = Vector3.zero;
                    obj.transform.localRotation = Quaternion.identity;

                    holdItems.Add(obj);
                    selectedObjects.Remove(obj);
                    MoveToNextWaypoint();
                }
            }
        }
    }
}