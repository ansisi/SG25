using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnergyUI : MonoBehaviour
{
    [SerializeField] private GameObject[] energyUIObjects; // 에너지 UI 오브젝트 배열
    [SerializeField] private GameObject gameOverPanel; // 게임 오버 패널
    [SerializeField] private int initialEnergyCount = 3; // 초기 에너지 개수

    private int energyCount; // 현재 에너지 개수
    private bool isGameOver = false; // 게임 오버 상태 확인

    void Start()
    {
        energyCount = initialEnergyCount;
        UpdateEnergyUI();

        TrashDespawnTimer.OnTrashDespawned += HandleTrashDespawned;
    }

    void OnDestroy()
    {
        TrashDespawnTimer.OnTrashDespawned -= HandleTrashDespawned;
    }

    void HandleTrashDespawned(GameObject trashObject)
{
    if (isGameOver) return; // 게임 오버 상태이면 업데이트 중지

    if (energyCount > 0)
    {
        energyCount--;
        Debug.Log("Energy decreased: " + energyCount); // 디버그 로그 추가
        UpdateEnergyUI();
    }

    if (energyCount <= 0 && !isGameOver)
    {
        Debug.Log("Game Over - The game has ended."); // 수정된 디버그 로그
        // gameOverPanel.SetActive(true); // 주석 처리 또는 제거
        isGameOver = true; // 게임 오버 상태 설정
        EndGame(); // 게임 종료 처리 함수 호출
    }
}

void EndGame()
{
    // 게임 종료 처리를 위한 추가적인 로직이 필요하다면 여기에 구현
    Debug.Log("The game has officially ended."); // 게임 종료 확인을 위한 디버그 메시지
}


    void UpdateEnergyUI()
    {
        Debug.Log("Updating Energy UI: " + energyCount); // 디버그 로그 추가
        for (int i = 0; i < energyUIObjects.Length; i++)
        {
            if (i < energyCount)
            {
                energyUIObjects[i].SetActive(true);
            }
            else
            {
                energyUIObjects[i].SetActive(false);
            }
        }
    }
}
