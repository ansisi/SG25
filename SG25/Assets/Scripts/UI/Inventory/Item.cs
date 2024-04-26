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
    public int index;
}
