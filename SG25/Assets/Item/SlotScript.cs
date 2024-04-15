using UnityEngine;

public class SlotScript : MonoBehaviour
{
    public ScriptableObject currentItem; // ������ Ÿ�� ���� (Item -> ScriptableObject)
    //public Image itemImage;

    public void SetItem(ScriptableObject item) // ������ Ÿ�� ���� (Item -> ScriptableObject)
    {
        currentItem = item;

        if (item != null)
        {
            // ������ �̹��� �ּ� ó�� (//itemImage.sprite = item.icon;)
            // ������ ���� ǥ��
            string itemName = item.name; // ������ �̸� ���� (item.itemName -> item.name)
            string itemDescription = item.ToString(); // ������ ���� ���ڿ� ��ȯ

            // ... (������ ���� ǥ�� ����) ...

            // �ܼ�â�� ������ ���� ���
            Debug.Log("������ ä����: " + itemName + " (" + itemDescription + ")");
        }
        else
        {
            //itemImage.sprite = null;
        }
    }
}
