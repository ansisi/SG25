using UnityEngine;
using System.Collections.Generic;

public class StoreManager : MonoBehaviour
{
    // ��ǰ ������ �����ϴ� ��ųʸ�
    [SerializeField] private Dictionary<string, int> itemPrices = new Dictionary<string, int>();

    // ������ ��ǰ ���
    [SerializeField] private List<string> selectedItems = new List<string>();

    // ��� ���
    private int totalCost;

    void Start()
    {
        // �ʱ�ȭ�� ���⼭ ���� �ʰ� Unity Inspector���� �����ϵ��� �մϴ�.
    }

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

    // ��� �Ϸ� �� �� ������ ��ȯ�ϴ� �Լ�
    public int CalculateTotalCost()
    {
        return totalCost;
    }

    // ��� �Ϸ� �� ������ ��ǰ ��ϰ� �� ������ �ʱ�ȭ�ϴ� �Լ�
    public void ClearSelection()
    {
        selectedItems.Clear();
        totalCost = 0;
    }

    // ������ ��Ŭ���Ͽ� ���ڵ带 ��� ��ó�� ��ǰ ���� �� Numpad�� �ݾ� �Է�
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

        // Numpad�� ����Ͽ� �ݾ� �Է�
        for (int i = 0; i <= 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Keypad0 + i))
            {
                totalCost = totalCost * 10 + i;
            }
        }

        // Enter�� ���� ����
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("���� �Ϸ�. �� ����: " + totalCost);
            int change = totalCost - CalculateTotalCost();
            Debug.Log("�Ž�����: " + change);
            ClearSelection();
        }
    }
}
