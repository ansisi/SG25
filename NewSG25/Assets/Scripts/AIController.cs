using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum CustomerState
{
    Idle,
    WalkingToShelf,
    PickingItem,
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

    void Start()
    {
        timer = new Timer();
        agent = GetComponent<NavMeshAgent>();
        AssignPriority();
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
                    // 아이템이 없을 경우 만족도 감소
                    if (GameManager.Instance != null && GameManager.Instance.satisfactionManager != null)
                    {
                        GameManager.Instance.satisfactionManager.DecreaseSatisfaction();
                    }
                    //

                    // 선반에 아이템이 없으면 1초 기다렸다가 재시도
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
                        ChangeState(CustomerState.WalkingToCounter, waitTime);
                        animator.CrossFade("Walk", 0);
                        animator.ResetTrigger("MotionTrigger");
                    }
                }
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

        if (timer.IsFinished())
        {
            if (myItem != null && myItem.Count != 0)
            {
                offSet += new Vector3(1 * myItem.Count - 2.5f, 0f, 0f);

                GameObject itemToPlace = myItem[myItem.Count - 1];

                itemToPlace.transform.position = counter.transform.position + offSet;
                itemToPlace.transform.parent = null;
                itemToPlace.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

                itemToPlace.AddComponent<Rigidbody>();

                counterItem.Add(itemToPlace);

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
        if (counterItem.Count <= 0)
        {
            isFinishedCalcPrice = true;

            ChangeState(CustomerState.GivingMoney, waitTime);
        }

        //if (isFinishedCalcPrice)
        //{
        //    ChangeState(CustomerState.GivingMoney, waitTime);


        //    Debug.Log("결제를 기다리는 중");
        //}
    }

    void GivingMoney()
    {
        if (isFinishedCalcPrice)
        {
            
            if (moneyToGive.Count <= 0)
            {
                GiveMoney(totalAmount);
                totalAmount = 0;
                ChangeState(CustomerState.LeavingStore, waitTime);
                
            }
        }   
    }

    void LeavingStore()
    {
        
        
        target = exitPoint;
        MoveToTarget();

        if (timer.IsFinished() && isMoveDone)
        {
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
        tempItemData.cost = item.ItemData.cost;
        tempItemData.IconImage = item.ItemData.IconImage;
        tempItemData.ObjectModel = item.ItemData.ObjectModel;

        temp.transform.position = handPos.transform.position + offSet;
        temp.transform.parent = handPos;

        myItem.Add(temp);
    }

    void GiveMoney(int amount)
    {
        Array.Sort(moneyPrefabs, (a, b) => b.money.value.CompareTo(a.money.value));

        Vector3 spawnPosition = new Vector3(counter.position.x,counter.position.y + 0.55f,counter.position.z );

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
}



