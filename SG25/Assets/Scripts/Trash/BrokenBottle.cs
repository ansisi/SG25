using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenBottle : MonoBehaviour
{
    private int clickCount = 0;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // ���콺 ���� ��ư�� Ŭ���Ǿ��� ��
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Raycast�� ���� ���� ������Ʈ�� �����ϰ� BrokenBottle �±׸� ������ �ִٸ� Ŭ�� Ƚ�� ����
                if (hit.collider.CompareTag("BrokenBottle"))
                {
                    clickCount++;

                    // �ܼ�â�� Ŭ�� Ƚ�� ���
                    Debug.Log("Ŭ�� Ƚ��: " + clickCount);

                    // Ŭ�� Ƚ���� 3�� �Ǹ� ������Ʈ ����
                    if (clickCount == 3)
                    {
                        Destroy(hit.collider.gameObject);
                        clickCount = 0;
                    }
                }
            }
        }
    }
}
