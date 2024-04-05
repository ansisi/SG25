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
        frozen,
        Cigarette
    }

    public ItemType itemType;


    public string ItemName;
    public int price;


    public Sprite icon;
    public string description;
}