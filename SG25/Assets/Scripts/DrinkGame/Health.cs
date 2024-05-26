using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Scene ���� ����� ����ϱ� ���� �߰�

public class Health : MonoBehaviour
{

    public Image[] healthIcons; // ü�� ĭ�� ǥ���ϴ� �̹��� �迭
    private int maxHealth = 3; // �ִ� ü�� ĭ ��
    private int currentHealth; // ���� ü�� ĭ ��

    private HealthManager healthManager; // HealthManager �ν��Ͻ� ����

    private void Start()
    {
        currentHealth = maxHealth; // ������ �� �ִ� ü������ �ʱ�ȭ
        UpdateHealthBar();

        // HealthManager �̱��� �ν��Ͻ��� ����
        healthManager = HealthManager.Instance;

        if (healthManager == null)
        {
            Debug.LogError("HealthManager �ν��Ͻ��� ã�� �� �����ϴ�.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // �浹�� ������Ʈ�� ��������� Ȯ��
        if (other.CompareTag("Drink"))
        {
            Debug.Log("������� �ٴڿ� ��ҽ��ϴ�.");
            DecreaseHealth(); // ������� �ٴڿ� ������Ƿ� ü���� ����
        }
    }

    private void DecreaseHealth()
    {
        currentHealth--; // ü���� ����

        if (currentHealth <= 0)
        {
            EndGame(); // ü���� 0���� ������ ���� ���� ó�� ���� ����
        }

        UpdateHealthBar(); // ü�� ĭ �̹����� ������Ʈ
    }

    private void UpdateHealthBar()
    {
        for (int i = 0; i < maxHealth; i++)
        {
            healthIcons[i].enabled = i < currentHealth; // ü�� ĭ �̹����� ������Ʈ
        }
    }

    private void EndGame()
    {
        Debug.Log("���� ����!");

        // HealthManager�� �����ϴ��� Ȯ���� �� ü���� ����
        if (healthManager != null)
        {
            healthManager.DecreaseHealth(10); // ü���� -10 ����
        }

        // ���� ���� �� SampleScene���� �̵�
        SceneManager.LoadScene("AiTestScene");
    }
}
