using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int money = 100;
    public int energy = 100;

    public int currentEnergy;
    public int currentMoney;

    TypingGame typingGameInstance;

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

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            SceneManager.LoadScene(1);
        }
    }
}
