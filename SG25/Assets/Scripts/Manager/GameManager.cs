using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    //���� ����ġ �ڵ�
    public int money = 100;
    public int energy = 100;
    public int experience = 0; // ����ġ ���� �߰�

    public int currentEnergy;
    public int currentMoney;
    public int currentExperience; // ���� ����ġ ���� �߰�

    TypingGame typingGameInstance;
    private int level;

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
        currentExperience += amount;
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

    public void Update()
    {
        CheckForLevelUp(); // �� �����Ӹ��� ���� �� Ȯ��

        if (Input.GetKeyDown(KeyCode.V))
        {
            SceneManager.LoadScene(1);
        }
    }
}
