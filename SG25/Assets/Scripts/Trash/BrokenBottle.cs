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
            // 마우스 왼쪽 버튼이 클릭되었을 때
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Raycast로 깨진 술병 오브젝트를 검출하고 BrokenBottle 태그를 가지고 있다면 클릭 횟수 증가
                if (hit.collider.CompareTag("BrokenBottle"))
                {
                    clickCount++;

                    // 콘솔창에 클릭 횟수 출력
                    Debug.Log("클릭 횟수: " + clickCount);

                    // 클릭 횟수가 3이 되면 오브젝트 삭제
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
