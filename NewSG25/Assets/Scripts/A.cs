using UnityEngine;
using System;
using System.Collections;

public class A : MonoBehaviour
{
    public delegate void TrashGenerated(GameObject trashObject);
    public static event TrashGenerated OnTrashGenerated; // �����Ⱑ ������ �� �߻��ϴ� �̺�Ʈ

    public GameObject trashPrefab; // ������ ������ ������

    public float minInterval = 10f; // �ּ� ���� ���� (��)
    public float maxInterval = 20f; // �ִ� ���� ���� (��)

    private float nextTime; // ���� ������ ���� �ð�
    private int maxTrashCount = 5; // �ִ� ��� ������ ����
    private int minTrashCount = 1; // �ּ� ��� ������ ����

    void Start()
    {
        nextTime = Time.time + UnityEngine.Random.Range(minInterval, maxInterval);
    }

    void Update()
    {
        // ���� �ð��� ���� ���� �ð����� ũ�ų� ������
        if (Time.time >= nextTime)
        {
            // ���� ���� �ִ� "Trash" �±׸� ���� ������Ʈ�� ���� Ȯ��
            int trashCount = GameObject.FindGameObjectsWithTag("Trash").Length;

            // �ִ� ��� �������� ������ ������ ����
            if (trashCount < maxTrashCount)
            {
                // ������ ������ ���� �������� ���� (�ּ� ��� ���� ~ �ִ� ��� ����)
                int trashToSpawn = UnityEngine.Random.Range(minTrashCount, maxTrashCount + 1);

                for (int i = 0; i < trashToSpawn; i++)
                {
                    GameObject newTrash = Instantiate(trashPrefab, GetRandomPosition(), Quaternion.identity);
                    if (OnTrashGenerated != null)
                    {
                        OnTrashGenerated(newTrash); // �����Ⱑ ������ �� �̺�Ʈ �߻�
                    }
                    StartCoroutine(DestroyTrash(newTrash)); // ������ �����⸦ 5�� �Ŀ� �����ϴ� �ڷ�ƾ ����
                }

                // ���� ������ ���� �ð� ����
                nextTime = Time.time + UnityEngine.Random.Range(minInterval, maxInterval);
            }
        }
    }

    IEnumerator DestroyTrash(GameObject trashObject)
    {
        yield return new WaitForSeconds(5f); // 5�� ���
        if (trashObject != null)
        {
            Destroy(trashObject); // ������ ����
        }
    }

    Vector3 GetRandomPosition()
    {
        // �� ũ��
        float mapWidth = 10.0f;
        float mapHeight = 10.0f;

        // ���� ��ġ ����
        Vector3 randomPosition = new Vector3(
            UnityEngine.Random.Range(-mapWidth / 2, mapWidth / 2),
            0.2f, // Y ��ǥ�� 1�� ����
            UnityEngine.Random.Range(-mapHeight / 2, mapHeight / 2)
        );

        // Raycast �˻�
        int layerMask = ~LayerMask.GetMask("NoCollision"); // "NoCollision" Layer ����
        RaycastHit hit;

        // �浹 �߻� �� ���ο� ��ġ ���
        while (Physics.Raycast(randomPosition, Vector3.down, out hit, 1.0f, layerMask))
        {
            randomPosition = hit.point + Vector3.up * 0.1f; // �浹 �������� �ణ ���� ��ġ
        }

        // �浹���� �ʴ� ��ġ ��ȯ
        return randomPosition;
    }
}
