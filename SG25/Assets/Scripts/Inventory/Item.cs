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

    [System.Serializable]
    public struct STAT
    {
        public string ItemName;
        public int price;
    }

    public List<STAT> stats = new List<STAT>();

    public Sprite icon;
    public string description;
}
