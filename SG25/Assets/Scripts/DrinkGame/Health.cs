using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Scene 관련 기능을 사용하기 위해 추가

public class Health : MonoBehaviour
{
    public Image[] healthIcons; // 체력 칸을 표시하는 이미지 배열

    private int maxHealth = 3; // 최대 체력 칸 수
    private int currentHealth; // 현재 체력 칸 수



    void Start()
    {
        currentHealth = maxHealth; // 시작할 때 최대 체력으로 초기화
        UpdateHealthBar();
    }

    void OnTriggerEnter(Collider other)
    {
        // 충돌한 오브젝트가 음료수인지 확인합니다.
        if (other.CompareTag("Drink"))
        {
            Debug.Log("음료수가 바닥에 닿았습니다.");
            // 음료수가 바닥에 닿았으므로 체력을 감소시킵니다.
            DecreaseHealth();
        }
    }

    void DecreaseHealth()
    {
        // 체력을 감소시킵니다.
        currentHealth--;

        // 체력이 0보다 작으면 게임 오버 처리 등을 수행합니다.
        if (currentHealth <= 0)
        {
            // 게임 오버 처리를 수행합니다.
            EndGame();
        }

        // 체력 칸 이미지를 업데이트합니다.
        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        // 체력 칸 이미지를 업데이트합니다.
        for (int i = 0; i < maxHealth; i++)
        {
            if (i < currentHealth)
            {
                // 현재 체력 칸 이전까지는 활성화된 이미지를 보여줍니다.
                healthIcons[i].enabled = true;
            }
            else
            {
                // 현재 체력 칸 이후부터는 비활성화된 이미지를 보여줍니다.
                healthIcons[i].enabled = false;
            }
        }
    }



    void EndGame()
    {
        // 게임 종료 처리를 수행합니다.
        Debug.Log("게임 종료!");

        // 게임 오버 시 SampleScene으로 이동하고 OrderPanel 상태 복원
        SceneManager.LoadScene("SampleScene");
    }
}
