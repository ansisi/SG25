using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drink : MonoBehaviour
{
    public float speed = 5f; // 음료 이동 속도

    void Update()
    {
        // 음료를 아래쪽으로 이동시킵니다.
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        // 화면 밖으로 나가면 삭제합니다.
        if (transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }
}
