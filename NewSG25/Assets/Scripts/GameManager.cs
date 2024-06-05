using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject trashObject;
    public int money = 100; // 플레이어의 현재 소유 금액    
    public int energy = 100; // 플레이어의 현재 에너지  
    public int currentMoney;
    public int currentExperience;   
    
    public float trashTimer;
    public float checkCount;
    public int trashCounter;

    private void Awake()
    {     
        Instance = this;           
        currentMoney = money;
    }

    public void Update()
    {
        Timer();



    }

    public void Timer()
    {
        trashTimer -= Time.deltaTime;

        if (checkCount >= 0)
            checkCount -= Time.deltaTime;

        if (trashTimer <= 0)
        {
            GenTrash();
            trashTimer += 20.0f;
            checkCount = 5.0f;
        }

        if (checkCount <= 0)
        {
            CheckTrashCount();
        }
    }

    public void CheckTrashCount()
    {
        Debug.Log("쓰레기 체크 : " + trashCounter);
    }

    public void TrashCount(int count)
    {
        trashCounter += count;
    }

    public void GenTrash()
    {

        for (int i = 0; i < 5; i++)
        {
            // 맵 크기
            float mapWidth = 10.0f;
            float mapHeight = 10.0f;

            // 랜덤 위치 생성
            Vector3 randomPosition = new Vector3(
                Random.Range(-mapWidth / 2, mapWidth / 2),
                0.2f, // Y 좌표를 1로 고정
                Random.Range(-mapHeight / 2, mapHeight / 2)
            );

            GameObject temp = Instantiate(trashObject);
            temp.transform.position = randomPosition;

            trashCounter++;
        }
        
    }

    public void MoneyIncrease(int amount)
    {
        currentMoney += amount;
    }

    public void MoneyDecrease(int amount)
    {
        currentMoney -= amount;
    }
}
