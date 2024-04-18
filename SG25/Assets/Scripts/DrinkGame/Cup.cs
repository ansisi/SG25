using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cup : MonoBehaviour
{
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
                }
            }
        }
    }
}
