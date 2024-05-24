using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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
        drinkButton.onClick.AddListener(OnDrinkButtonClick);
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

        playerCtrl = FindFirstObjectByType<PlayerCtrl>();
        playerCtrl.PanelOn();
    }
    public void OnEnable()
    {
        playerCtrl.PanelOn();
    }
    public void OnDisable()
    {
        playerCtrl.PanelOff();
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

    private void OnDrinkButtonClick()
    {
        SaveState();
        SceneManager.LoadScene("DrinkMiniGameScene");
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
    private void DrinkButton()
    {
        SceneManager.LoadScene("DrinkMiniGameScene");
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if(cartPanel.activeInHierarchy == false)
            {
                orderPanel.SetActive(false);
                playerCtrl.PanelOff();
            }    

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

            TMP_InputField itemCountInputField = image.gameObject.transform.GetChild(3).GetComponent<TMP_InputField>();
            itemCountInputField.text = "1";

            Button addToCartButton = image.gameObject.transform.GetChild(2).GetComponent<Button>();
            addToCartButton.onClick.AddListener(() => AddToCart(item, itemCountInputField));

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

    public void AddToCart(Item item, TMP_InputField input)
    {
        // ������ �����տ��� ��ǲ �ʵ带 ã���ϴ�
        string inputText = input.text;

        // �Է� ���ڿ��� ��� �ִ��� �Ǵ� ���ڰ� �ƴ� ���ڸ� �����ϴ��� Ȯ��
        if (string.IsNullOrEmpty(inputText) || !int.TryParse(inputText, out int itemCount) || itemCount < 1)
        {
            // ���� ó�� �Ǵ� �޽��� ǥ��
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

    private void SaveState()
    {
        // OrderPanel�� ���¸� �����ϴ� ���� �߰� (��: PlayerPrefs ���)
        // �ʿ信 ���� ������ ������ ���� ����� ���
    }

    private void LoadState()
    {
        // OrderPanel�� ���¸� �����ϴ� ���� �߰� (��: PlayerPrefs ���)
        // �ʿ信 ���� ������ ������ ���� ����� ���
    }
}

