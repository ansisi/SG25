using UnityEngine;

public class GameManager_final : MonoBehaviour
{
    public int startingMoney = 100;
    private int currentMoney;

    private StoreManager storeManager;

    // ���� ���� �� ȣ��Ǵ� �Լ�
    void Start()
    {
        currentMoney = startingMoney;
        Debug.Log("���ӸӴ� �ʱ�ȭ: " + currentMoney);

        storeManager = GetComponent<StoreManager>();
    }

    // ���� ���ӸӴ� ��ȯ
    public int GetCurrentMoney()
    {
        return currentMoney;
    }

    // ���ӸӴ� �߰�
    public void AddMoney(int amount)
    {
        currentMoney += amount;
        Debug.Log("���ӸӴ� �߰�: " + amount + ", ���� ���ӸӴ�: " + currentMoney);
    }

    // ���ӸӴ� ����
    public void SpendMoney(int amount)
    {
        if (currentMoney >= amount)
        {
            currentMoney -= amount;
            Debug.Log("���ӸӴ� �Һ�: " + amount + ", ���� ���ӸӴ�: " + currentMoney);
        }
        else
        {
            Debug.Log("���ӸӴϰ� �����մϴ�!");
        }
    }

    // ���ӸӴϷ� ��ǰ ���� �Լ�
    public void BuyItem(string itemName)
    {
        if (storeManager != null)
        {
            int itemPrice = storeManager.GetItemPrice(itemName);
            if (itemPrice > 0)
            {
                if (currentMoney >= itemPrice)
                {
                    storeManager.SelectItem(itemName);
                    SpendMoney(itemPrice);
                }
                else
                {
                    Debug.Log("���ӸӴϰ� �����Ͽ� " + itemName + "��(��) ������ �� �����ϴ�.");
                }
            }
            else
            {
                Debug.Log(itemName + "��(��) �������� �ʴ� ��ǰ�Դϴ�.");
            }
        }
        else
        {
            Debug.Log("StoreManager�� �����ϴ�.");
        }
    }
}
