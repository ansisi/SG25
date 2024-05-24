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



    void Start()
    {
        currentHealth = maxHealth; // ������ �� �ִ� ü������ �ʱ�ȭ
        UpdateHealthBar();
    }

    void OnTriggerEnter(Collider other)
    {
        // �浹�� ������Ʈ�� ��������� Ȯ���մϴ�.
        if (other.CompareTag("Drink"))
        {
            Debug.Log("������� �ٴڿ� ��ҽ��ϴ�.");
            // ������� �ٴڿ� ������Ƿ� ü���� ���ҽ�ŵ�ϴ�.
            DecreaseHealth();
        }
    }

    void DecreaseHealth()
    {
        // ü���� ���ҽ�ŵ�ϴ�.
        currentHealth--;

        // ü���� 0���� ������ ���� ���� ó�� ���� �����մϴ�.
        if (currentHealth <= 0)
        {
            // ���� ���� ó���� �����մϴ�.
            EndGame();
        }

        // ü�� ĭ �̹����� ������Ʈ�մϴ�.
        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        // ü�� ĭ �̹����� ������Ʈ�մϴ�.
        for (int i = 0; i < maxHealth; i++)
        {
            if (i < currentHealth)
            {
                // ���� ü�� ĭ ���������� Ȱ��ȭ�� �̹����� �����ݴϴ�.
                healthIcons[i].enabled = true;
            }
            else
            {
                // ���� ü�� ĭ ���ĺ��ʹ� ��Ȱ��ȭ�� �̹����� �����ݴϴ�.
                healthIcons[i].enabled = false;
            }
        }
    }



    void EndGame()
    {
        // ���� ���� ó���� �����մϴ�.
        Debug.Log("���� ����!");

        // ���� ���� �� SampleScene���� �̵��ϰ� OrderPanel ���� ����
        SceneManager.LoadScene("SampleScene");
    }
}
