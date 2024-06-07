using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShelfShopPanel : MonoBehaviour
{
    public GameObject[] shelfButtons; 
    public List<Shelf> shelves; 
    public TextMeshProUGUI playerMoneyText; 
    private int playerMoney;
    private int baseUnlockCost = 5000;

    public GameObject shelfShopPanel;
    public GameObject ShelfShopPanel1;
    public GameObject ShelfShopPanel2;

    void Start()
    {
        playerMoney = GameManager.Instance.currentMoney;
        UpdatePlayerMoneyText();
        InitializeShelves();
        InitializeShelfButtons();
    }

    void InitializeShelves()
    {
        for (int i = 0; i < shelves.Count; i++)
        {
            if (i < 2)
            {
                shelves[i].gameObject.SetActive(true);
            }
            else
            {
                shelves[i].gameObject.SetActive(false);
            }
        }
    }

    void InitializeShelfButtons()
    {
        for (int i = 2; i < shelves.Count; i++)
        {
            int index = i;
            int unlockCost = baseUnlockCost * (index - 1);

            if (index - 2 < shelfButtons.Length)
            {
                TextMeshProUGUI buttonText = shelfButtons[index - 2].GetComponentInChildren<TextMeshProUGUI>();
                buttonText.text = unlockCost.ToString();
                shelfButtons[index - 2].GetComponent<Button>().onClick.AddListener(() => UnlockShelf(index, unlockCost, buttonText));
            }
        }
    }

    void UnlockShelf(int index, int unlockCost, TextMeshProUGUI buttonText)
    {
        if (playerMoney >= unlockCost)
        {
            playerMoney -= unlockCost;
            UpdatePlayerMoneyText();
            shelves[index].gameObject.SetActive(true);
            buttonText.text = "SOLD OUT";
            buttonText.transform.parent.GetComponent<Button>().interactable = false; 
        }
        else
        {
            Debug.Log("Not enough money to unlock " + shelves[index].name);
        }
    }

    public void NextPanel()
    {
        ShelfShopPanel1.SetActive(false);
        ShelfShopPanel2.SetActive(true);
    }
    public void PreviousPanel()
    {
        ShelfShopPanel1.SetActive(true); 
        ShelfShopPanel2.SetActive(false);
    }

    public void ShelfShopPanelOn()
    {
        ShelfShopPanel1.SetActive(true);
    }
    public void ClosePanel()
    {
        ShelfShopPanel1.SetActive(false);
        ShelfShopPanel2.SetActive(false);
    }

    void UpdatePlayerMoneyText()
    {
        playerMoneyText.text = "Money: " + playerMoney;
    }
}
