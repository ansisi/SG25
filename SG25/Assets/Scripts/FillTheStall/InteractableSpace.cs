using UnityEngine;

public class InteractableSpace : MonoBehaviour
{
    // ������ ���� ��ġ
    public Transform spawnPoint;

    // ������ ������ �迭
    public GameObject[] itemPrefabs;

    // ������ ������ Ȯ�� �迭 (�� Ȯ�� 100% ���� ��)
    [Range(0.0f, 1.0f)]
    public float[] itemProbabilities;

    // ������ ���� ���� �÷���
    private bool isItemSpawned = false;

    // ������ ������Ʈ (������ ���� �뵵)
    public GameObject displayObject;

    // �����뿡 ������ ������ ������
    public GameObject displayItemPrefab;

    // �����뿡 ������ ������ ����
    public int displayItemCount = 3;

    void Start()
    {
        // ������ ������Ʈ ����
        if (displayObject == null)
        {
            displayObject = transform.Find("DisplayObject").gameObject;
        }

        // ������ ������ �ʱ�ȭ
        UpdateDisplayItems();
    }

    void Update()
    {
        // ���� Ŭ�� �� ������ ����
        if (Input.GetMouseButtonDown(0) && !isItemSpawned)
        {
            SpawnItem();
            isItemSpawned = true;
        }
    }

    // ������ ���� �Լ�
    private void SpawnItem()
    {
        // ���� Ȯ�� ������� ������ �ε��� ����
        int itemIndex = RandomSelectionBasedOnProbability(itemProbabilities);

        // ������ ������ �ν��Ͻ� ����
        GameObject itemInstance = Instantiate(itemPrefabs[itemIndex], spawnPoint.position, spawnPoint.rotation);

        // ������ �±� ����
        itemInstance.tag = "Item";

        // ������ ��ũ��Ʈ �ʱ�ȭ (�ʿ��� ���)
        Consumable consumable = itemInstance.GetComponent<Consumable>();
        if (consumable != null)
        {
            // ������ ���� ����
            consumable.item = new Item(); // ������ ���� ���� �ڵ� �߰�
        }

        // ������ ������ ������Ʈ
        UpdateDisplayItems();
    }

    // ������ ������ ������Ʈ �Լ�
    private void UpdateDisplayItems()
    {
        // ���� ������ ������ ����
        foreach (Transform child in displayObject.transform)
        {
            Destroy(child.gameObject);
        }

        // ���ο� ������ ������ ����
        for (int i = 0; i < displayItemCount; i++)
        {
            GameObject displayItemInstance = Instantiate(displayItemPrefab, displayObject.transform);
            displayItemInstance.transform.localPosition = new Vector3(i * 0.5f, 0, 0); // ���� ����
        }
    }

    // ���� ���� �Լ� (Ȯ�� ���)
    private int RandomSelectionBasedOnProbability(float[] probabilities)
    {
        float totalProbability = 0.0f;
    foreach (float probability in probabilities)
        {
            totalProbability += probability;
        }

        float randomValue = Random.value * totalProbability;
        float accumulatedProbability = 0.0f;
        for (int i = 0; i < probabilities.Length; i++)
        {
            accumulatedProbability += probabilities[i];
            if (randomValue < accumulatedProbability)
            {
                return i; // ���� return �� �߰�
            }
        }

        // ��� Ȯ���� �� ������� ���� ��� (��: Ȯ�� ���� 1.0���� ����)
        Debug.LogError("����: ��� Ȯ���� ������ �ʾҽ��ϴ�.");
        return probabilities.Length - 1; // ������ �ε��� ��ȯ (�⺻��)
    }

}
