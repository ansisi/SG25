using UnityEngine;

public class TrashSpawner : MonoBehaviour
{
    public GameObject trashPrefab; // ������ ������
    public float trashSpawnTime = 5f; // ������ ���� ���� (��)

    private float timeSinceLastTrash = 0f;

    void FixedUpdate()  
    {
        timeSinceLastTrash += Time.deltaTime;

        if (timeSinceLastTrash > trashSpawnTime)
        {
            // ���� �ð� ���ݸ��� ������ ����
            Instantiate(trashPrefab, transform.position, transform.rotation);
            timeSinceLastTrash = 0f;
        }
    }



}
