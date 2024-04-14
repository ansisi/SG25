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

    public GameObject itemPrefab;
    public GameObject cartPanel;
    public GameObject orderPanel;

    public TextMeshProUGUI cartItemCountText;
    public TextMeshProUGUI totalPriceText;

    private Dictionary<Item, int> cartItemCounts => GameManager.Instance.cartItemCounts;
    private int totalPrice = 0;

    public PlayerCtrl playerCtrl;

    void Start()
    {
        cartPanel.SetActive(false);

        snackButton.onClick.AddListener(() => ShowItemsByType(Item.ItemType.Snack));
        drinkButton.onClick.AddListener(() => ShowItemsByType(Item.ItemType.Drink));
        frozenButton.onClick.AddListener(() => ShowItemsByType(Item.ItemType.Frozen));
        cigaretteButton.onClick.AddListener(() => ShowItemsByType(Item.ItemType.Cigarette));

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

        TMP_InputField itemCountInputField = itemPrefab.GetComponentInChildren<TMP_InputField>();
        itemCountInputField.text = "1";

        ShowAllItems();
        UpdateCartItemCountText();

        playerCtrl = FindObjectOfType<PlayerCtrl>();
        playerCtrl.PanelOn();
    }

    public void ShowAllItems()
    {
        filteredItems = allItems;
        DisplayItems(filteredItems);
    }

    public void ShowItemsByType(Item.ItemType type)
    {
        filteredItems = FilterItemsByType(type);
        DisplayItems(filteredItems);
    }

    private void CartButton()
    {
        cartPanel.SetActive(true);
        CartPanel _cartPanel = cartPanel.GetComponent<CartPanel>();
        if (_cartPanel != null)
        {
            _cartPanel.UpdateItems(cartItemCounts);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            orderPanel.SetActive(false);
            playerCtrl.PanelOff();
        }
    }

    private List<Item> FilterItemsByType(Item.ItemType type)
    {
        List<Item> filteredList = new List<Item>();
        foreach (Item item in allItems)
        {
            if (item.itemType == type)
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
        // 아이템 프리팹에서 인풋 필드를 찾습니다.
        TMP_InputField itemCountInputField = itemPrefab.GetComponentInChildren<TMP_InputField>();
        if (itemCountInputField == null)
        {
            Debug.LogError("Input field not found in item prefab.");
            return;
        }

        string inputText = itemCountInputField.text;

        // 입력 문자열이 비어 있는지 또는 숫자가 아닌 문자를 포함하는지 확인
        if (string.IsNullOrEmpty(inputText) || !int.TryParse(inputText, out int itemCount) || itemCount < 1)
        {
            // 오류 처리 또는 메시지 표시
            Debug.LogError("Invalid input for item count: " + inputText);
            return;
        }

        if (cartItemCounts.ContainsKey(item))
        {
            cartItemCounts[item] += itemCount;
        }
        else
        {
            cartItemCounts[item] = itemCount;
        }

        UpdateCartItemCountText();
    }

    private void UpdateCartItemCountText()
    {
        int totalCartItemCount = 0;
        totalPrice = 0;
        foreach (var pair in cartItemCounts)
        {
            totalCartItemCount += pair.Value;
            totalPrice += pair.Key.price * pair.Value;
        }
        cartItemCountText.text = totalCartItemCount.ToString();
        totalPriceText.text = "$" + totalPrice.ToString();
    }
}

