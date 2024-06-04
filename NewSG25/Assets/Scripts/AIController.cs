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

    public Transform target;
    public Transform counter;
    public Transform arm;
    public Transform exitPoint;

    public List<GameObject> targetPos = new List<GameObject>();
    public List<GameObject> myItem = new List<GameObject>();

    public int cntToPick = 2;
    private int cntPicked = 0;

    private static int nextPriority = 0;
    private static readonly object priorityLock = new object();

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
            target = targetPos[Random.Range(0, targetPos.Count)].transform;
            MoveToTarget();
            ChangeState(CustomerState.WalkingToShelf, waitTime);
        }
    }

    void WalkingToShelf()
    {
        if (timer.IsFinished() && isMoveDone )
        {
            ChangeState(CustomerState.PickingItem, waitTime);
        }
    }

    void PickingItem()
    {
        if (timer.IsFinished())
        {           
            Shelf shelf = target.GetComponent<Shelf>();
            
            if (shelf != null)
            {
                GameObject itemPicked = shelf.RandomPickItem();

                if(itemPicked == null)
                {
                    timer.Set(1.0f);
                }

                if (itemPicked != null )
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
        }
    }

    void PlacingItem()
    {
        Vector3 offSet = new Vector3 (0f, 1f, 0f);

        if (timer.IsFinished())
        {
            if (myItem != null && myItem.Count != 0)
            {
                offSet += new Vector3 (1 * myItem.Count - 2.5f, 0f, 0f);
                myItem[myItem.Count-1].transform.position = counter.transform.position + offSet;
                myItem[myItem.Count - 1].transform.parent = null;              
                myItem[myItem.Count - 1].transform.rotation = Quaternion.Euler(0f,0f,0f);
                myItem[myItem.Count - 1].AddComponent<Rigidbody>();
                myItem.RemoveAt(myItem.Count - 1);
                timer.Set(0.1f);
            }
            else
            {
                cntPicked = 0;              
                ChangeState(CustomerState.Idle, waitTime);
            }
        }
    }

    void WaitingCalcPrice()
    {
        if (isFinishedCalcPrice)
        {
            ChangeState(CustomerState.GivingMoney, waitTime);
        }
    }

    void GivingMoney()
    {
        //if (애니메이션 종료시 확인 Bool)
        {
            ChangeState(CustomerState.GivingMoney, waitTime);
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

    public void GoToHand(Transform handPos, GameObject item)
    {       
        Vector3 offSet = Vector3.zero;

        if (myItem.Count > 0)
        {
            for (int i = 0; i < myItem.Count; i++)
            {
                offSet += new Vector3(0f, myItem[i].GetComponent<BoxCollider>().size.y * myItem[i].transform.localScale.y, 0f);
            }
        }

        GameObject temp = (GameObject)Instantiate(item);
        temp.transform.position = handPos.transform.position + offSet;
        temp.transform.parent = handPos;

        myItem.Add(temp);
       
    }

}
