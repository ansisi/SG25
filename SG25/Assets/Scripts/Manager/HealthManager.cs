using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthManager : MonoBehaviour
{
    public static HealthManager Instance;

    public TextMeshProUGUI healthText; // ü�� ǥ�� �ؽ�Ʈ
    private int health; // �ʱ� ü�� ��

    private void Awake()
    {
        // �̱��� �ν��Ͻ� ����
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ �ÿ��� �ı����� �ʵ��� ����
            
        }
        else
        {
            Destroy(gameObject); // �̹� �ν��Ͻ��� ������ ���ο� ��ü�� �ı�
        }
    }
    void Start()
    {
        health = 100;
        UpdateHealthText();
    }

    public void DecreaseHealth(int amount)
    {
        health -= amount;
        if (health < 0) 
            health = 0; // ü���� 0 ���Ϸ� �������� �ʵ��� ��

        UpdateHealthText();
    }
    public void IncreaseHealth(int amount)
    {
        health += amount;
        

        UpdateHealthText();
    }
    private void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = "+" + health;
        }
        else
        {
            Debug.LogError("HealthText is not assigned in the HealthManager.");
        }
    }
}
