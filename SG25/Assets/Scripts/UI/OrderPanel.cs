using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderPanel : MonoBehaviour
{
    public List<Item> allItems;
    public GridLayoutGroup gridLayoutGroup;
    //public GameObject[] itemPrefabs;
    public Button allButton;
    public Button snackButton;
    public Button drinkButton;
    public Button frozenButton;
    public Button cigaretteButton;

    private List<Item> filteredItems;

    void Start()
    {
        allButton.onClick.AddListener(ShowAllItems);
        snackButton.onClick.AddListener(ShowSnackItems);
        drinkButton.onClick.AddListener(ShowDrinkItems);
        frozenButton.onClick.AddListener(ShowFrozenItems);
        cigaretteButton.onClick.AddListener(ShowCigaretteItems);

        ShowAllItems();
    }

    public void ShowAllItems()
    {
        filteredItems = allItems;
        DisplayItems(filteredItems);
    }

    public void ShowSnackItems()
    {
        filteredItems = FilterItemsByType(Item.ItemType.Snack);
        DisplayItems(filteredItems);
    }
    public void ShowDrinkItems()
    {
        filteredItems = FilterItemsByType(Item.ItemType.Drink);
        DisplayItems(filteredItems);
    }
    public void ShowFrozenItems()
    {
        filteredItems = FilterItemsByType(Item.ItemType.Frozen);
        DisplayItems(filteredItems);
    }
    public void ShowCigaretteItems()
    {
        filteredItems = FilterItemsByType(Item.ItemType.Cigarette);
        DisplayItems(filteredItems);
    }

    private List<Item> FilterItemsByType(Item.ItemType type)
    {
        List<Item> filteredList = new List<Item>();
        foreach (Item item in allItems)
        {
            if (item != null && item.itemType == type)
            {
                filteredList.Add(item);
            }
        }
        return filteredList;
    }

    private void DisplayItems(List<Item> items)
    {
        ClearGridLayout();

        foreach (Item item in items)
        {
            if (item == null || item.icon == null)
            {
                continue;
            }

            GameObject newItem = new GameObject("Item");
            Image image = newItem.AddComponent<Image>();

            RectTransform rectTransform = newItem.GetComponent<RectTransform>();
            rectTransform.SetParent(gridLayoutGroup.transform, false);

            image.sprite = item.icon;
        }
    }

    private void ClearGridLayout()
    {
        foreach (Transform child in gridLayoutGroup.transform)
        {
            Destroy(child.gameObject);
        }
    }
}