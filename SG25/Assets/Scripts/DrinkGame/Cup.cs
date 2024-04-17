using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cup : MonoBehaviour
{
    public float speed = 5f; // 컵의 이동 속도

    void Update()
    {
        // 마우스 위치를 가져옵니다.
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; // z 축 값을 0으로 설정하여 2D 공간에서의 위치로 변환합니다.

        // 컵을 마우스 위치로 이동시킵니다.
        transform.position = Vector3.Lerp(transform.position, mousePosition, speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        // 충돌한 오브젝트가 음료인지 확인합니다.
        if (other.CompareTag("Drink"))
        {
            // 마우스 클릭을 확인합니다.
            if (Input.GetMouseButtonDown(0))
            {
                // 음료를 파괴합니다.
                Destroy(other.gameObject);

                // TODO: 점수를 증가시키거나 다른 동작을 수행합니다.
            }
        }
    }
}
