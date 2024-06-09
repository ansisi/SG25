using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckoutSystem : MonoBehaviour
{
    // ���õ� ������ ���
    public List<Item> selectedItems = new List<Item>();
    // �÷��̾��� ��
    public int playerMoney = 0;
    // �ܾ��� ǥ���� �ؽ�Ʈ
    public TextMesh changeText;

    // ������ ó���ϴ� �޼���
    public void ProcessPayment()
    {
        int totalPrice = 0;

        // ���õ� ��� �������� ������ �ջ�
        foreach (Item item in selectedItems)
        {
            totalPrice += item.price;
        }

        // �÷��̾��� ���� ������ �Ѿ� �̻����� Ȯ��
        if (playerMoney >= totalPrice)
        {
            // ���� ����
            playerMoney -= totalPrice;
            int change = playerMoney;
            changeText.text = "Change: " + change.ToString() + "��";

            // ���� �Ϸ� �� ���õ� ������ ��� �ʱ�ȭ
            selectedItems.Clear();
        }
        else
        {
            Debug.Log("Not enough money");
        }
    }
}
