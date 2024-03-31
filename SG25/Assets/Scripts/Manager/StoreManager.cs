using UnityEngine;
using System.Collections.Generic;

/*StoreManager클래스는 
1.상품 가격 설정 및 관리
2.상품 선택, 총 가격 계산, 결제
3.거스름돈 계산 및 반환
4.상품 선택 초기화
5.마우스 클릭 또는 Numpad를 통해 상품 선택 및 금액을 입력하는
상품 관리와 결재 시스템에 집중한 코드 입니다.*/


public class StoreManager : MonoBehaviour
{
    // 상품 가격을 저장하는 딕셔너리
    [SerializeField] private Dictionary<string, int> itemPrices = new Dictionary<string, int>();

    // 선택한 상품 목록
    [SerializeField] private List<string> selectedItems = new List<string>();

    // 결제 금액
    private int totalCost;

    // 거스름돈
    private int change;

    // 상품 가격을 설정하는 함수
    public void SetItemPrice(string itemName, int price)
    {
        if (!itemPrices.ContainsKey(itemName))
        {
            itemPrices.Add(itemName, price);
        }
        else
        {
            itemPrices[itemName] = price;
        }
    }

    // 상품 가격을 반환하는 함수
    public int GetItemPrice(string itemName)
    {
        if (itemPrices.ContainsKey(itemName))
        {
            return itemPrices[itemName];
        }
        return -1; // 해당 상품이 없을 경우 -1을 반환하거나 다른 방식으로 처리할 수 있습니다.
    }

    // 상품을 선택하고 가격을 계산하는 함수
    public void SelectItem(string itemName)
    {
        if (itemPrices.ContainsKey(itemName))
        {
            selectedItems.Add(itemName);
            totalCost += itemPrices[itemName];
            Debug.Log(itemName + " 선택됨. 현재 총 가격: " + totalCost);
        }
        else
        {
            Debug.Log(itemName + "은(는) 판매하지 않는 상품입니다.");
        }
    }

    // 결제 후 총 가격을 반환하는 함수
    public int CalculateTotalCost()
    {
        return totalCost;
    }

    // 거스름돈을 계산하는 함수
    private void CalculateChange(int paidAmount)
    {
        change = paidAmount - totalCost;
    }

    // 결제 후 거스름돈을 반환하는 함수
    public int GetChange()
    {
        return change;
    }

    // 선택한 상품 목록과 총 가격을 초기화하는 함수
    public void ClearSelection()
    {
        selectedItems.Clear();
        totalCost = 0;
        change = 0;
    }

    // 물건을 선택하는 기능
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                GameObject obj = hit.transform.gameObject;
                if (obj.CompareTag("Product"))
                {
                    string itemName = obj.name;
                    SelectItem(itemName);
                }
            }
        }
    }

    // Numpad로 입력한 금액을 계산하여 반환하는 함수
    private int CalculatePaidAmount()
    {
        int paidAmount = 0;
        for (int i = 0; i <= 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Keypad0 + i))
            {
                paidAmount = paidAmount * 10 + i;
            }
        }
        return paidAmount;
    }

    // 결제 기능
    void FixedUpdate()
    {
        // Enter 키를 누르면 결제가 이루어지고 거스름돈이 계산됩니다.
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("결제 완료. 총 가격: " + totalCost);
            int paidAmount = CalculatePaidAmount(); // 유저가 입력한 금액을 받아옴
            CalculateChange(paidAmount); // 거스름돈 계산
            Debug.Log("거스름돈: " + change);
            ClearSelection(); // 선택한 상품 목록과 총 가격 초기화

            // 결제 완료 후 거스름돈을 알려주는 메시지를 출력합니다.
            Debug.Log("거스름돈은 " + change + "원입니다.");
        }
    }
}
