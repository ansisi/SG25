using UnityEngine;

public class GameManager_final : MonoBehaviour
{
    public int startingMoney = 100;
    private int currentMoney;

    private StoreManager storeManager;

    // 게임 시작 시 호출되는 함수
    void Start()
    {
        currentMoney = startingMoney;
        Debug.Log("게임머니 초기화: " + currentMoney);

        storeManager = GetComponent<StoreManager>();
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

    // 게임머니로 상품 구매 함수
    public void BuyItem(string itemName)
    {
        if (storeManager != null)
        {
            int itemPrice = storeManager.GetItemPrice(itemName);
            if (itemPrice > 0)
            {
                if (currentMoney >= itemPrice)
                {
                    storeManager.SelectItem(itemName);
                    SpendMoney(itemPrice);
                }
                else
                {
                    Debug.Log("게임머니가 부족하여 " + itemName + "을(를) 구매할 수 없습니다.");
                }
            }
            else
            {
                Debug.Log(itemName + "은(는) 존재하지 않는 상품입니다.");
            }
        }
        else
        {
            Debug.Log("StoreManager가 없습니다.");
        }
    }
}
