using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StoreManager : MonoBehaviour
{
    public static StoreManager Instance;

    public int currentMoney;
    private int previousMoney;

    private int totalSales;
    private int inputChangeMoney;
    private int userInputMoney;
    private int receivedMoney;

    private bool itemSelected = false;
    private bool moneySelected = false;

    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI receivedMoneyText;
    public TextMeshProUGUI totalMoneyText;
    public TextMeshProUGUI changeText;
    public TextMeshProUGUI inputText;
    public TextMeshProUGUI inputChangeText;

    private List<Item> selectedItems = new List<Item>();

    private void Start()
    {
        // GameManager�� �ν��Ͻ��� ������ currentMoney�� ������ ����
        if (GameManager.Instance != null)
        {
            currentMoney = GameManager.Instance.currentMoney;
            previousMoney = currentMoney;
            UpdateMoneyUI();
            UpdateTotalMoneyUI();
        }
        else
        {
            Debug.LogError("GameManager �ν��Ͻ��� ã�� �� �����ϴ�!");
        }
    }

    public void SelectItem(Item item)
    {
        selectedItems.Add(item);
        UpdateTotalMoneyUI();
        itemSelected = true;
        inputText.text = "";
    }

    public void SelectMoney()
    {
        moneySelected = true;
        inputText.text = "";
    }

    public void ClearNumber()
    {
        if (itemSelected && moneySelected)
        {
            userInputMoney = 0;
            inputText.text = "";
        }
    }

    public void CalculatePaidAmount()
    {
        if (!itemSelected || !moneySelected) return;

        int totalMoney = 0;
        foreach (Item item in selectedItems)
        {
            totalMoney += item.price;
        }

        int receivedMoney = int.Parse(receivedMoneyText.text.Replace(",", ""));
        int change = receivedMoney - totalMoney;

        if (change >= 0)
        {
            int changeAmount = userInputMoney;

            // �Ž����� ����� �� ���� �� �г�Ƽ ����
            if (changeAmount != change)
            {
                // �� ��� ���
                Debug.LogError("�մ�: ����� ����???");
                Debug.LogError("�մ�: ���ϴ����̾�!!");

                // ������ ���� (����)
                GameManager.Instance.EnergyDecrease(10);

                // �Ž����� ����
                change = changeAmount;
            }

            // GameManager�� currentMoney ����
            GameManager.Instance.currentMoney += receivedMoney - userInputMoney;

            // StoreManager�� currentMoney�� ����
            currentMoney = GameManager.Instance.currentMoney;

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

            itemSelected = false;
            moneySelected = false;

            Invoke("ClearInputChangeText", 2f);
        }
    }

    private void ClearInputChangeText()
    {
        inputChangeText.text = "";
    }

    public void UpdateMoneyUI()
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

        totalMoneyText.text = totalMoney.ToString();
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.currentMoney != previousMoney)
        {
            currentMoney = GameManager.Instance.currentMoney;
            UpdateMoneyUI();
            previousMoney = currentMoney;
        }

        if (itemSelected && moneySelected && userInputMoney < 1000000)
        {
            for (int i = 0; i <= 9; i++)
            {
                if (Input.GetKeyDown(KeyCode.Keypad0 + i) || Input.GetKeyDown(KeyCode.Alpha0 + i))
                {
                    userInputMoney = userInputMoney * 10 + i;
                    inputText.text = userInputMoney.ToString();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            CalculatePaidAmount();
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
                        receivedMoneyText.text = moneyConsumable.money.value.ToString();
                        Destroy(hit.collider.gameObject);

                        SelectMoney();
                    }
                }
            }
        }
    }
}

