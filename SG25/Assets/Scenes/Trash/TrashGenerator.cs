using UnityEngine;

public class TrashGenerator : MonoBehaviour
{
    public GameObject trashPrefab; // ������ ������ ������

    public float minInterval = 10f; // �ּ� ���� ���� (��)
    public float maxInterval = 20f; // �ִ� ���� ���� (��)

    private float nextTime; // ���� ������ ���� �ð�
    private int maxTrashCount = 5; // �ִ� ��� ������ ����

    private HealthManager healthManager;

    void Start()
    {
        nextTime = Time.time + Random.Range(minInterval, maxInterval);

        healthManager = HealthManager.Instance;

        if (healthManager == null)
        {
            Debug.LogError("HealthManager �ν��Ͻ��� ã�� �� �����ϴ�.");
        }
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
                Instantiate(trashPrefab, GetRandomPosition(), Quaternion.identity);

                // ... (������ ���� üũ �� �մ� �˸� �ڵ�)

                // ���� ������ ���� �ð� ����
                nextTime = Time.time + Random.Range(minInterval, maxInterval);
            }
        }
    }

    Vector3 GetRandomPosition()
    {
        // �� ũ��
        float mapWidth = 10.0f;
        float mapHeight = 10.0f;

        // �� ��� �� ���� ��ġ ����
        return new Vector3(
            Random.Range(-mapWidth / 2, mapWidth / 2),
            0.2f, // Y ��ǥ�� 1�� ����
            Random.Range(-mapHeight / 2, mapHeight / 2)
        );
    }
    private void OnTriggerEnter(Collider other)
    {
        // �浹�� ������Ʈ�� ���������� Ȯ��
        if (other.CompareTag("Trash"))
        {
            // �����⸦ ġ�����Ƿ� ü���� 5 ������Ŵ
            healthManager.IncreaseHealth(5);
        }
    }


}
