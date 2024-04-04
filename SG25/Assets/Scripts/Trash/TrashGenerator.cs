
using UnityEngine;

public class TrashGenerator : MonoBehaviour
{
    public GameObject trashPrefab; // ������ ������ ������

    public float minInterval = 10f; // �ּ� ���� ���� (��)
    public float maxInterval = 20f; // �ִ� ���� ���� (��)

    private float nextTime; // ���� ������ ���� �ð�

    void Start()
    {
        nextTime = Time.time + Random.Range(minInterval, maxInterval);
    }

    void Update()
    {
        // ���� �ð��� ���� ���� �ð����� ũ�ų� ������
        if (Time.time >= nextTime)
        {
            // ������ ����
            Instantiate(trashPrefab, GetRandomPosition(), Quaternion.identity);

            // ������ ������ 3�� �̻��̸� �մԿ��� �˸�
            if (GameObject.FindGameObjectsWithTag("Trash").Length >= 3)
            {
                // ���⿡ �մԿ��� �˸��� ������ �ڵ带 �ۼ��մϴ�.
                // ��:

                GameObject[] customers = GameObject.FindGameObjectsWithTag("Customer");
                foreach (GameObject customer in customers)
                {
                    customer.GetComponent<Customer>().Angry();
                }
            }

            // ���� ������ ���� �ð� ����
            nextTime = Time.time + Random.Range(minInterval, maxInterval);
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
            1.0f, // Y ��ǥ�� 1�� ����
            Random.Range(-mapHeight / 2, mapHeight / 2)
        );
    }
}