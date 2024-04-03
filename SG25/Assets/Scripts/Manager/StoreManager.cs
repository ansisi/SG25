using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StoreManager : MonoBehaviour
{
    private int totalMoney;
    private int change;
    public int currentMoney;
    private int totalSales;
    private int inputChangeMoney;
    private int userInputMoney;
    private int receivedMoney;

    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI receivedMoneyText;
    public TextMeshProUGUI totalMoneyText;
    public TextMeshProUGUI changeText;
    public TextMeshProUGUI inputText;
    public TextMeshProUGUI inputChangeText;

    private List<Item> selectedItems = new List<Item>();

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            currentMoney = GameManager.Instance.money;
            UpdateMoneyUI();
            UpdateTotalMoneyUI();
        }
        else
        {
            Debug.LogError("GameManager 인스턴스를 찾을 수 없습니다!");
        }
    }

    public void SelectItem(Item item)
    {
        selectedItems.Add(item);
        UpdateTotalMoneyUI();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            CalculatePaidAmount();
        }

        for (int i = 0; i <= 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Keypad0 + i) || Input.GetKeyDown(KeyCode.Alpha0 + i))
            {
                userInputMoney = userInputMoney * 10 + i;
                inputText.text = userInputMoney.ToString();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Product"))
                {
                    Consumable consumable = hit.collider.GetComponent<Consumable>();
                    if (consumable != null)
                    {
                        Item item = consumable.item;
                        if (item != null)
                        {
                            SelectItem(item);
                            Destroy(hit.collider.gameObject);
                        }
                    }
                }

                else
                {
                    MoneyConsumable moneyConsumable = hit.collider.GetComponent<MoneyConsumable>();
                    if (moneyConsumable != null)
                    {
                        receivedMoneyText.text = "받은 돈 " + moneyConsumable.money.value.ToString();
                        Destroy(hit.collider.gameObject);

                        int receivedMoney = int.Parse(receivedMoneyText.text);
                        int totalMoney = 0;
                        foreach (Item item in selectedItems)
                        {
                            totalMoney += item.price;
                        }
                        int change = receivedMoney - totalMoney;

                        changeText.text = "거스름돈 " + change.ToString();
                    }
                }

            }
        }
    }

    private void CalculatePaidAmount()
    {
        int totalMoney = 0;
        foreach (Item item in selectedItems)
        {
            totalMoney += item.price;
        }
        int receivedMoney = int.Parse(receivedMoneyText.text);
        int change = receivedMoney - totalMoney;

        if (change >= 0)
        {
            int changeAmount = userInputMoney;

            currentMoney += receivedMoney - userInputMoney;
            totalSales += receivedMoney - userInputMoney;
            UpdateMoneyUI();
            UpdateTotalMoneyUI();
            selectedItems.Clear();
            receivedMoneyText.text = "0";
            changeText.text = change.ToString();

            if (changeAmount != change)
            {
                inputChangeText.color = Color.red;
            }
            else
            {
                inputChangeText.color = Color.green;
            }

            inputChangeText.text = changeAmount.ToString();

            inputText.text = "";
            receivedMoneyText.text = "";
            changeText.text = "";
            totalMoneyText.text = "";

            Invoke("ClearInputChangeText", 2f);
        }
    }

    private void ClearInputChangeText()
    {
        inputChangeText.text = "";
    }

    private void UpdateMoneyUI()
    {
        moneyText.text = currentMoney.ToString();
    }

    private void UpdateTotalMoneyUI()
    {
        int totalMoney = 0;
        foreach (Item item in selectedItems)
        {
            totalMoney += item.price;
        }

        totalMoneyText.text = "총 상품 금액 " + totalMoney.ToString();
    }
}
