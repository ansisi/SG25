using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    public Dictionary<Item, int> cartItemCounts = new Dictionary<Item, int>();

    // �÷��̾��� ���� ���� �ݾ�
    public int money = 100;
    // �÷��̾��� ���� ������
    public int energy = 100;
    // �÷��̾��� ���� ����ġ
    public int experience = 0;

    // ���� ������� ���Ǵ� 
    public int currentEnergy;
    public int currentMoney;
    public int currentExperience;

    
    public int level;

    private DateTime startTime; // ���� ���� �ð�

    public int levelUpThreshold = 5; // ������ ����ġ ���ذ�
    public int maxExperience = 200; // �ʱ� �ִ� ����ġ
    public int maxExperienceHighLevel = 500; // ���� ���� �ִ� ����ġ





    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        currentEnergy = energy;
        currentMoney = money;
        currentExperience = experience; // �ʱⰪ ����
        startTime = DateTime.Now; // ���� ���� �ð� ����
    }

    //������ ���� �ڵ�
    public void EnergyIncrease(int amount)
    {
        currentEnergy += amount;
    }

    public void EnergyDecrease(int amount)
    {
        currentEnergy -= amount;
    }

    public void MoneyIncrease(int amount)
    {
        currentMoney += amount;
    }

    public void MoneyDecrease(int amount)
    {
        currentMoney -= amount;
    }

    // ����ġ �� ȹ���ϴ� �Լ�
    public void GainExperience(int amount)
    {
        // ���� ���� ������ ���� �ִ��� Ȯ��
        if (IsWithinFirstWeek())
        {
            // ����ġ ȹ�� ����ġ�� �ִ� ����ġ�� ����
            currentExperience = Mathf.Min(currentExperience + amount, maxExperience);
        }
        else
        {
            // ���� ���ؿ� ���� ����ġ ȹ�� ����ġ ����
            if (level >= levelUpThreshold)
            {
                currentExperience = Mathf.Min(currentExperience + amount, maxExperienceHighLevel);
            }
            else
            {
                currentExperience = Mathf.Min(currentExperience + amount, maxExperience);
            }
        }
    }

    // ����ġ���Ҵ� �Լ�
    public void LoseExperience(int amount)
    {
        currentExperience -= amount;
    }

    // ������ üũ �Լ�
    public void CheckForLevelUp()
    {
        if (currentExperience >= 100) // ������ ����: ����ġ 100 �̻�
        {
            currentExperience -= 100; // ������ �� ����ġ ����
            level++; // ���� ����

            // �������� ���� ���� ó�� (����ġ ȹ�淮 ���� ��)
        }
    }

    public void StartCalculation()
    {
        // ��� ���� (Ÿ���� ���� ��)
    }

    public void FinishCalculation(bool isCorrect)
    {
        if (isCorrect)
        {
            GainExperience(20); // ���� �� ����ġ ȹ��
        }

        CheckForLevelUp();
    }

    public bool IsWithinFirstWeek()
    {
        // ���� ���� �ð��� ���� �ð��� ���� ���
        TimeSpan timeDiff = DateTime.Now - startTime;

        // ���� ���� ������ �̳����� Ȯ��
        return timeDiff.TotalDays <= 7;
    }

    public void Update()
    {
        CheckForLevelUp(); // �� ������ ������ üũ
    }

    // OrderPanel ���¸� PlayerPrefs �Ǵ� �ٸ� ����ҿ� �����ϴ� �Լ�
    public void SaveOrderPanelState()
    {
        // ��ٱ��� ������ ���� ����
        foreach (var pair in cartItemCounts)
        {
            PlayerPrefs.SetInt(pair.Key.itemName, pair.Value);
        }

        // �ʿ��ϴٸ� ��Ÿ ���� ������ ����
        PlayerPrefs.Save();
    }

    // OrderPanel ���¸� PlayerPrefs �Ǵ� �ٸ� ����ҿ��� �ҷ����� �Լ�
    public void LoadOrderPanelState()
    {
        foreach (var item in cartItemCounts.Keys)
        {
            if (PlayerPrefs.HasKey(item.itemName))
            {
                cartItemCounts[item] = PlayerPrefs.GetInt(item.itemName);
            }
        }

        // �ʿ��ϴٸ� ��Ÿ ���� ������ �ҷ�����
    }
}
