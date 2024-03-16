using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Slot : MonoBehaviour
{
    [SerializeField]
    private Image image;
    [SerializeField]
    private TMP_Text itemCountText;

    private Item _item;
    private int _itemCount;

    public Item item
    {
        get { return _item; }
        set
        {
            _item = value;

            if (_item != null)
            {
                image.sprite = _item.icon;
                image.color = new Color(1, 1, 1, 1);
            }
            else
            {
                image.color = new Color(1, 1, 1, 0);
            }
        }
    }

    public int itemCount
    {
        get { return _itemCount; }
        set
        {
            _itemCount = value;

            if (_itemCount > 1)
            {
                itemCountText.text = _itemCount.ToString();
            }
            else
            {
                itemCountText.text = "";
            }
        }
    }

    public void AddItem(Item newItem, int count)
    {
        item = newItem;
        itemCount = count;
    }

    public void ClearSlot()
    {
        item = null;
        itemCount = 0;
    }
}