using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;   
    public int money = 100; // 플레이어의 현재 소유 금액    
    public int energy = 100; // 플레이어의 현재 에너지  
    public int currentMoney;
    public int currentExperience;    
    private DateTime startTime; // 게임 시작 시간 

    private void Awake()
    {     
        Instance = this;           
        currentMoney = money;
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
