// TrashGenerator.cs

using UnityEngine;

public class TrashGenerator : MonoBehaviour
{
    public GameObject garbagePrefab; // 쓰레기 프리팹
    public int mapWidth = 10; // 맵 가로 길이
    public int mapHeight = 10; // 맵 세로 길이
    public float spawnInterval = 30f; // 쓰레기 생성 간격 (초)
    public float spawnChance = 0.5f; // 쓰레기 생성 확률
    public float trashTime;

    private Customer customer; // 손님 클래스 참조
    private float lastSpawnTime; // 마지막 쓰레기 생성 시간
    private int garbageCount = 0; // 생성된 쓰레기 개수

    void Start()
    {
        customer = FindObjectOfType<Customer>(); // 손님 클래스 인스턴스 찾기
        lastSpawnTime = Time.time; // 시작 시간 설정
        GenerateGarbage(); // 쓰레기 생성 시작
    }

    void GenerateGarbage()
    {
        // 무작위 확률에 따라 쓰레기 생성 여부 결정
        if (Random.value < spawnChance)
        {
            // 무작위 위치 생성
            float randomX = Random.Range(0, mapWidth);
            float randomY = Random.Range(0, mapHeight);
            Vector3 spawnPosition = new Vector3(randomX, 0f, randomY);

            // 쓰레기 프리팹을 무작위 위치에 생성
            Instantiate(garbagePrefab, spawnPosition, Quaternion.identity);

            // 생성 위치를 콘솔에 출력
            Debug.Log("쓰레기가 생성되었습니다. 위치: " + spawnPosition);

            // 생성된 쓰레기 개수 증가
            garbageCount++;

            // 생성된 쓰레기가 4개 이상이면 손님 대사 출력
            if (garbageCount >= 4 && customer != null)
            {
                customer.NotifyTrashCreated();
                // 쓰레기 개수 초기화
                garbageCount = 0;
            }
        }

        // 다음 쓰레기 생성 간격 설정
        Invoke("GenerateGarbage", spawnInterval);
    }
}
//// void SpawnItem()
//{
//    float randomX = Random.Range(-13f, 13f);
//    Vector3 spawnPosition = new Vector3(randomX, 0f, randomX);
//    Instantiate(NapzakPrefab, spawnPosition, Quaternion.identity);
//}