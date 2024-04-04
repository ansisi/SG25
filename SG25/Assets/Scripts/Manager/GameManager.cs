using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int money = 100;
    public int energy = 100;

    public int currentEnergy;
    public int currentMoney;

    TypingGame typingGameInstance = new TypingGame();

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

    public void EnergyDecrease(int amount)
    {
        currentEnergy -= amount;
    }


}
