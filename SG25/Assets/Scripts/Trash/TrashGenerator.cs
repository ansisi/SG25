// TrashGenerator.cs

using UnityEngine;

public class TrashGenerator : MonoBehaviour
{
    public GameObject garbagePrefab; // ������ ������
    public int mapWidth = 10; // �� ���� ����
    public int mapHeight = 10; // �� ���� ����
    public float spawnInterval = 30f; // ������ ���� ���� (��)
    public float spawnChance = 0.5f; // ������ ���� Ȯ��
    public float trashTime;

    private Customer customer; // �մ� Ŭ���� ����
    private float lastSpawnTime; // ������ ������ ���� �ð�
    private int garbageCount = 0; // ������ ������ ����

    void Start()
    {
        customer = FindObjectOfType<Customer>(); // �մ� Ŭ���� �ν��Ͻ� ã��
        lastSpawnTime = Time.time; // ���� �ð� ����
        GenerateGarbage(); // ������ ���� ����
    }

    void GenerateGarbage()
    {
        // ������ Ȯ���� ���� ������ ���� ���� ����
        if (Random.value < spawnChance)
        {
            // ������ ��ġ ����
            float randomX = Random.Range(0, mapWidth);
            float randomY = Random.Range(0, mapHeight);
            Vector3 spawnPosition = new Vector3(randomX, 0f, randomY);

            // ������ �������� ������ ��ġ�� ����
            Instantiate(garbagePrefab, spawnPosition, Quaternion.identity);

            // ���� ��ġ�� �ֿܼ� ���
            Debug.Log("�����Ⱑ �����Ǿ����ϴ�. ��ġ: " + spawnPosition);

            // ������ ������ ���� ����
            garbageCount++;

            // ������ �����Ⱑ 4�� �̻��̸� �մ� ��� ���
            if (garbageCount >= 4 && customer != null)
            {
                customer.NotifyTrashCreated();
                // ������ ���� �ʱ�ȭ
                garbageCount = 0;
            }
        }

        // ���� ������ ���� ���� ����
        Invoke("GenerateGarbage", spawnInterval);
    }
}
//// void SpawnItem()
//{
//    float randomX = Random.Range(-13f, 13f);
//    Vector3 spawnPosition = new Vector3(randomX, 0f, randomX);
//    Instantiate(NapzakPrefab, spawnPosition, Quaternion.identity);
//}