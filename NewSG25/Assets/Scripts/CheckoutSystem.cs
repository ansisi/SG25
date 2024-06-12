using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CheckoutSystem : MonoBehaviour
{
    // ���õ� ������ ���
    public List<ItemData> selectedItems = new List<ItemData>();

    public int totalCost;
    // �ܾ��� ǥ���� �ؽ�Ʈ
    public TextMeshProUGUI totalCostText;
    public TextMeshProUGUI takeMoneyText;
    public AIController aiController;

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
                    ItemData itemData = hit.collider.gameObject.GetComponent<ItemData>();

                    // �̹� ���õ� ���������� Ȯ��
                    if (!selectedItems.Contains(itemData))
                    {
                        selectedItems.Add(itemData);
                        ProcessPayment();
                    }

                    hit.collider.gameObject.SetActive(false);
                }

                if (hit.collider.CompareTag("Money"))
                {
                    Money money = hit.collider.gameObject.GetComponent<Money>();
                    GameManager.Instance.currentMoney += money.money.value;

                    takeMoney += money.money.value;
                    takeMoneyText.text = takeMoney.ToString("N0");

                    Destroy(hit.collider.gameObject);
                }
            }
        }
    }

    // ������ ó���ϴ� �޼���
    public void ProcessPayment()
    {
        Debug.Log("ProcessPayment");

        // ���� ó�� ���� �ʱ�ȭ
        totalCost = 0;

        // ���õ� ��� �������� ������ �ջ�
        foreach (ItemData item in selectedItems)
        {
            totalCost += item.sellCost;
        }

        if (totalCostText != null)
        {
            totalCostText.text = totalCost.ToString("N0") + "��";
        }

        // ���� �ð� �Ŀ� ���õ� ������ ����� �ʱ�ȭ
        StartCoroutine(ResetSelectedItemsAfterDelay(3f));
    }

    private IEnumerator ResetSelectedItemsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        selectedItems.Clear();
        totalCost = 0;
        takeMoney = 0;

        if (totalCostText != null)
        {
            totalCostText.text = totalCost.ToString("N0");
        }
        if (takeMoneyText != null)
        {
            takeMoneyText.text = takeMoney.ToString("N0");
        }
    }
}
