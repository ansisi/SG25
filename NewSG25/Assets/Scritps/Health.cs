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

    private HealthManager healthManager; // HealthManager 인스턴스 참조

    private void Start()
    {
        currentHealth = maxHealth; // 시작할 때 최대 체력으로 초기화
        UpdateHealthBar();

        // HealthManager 싱글톤 인스턴스를 참조
        healthManager = HealthManager.Instance;

        if (healthManager == null)
        {
            Debug.LogError("HealthManager 인스턴스를 찾을 수 없습니다.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 충돌한 오브젝트가 음료수인지 확인
        if (other.CompareTag("Drink"))
        {
            Debug.Log("음료수가 바닥에 닿았습니다.");
            DecreaseHealth(); // 음료수가 바닥에 닿았으므로 체력을 감소
        }
    }

    private void DecreaseHealth()
    {
        currentHealth--; // 체력을 감소

        if (currentHealth <= 0)
        {
            EndGame(); // 체력이 0보다 작으면 게임 오버 처리 등을 수행
        }

        UpdateHealthBar(); // 체력 칸 이미지를 업데이트
    }

    private void UpdateHealthBar()
    {
        for (int i = 0; i < maxHealth; i++)
        {
            healthIcons[i].enabled = i < currentHealth; // 체력 칸 이미지를 업데이트
        }
    }

    private void EndGame()
    {
        Debug.Log("게임 종료!");

        SceneManager.sceneLoaded += OnSceneLoaded;

        // 게임 오버 시 SampleScene으로 이동
        SceneManager.LoadScene("AiTestScene");
    }

    // 씬이 로드된 후 호출되는 이벤트 핸들러
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // HealthManager가 존재하는지 확인한 후 체력을 감소
        if (healthManager != null)
        {
            healthManager.DecreaseHealth(10); // 체력을 -10 감소
        }

        // 이벤트 핸들러 등록 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;

        Debug.Log("Scene loaded: " + scene.name);
    }
}
