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
            return null; // 모든 아이템이 사용된 경우 null 반환
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

    // 필요한 경우 usedIndices를 초기화하는 메소드 추가
    public void ResetUsedIndices()
    {
        usedIndices.Clear();
    }

}
