using UnityEngine;

//���� �÷��� �� �ڵ� �Դϴ�. ��� �ڵ� �ƴϿ���

public class GameManager : MonoBehaviour
{
    public int startingMoney = 100;
    private int currentMoney;

    // ���� ���� �� ȣ��Ǵ� �Լ�
    void Start()
    {
        currentMoney = startingMoney;
        Debug.Log("���ӸӴ� �ʱ�ȭ: " + currentMoney);
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
}
