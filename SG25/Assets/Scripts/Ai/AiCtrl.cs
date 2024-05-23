using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiCtrl : MonoBehaviour
{
    public Transform itemHoldPoint;
    public Transform itemDropPoint;

    public Collider agentCollider;

    public MoneyConsumable[] moneyPrefabs; // ���� ������ �� ������ �迭
    public Transform[] waypoints;

    private NavMeshAgent agent;
    private Transform currentShelfInRange;

    public float movementSpeed = 20f;
    private int currentWaypointIndex = 0;

    private List<Transform> checkedShelves = new List<Transform>();
    private List<Item> heldItems = new List<Item>();

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
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
            Debug.Log("���� ��������Ʈ�� �̵� ��: " + waypoints[currentWaypointIndex].name);
            Debug.Log("���� ������: " + waypoints[currentWaypointIndex].position);
        }
    }

    IEnumerator WaitForOneSecond()
    {
        agentCollider.enabled = true;
        transform.rotation = Quaternion.Euler(0, 0, 0);

        Debug.Log("GetItems ȣ�� �� currentShelfInRange: " + (currentShelfInRange != null ? currentShelfInRange.name : "null"));
        GetItems();

        yield return new WaitForSeconds(2f);

        agentCollider.enabled = false;
        transform.rotation = Quaternion.identity;

        currentWaypointIndex++;
        if (currentWaypointIndex >= waypoints.Length)
        {
            StartCoroutine(DropItemsSequentially());
        }
        else
        {
            MoveToNextWaypoint();
        }
    }

    private List<Item> SearchItems(Transform shelf, int itemCount)
    {
        List<Item> targetItems = new List<Item>();
        Transform[] slots = shelf.GetComponentsInChildren<Transform>();

        Debug.Log("����: " + shelf.name + " - �ڽ� ����: " + slots.Length);

        List<Transform> itemSlots = new List<Transform>();
        foreach (Transform slot in slots)
        {
            if (slot.CompareTag("Slot") && slot.childCount > 0)
            {
                itemSlots.Add(slot);
            }
        }

        Debug.Log("ã�� ���� ����: " + itemSlots.Count);

        if (itemSlots.Count > 0)
        {
            HashSet<int> indices = new HashSet<int>();

            while (indices.Count < Mathf.Min(itemCount, itemSlots.Count))
            {
                int randomIndex = Random.Range(0, itemSlots.Count);
                if (indices.Add(randomIndex)) // ������ �ε����� �ߺ����� ������ �߰�
                {
                    Transform slotWithItem = itemSlots[randomIndex];
                    foreach (Transform child in slotWithItem)
                    {
                        Consumable consumable = child.GetComponent<Consumable>();
                        if (consumable != null)
                        {
                            targetItems.Add(consumable.item);
                            Destroy(child.gameObject);
                            break;
                        }
                    }
                }
            }
        }
        else
        {
            currentWaypointIndex++;
            Debug.Log("�������� ã�� ���߽��ϴ�. ���� ��������Ʈ�� �̵� ��.");
        }
        return targetItems;
    }

    private void GetItems()
    {
        int itemCount = 3;

        if (currentShelfInRange != null)
        {
            Debug.Log("���ݿ��� ������ �˻� ��: " + currentShelfInRange.name);
            List<Item> targetItems = SearchItems(currentShelfInRange, itemCount);
            if (targetItems.Count > 0)
            {
                foreach (Item item in targetItems)
                {
                    MoveItemToHoldPoint(item);
                }
            }
            else
            {
                MoveToNextWaypoint();
            }
        }
        else
        {
            Debug.Log("currentShelfInRange�� null�Դϴ�.");
        }
    }

    private void MoveItemToHoldPoint(Item item)
    {
        if (heldItems.Count >= 5)
        {
            Debug.Log("��� �ִ� �������� �ʹ� �����ϴ�. �� �̻� �߰��� �� �����ϴ�.");
            return;
        }

        if (heldItems.Contains(item))
        {
            Debug.Log("�̹� ��� �ִ� ������: " + item.name);
            return;
        }

        GameObject itemInstance = Instantiate(item.itemPrefab);
        Transform itemTransform = itemInstance.transform;
        itemTransform.SetParent(itemHoldPoint);
        itemTransform.localPosition = Vector3.zero;
        itemTransform.localRotation = Quaternion.identity;

        heldItems.Add(item);
        Debug.Log("������ ���: " + item.name + " (" + heldItems.Count + "/" + 5 + ")");
    }

    private IEnumerator DropItemsSequentially()
    {
        Debug.Log("DropItemsSequentially ����");
        int totalPrice = CalculateTotalPrice(); // �տ� ��� �ִ� �������� �� ������ ���� ����մϴ�.
        Debug.Log("�� ����: " + totalPrice);

        while (heldItems.Count > 0)
        {
            Debug.Log("heldItems.Count: " + heldItems.Count);
            if (itemDropPoint.childCount < 5)
            {
                Item item = heldItems[0];
                Debug.Log("������ ��� ��: " + item.name);

                // �������� �θ� �����Ͽ� ��� ����Ʈ�� �̵���ŵ�ϴ�.
                Transform itemTransform = itemHoldPoint.GetChild(0); // itemHoldPoint�� ù ��° �ڽ��� �����ɴϴ�.
                itemTransform.SetParent(itemDropPoint);
                itemTransform.localPosition = Vector3.zero;
                itemTransform.localRotation = Quaternion.identity;
                itemTransform.tag = "Product";

                heldItems.RemoveAt(0); // heldItems ����Ʈ���� �������� �����մϴ�.
                Debug.Log("������ ��� �Ϸ�: " + item.name + " (" + heldItems.Count + " ������ ����)");

                yield return new WaitForSeconds(1f); // ���� �ð� ���� �������� ��� ����Ʈ�� �ε��� ����մϴ�.
            }
            else
            {
                Debug.Log("itemDropPoint�� �������� 5�� �̻� ����, ��� ��...");
                yield return new WaitForSeconds(0.5f); // 0.5�� �������� ��Ȯ��
            }
        }

        Debug.Log("��� ������ ��� �Ϸ�. ���� �����մϴ�.");
        GiveMoney(totalPrice); // �������� ��� ����� �Ŀ� ���� �ִ� �Լ��� ȣ���մϴ�.
        Debug.Log("GiveMoney ȣ�� �Ϸ�");

        // ��� �۾� �Ϸ� �� ���� ���·� �Ѿ�� ���� ����� �޽���
        Debug.Log("��� �۾� �Ϸ�. AiCtrl �۾� �Ϸ�");
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter: " + other.gameObject.name);

        if (other.CompareTag("Waypoint"))
        {
            if (agent.remainingDistance < agent.stoppingDistance + 0.5f && !agent.pathPending)
            {
                Debug.Log("�ڷ�ƾ ����");
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

    private int CalculateTotalPrice()
    {
        int totalPrice = 0;
        foreach (Item item in heldItems)
        {
            totalPrice += item.price;
        }
        Debug.Log("��� �ִ� �������� �� ����: " + totalPrice);
        return totalPrice;
    }

    private void GiveMoney(int amount)
    {
        Debug.Log("GiveMoney ȣ���. �ݾ�: " + amount);
        int remainingAmount = amount;
        System.Random random = new System.Random();
        while (remainingAmount > 0)
        {
            int moneyValue = random.Next(500, 5001); // 500���� 5000 ������ ������ �ݾ��� �����մϴ�.
            moneyValue = Mathf.Min(moneyValue, remainingAmount); // ���� �ݾ׺��� ū ���� ���� �� ������ �����մϴ�.

            foreach (MoneyConsumable moneyPrefab in moneyPrefabs)
            {
                if (moneyValue >= moneyPrefab.money.value)
                {
                    GameObject moneyObj = Instantiate(moneyPrefab.money.moneyPrefab, itemHoldPoint);
                    moneyObj.transform.localPosition = Vector3.zero;
                    moneyObj.transform.localRotation = Quaternion.identity;
                    remainingAmount -= moneyPrefab.money.value;
                    Debug.Log("�� ����. �ݾ�: " + moneyPrefab.money.value + ", ���� �ݾ�: " + remainingAmount);
                    break;
                }
            }
        }

        // ��� �� ���� �� ����� �޽���
        Debug.Log("�� ���� �Ϸ�. GiveMoney �۾� �Ϸ�");
    }
}
