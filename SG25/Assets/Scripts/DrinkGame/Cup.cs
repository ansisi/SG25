using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cup : MonoBehaviour
{
    private int clickCount = 0; // 클릭 횟수를 저장하는 변수
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

                    // 클릭 횟수를 증가시킵니다.
                    clickCount++;

                    // 클릭 횟수가 5회 이상이면 게임 종료 및 Scene 전환
                    if (clickCount >= 5)
                    {
                        EndGame();
                    }
                }
            }
        }
    }
    // 게임 종료 및 Scene 전환을 처리하는 메서드
    void EndGame()
    {
        // 게임 종료 처리를 수행합니다.
        Debug.Log("게임 종료!");

        // 게임 오버 시 AiMapScene으로 이동합니다.
        SceneManager.LoadScene("AiTestScene");
    }
}
