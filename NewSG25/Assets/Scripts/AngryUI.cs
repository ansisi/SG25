using UnityEngine;
using UnityEngine.UI;

public class AngryUI : MonoBehaviour
{
    [SerializeField] private GameObject angryUIGauge; // 에너지 UI 오브젝트 배열

    private int remainingAngryCount; // 남은 에너지 개수
    private bool isGameOver = false; // 게임 오버 상태 확인

    void Start()
    {
        //remainingAngryCount = energyUIObjects.Length; // 남은 에너지 개수를 초기화
        UpdateEnergyUI(); // 초기 에너지 UI 설정

        
    }

    void OnDestroy()
    {
        
    }

    void HandleTrashDespawned(GameObject trashObject)
    {
        if (isGameOver) return; // 게임 오버 상태이면 업데이트 중지

        remainingAngryCount--; // 남은 에너지 개수 감소
        UpdateEnergyUI(); // UI 업데이트

        if (remainingAngryCount <= 0 && !isGameOver)
        {
            Debug.Log("Game Over - The game has ended."); // 게임 종료 로그 출력
            // gameOverPanel.SetActive(true); // 게임 오버 패널 활성화 (주석 처리 또는 제거)
            isGameOver = true; // 게임 오버 상태 설정
            EndGame(); // 게임 종료 처리 함수 호출
        }
    }

    void EndGame()
    {
        // 게임 종료 처리를 위한 추가적인 로직이 필요하다면 여기에 구현
        Debug.Log("The game has officially ended."); // 게임 종료 확인 로그 출력
    }

    void UpdateEnergyUI()
    {
        Debug.Log("Updating Energy UI: " + remainingAngryCount); // 에너지 UI 업데이트 로그 출력

            angryUIGauge.SetActive(false); // 남은 에너지 개수에 따라 UI 활성화 설정
            }
}
