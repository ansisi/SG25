using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject drinkPrefab; // 생성할 음료의 프리팹
    public float spawnInterval = 2f; // 음료 생성 간격
    public float spawnRange = 2f; // 음료 생성 위치의 편차

    private float nextSpawnTime; // 다음 음료 생성 시간
    private bool isGameOver = false; // 게임 오버 상태

    void Start()
    {
        // 다음 음료 생성 시간 초기화
        nextSpawnTime = Time.time + spawnInterval;

        // GameManager의 Instance가 없으면 에러 메시지 출력
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager Instance를 찾을 수 없습니다.");
        }
    }

    void Update()
    {
        // 현재 시간이 다음 생성 시간보다 크면 음료를 생성합니다.
        if (!isGameOver && Time.time >= nextSpawnTime)
        {
            SpawnDrink();
            nextSpawnTime = Time.time + spawnInterval; // 다음 음료 생성 시간 갱신
        }
    }

    void SpawnDrink()
    {
        // 랜덤한 위치에 음료를 생성합니다.
        Vector3 spawnPosition = transform.position + new Vector3(Random.Range(-spawnRange, spawnRange), 0f, 0f);
        Instantiate(drinkPrefab, spawnPosition, Quaternion.identity);
    }

    // 게임 오버 상태를 설정하는 함수
    public void SetGameOver(bool gameOver)
    {
        isGameOver = gameOver;

        
    }

}
