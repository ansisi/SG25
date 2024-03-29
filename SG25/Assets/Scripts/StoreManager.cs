using UnityEngine;
using System.Collections.Generic;

public class StoreManager : MonoBehaviour
{
    // 상품 가격을 저장하는 딕셔너리
    [SerializeField] private Dictionary<string, int> itemPrices = new Dictionary<string, int>();

    // 선택한 상품 목록
    [SerializeField] private List<string> selectedItems = new List<string>();

    // 계산 결과
    private int totalCost;

    void Start()
    {
        // 초기화는 여기서 하지 않고 Unity Inspector에서 설정하도록 합니다.
    }

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

    // 계산 완료 후 총 가격을 반환하는 함수
    public int CalculateTotalCost()
    {
        return totalCost;
    }

    // 계산 완료 후 선택한 상품 목록과 총 가격을 초기화하는 함수
    public void ClearSelection()
    {
        selectedItems.Clear();
        totalCost = 0;
    }

    // 물건을 좌클릭하여 바코드를 찍는 것처럼 상품 선택 및 Numpad로 금액 입력
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

        // Numpad를 사용하여 금액 입력
        for (int i = 0; i <= 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Keypad0 + i))
            {
                totalCost = totalCost * 10 + i;
            }
        }

        // Enter를 눌러 결제
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("결제 완료. 총 가격: " + totalCost);
            int change = totalCost - CalculateTotalCost();
            Debug.Log("거스름돈: " + change);
            ClearSelection();
        }
    }
}
