using UnityEngine;

public class TrashProduce : MonoBehaviour
{
    public delegate void TrashGenerated(GameObject trashObject);
    public static event TrashGenerated OnTrashGenerated; // 쓰레기가 생성될 때 발생하는 이벤트

    public GameObject trashPrefab; // 생성할 쓰레기 프리팹

    public float minInterval = 10f; // 최소 생성 간격 (초)
    public float maxInterval = 20f; // 최대 생성 간격 (초)

    private float nextTime; // 다음 쓰레기 생성 시간
    private int maxTrashCount = 5; // 최대 허용 쓰레기 개수
    private int minTrashCount = 1; // 최소 허용 쓰레기 개수

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
                // 생성할 쓰레기 개수 랜덤으로 결정 (최소 허용 개수 ~ 최대 허용 개수)
                int trashToSpawn = Random.Range(minTrashCount, maxTrashCount + 1);

                for (int i = 0; i < trashToSpawn; i++)
                {
                    GameObject newTrash = Instantiate(trashPrefab, GetRandomPosition(), Quaternion.identity);
                    if (OnTrashGenerated != null)
                    {
                        OnTrashGenerated(newTrash); // 쓰레기가 생성될 때 이벤트 발생
                    }
                    // ... (쓰레기 개수 체크 및 손님 알림 코드)
                }

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

        // 랜덤 위치 생성
        Vector3 randomPosition = new Vector3(
            Random.Range(-mapWidth / 2, mapWidth / 2),
            0.2f, // Y 좌표를 1로 고정
            Random.Range(-mapHeight / 2, mapHeight / 2)
        );

        // Raycast 검사
        int layerMask = ~LayerMask.GetMask("NoCollision"); // "NoCollision" Layer 제외
        RaycastHit hit;

        // 충돌 발생 시 새로운 위치 계산
        while (Physics.Raycast(randomPosition, Vector3.down, out hit, 1.0f, layerMask))
        {
            randomPosition = hit.point + Vector3.up * 0.1f; // 충돌 지점에서 약간 위쪽 위치
        }

        // 충돌하지 않는 위치 반환
        return randomPosition;
    }
}
