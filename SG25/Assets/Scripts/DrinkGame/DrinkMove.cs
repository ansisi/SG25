using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DrinkMove : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        // �浹�� ������Ʈ�� �÷��̾����� Ȯ���մϴ�.
        if (other.CompareTag("Player"))
        {
            // �÷��̾�� �浹�ϸ� ���� �̴ϰ��� ������ �Ѿ�ϴ�.
            SceneManager.LoadScene("DrinkMiniGameScene");
        }
    }
}
