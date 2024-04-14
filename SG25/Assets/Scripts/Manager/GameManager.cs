using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Dictionary<Item, int> cartItemCounts = new Dictionary<Item, int>();

    // ���� ����ġ �ڵ�
    public int money = 100;
    public int energy = 100;
    public int experience = 0; // ����ġ ���� �߰�

    public int currentEnergy;
    public int currentMoney;
    private int currentExperience; // ���� ����ġ ���� �߰�

    TypingGame typingGameInstance;
    public int level;

    private DateTime startTime; // �÷��� ���� �ð�

    public int levelUpThreshold = 5; // ���� �� �ѵ�
    public int maxExperience = 200; // �ִ� ����ġ (������ �̳�)
    public int maxExperienceHighLevel = 500; // ����� �ִ� ����ġ

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
        currentExperience = experience; // ����ġ �ʱ�ȭ
        startTime = DateTime.Now; // �÷��� ���� �ð� ����
    }

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

    // ����ġ ���� �Լ�
    public void GainExperience(int amount)
    {
        // ������ �̳����� Ȯ��
        if (IsWithinFirstWeek())
        {
            // �ִ� ����ġ�� �ʰ����� �ʵ��� ����
            currentExperience = Mathf.Min(currentExperience + amount, maxExperience);
        }
        else
        {
            // ���� ������ n���� �̻����� Ȯ��
            if (level >= levelUpThreshold)
            {
                // ����� �ִ� ����ġ�� �ʰ����� �ʵ��� ����
                currentExperience = Mathf.Min(currentExperience + amount, maxExperienceHighLevel);
            }
            else
            {
                // �Ϲ� �ִ� ����ġ�� �ʰ����� �ʵ��� ����
                currentExperience = Mathf.Min(currentExperience + amount, maxExperience);
            }
        }
    }

    // ����ġ ���� �Լ�
    public void LoseExperience(int amount)
    {
        currentExperience -= amount;
    }

    // ����ġ ���� �� ��� (����)
    public void CheckForLevelUp()
    {
        if (currentExperience >= 100) // ������ ����
        {
            currentExperience -= 100; // ����ġ ����
            level++; // ���� ����

            // ���� �� ȿ�� ���� (��: �ɷ�ġ ���, ���ο� ��� ���� ��)
        }
    }

    public void StartCalculation()
    {
        // ��� ���� �� �ڵ� ���� (��: Ÿ���� ���� ����)
    }

    public void FinishCalculation(bool isCorrect)
    {
        if (isCorrect)
        {
            GainExperience(20); // n = 20���� ����
        }

        CheckForLevelUp();
    }

    public bool IsWithinFirstWeek()
    {
        // �÷��� ���� �ð��� �����ϴ� ������ �ʿ��մϴ�.
        // ����: DateTime startTime = DateTime.Now;

        // ���� �ð��� �÷��� ���� �ð��� ���̸� ����մϴ�.
        TimeSpan timeDiff = DateTime.Now - startTime;

        // ������ �̳����� Ȯ���մϴ�.
        return timeDiff.TotalDays <= 7;
    }

    public void Update()
    {
        CheckForLevelUp(); // �� �����Ӹ��� ���� �� Ȯ��
    }
}
