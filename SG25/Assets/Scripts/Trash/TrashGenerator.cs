using UnityEngine;
using System.Collections.Generic;

public class TrashGenerator : MonoBehaviour
{
    public GameObject garbagePrefab; // 쓰레기 프리팹
    public int mapWidth = 10; // 맵 가로 길이
    public int mapHeight = 10; // 맵 세로 길이
    public float minSpawnInterval = 10f; // 최소 쓰레기 생성 간격 (초)
    public float maxSpawnInterval = 30f; // 최대 쓰레기 생성 간격 (초)
    public float spawnChance = 0.5f; // 쓰레기 생성 확률

    public List<string> customerDialogues = new List<string>() {
        "더러워요!", "이게 뭐에요?", "냄새 나네요!", "빨리 치워주세요!"
    }; // 손님 대사 리스트
    private int garbageCount = 0; // 생성된 쓰레기 개수

    void Start()
    {
        // 시작할 때 쓰레기 생성 시작
        GenerateGarbage();
    }

    void GenerateGarbage()
    {
        // 무작위 확률에 따라 쓰레기 생성 여부 결정
        if (Random.value < spawnChance)
        {
            // 무작위 위치 생성
            float randomX = Random.Range(0, mapWidth);
            float randomY = Random.Range(0, mapHeight);
            Vector3 spawnPosition = new Vector3(randomX, 0.5f, randomY); // 높이 조정

            // 쓰레기 프리팹을 무작위 위치에 생성
            Instantiate(garbagePrefab, spawnPosition, Quaternion.identity);

            // 생성된 쓰레기 정보 콘솔에 출력
            Debug.Log("쓰레기가 생성되었습니다. 위치: " + spawnPosition);

            // 생성된 쓰레기 개수 증가
            garbageCount++;

            // 생성된 쓰레기가 2개 이상이면 손님 대사 출력
            if (garbageCount >= 2)
            {
                Complain();
                // 손님 대사가 출력된 후, 쓰레기 개수 초기화
                garbageCount = 0;
            }
        }

        // 다음 쓰레기 생성 간격 설정
        float nextSpawnTime = Random.Range(minSpawnInterval, maxSpawnInterval);
        Invoke("GenerateGarbage", nextSpawnTime);
    }

    // 손님 대사 출력 메서드
    void Complain()
    {
        if (customerDialogues.Count > 0)
        {
            // 무작위 대사 선택
            int randomIndex = Random.Range(0, customerDialogues.Count);
            string dialogue = customerDialogues[randomIndex];
            Debug.Log("손님 대사: " + dialogue);

            // 대사 출력 후 리스트에서 제거
            customerDialogues.RemoveAt(randomIndex);
        }
    }
}
