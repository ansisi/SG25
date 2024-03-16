using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [System.Serializable]
    public class InventoryItem
    {
        public Item item;
        public int count;

        public InventoryItem(Item item, int count)
        {
            this.item = item;
            this.count = count;
        }
    }

    public List<InventoryItem> items = new List<InventoryItem>();

    [SerializeField]
    private Transform slotParent;
    [SerializeField]
    private Slot[] slots;

    void Awake()
    {
        ClearInventory();
    }

    public void FreshSlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < items.Count)
            {
                slots[i].AddItem(items[i].item, items[i].count);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }

    public void AddItem(Item newItem)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].item == newItem)
            {
                items[i].count++;
                FreshSlot();
                return;
            }
        }

        if (items.Count < slots.Length)
        {
            items.Add(new InventoryItem(newItem, 1));
            FreshSlot();
        }
        else
        {
            Debug.Log("인벤토리가 가득 찼습니다.");
        }
    }

    public void ClearInventory()
    {
        items.Clear();
        FreshSlot(); 
    }
}