using System.Collections;
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

public class AiCtrl : MonoBehaviour
{
    public CustomerState currentState;
    private Timer timer;
    private NavMeshAgent agent;
    public Animator animator;

    public bool isMoveDone = false;

    public Transform target;
    public Transform counter;
    public Transform arm;
    public Transform exitPoint;

    public List<Shelf> targetPos = new List<Shelf>();
    public List<Consumable> myItem = new List<Consumable>();

    private static int nextPriority = 0;
    private static readonly object priorityLock = new object();

    public int itemsToPick = 5;
    private int itemsPicked = 0;
    private int totalCost;

    public MoneyConsumable[] moneyPrefabs;

    void Start()
    {
        timer = new Timer();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
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
        isMoveDone = false;

        if (target != null)
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

        ResetAnimatorTriggers();

        switch (currentState)
        {
            case CustomerState.Idle:
                animator.SetBool("Putting", false);
                Debug.Log("Animator Walking set to false");
                break;
            case CustomerState.WalkingToShelf:
                animator.SetBool("Walking", true);
                Debug.Log("Animator Walking set to true");
                break;
            case CustomerState.PickingItem:
                animator.SetBool("Picking", true);
                Debug.Log("Animator Picking set to true");
                break;
            case CustomerState.WalkingToCounter:
                animator.SetBool("Walking", true);
                Debug.Log("Animator Walking set to true");
                break;
            case CustomerState.PlacingItem:
                animator.SetBool("Putting", true);
                Debug.Log("Animator Putting set to true");
                break;
            case CustomerState.GivingMoney:
                animator.SetBool("Putting", true);
                Debug.Log("Animator Putting set to true");
                break;
            case CustomerState.LeavingStore:
                animator.SetBool("Walking", true);
                Debug.Log("Animator Walking set to true");
                break;
        }
    }

    void ResetAnimatorTriggers()
    {
        animator.SetBool("Walking", false);
        animator.SetBool("Picking", false);
        animator.SetBool("Putting", false);
        Debug.Log("Animator triggers reset: Walking, Picking, Putting set to false");
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
                Consumable[] allItems = FindObjectsOfType<Consumable>();

                Consumable randomObj = allItems[Random.Range(0, allItems.Length)];

                if (randomObj != null)
                {
                    myItem.Add(randomObj);

                    randomObj.transform.parent = arm;
                    randomObj.transform.localEulerAngles = Vector3.zero;
                    randomObj.transform.localPosition = Vector3.zero;

                    itemsPicked++;
                    timer.Set(0.5f);
                    animator.SetBool("Picking", false);
                }
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
            totalCost = CalculateTotalCost();
            Debug.Log(totalCost);

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
                ChangeState(CustomerState.GivingMoney, 2.0f);
            }
        }
    }
    void GivingMoney()
    {
        if (timer.IsFinished())
        {
            int moneyToGive;

            if (Random.value < 0.5f)
            {
                moneyToGive = totalCost;
            }
            else
            {
                moneyToGive = Random.Range(totalCost, 50001);
            }

            int remainingAmount = moneyToGive;
            Debug.Log($"Total money to give: {moneyToGive}");

            System.Array.Sort(moneyPrefabs, (x, y) => y.money.value.CompareTo(x.money.value));

            while (remainingAmount > 0)
            {
                bool moneyGiven = false;

                foreach (MoneyConsumable moneyPrefab in moneyPrefabs)
                {
                    if (moneyPrefab.money.value <= remainingAmount)
                    {
                        GameObject moneyObj = Instantiate(moneyPrefab.money.moneyPrefab, arm);
                        moneyObj.transform.localPosition = Vector3.zero;
                        moneyObj.transform.localRotation = Quaternion.identity;
                        remainingAmount -= moneyPrefab.money.value;
                        Debug.Log($"Given money: {moneyPrefab.money.value}, remaining amount: {remainingAmount}");
                        moneyGiven = true;
                        break;
                    }
                }

                if (!moneyGiven)
                {
                    break;
                }
            }

            ChangeState(CustomerState.LeavingStore, 2.0f);
        }
    }

    int CalculateTotalCost()
    {
        int totalCost = 0;
        foreach (var consumable in myItem)
        {
            if (consumable != null)
            {
                totalCost += consumable.item.price;
            }
        }
        Debug.Log($"Total cost calculated: {totalCost}");
        return totalCost;
    }

    void LeavingStore()
    {
        target = exitPoint.transform;
    }
}
