using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public enum CustomerState
{
    Idle,
    WalkingToShelf,
    PickingItem,
    WalkingToCounter,
    PlacingItem 
}

public class Timer
{
    private float timeRemaining;

    public void Set(float time)
    {
        timeRemaining = time;
    }

    public void Update(float deltaTime)
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= deltaTime;
        }
    }

    public bool IsFinished()
    {
        return timeRemaining <= 0;
    }
}

public class AiCtrl : MonoBehaviour
{
    public CustomerState currentState;
    private Timer timer;
    private NavMeshAgent agent;
    public Item item;
    public bool isMoveDone = false;

    public Transform target;
    public Transform counter;
    public Transform arm;

    public List<Shelf> targetPos = new List<Shelf>();
    public List<Consumable> myItem = new List<Consumable>();

    private static int nextPriority = 0;
    private static readonly object priorityLock = new object();

    public int itemsToPick = 5;
    private int itemsPicked = 0;

    void Start()
     {
        timer = new Timer();
        agent = GetComponent<NavMeshAgent>();
        AssignPriority();
        currentState = CustomerState.Idle;
     }

    void AssignPriority()
    {
        lock (priorityLock)
        {
            agent.avoidancePriority = nextPriority;
            nextPriority = (nextPriority + 1) % 100;
        }
    }

    void MoveToTarget()
    {
        isMoveDone = true;

        if(target != null)
        {
            agent.SetDestination(target.position);
        }
    }

    void Update()
    {
        timer.Update(Time.deltaTime);

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                isMoveDone = true;
            }
        }

        switch (currentState)
        {
            case CustomerState.Idle:
                Idle();
                break;
            case CustomerState.WalkingToShelf:
                WalkingToShelf();
                break;
            case CustomerState.PickingItem:
                PickingItem();
                break;
            case CustomerState.WalkingToCounter:
                WalkingToCounter();
                break;
            case CustomerState.PlacingItem:
                PlacingItem();
                break;
        }
    }

    void ChangeState(CustomerState nextState, float waitTime = 0.0f)
    {
        currentState = nextState;
        timer.Set(waitTime);
    }

    void Idle()
    {
        if (timer.IsFinished())
        {
            target = targetPos[Random.Range(0, targetPos.Count)].transform;
            MoveToTarget();
            ChangeState(CustomerState.WalkingToShelf, 2.0f);
        }
    }

    void WalkingToShelf()
    {
        if (timer.IsFinished() && isMoveDone)
        {
            ChangeState(CustomerState.PickingItem, 2.0f);
        }
    }

    void PickingItem()
    {
        if (timer.IsFinished())
        {
            if (itemsPicked < itemsToPick)
            {
                GameObject itemObj = Instantiate(item.itemPrefab);

                Consumable consumable = itemObj.GetComponent<Consumable>();

                consumable.item = item;

                myItem.Add(consumable);
                itemObj.transform.parent = arm.transform;
                itemObj.transform.localEulerAngles = Vector3.zero;
                itemObj.transform.localPosition = new Vector3(0, itemsPicked * 2f, 0);

                itemsPicked++;
                timer.Set(0.5f);
            }
            else
            {
                target = counter;
                MoveToTarget();
                ChangeState(CustomerState.WalkingToCounter, 2.0f);
            }
        }
    }

    void WalkingToCounter()
    {
        if (timer.IsFinished() && isMoveDone)
        {
            ChangeState(CustomerState.PlacingItem, 2.0f);
        }
    }

    void PlacingItem()
    {
        if (timer.IsFinished())
        {
            if (myItem.Count != 0)
            {
                myItem[0].transform.position = counter.transform.position;
                myItem[0].transform.parent = counter.transform;
                myItem.RemoveAt(0);

                timer.Set(0.1f);
            }
            else
            {
                ChangeState(CustomerState.Idle, 2.0f);
            }

        }
    }




    /*public Transform itemHoldPoint;
    public Transform itemDropPoint;

    public Collider agentCollider;

    public MoneyConsumable[] moneyPrefabs; // 여러 종류의 돈 프리팹 배열
    public Transform[] waypoints;

    private NavMeshAgent agent;
    private Transform currentShelfInRange;

    public float movementSpeed = 20f;
    private int currentWaypointIndex = 0;

    private List<Transform> checkedShelves = new List<Transform>();
    private List<Item> heldItems = new List<Item>();

    public List<Consumable> holdItem = new List<Consumable>();      



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
            Debug.Log("다음 웨이포인트로 이동 중: " + waypoints[currentWaypointIndex].name);
            Debug.Log("현재 목적지: " + waypoints[currentWaypointIndex].position);
        }
    }

    IEnumerator WaitForOneSecond()
    {
        agentCollider.enabled = true;
        transform.rotation = Quaternion.Euler(0, 0, 0);

        Debug.Log("GetItems 호출 전 currentShelfInRange: " + (currentShelfInRange != null ? currentShelfInRange.name : "null"));
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

        Debug.Log("선반: " + shelf.name + " - 자식 개수: " + slots.Length);

        List<Transform> itemSlots = new List<Transform>();
        foreach (Transform slot in slots)
        {
            if (slot.CompareTag("Slot") && slot.childCount > 0)
            {
                itemSlots.Add(slot);
            }
        }

        Debug.Log("찾은 슬롯 개수: " + itemSlots.Count);

        if (itemSlots.Count > 0)
        {
            HashSet<int> indices = new HashSet<int>();

            while (indices.Count < Mathf.Min(itemCount, itemSlots.Count))
            {
                int randomIndex = Random.Range(0, itemSlots.Count);
                if (indices.Add(randomIndex)) // 무작위 인덱스가 중복되지 않으면 추가
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
            Debug.Log("아이템을 찾지 못했습니다. 다음 웨이포인트로 이동 중.");
        }
        return targetItems;
    }

    private void GetItems()
    {
        int itemCount = 3;

        if (currentShelfInRange != null)
        {
            Debug.Log("선반에서 아이템 검색 중: " + currentShelfInRange.name);
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
            Debug.Log("currentShelfInRange가 null입니다.");
        }
    }

    private void MoveItemToHoldPoint(Item item)
    {
        if (heldItems.Count >= 5)
        {
            Debug.Log("들고 있는 아이템이 너무 많습니다. 더 이상 추가할 수 없습니다.");
            return;
        }

        if (heldItems.Contains(item))
        {
            Debug.Log("이미 들고 있는 아이템: " + item.name);
            return;
        }

        GameObject itemInstance = Instantiate(item.itemPrefab);
        Transform itemTransform = itemInstance.transform;
        itemTransform.SetParent(itemHoldPoint);
        itemTransform.localPosition = Vector3.zero;
        itemTransform.localRotation = Quaternion.identity;

        heldItems.Add(item);
        Debug.Log("아이템 들기: " + item.name + " (" + heldItems.Count + "/" + 5 + ")");
    }

    private IEnumerator DropItemsSequentially()
    {
        Debug.Log("DropItemsSequentially 시작");
        int totalPrice = CalculateTotalPrice(); // 손에 들고 있는 아이템의 총 가격을 먼저 계산합니다.
        Debug.Log("총 가격: " + totalPrice);

        for(int i = 0; i < holdItem.Count; i++)
        {
            //holdItem[i].gameObject.transform.position = 
        }

        for(int i = 0; i < heldItems.Count; i++)
        {
            Transform itemTransform = itemHoldPoint.GetChild(0); // itemHoldPoint의 첫 번째 자식을 가져옵니다.
            itemTransform.SetParent(itemDropPoint);
            itemTransform.localPosition = Vector3.zero;
            itemTransform.localRotation = Quaternion.identity;
            itemTransform.tag = "Product";

            yield return new WaitForSeconds(0.5f); // 0.5초 간격으로 재확인
        }

        heldItems.Clear();

        Debug.Log("모든 아이템 드롭 완료. 돈을 지급합니다.");
        GiveMoney(totalPrice); // 아이템을 모두 드롭한 후에 돈을 주는 함수를 호출합니다.
        Debug.Log("GiveMoney 호출 완료");

        // 모든 작업 완료 후 다음 상태로 넘어가기 위한 디버그 메시지
        Debug.Log("모든 작업 완료. AiCtrl 작업 완료");
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter: " + other.gameObject.name);

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

    private int CalculateTotalPrice()
    {
        int totalPrice = 0;
        foreach (Item item in heldItems)
        {
            totalPrice += item.price;
        }
        Debug.Log("들고 있는 아이템의 총 가격: " + totalPrice);
        return totalPrice;
    }

    private void GiveMoney(int amount)
    {
        Debug.Log("GiveMoney 호출됨. 금액: " + amount);
        int remainingAmount = amount;
        System.Random random = new System.Random();
        while (remainingAmount > 0)
        {
            int moneyValue = random.Next(500, 5001); // 500에서 5000 사이의 랜덤한 금액을 선택합니다.
            moneyValue = Mathf.Min(moneyValue, remainingAmount); // 남은 금액보다 큰 값이 나올 수 없도록 조정합니다.

            foreach (MoneyConsumable moneyPrefab in moneyPrefabs)
            {
                if (moneyValue >= moneyPrefab.money.value)
                {
                    GameObject moneyObj = Instantiate(moneyPrefab.money.moneyPrefab, itemHoldPoint);
                    moneyObj.transform.localPosition = Vector3.zero;
                    moneyObj.transform.localRotation = Quaternion.identity;
                    remainingAmount -= moneyPrefab.money.value;
                    Debug.Log("돈 지급. 금액: " + moneyPrefab.money.value + ", 남은 금액: " + remainingAmount);
                    break;
                }
            }
        }

        // 모든 돈 지급 후 디버그 메시지
        Debug.Log("돈 지급 완료. GiveMoney 작업 완료");
    }*/
}
