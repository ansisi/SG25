using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheckoutSystem : MonoBehaviour
{
    // ���õ� ������ ���
    public List<itemModel> selectedItems = new List<itemModel>();
    // �÷��̾��� ��
    //public int playerMoney = 0;

    public int totalCost;
    // �ܾ��� ǥ���� �ؽ�Ʈ
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
                    //selectedItems.Add(gameObject.GetComponent<itemModel>());
                    ProcessPayment();
                }
            }
        }
    }

        // ������ ó���ϴ� �޼���
        public void ProcessPayment()
        {
            Debug.Log("��ǰ����������");
            // ���õ� ��� �������� ������ �ջ�
            foreach (itemModel item in selectedItems)
            {
                totalCost += item.cost;
            }

            // �÷��̾��� ���� ������ �Ѿ� �̻����� Ȯ��
            if (GameManager.Instance.currentMoney >= totalCost)
            {
                // ���� ����
                GameManager.Instance.currentMoney -= totalCost;
                //int change = GameManager.Instance.currentMoney;
                totalCostText.text = totalCost.ToString("N0") + "��";

                // ���� �Ϸ� �� ���õ� ������ ��� �ʱ�ȭ
                selectedItems.Clear();
            }
            else
            {
                Debug.Log("Not enough money");
            }
        }
    }
