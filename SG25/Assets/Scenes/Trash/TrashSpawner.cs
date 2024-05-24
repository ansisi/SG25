using UnityEngine;

public class TrashSpawner : MonoBehaviour
{
    public GameObject trashPrefab; // 쓰레기 프리팹
    public float trashSpawnTime = 5f; // 쓰레기 생성 간격 (초)

    private float timeSinceLastTrash = 0f;

    void FixedUpdate()  
    {
        timeSinceLastTrash += Time.deltaTime;

        if (timeSinceLastTrash > trashSpawnTime)
        {
            // 일정 시간 간격마다 쓰레기 생성
            Instantiate(trashPrefab, transform.position, transform.rotation);
            timeSinceLastTrash = 0f;
        }
    }



}
