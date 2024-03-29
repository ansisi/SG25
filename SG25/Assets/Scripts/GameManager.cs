using UnityEngine;

//게임 플레이 돈 코드 입니다. 계산 코드 아니에요

public class GameManager : MonoBehaviour
{
    public int startingMoney = 100;
    private int currentMoney;

    // 게임 시작 시 호출되는 함수
    void Start()
    {
        currentMoney = startingMoney;
        Debug.Log("게임머니 초기화: " + currentMoney);
    }

    // 현재 게임머니 반환
    public int GetCurrentMoney()
    {
        return currentMoney;
    }

    // 게임머니 추가
    public void AddMoney(int amount)
    {
        currentMoney += amount;
        Debug.Log("게임머니 추가: " + amount + ", 현재 게임머니: " + currentMoney);
    }

    // 게임머니 감소
    public void SpendMoney(int amount)
    {
        if (currentMoney >= amount)
        {
            currentMoney -= amount;
            Debug.Log("게임머니 소비: " + amount + ", 현재 게임머니: " + currentMoney);
        }
        else
        {
            Debug.Log("게임머니가 부족합니다!");
        }
    }
}
