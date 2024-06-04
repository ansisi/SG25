using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;   
    public int money = 100; // �÷��̾��� ���� ���� �ݾ�    
    public int energy = 100; // �÷��̾��� ���� ������  
    public int currentMoney;
    public int currentExperience;    
    private DateTime startTime; // ���� ���� �ð� 

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
