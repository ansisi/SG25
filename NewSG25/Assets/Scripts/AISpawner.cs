using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawner : MonoBehaviour
{
    public GameObject CustomerPrefab;
    public float spawnRateMin = 5.0f;
    public float spawnRateMax = 10.0f;

    private float spawnRate;
    private float timeAfterSpawn;

    void Start()
    {
        timeAfterSpawn = 0f;
        spawnRate = Random.Range(spawnRateMin, spawnRateMax);
    }

    // Update is called once per frame
    void Update()
    {
        timeAfterSpawn += Time.deltaTime;

        if(timeAfterSpawn >= spawnRate)
        {
            timeAfterSpawn = 0f;
            GameObject customer = Instantiate(CustomerPrefab);
            spawnRate = Random.Range(spawnRateMin, spawnRateMax);
        }
    }
}
