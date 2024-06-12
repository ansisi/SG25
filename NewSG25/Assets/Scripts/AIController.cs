using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum CustomerState
{
    Idle,
    WalkingToShelf,
    PickingItem,
    WaitCounter,
    WalkingToCounter,
    PlacingItem,
    WaitingCalcPrice,
    GivingMoney,
    LeavingStore
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

public class AIController : MonoBehaviour
{
    public CheckoutSystem checkoutSystem;
    public PlayerCtrl playerCtrl;
    public float itemDelay = 0.1f;
    public float waitTime = 0.5f;
    public bool isFinishedCalcPrice = false;
    public int totalAmount = 0;

    public CustomerState currentState;
    private Timer timer;
    public NavMeshAgent agent;
    public bool isMoveDone = false;
    public Money[] moneyPrefabs;

    public Transform target;
    public Transform counter;
    public Transform arm;
    public Transform exitPoint;

    public List<GameObject> targetPos = new List<GameObject>();
    public List<GameObject> myItem = new List<GameObject>();
    public List<GameObject> counterItem = new List<GameObject>();
    public List<GameObject> moneyToGive = new List<GameObject>();
    public List<Transform> counterLine = new List<Transform>();

    public int cntToPick = 2;
    private int cntPicked = 0;

    private static int nextPriority = 0;
    private static readonly object priorityLock = new object();

    public Animator animator;

    void AssignPriority()
    {
        lock (priorityLock)
        {
            agent.avoidancePriority = nextPriority;
            nextPriority = (nextPriority + 1) % 100;
        }
    }

    private void Start()
    {
        checkoutSystem = FindObjectOfType<CheckoutSystem>();
        playerCtrl = FindObjectOfType<PlayerCtrl>();
        timer = new Timer();
        agent = GetComponent<NavMeshAgent>();
        SearchShelfs();
        AssignPriority();
        counter = GameObject.Find("Counter").transform;
        exitPoint = GameObject.Find("Exit").transform;
        currentState = CustomerState.Idle;
    }

    void Update()
    {
        timer.Update(Time.deltaTime);

        if (!agent.hasPath && agent.remainingDistance <= agent.stoppingDistance)
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
            case CustomerState.WaitCounter:
                WaitCounter();
                break;
            case CustomerState.WalkingToCounter:
                WalkingToCounter();
                break;
            case CustomerState.PlacingItem:
                PlacingItem();
                break;
            case CustomerState.WaitingCalcPrice:
                WaitingCalcPrice();
                break;
            case CustomerState.GivingMoney:
                GivingMoney();
                break;
            case CustomerState.LeavingStore:
                LeavingStore();
                break;
        }
    }

    void SearchShelfs()
    {
        for (int i = 1; i <= 12; i++)
        {
            GameObject shelf = GameObject.Find("Shelf" + i);
            if (shelf != null)
            {
                targetPos.Add(shelf);
            }
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
            List<GameObject> activeShelves = GetActiveShelves();
            if (activeShelves.Count > 0)
            {
                target = targetPos[UnityEngine.Random.Range(0, activeShelves.Count)].transform;
                MoveToTarget();
                ChangeState(CustomerState.WalkingToShelf, waitTime);
                animator.CrossFade("Walk", 0);
                animator.ResetTrigger("MotionTrigger");
            }
        }
    }

    List<GameObject> GetActiveShelves()
    {
        List<GameObject> activeShelves = new List<GameObject>();
        foreach (GameObject shelf in targetPos)
        {
            if (shelf.activeInHierarchy)
            {
                activeShelves.Add(shelf);
            }
        }
        return activeShelves;
    }

    void WalkingToShelf()
    {
        if (timer.IsFinished() && isMoveDone)
        {
            ChangeState(CustomerState.PickingItem, waitTime);
            animator.SetTrigger("MotionTrigger");
        }
    }

    void PickingItem()
    {
        if (timer.IsFinished())
        {
            Shelf shelf = target.GetComponent<Shelf>();

            if (shelf != null)
            {
                ItemDataStruct itemPicked = shelf.RandomPickItem();

                if (itemPicked == null)
                {
                    if (GameManager.Instance != null && GameManager.Instance.satisfactionManager != null)
                    {
                        GameManager.Instance.satisfactionManager.DecreaseSatisfaction();
                    }
                    timer.Set(1.0f);
                }
                else
                {
                    GoToHand(arm, itemPicked);

                    if (cntPicked < cntToPick)
                    {
                        cntPicked++;
                        timer.Set(itemDelay);
                    }
                    else
                    {
                        target = counter;
                        MoveToTarget();
                        ChangeState(CustomerState.WaitCounter, waitTime);
                        animator.CrossFade("Walk", 0);
                        animator.ResetTrigger("MotionTrigger");
                    }
                }
            }
        }
    }

    void WaitCounter()
    {
        AIController[] allAIs = FindObjectsOfType<AIController>();

        bool isCounterOccupied = false;
        foreach (var ai in allAIs)
        {
            if (ai != this && ai.currentState == CustomerState.LeavingStore || ai.currentState == CustomerState.WaitingCalcPrice || ai.currentState == CustomerState.GivingMoney)
            {
                isCounterOccupied = true;
                break;
            }
        }

        if (!isCounterOccupied)
        {
            ChangeState(CustomerState.WalkingToCounter, waitTime);
            animator.CrossFade("Walk", 0);
            animator.ResetTrigger("MotionTrigger");
        }
        else
        {
            Transform availablePosition = GetAvailableCounterLinePosition();
            if (availablePosition != null)
            {
                target = availablePosition;
                MoveToTarget();
            }
        }
    }

    void WalkingToCounter()
    {
        if (timer.IsFinished() && isMoveDone)
        {
            ChangeState(CustomerState.PlacingItem, waitTime);
            animator.SetTrigger("MotionTrigger");
        }
    }

    void PlacingItem()
    {
        Vector3 offSet = new Vector3(0f, 1f, 0f);

        if (timer.IsFinished() && isMoveDone)
        {
            if (myItem != null && myItem.Count != 0)
            {
                offSet += new Vector3(1 * myItem.Count - 2.5f, 0f, 0f);

                GameObject itemToPlace = myItem[myItem.Count - 1];

                itemToPlace.transform.position = counter.transform.position + offSet;
                itemToPlace.transform.parent = null;
                itemToPlace.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

                itemToPlace.AddComponent<Rigidbody>();

                ItemData itemData = itemToPlace.GetComponent<ItemData>();
                if (itemData != null)
                {
                    totalAmount += itemData.sellCost;
                }

                counterItem.Add(itemToPlace);

                itemToPlace.tag = "Item";

                myItem.RemoveAt(myItem.Count - 1);

                timer.Set(0.1f);

                Debug.Log("결제가 완료되지 않았습니다.");
            }
            else
            {
                cntPicked = 0;    
                ChangeState(CustomerState.WaitingCalcPrice, waitTime);
                
                animator.SetTrigger("MotionTrigger");
            }
        }
    }

    void WaitingCalcPrice()
    {
        bool allItemsDisabled = true;

        foreach (var item in counterItem)
        {
            if (item.activeSelf)
            {
                allItemsDisabled = false;
                break;
            }
        }

        if (allItemsDisabled)
        {
            for (int i = 0; i < counterItem.Count; i++)
            {
                Destroy(counterItem[i]);
                counterItem.Clear();
            }   
            isFinishedCalcPrice = true;
            ChangeState(CustomerState.GivingMoney, waitTime);
            Debug.Log("모든 아이템이 비활성화되어 결제를 기다립니다.");
        }
    }

    void GivingMoney()
    {
        if (isFinishedCalcPrice)
        {
            if (moneyToGive.Count <= 0)
            {
                GiveMoney(totalAmount);
                totalAmount = 0;
                Debug.Log("도둑");
            }
            ChangeState(CustomerState.LeavingStore, waitTime);
        }
    }

    void LeavingStore()
    {
        target = exitPoint;
        agent.SetDestination(target.position);

        if (timer.IsFinished() && isMoveDone)
        {
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
                Destroy(gameObject);
        }
    }

    void MoveToTarget()
    {
        isMoveDone = false;
        if (targetPos != null)
        {
            agent.SetDestination(target.position);
        }
    }

    public void GoToHand(Transform handPos, ItemDataStruct item)
    {
        Vector3 offSet = Vector3.zero;
        if (myItem.Count > 0)
        {
            for (int i = 0; i < myItem.Count; i++)
            {
                offSet += new Vector3(0f, myItem[i].GetComponent<BoxCollider>().size.y * myItem[i].transform.localScale.y, 0f);
            }
        }

        GameObject temp = Instantiate(item.gameObject);
        ItemData tempItemData = temp.AddComponent<ItemData>();
        tempItemData.itemIndex = item.ItemData.itemIndex;
        tempItemData.ItemName = item.ItemData.ItemName;
        tempItemData.sellCost = item.ItemData.sellCost;
        tempItemData.IconImage = item.ItemData.IconImage;
        tempItemData.ObjectModel = item.ItemData.ObjectModel;

        temp.transform.position = handPos.transform.position + offSet;
        temp.transform.parent = handPos;

        myItem.Add(temp);
    }

    void GiveMoney(int amount)
    {
        System.Array.Sort(moneyPrefabs, (a, b) => b.money.value.CompareTo(a.money.value));

        Vector3 spawnPosition = new Vector3(counter.position.x, counter.position.y + -0.1f, counter.position.z);

        foreach (var moneyPrefab in moneyPrefabs)
        {
            if (amount >= moneyPrefab.money.value)
            {
                int count = amount / moneyPrefab.money.value;
                amount -= count * moneyPrefab.money.value;
                for (int i = 0; i < count; i++)
                {
                    GameObject moneyInstance = Instantiate(moneyPrefab.money.moneyPrefab, spawnPosition, Quaternion.identity);
                    moneyToGive.Add(moneyInstance);
                }
            }
        }
    }

    Transform GetAvailableCounterLinePosition()
    {
        foreach (Transform position in counterLine)
        {
            bool positionOccupied = false;
            AIController[] allAIs = FindObjectsOfType<AIController>();
            foreach (var ai in allAIs)
            {
                if (ai != this && ai.target == position)
                {
                    positionOccupied = true;
                    break;
                }
            }

            if (!positionOccupied)
            {
                return position;
            }
        }
        return null;
    }
}