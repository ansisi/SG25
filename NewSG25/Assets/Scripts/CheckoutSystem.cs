using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheckoutSystem : MonoBehaviour
{
    // 선택된 아이템 목록
    public List<ItemData> selectedItems = new List<ItemData>();
    // 플레이어의 돈
    //public int playerMoney = 0;

    public int totalCost;
    // 잔액을 표시할 텍스트
    public TextMeshProUGUI totalCostText;

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

            // 플레이어의 돈이 결제할 총액 이상인지 확인
            if (GameManager.Instance.currentMoney >= totalCost)
            {
                // 결제 진행
                GameManager.Instance.currentMoney -= totalCost;
                //int change = GameManager.Instance.currentMoney;
                if(totalCostText != null)
                {
                    totalCostText.text = totalCost.ToString("N0") + "원";
                }


                // 결제 완료 후 선택된 아이템 목록 초기화
                selectedItems.Clear();
            }
            else
            {
                Debug.Log("Not enough money");
            }
        }
    }
