using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "ScriptableObjects/ItemModel")]
[SerializeField]
public class itemModel : ScriptableObject
{
    public enum ITEMTYPE
    {
        WATER
    }

    public int itemIndex;
    public string ItemName;
    public int cost;
    public Texture2D IconImage;
    public GameObject ObjectModel;

}
