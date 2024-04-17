using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cup : MonoBehaviour
{
    public float speed = 5f; // ���� �̵� �ӵ�

    void Update()
    {
        // ���콺 ��ġ�� �����ɴϴ�.
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; // z �� ���� 0���� �����Ͽ� 2D ���������� ��ġ�� ��ȯ�մϴ�.

        // ���� ���콺 ��ġ�� �̵���ŵ�ϴ�.
        transform.position = Vector3.Lerp(transform.position, mousePosition, speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        // �浹�� ������Ʈ�� �������� Ȯ���մϴ�.
        if (other.CompareTag("Drink"))
        {
            // ���콺 Ŭ���� Ȯ���մϴ�.
            if (Input.GetMouseButtonDown(0))
            {
                // ���Ḧ �ı��մϴ�.
                Destroy(other.gameObject);

                // TODO: ������ ������Ű�ų� �ٸ� ������ �����մϴ�.
            }
        }
    }
}
