using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Unity.VisualScripting;

public class MyShelfsPanel : MonoBehaviour
{
    FirstPersonController firstPersonController;

    [SerializeField]
    public itemModel[] itemModels;
    public GameObject itemImagePrefab;
    public GameObject itemListContent;

    public GameObject shelfImage;
    public GameObject shelfListContent;

    public GameObject itemPanel;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemCount;
    public TextMeshProUGUI itemTotalCost;
    public TextMeshProUGUI noMoneyText;
    public Image selectedItemImage;

    public Button paymentButton;
    public Button cancelButton;
    public Button countUpButton;
    public Button countDownButton;

    private int currentCount = 1;
    private int totalCost;
    private itemModel selectedItem;
    private Shelf selectedShelf;

    public GameObject itemShopPanel;

    public List<Shelf> shelves;

    private FirstPersonController playerCtrl;

    void Start()
    {
        playerCtrl = FindObjectOfType<FirstPersonController>();
        itemModels = Resources.LoadAll<itemModel>("");
        CreateItemList();
        CreateShelfList();
        InitializeButtons();
    }

    private void Update()
    {
        playerCtrl.PanelOn();
    }

    public void CreateItemList()
    {
        for (int i = 0; i < itemModels.Length; i++)
        {
            GameObject itemImage = Instantiate(itemImagePrefab, itemListContent.transform);
            Image imageComponent = itemImage.GetComponent<Image>();

            if (imageComponent != null && itemModels[i].IconImage != null)
            {
                Sprite iconSprite = Sprite.Create(itemModels[i].IconImage,
                                                  new Rect(0.0f, 0.0f, itemModels[i].IconImage.width, itemModels[i].IconImage.height),
                                                  new Vector2(0.5f, 0.5f));
                imageComponent.sprite = iconSprite;

                TextMeshProUGUI itemText = itemImage.GetComponentInChildren<TextMeshProUGUI>();
                if (itemText != null)
                {
                    itemText.text = itemModels[i].buyCost.ToString("N0");
                }
                itemModel currentItem = itemModels[i];
                Button itemButton = itemImage.GetComponentInChildren<Button>();
                if (itemButton != null)
                {
                    itemButton.onClick.AddListener(() => OnItemButtonClick(currentItem));
                }
            }
        }
    }

    public void CreateShelfList()
    {
        for (int i = 0; i < shelves.Count; i++)
        {
            GameObject shelfUI = Instantiate(shelfImage, shelfListContent.transform);
            Shelf shelfComponent = shelves[i];

            TextMeshProUGUI shelfText = shelfUI.GetComponentInChildren<TextMeshProUGUI>();
            if (shelfText != null)
            {
                shelfText.text = "Shelf " + (i + 1);
            }

            Button shelfButton = shelfUI.GetComponentInChildren<Button>();
            if (shelfButton != null)
            {
                Shelf currentShelf = shelves[i];
                shelfButton.onClick.AddListener(() => OnShelfButtonClick(currentShelf));
            }
        }
    }

    public void GoMyshelfsPanel()
    {
        itemShopPanel.SetActive(false);
    }

    public void PanelCancel()
    {
        playerCtrl.PanelOff();
        itemShopPanel.SetActive(false);
        gameObject.SetActive(false);
    }

    void InitializeButtons()
    {
        countUpButton.onClick.AddListener(OnCountUpButtonClick);
        countDownButton.onClick.AddListener(OnCountDownButtonClick);
        cancelButton.onClick.AddListener(OnCancelButtonClick);
        paymentButton.onClick.AddListener(OnPaymentButtonClick);
    }

    void OnShelfButtonClick(Shelf shelf)
    {
        itemShopPanel.SetActive(true);

        selectedShelf = shelf;
    }

    void OnItemButtonClick(itemModel item)
    {
        itemPanel.SetActive(true);

        selectedItem = item;
        itemName.text = item.ItemName;
        Sprite iconSprite = Sprite.Create(item.IconImage,
                                          new Rect(0.0f, 0.0f, item.IconImage.width, item.IconImage.height),
                                          new Vector2(0.5f, 0.5f));
        selectedItemImage.sprite = iconSprite;
        currentCount = 1;
        UpdateItemCount();
        UpdateTotalCost();
    }

    void OnCountUpButtonClick()
    {
        currentCount++;
        UpdateItemCount();
        UpdateTotalCost();
    }

    void OnCountDownButtonClick()
    {
        if (currentCount > 1)
        {
            currentCount--;
            UpdateItemCount();
            UpdateTotalCost();
        }
    }

    void OnCancelButtonClick()
    {
        itemPanel.SetActive(false);
    }

    void OnPaymentButtonClick()
    {
        if (GameManager.Instance.currentMoney >= totalCost)
        {
            int addedCount = selectedShelf.AddItemToShelf(selectedItem, currentCount);
            if (addedCount > 0)
            {
                int actualCost = selectedItem.buyCost * addedCount;
                GameManager.Instance.MoneyDecrease(actualCost);
                itemPanel.SetActive(false);

                if (addedCount < currentCount)
                {
                    noMoneyText.text = "Áø¿­´ë°¡ ²Ë Ã¡½À´Ï´Ù!";
                    noMoneyText.gameObject.SetActive(true);
                    Invoke("TextInvoke", 2.0f);
                }
            }
            else
            {
                noMoneyText.text = "Áø¿­´ë°¡ ²Ë Ã¡½À´Ï´Ù!";
                noMoneyText.gameObject.SetActive(true);
                Invoke("TextInvoke", 2.0f);
            }
        }
        else
        {
            noMoneyText.text = "µ·ÀÌ ¾ø¾î¿©~~~~";
            noMoneyText.gameObject.SetActive(true);
            Invoke("TextInvoke", 2.0f);
        }
    }

    void UpdateItemCount()
    {
        itemCount.text = currentCount.ToString();
    }

    void UpdateTotalCost()
    {
        totalCost = selectedItem.buyCost * currentCount;
        itemTotalCost.text = totalCost.ToString("N0");
    }

    void TextInvoke()
    {
        noMoneyText.gameObject.SetActive(false);
    }
}
