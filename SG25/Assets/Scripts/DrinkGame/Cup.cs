using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cup : MonoBehaviour
{
    private int clickCount = 0; // Ŭ�� Ƚ���� �����ϴ� ����
    void Update()
    {
        // ���콺 ���� ��ư�� Ŭ���Ǿ����� Ȯ���մϴ�.
        if (Input.GetMouseButtonDown(0))
        {
            // ���콺 Ŭ�� ��ġ���� Raycast�� �߻��մϴ�.
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Raycast�� � ������Ʈ�� �浹�ߴ��� Ȯ���մϴ�.
            if (Physics.Raycast(ray, out hit))
            {
                // �浹�� ������Ʈ�� ��������� Ȯ���մϴ�.
                if (hit.collider.CompareTag("Drink"))
                {
                    // ����� ������Ʈ�� �ı��մϴ�.
                    Destroy(hit.collider.gameObject);

                    // Ŭ�� Ƚ���� ������ŵ�ϴ�.
                    clickCount++;

                    // Ŭ�� Ƚ���� 5ȸ �̻��̸� ���� ���� �� Scene ��ȯ
                    if (clickCount >= 5)
                    {
                        EndGame();
                    }
                }
            }
        }
    }
    // ���� ���� �� Scene ��ȯ�� ó���ϴ� �޼���
    void EndGame()
    {
        // ���� ���� ó���� �����մϴ�.
        Debug.Log("���� ����!");

        // ���� ���� �� AiMapScene���� �̵��մϴ�.
        SceneManager.LoadScene("AiTestScene");
    }
}
