using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OrderPanel : MonoBehaviour
{
    public List<Item> allItems;
    private List<Item> filteredItems;

    public GridLayoutGroup gridLayoutGroup;

    public Button allButton;
    public Button snackButton;
    public Button drinkButton;
    public Button frozenButton;
    public Button cigaretteButton;
    public Button cartButton;
    public Button cartCloseButton;

    public GameObject itemPrefab;
    public GameObject cartPanel;
    public GameObject orderPanel;

    public TextMeshProUGUI cartItemCountText;
    public TextMeshProUGUI totalPriceText;

    private int cartItemCount = 0;
    private int totalPrice = 0;

    private bool isOrderPanelOn = false;
    private bool isCartPanelOn = false;

    public PlayerCtrl playerCtrl;

    void Start()
    {
        orderPanel.SetActive(false);

        snackButton.onClick.AddListener(ShowSnackItems);
        drinkButton.onClick.AddListener(ShowDrinkItems);
        frozenButton.onClick.AddListener(ShowFrozenItems);
        cigaretteButton.onClick.AddListener(ShowCigaretteItems);

        cartButton.onClick.AddListener(CartButton);

        if (gridLayoutGroup == null)
        {
            Debug.LogError("gridLayoutGroup is not assigned in OrderPanel.");
            return;
        }

        if (itemPrefab == null)
        {
            Debug.LogError("itemPrefab is not assigned in OrderPanel.");
            return;
        }

        ShowAllItems();
        UpdateCartItemCountText();

        playerCtrl = FindObjectOfType<PlayerCtrl>();
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

    private void CartButton()
    {
        cartPanel.SetActive(true);
    }

    private void CartClose()
    {
        cartPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isOrderPanelOn && isCartPanelOn)
            {
                CartClose();
            }

            else if (isOrderPanelOn)
            {
                OffOrderPanel();
            }
        }



        if (Input.GetKeyDown(KeyCode.C))
        {
            orderPanel.SetActive(false);
        }
    }

    public void OnOrderPanel()
    {
        isOrderPanelOn = true;
    }

    public void OffOrderPanel()
    {
        isOrderPanelOn = false;
    }


    private List<Item> FilterItemsByType(Item.ItemType type)
    {
        List<Item> filteredList = new List<Item>();
        foreach (Item item in allItems)
        {
            if (item != null)
            {
                if (item.itemType == type)
                {
                    filteredList.Add(item);
                }
            }
        }
        return filteredList;
    }

    private void DisplayItems(List<Item> items)
    {
        ClearGridLayout();

        foreach (Item item in items)
        {
            GameObject newItem = Instantiate(itemPrefab, gridLayoutGroup.transform);

            Image image = newItem.GetComponentInChildren<Image>();
            image.sprite = item.icon;

            TextMeshProUGUI nameText = image.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            nameText.text = item.itemName;

            TextMeshProUGUI priceText = image.gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            priceText.text = item.price.ToString();

            Button addToCartButton = image.gameObject.transform.GetChild(2).GetComponent<Button>();
            addToCartButton.onClick.AddListener(() => AddToCart(item));

            TMP_InputField itemCountInputField = image.gameObject.transform.GetChild(3).GetComponent<TMP_InputField>();
            itemCountInputField.text = "1";

            Button increaseButton = image.gameObject.transform.GetChild(4).GetComponent<Button>();
            increaseButton.onClick.AddListener(() => IncreaseItemCount(itemCountInputField));

            Button decreaseButton = image.gameObject.transform.GetChild(5).GetComponent<Button>();
            decreaseButton.onClick.AddListener(() => DecreaseItemCount(itemCountInputField));
        }
    }

    private void ClearGridLayout()
    {
        foreach (Transform child in gridLayoutGroup.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void IncreaseItemCount(TMP_InputField itemCountInputField)
    {
        int itemCount = int.Parse(itemCountInputField.text);
        itemCount++;
        itemCountInputField.text = itemCount.ToString();
    }

    public void DecreaseItemCount(TMP_InputField itemCountInputField)
    {
        int itemCount = int.Parse(itemCountInputField.text);
        if (itemCount > 1)
        {
            itemCount--;
            itemCountInputField.text = itemCount.ToString();
        }
    }

    public void AddToCart(Item item)
    {
        cartItemCount++;
        totalPrice += item.price;
        UpdateCartItemCountText();
    }

    private void UpdateCartItemCountText()
    {
        cartItemCountText.text = cartItemCount.ToString();
        totalPriceText.text = "$" + totalPrice.ToString();
    }
}