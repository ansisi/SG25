using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CheckoutSystem : MonoBehaviour
{
    // 선택된 아이템 목록
    public List<ItemData> selectedItems = new List<ItemData>();

    public AIController aiController;

    public int totalCost;
    // 잔액을 표시할 텍스트
    public TextMeshProUGUI totalCostText;
    public TextMeshProUGUI takeMoneyText;

    private int takeMoney = 0;

    private void Start()
    {
        aiController = FindObjectOfType<AIController>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Item"))
                {
                    selectedItems.Add(hit.collider.gameObject.GetComponent<ItemData>());
                    ProcessPayment();
                    aiController.counterItem.Remove(hit.collider.gameObject);
                    Destroy(hit.collider.gameObject);
                }

                if (hit.collider.CompareTag("Money"))
                {
                    Money money = hit.collider.gameObject.GetComponent<Money>();
                    aiController.moneyToGive.Remove(hit.collider.gameObject);
                    GameManager.Instance.currentMoney += money.money.value;

                    takeMoney += money.money.value;

                    takeMoneyText.text = takeMoney.ToString("N0");
                    Destroy(hit.collider.gameObject);
                }
            }

        }

    }

    // 결제를 처리하는 메서드
    public void ProcessPayment()
    {
        Debug.Log("ProcessPayment");

        // 선택된 모든 아이템의 가격을 합산
        foreach (ItemData item in selectedItems)
        {
            totalCost += item.cost;
        }
        if (totalCostText != null)
        {
            totalCostText.text = totalCost.ToString("N0") + "원";
        }

        aiController.totalAmount = totalCost;
    }
}
