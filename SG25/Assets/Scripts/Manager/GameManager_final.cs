using UnityEngine;

/*GameManager_finaŬ������ 
1.�ԾϸӴ� �ʰ�ȭ �� ����
2.���ӸӴ� �߰� �� ����
3.������ ���� �� ���ӸӴ� ����
4.���ӸӴ� ���� �� ��������
���ӸӴ� ������ ������ ���� �ý��ۿ� ������ �� �ڵ��Դϴ�.*/

/*public class GameManager_final : MonoBehaviour
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
        Item item = Resources.Load<Item>("Items/" + itemName); // ������ ���ҽ� �ε�
        if (item != null)
        {
            // �������� ù ��° STAT ����ü�� price �ʵ带 ������
            int itemPrice = item.stats.Count > 0 ? item.stats[0].price : 0;

            if (itemPrice > 0)
            {
                if (currentMoney >= itemPrice)
                {
                    SpendMoney(itemPrice);
                    Debug.Log(itemName + "��(��) �����Ͽ����ϴ�.");
                    // ���⿡ �������� �÷��̾� �κ��丮�� �߰��ϴ� �ڵ带 �߰��� �� �ֽ��ϴ�.
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
            Debug.Log(itemName + "��(��) ã�� �� �����ϴ�.");
        }
    }
}
*/
