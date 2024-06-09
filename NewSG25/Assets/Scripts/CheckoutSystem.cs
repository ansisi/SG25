using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckoutSystem : MonoBehaviour
{
    // 선택된 아이템 목록
    public List<Item> selectedItems = new List<Item>();
    // 플레이어의 돈
    public int playerMoney = 0;
    // 잔액을 표시할 텍스트
    public TextMesh changeText;

    // 결제를 처리하는 메서드
    public void ProcessPayment()
    {
        int totalPrice = 0;

        // 선택된 모든 아이템의 가격을 합산
        foreach (Item item in selectedItems)
        {
            totalPrice += item.price;
        }

        // 플레이어의 돈이 결제할 총액 이상인지 확인
        if (playerMoney >= totalPrice)
        {
            // 결제 진행
            playerMoney -= totalPrice;
            int change = playerMoney;
            changeText.text = "Change: " + change.ToString() + "원";

            // 결제 완료 후 선택된 아이템 목록 초기화
            selectedItems.Clear();
        }
        else
        {
            Debug.Log("Not enough money");
        }
    }
}
