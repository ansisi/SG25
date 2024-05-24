using UnityEngine;

public class TrashGenerator : MonoBehaviour
{
    public GameObject trashPrefab; // 생성할 쓰레기 프리팹

    public float minInterval = 10f; // 최소 생성 간격 (초)
    public float maxInterval = 20f; // 최대 생성 간격 (초)

    private float nextTime; // 다음 쓰레기 생성 시간
    private int maxTrashCount = 5; // 최대 허용 쓰레기 개수

    void Start()
    {
        nextTime = Time.time + Random.Range(minInterval, maxInterval);
    }

    void Update()
    {

        // 현재 시간이 다음 생성 시간보다 크거나 같으면
        if (Time.time >= nextTime)
        {
            // 현재 씬에 있는 "Trash" 태그를 가진 오브젝트의 개수 확인
            int trashCount = GameObject.FindGameObjectsWithTag("Trash").Length;

            // 최대 허용 개수보다 적으면 쓰레기 생성
            if (trashCount < maxTrashCount)
            {
                Instantiate(trashPrefab, GetRandomPosition(), Quaternion.identity);

                // ... (쓰레기 개수 체크 및 손님 알림 코드)

                // 다음 쓰레기 생성 시간 설정
                nextTime = Time.time + Random.Range(minInterval, maxInterval);
            }
        }
    }

    Vector3 GetRandomPosition()
    {
        // 맵 크기
        float mapWidth = 10.0f;
        float mapHeight = 10.0f;

        // 맵 경계 내 랜덤 위치 생성
        return new Vector3(
            Random.Range(-mapWidth / 2, mapWidth / 2),
            0.2f, // Y 좌표를 1로 고정
            Random.Range(-mapHeight / 2, mapHeight / 2)
        );
    }



}
