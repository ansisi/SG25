using UnityEngine;
using System;
using System.Collections;

public class TrashDespawnTimer : MonoBehaviour
{
    public float despawnTime = 5f; // ������ ���� �ð� (��)

    public static event Action<GameObject> OnTrashDespawned; // �����Ⱑ ������ �� �߻��ϴ� �̺�Ʈ

    private bool isDespawning = false; // �ı� ������ ����

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
        // ������ ���� �̺�Ʈ�� ���� �ڵ鷯�� �����մϴ�.
        TrashProduce.OnTrashGenerated -= HandleTrashGenerated;
    }

    // ������ ���� �̺�Ʈ �ڵ鷯
    void HandleTrashGenerated(GameObject trashObject)
    {
        // �����Ⱑ �����Ǹ� ���� Ÿ�̸Ӹ� �����մϴ�.
        StartCoroutine(DespawnTrashForGenerated(trashObject));
    }

    // ������ ���� Ÿ�̸� �ڷ�ƾ
    IEnumerator DespawnTrashForGenerated(GameObject trashObject)
    {
        yield return new WaitForSeconds(despawnTime);

        if (trashObject != null)
        {
            Debug.LogFormat("������ ������Ʈ�� �ı��Ǿ����ϴ�: �̸�: {0}, ��ġ: {1}", trashObject.name, trashObject.transform.position);
            if (OnTrashDespawned != null)
            {
                OnTrashDespawned(trashObject);
            }
            Destroy(trashObject);
        }
    }
}
