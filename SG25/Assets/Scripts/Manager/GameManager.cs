using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    //게임 경험치 코드
    public int money = 100;
    public int energy = 100;
    public int experience = 0; // 경험치 변수 추가

    public int currentEnergy;
    public int currentMoney;
    public int currentExperience; // 현재 경험치 변수 추가

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
        currentExperience = experience; // 경험치 초기화
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

    // 경험치 증가 함수
    public void GainExperience(int amount)
    {
        currentExperience += amount;
    }

    // 경험치 레벨 업 기능 (예시)
    public void CheckForLevelUp()
    {
        if (currentExperience >= 100) // 레벨업 조건
        {
            currentExperience -= 100; // 경험치 감소
            level++; // 레벨 증가
            // 레벨 업 효과 적용 (예: 능력치 상승, 새로운 기능 해제 등)
        }
    }

    public void Update()
    {
        CheckForLevelUp(); // 매 프레임마다 레벨 업 확인

        if (Input.GetKeyDown(KeyCode.V))
        {
            SceneManager.LoadScene(1);
        }
    }
}
