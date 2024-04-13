using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CartPanel : MonoBehaviour
{
    public GameObject itemListContainer;
    public GameObject itemListPrefab;
    public TextMeshProUGUI totalPriceText;
    public TextMeshProUGUI moneyLackText;

    private List<GameObject> itemPrefabs = new List<GameObject>();

    private Dictionary<Item, int> cartItemCounts => GameManager.Instance.cartItemCounts;
    private Dictionary<Item, TextMeshProUGUI> itemTextDict = new Dictionary<Item, TextMeshProUGUI>();
    private int totalPrice = 0;

    private void Start()
    {
        moneyLackText.gameObject.SetActive(false);
    }

    public void UpdateItems(Dictionary<Item, int> cartItemCounts)
    {
        foreach (GameObject item in itemPrefabs)
        {
            Destroy(item);
        }
        itemPrefabs.Clear();

        foreach (KeyValuePair<Item, int> pair in cartItemCounts)
        {
            if (itemTextDict.ContainsKey(pair.Key))
            {
                int currentItemCount = pair.Value;
                itemTextDict[pair.Key].text = "x " + currentItemCount.ToString();
            }
            else
            {
                GameObject itemListPrefabInstance = Instantiate(itemListPrefab, itemListContainer.transform);
                TextMeshProUGUI itemNameText = itemListPrefabInstance.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI itemCountText = itemListPrefabInstance.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

                itemNameText.text = pair.Key.itemName;
                itemCountText.text = "x " + pair.Value.ToString();

                itemTextDict[pair.Key] = itemCountText;
            }
        }

        UpdateTotalPrice();
    }

    private void UpdateTotalPrice()
    {
        totalPrice = 0;
        foreach (KeyValuePair<Item, TextMeshProUGUI> pair in itemTextDict)
        {
            int itemCount = int.Parse(pair.Value.text.Substring(2));
            totalPrice += pair.Key.price * itemCount;
        }
        totalPriceText.text = "$" + totalPrice.ToString();
    }

    public void Purchase()
    {
        if (GameManager.Instance.currentMoney >= totalPrice)
        {
            GameManager.Instance.MoneyDecrease(totalPrice);
            PlacePurchasedItems();
            itemTextDict.Clear();
            ResetTotalPrice();
        }
        else
        {
            moneyLackText.gameObject.SetActive(true);
            Invoke("HideMoneyLackText", 1f);
        }
    }

    private void HideMoneyLackText()
    {
        moneyLackText.gameObject.SetActive(false);
    }

    private void ResetTotalPrice()
    {
        totalPrice = 0;
        totalPriceText.text = "$" + totalPrice.ToString();
    }

    private void PlacePurchasedItems()
    {
        foreach (KeyValuePair<Item, int> pair in cartItemCounts)
        {
            GameObject item = pair.Key.itemPrefab;
            for (int i = 0; i < pair.Value; i++)
            {
                GameObject itemInstance = Instantiate(item, new Vector3(0, 1, 0), Quaternion.identity);
            }
        }
    }

    public void CartPanelOff()
    {
        gameObject.SetActive(false);
    }
}
