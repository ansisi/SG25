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
        // GameManager의 인스턴스가 있으면 currentMoney를 가져와 설정
        if (GameManager.Instance != null)
        {
            currentMoney = GameManager.Instance.currentMoney;
            previousMoney = currentMoney;
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

            // 거스름돈 제대로 안 줬을 때 패널티 적용
            if (changeAmount != change)
            {
                // 욕 대사 출력
                Debug.LogError("손님: 제대로 안해???");
                Debug.LogError("손님: 뭐하는짓이야!!");

                // 에너지 차감 (예시)
                GameManager.Instance.EnergyDecrease(10);

                // 거스름돈 차감
                change = changeAmount;
            }

            // GameManager의 currentMoney 갱신
            GameManager.Instance.currentMoney += receivedMoney - userInputMoney;

            // StoreManager의 currentMoney도 갱신
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

