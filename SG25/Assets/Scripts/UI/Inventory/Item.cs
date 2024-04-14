using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
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
}