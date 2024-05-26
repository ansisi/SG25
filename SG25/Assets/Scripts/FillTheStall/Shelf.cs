using System.Collections.Generic;
using UnityEngine;

public class Shelf : MonoBehaviour
{
    public int index;
    public Transform wayPoint1;
    public Transform wayPoint2;

    public Consumable[] items1;
    public Consumable[] items2;

    private HashSet<int> usedIndices1 = new HashSet<int>();
    private HashSet<int> usedIndices2 = new HashSet<int>();

    public List<Consumable> AddRandomItems1(out int itemCount)
    {
        List<Consumable> selectedItems = new List<Consumable>();
        itemCount = 0;

        if (usedIndices1.Count >= items1.Length)
        {
            Debug.LogWarning("모든 아이템이 사용되었습니다.");
            return selectedItems;
        }

        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, items1.Length);
        } while (usedIndices1.Contains(randomIndex));

        usedIndices1.Add(randomIndex);
        Consumable randomItem = items1[randomIndex];
        itemCount = Random.Range(1, 11);

        for (int i = 0; i < itemCount; i++)
        {
            selectedItems.Add(randomItem);
        }

        items1[randomIndex].gameObject.SetActive(false);
        return selectedItems;
    }

    public List<Consumable> AddRandomItems2(out int itemCount)
    {
        List<Consumable> selectedItems = new List<Consumable>();
        itemCount = 0;

        if (usedIndices2.Count >= items2.Length)
        {
            Debug.LogWarning("모든 아이템이 사용되었습니다.");
            return selectedItems;
        }

        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, items2.Length);
        } while (usedIndices2.Contains(randomIndex));

        usedIndices2.Add(randomIndex);
        Consumable randomItem = items2[randomIndex];
        itemCount = Random.Range(1, 11);

        for (int i = 0; i < itemCount; i++)
        {
            selectedItems.Add(randomItem);
        }

        items2[randomIndex].gameObject.SetActive(false);
        return selectedItems;
    }

    public void GotoHand(Transform handpos, List<Consumable> consumables)
    {
        foreach (Consumable consumable in consumables)
        {
            GameObject temp = Instantiate(consumable.gameObject);
            temp.SetActive(true);
            temp.transform.SetParent(handpos);
            temp.transform.position = handpos.transform.position;
        }
    }

    public void ResetUsedIndices()
    {
        usedIndices1.Clear();
        usedIndices2.Clear();
    }
}
