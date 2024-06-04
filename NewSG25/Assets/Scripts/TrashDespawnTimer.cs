using UnityEngine;
using System;
using System.Collections;

public class TrashDespawnTimer : MonoBehaviour
{
    public float despawnTime = 5f; // 쓰레기 삭제 시간 (초)

    public static event Action<GameObject> OnTrashDespawned; // 쓰레기가 삭제될 때 발생하는 이벤트

    private bool isDespawning = false; // 파괴 중인지 여부

    void Start()
    {
        StartCoroutine(DespawnTrash());
    }

    IEnumerator DespawnTrash()
    {
        yield return new WaitForSeconds(despawnTime);
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        // 쓰레기 생성 이벤트에 대한 핸들러를 해제합니다.
        TrashProduce.OnTrashGenerated -= HandleTrashGenerated;
    }

    // 쓰레기 생성 이벤트 핸들러
    void HandleTrashGenerated(GameObject trashObject)
    {
        // 쓰레기가 생성되면 삭제 타이머를 시작합니다.
        StartCoroutine(DespawnTrashForGenerated(trashObject));
    }

    // 쓰레기 삭제 타이머 코루틴
    IEnumerator DespawnTrashForGenerated(GameObject trashObject)
    {
        yield return new WaitForSeconds(despawnTime);

        if (trashObject != null)
        {
            Debug.LogFormat("쓰레기 오브젝트가 파괴되었습니다: 이름: {0}, 위치: {1}", trashObject.name, trashObject.transform.position);
            if (OnTrashDespawned != null)
            {
                OnTrashDespawned(trashObject);
            }
            Destroy(trashObject);
        }
    }
}
