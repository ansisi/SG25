using UnityEngine;

public class Item : ScriptableObject
{
    public enum ItemType
    {
        Snack,
        Drink,
        Frozen,
        Cigarette
    }

    public ItemType itemType;
    public string itemName;
    public int price;
    public GameObject itemPrefab;
    public Sprite icon;
    public string description;

    // 콘솔창에 아이템 정보 출력 메서드
    public void PrintItemInfo()
    {
        Debug.Log("아이템 정보: " + itemName + " (" + description + ")");
    }
}
