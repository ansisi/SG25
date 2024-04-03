using UnityEngine;
using System.Collections.Generic;

public class TrashGenerator : MonoBehaviour
{
    public GameObject garbagePrefab; // ������ ������
    public int mapWidth = 10; // �� ���� ����
    public int mapHeight = 10; // �� ���� ����
    public float minSpawnInterval = 10f; // �ּ� ������ ���� ���� (��)
    public float maxSpawnInterval = 30f; // �ִ� ������ ���� ���� (��)
    public float spawnChance = 0.5f; // ������ ���� Ȯ��

    public List<string> customerDialogues = new List<string>() {
        "��������!", "�̰� ������?", "���� ���׿�!", "���� ġ���ּ���!"
    }; // �մ� ��� ����Ʈ
    private int garbageCount = 0; // ������ ������ ����

    void Start()
    {
        // ������ �� ������ ���� ����
        GenerateGarbage();
    }

    void GenerateGarbage()
    {
        // ������ Ȯ���� ���� ������ ���� ���� ����
        if (Random.value < spawnChance)
        {
            // ������ ��ġ ����
            float randomX = Random.Range(0, mapWidth);
            float randomY = Random.Range(0, mapHeight);
            Vector3 spawnPosition = new Vector3(randomX, 0.5f, randomY); // ���� ����

            // ������ �������� ������ ��ġ�� ����
            Instantiate(garbagePrefab, spawnPosition, Quaternion.identity);

            // ������ ������ ���� �ֿܼ� ���
            Debug.Log("�����Ⱑ �����Ǿ����ϴ�. ��ġ: " + spawnPosition);

            // ������ ������ ���� ����
            garbageCount++;

            // ������ �����Ⱑ 2�� �̻��̸� �մ� ��� ���
            if (garbageCount >= 2)
            {
                Complain();
                // �մ� ��簡 ��µ� ��, ������ ���� �ʱ�ȭ
                garbageCount = 0;
            }
        }

        // ���� ������ ���� ���� ����
        float nextSpawnTime = Random.Range(minSpawnInterval, maxSpawnInterval);
        Invoke("GenerateGarbage", nextSpawnTime);
    }

    // �մ� ��� ��� �޼���
    void Complain()
    {
        if (customerDialogues.Count > 0)
        {
            // ������ ��� ����
            int randomIndex = Random.Range(0, customerDialogues.Count);
            string dialogue = customerDialogues[randomIndex];
            Debug.Log("�մ� ���: " + dialogue);

            // ��� ��� �� ����Ʈ���� ����
            customerDialogues.RemoveAt(randomIndex);
        }
    }
}
