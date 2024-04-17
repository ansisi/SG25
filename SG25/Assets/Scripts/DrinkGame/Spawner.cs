using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject drinkPrefab; // ������ ������ ������
    public float spawnInterval = 2f; // ���� ���� ����
    public float spawnRange = 2f; // ���� ���� ��ġ�� ����

    private float nextSpawnTime; // ���� ���� ���� �ð�

    void Start()
    {
        // ���� ���� ���� �ð� �ʱ�ȭ
        nextSpawnTime = Time.time + spawnInterval;
    }

    void Update()
    {
        // ���� �ð��� ���� ���� �ð����� ũ�� ���Ḧ �����մϴ�.
        if (Time.time >= nextSpawnTime)
        {
            SpawnDrink();
            nextSpawnTime = Time.time + spawnInterval; // ���� ���� ���� �ð� ����
        }
    }

    void SpawnDrink()
    {
        // ������ ��ġ�� ���Ḧ �����մϴ�.
        Vector3 spawnPosition = transform.position + new Vector3(Random.Range(-spawnRange, spawnRange), 0f, 0f);
        Instantiate(drinkPrefab, spawnPosition, Quaternion.identity);
    }
}
