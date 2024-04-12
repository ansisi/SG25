using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // 게임 경험치 코드
    public int money = 100;
    public int energy = 100;
    public int experience = 0; // 경험치 변수 추가

    public int currentEnergy;
    public int currentMoney;
    public int currentExperience; // 현재 경험치 변수 추가

    TypingGame typingGameInstance;
    public int level;

    private DateTime startTime; // 플레이 시작 시간

    public int levelUpThreshold = 5; // 레벨 업 한도
    public int maxExperience = 200; // 최대 경험치 (일주일 이내)
    public int maxExperienceHighLevel = 500; // 고레벨 최대 경험치

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
        startTime = DateTime.Now; // 플레이 시작 시간 저장
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
        // 일주일 이내인지 확인
        if (IsWithinFirstWeek())
        {
            // 최대 경험치를 초과하지 않도록 제한
            currentExperience = Mathf.Min(currentExperience + amount, maxExperience);
        }
        else
        {
            // 현재 레벨이 n레벨 이상인지 확인
            if (level >= levelUpThreshold)
            {
                // 고레벨 최대 경험치를 초과하지 않도록 제한
                currentExperience = Mathf.Min(currentExperience + amount, maxExperienceHighLevel);
            }
            else
            {
                // 일반 최대 경험치를 초과하지 않도록 제한
                currentExperience = Mathf.Min(currentExperience + amount, maxExperience);
            }
        }
    }

    // 경험치 감소 함수
    public void LoseExperience(int amount)
    {
        currentExperience -= amount;
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

    public void StartCalculation()
    {
        // 계산 시작 시 코드 실행 (예: 타이핑 게임 시작)
    }

    public void FinishCalculation(bool isCorrect)
    {
        if (isCorrect)
        {
            GainExperience(20); // n = 20으로 설정
        }

        CheckForLevelUp();
    }

    public bool IsWithinFirstWeek()
    {
        // 플레이 시작 시간을 저장하는 변수가 필요합니다.
        // 예시: DateTime startTime = DateTime.Now;

        // 현재 시간과 플레이 시작 시간의 차이를 계산합니다.
        TimeSpan timeDiff = DateTime.Now - startTime;

        // 일주일 이내인지 확인합니다.
        return timeDiff.TotalDays <= 7;
    }

    public void Update()
    {
        CheckForLevelUp(); // 매 프레임마다 레벨 업 확인
    }
}
