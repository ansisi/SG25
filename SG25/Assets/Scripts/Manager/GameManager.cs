using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    public Dictionary<Item, int> cartItemCounts = new Dictionary<Item, int>();

    // 플레이어의 현재 소유 금액
    public int money = 100;
    // 플레이어의 현재 에너지
    public int energy = 100;
    // 플레이어의 현재 경험치
    public int experience = 0;

    // 게임 실행中に 사용되는 
    public int currentEnergy;
    public int currentMoney;
    public int currentExperience;

    TypingGame typingGameInstance;
    public int level;

    private DateTime startTime; // 게임 시작 시간

    public int levelUpThreshold = 5; // 레벨업 경험치 기준값
    public int maxExperience = 200; // 초기 최대 경험치
    public int maxExperienceHighLevel = 500; // 상위 레벨 최대 경험치

   

    

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
        currentExperience = experience; // 초기값 설정
        startTime = DateTime.Now; // 게임 시작 시간 저장
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

    // 경험치 를 획득하는 함수
    public void GainExperience(int amount)
    {
        // 게임 시작 일주일 내에 있는지 확인
        if (IsWithinFirstWeek())
        {
            // 경험치 획득 상한치를 최대 경험치로 설정
            currentExperience = Mathf.Min(currentExperience + amount, maxExperience);
        }
        else
        {
            // 레벨 기준에 따라 경험치 획득 상한치 조정
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

    // 경험치를잃는 함수
    public void LoseExperience(int amount)
    {
        currentExperience -= amount;
    }

    // 레벨업 체크 함수
    public void CheckForLevelUp()
    {
        if (currentExperience >= 100) // 레벨업 조건: 경험치 100 이상
        {
            currentExperience -= 100; // 레벨업 시 경험치 차감
            level++; // 레벨 증가

            // 레벨업에 따른 보상 처리 (경험치 획득량 증가 등)
        }
    }

    public void StartCalculation()
    {
        // 계산 시작 (타이핑 게임 등)
    }

    public void FinishCalculation(bool isCorrect)
    {
        if (isCorrect)
        {
            GainExperience(20); // 정답 시 경험치 획득
        }

        CheckForLevelUp();
    }

    public bool IsWithinFirstWeek()
    {
        // 게임 시작 시간과 현재 시간의 차이 계산
        TimeSpan timeDiff = DateTime.Now - startTime;

        // 게임 시작 일주일 이내인지 확인
        return timeDiff.TotalDays <= 7;
    }

    public void Update()
    {
        CheckForLevelUp(); // 매 프레임 레벨업 체크
    }

   
}
