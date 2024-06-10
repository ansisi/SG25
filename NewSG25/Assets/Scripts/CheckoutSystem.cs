using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheckoutSystem : MonoBehaviour
{
    // ���õ� ������ ���
    public List<ItemData> selectedItems = new List<ItemData>();
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
                    selectedItems.Add(hit.collider.gameObject.GetComponent<ItemData>());                   
                    ProcessPayment();
                    Destroy(hit.collider.gameObject);
                }
            }
        }
    }

        // ������ ó���ϴ� �޼���
        public void ProcessPayment()
        {
            Debug.Log("ProcessPayment");

            // ���õ� ��� �������� ������ �ջ�
            foreach (ItemData item in selectedItems)
            {
                totalCost += item.cost;
            }

            // �÷��̾��� ���� ������ �Ѿ� �̻����� Ȯ��
            if (GameManager.Instance.currentMoney >= totalCost)
            {
                // ���� ����
                GameManager.Instance.currentMoney -= totalCost;
                //int change = GameManager.Instance.currentMoney;
                if(totalCostText != null)
                {
                    totalCostText.text = totalCost.ToString("N0") + "��";
                }


                // ���� �Ϸ� �� ���õ� ������ ��� �ʱ�ȭ
                selectedItems.Clear();
            }
            else
            {
                Debug.Log("Not enough money");
            }
        }
    }
