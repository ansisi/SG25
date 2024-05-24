using UnityEngine;
using System.Collections.Generic;

public class Shelf : MonoBehaviour
{

    public int index;
    public Transform wayPoint;

    public Consumable[] items;

    private HashSet<int> usedIndices = new HashSet<int>();

    private Consumable AddRandomItem()
    {
        if (usedIndices.Count >= items.Length)
        {
            Debug.LogWarning("All items have been used.");
            return null; // ��� �������� ���� ��� null ��ȯ
        }

        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, items.Length);
        } while (usedIndices.Contains(randomIndex));

        usedIndices.Add(randomIndex);

        Consumable randomItem = items[randomIndex];
        items[randomIndex].gameObject.SetActive(false);
        return randomItem;
    }

    public void GotoHand(Transform handpos , Consumable consumable)
    {
        GameObject temp = (GameObject)Instantiate(consumable.gameObject);
        temp.transform.position = handpos.transform.position;
    }

    // �ʿ��� ��� usedIndices�� �ʱ�ȭ�ϴ� �޼ҵ� �߰�
    public void ResetUsedIndices()
    {
        usedIndices.Clear();
    }

}
