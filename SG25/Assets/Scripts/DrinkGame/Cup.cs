using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cup : MonoBehaviour
{
    void Update()
    {
        // 마우스 왼쪽 버튼이 클릭되었는지 확인합니다.
        if (Input.GetMouseButtonDown(0))
        {
            // 마우스 클릭 위치에서 Raycast를 발사합니다.
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Raycast가 어떤 오브젝트와 충돌했는지 확인합니다.
            if (Physics.Raycast(ray, out hit))
            {
                // 충돌한 오브젝트가 음료수인지 확인합니다.
                if (hit.collider.CompareTag("Drink"))
                {
                    // 음료수 오브젝트를 파괴합니다.
                    Destroy(hit.collider.gameObject);
                }
            }
        }
    }
}
