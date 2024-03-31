using UnityEngine;
using System.Collections.Generic;

/*StoreManagerŬ������ 
1.��ǰ ���� ���� �� ����
2.��ǰ ����, �� ���� ���, ����
3.�Ž����� ��� �� ��ȯ
4.��ǰ ���� �ʱ�ȭ
5.���콺 Ŭ�� �Ǵ� Numpad�� ���� ��ǰ ���� �� �ݾ��� �Է��ϴ�
��ǰ ������ ���� �ý��ۿ� ������ �ڵ� �Դϴ�.*/


public class StoreManager : MonoBehaviour
{
    // ��ǰ ������ �����ϴ� ��ųʸ�
    [SerializeField] private Dictionary<string, int> itemPrices = new Dictionary<string, int>();

    // ������ ��ǰ ���
    [SerializeField] private List<string> selectedItems = new List<string>();

    // ���� �ݾ�
    private int totalCost;

    // �Ž�����
    private int change;

    // ��ǰ ������ �����ϴ� �Լ�
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

    // ��ǰ ������ ��ȯ�ϴ� �Լ�
    public int GetItemPrice(string itemName)
    {
        if (itemPrices.ContainsKey(itemName))
        {
            return itemPrices[itemName];
        }
        return -1; // �ش� ��ǰ�� ���� ��� -1�� ��ȯ�ϰų� �ٸ� ������� ó���� �� �ֽ��ϴ�.
    }

    // ��ǰ�� �����ϰ� ������ ����ϴ� �Լ�
    public void SelectItem(string itemName)
    {
        if (itemPrices.ContainsKey(itemName))
        {
            selectedItems.Add(itemName);
            totalCost += itemPrices[itemName];
            Debug.Log(itemName + " ���õ�. ���� �� ����: " + totalCost);
        }
        else
        {
            Debug.Log(itemName + "��(��) �Ǹ����� �ʴ� ��ǰ�Դϴ�.");
        }
    }

    // ���� �� �� ������ ��ȯ�ϴ� �Լ�
    public int CalculateTotalCost()
    {
        return totalCost;
    }

    // �Ž������� ����ϴ� �Լ�
    private void CalculateChange(int paidAmount)
    {
        change = paidAmount - totalCost;
    }

    // ���� �� �Ž������� ��ȯ�ϴ� �Լ�
    public int GetChange()
    {
        return change;
    }

    // ������ ��ǰ ��ϰ� �� ������ �ʱ�ȭ�ϴ� �Լ�
    public void ClearSelection()
    {
        selectedItems.Clear();
        totalCost = 0;
        change = 0;
    }

    // ������ �����ϴ� ���
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

    // Numpad�� �Է��� �ݾ��� ����Ͽ� ��ȯ�ϴ� �Լ�
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

    // ���� ���
    void FixedUpdate()
    {
        // Enter Ű�� ������ ������ �̷������ �Ž������� ���˴ϴ�.
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("���� �Ϸ�. �� ����: " + totalCost);
            int paidAmount = CalculatePaidAmount(); // ������ �Է��� �ݾ��� �޾ƿ�
            CalculateChange(paidAmount); // �Ž����� ���
            Debug.Log("�Ž�����: " + change);
            ClearSelection(); // ������ ��ǰ ��ϰ� �� ���� �ʱ�ȭ

            // ���� �Ϸ� �� �Ž������� �˷��ִ� �޽����� ����մϴ�.
            Debug.Log("�Ž������� " + change + "���Դϴ�.");
        }
    }
}
