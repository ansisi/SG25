using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drink : MonoBehaviour
{
    public float speed = 5f; // ���� �̵� �ӵ�

    void Update()
    {
        // ���Ḧ �Ʒ������� �̵���ŵ�ϴ�.
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        // ȭ�� ������ ������ �����մϴ�.
        if (transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }
}
