using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthManager : MonoBehaviour
{
    public static HealthManager Instance;

    public TextMeshProUGUI healthText; // 체력 표시 텍스트
    private int health; // 초기 체력 값

    private void Awake()
    {
        // 싱글톤 인스턴스 설정
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 파괴되지 않도록 설정
            
        }
        else
        {
            Destroy(gameObject); // 이미 인스턴스가 있으면 새로운 객체를 파괴
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
            health = 0; // 체력이 0 이하로 내려가지 않도록 함

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
