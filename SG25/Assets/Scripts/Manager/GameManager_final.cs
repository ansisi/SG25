using UnityEngine;

/*GameManager_fina클래스는 
1.게암머니 초가화 및 관리
2.게임머니 추가 및 감소
3.아이템 구매 및 게임머니 차감
4.게임머니 부족 시 구매제한
게임머니 관리와 아이템 구매 시스템에 중점을 둔 코드입니다.*/

/*public class GameManager_final : MonoBehaviour
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
        Item item = Resources.Load<Item>("Items/" + itemName); // 아이템 리소스 로드
        if (item != null)
        {
            // 아이템의 첫 번째 STAT 구조체의 price 필드를 가져옴
            int itemPrice = item.stats.Count > 0 ? item.stats[0].price : 0;

            if (itemPrice > 0)
            {
                if (currentMoney >= itemPrice)
                {
                    SpendMoney(itemPrice);
                    Debug.Log(itemName + "을(를) 구매하였습니다.");
                    // 여기에 아이템을 플레이어 인벤토리에 추가하는 코드를 추가할 수 있습니다.
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
            Debug.Log(itemName + "을(를) 찾을 수 없습니다.");
        }
    }
}
*/
