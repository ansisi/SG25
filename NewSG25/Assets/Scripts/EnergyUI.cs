using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnergyUI : MonoBehaviour
{
    [SerializeField] private GameObject[] energyUIObjects; // ������ UI ������Ʈ �迭
    [SerializeField] private GameObject gameOverPanel; // ���� ���� �г�
    [SerializeField] private int initialEnergyCount = 3; // �ʱ� ������ ����

    private int energyCount; // ���� ������ ����
    private bool isGameOver = false; // ���� ���� ���� Ȯ��

    void Start()
    {
        energyCount = initialEnergyCount;
        UpdateEnergyUI();

        TrashDespawnTimer.OnTrashDespawned += HandleTrashDespawned;
    }

    void OnDestroy()
    {
        TrashDespawnTimer.OnTrashDespawned -= HandleTrashDespawned;
    }

    void HandleTrashDespawned(GameObject trashObject)
{
    if (isGameOver) return; // ���� ���� �����̸� ������Ʈ ����

    if (energyCount > 0)
    {
        energyCount--;
        Debug.Log("Energy decreased: " + energyCount); // ����� �α� �߰�
        UpdateEnergyUI();
    }

    if (energyCount <= 0 && !isGameOver)
    {
        Debug.Log("Game Over - The game has ended."); // ������ ����� �α�
        // gameOverPanel.SetActive(true); // �ּ� ó�� �Ǵ� ����
        isGameOver = true; // ���� ���� ���� ����
        EndGame(); // ���� ���� ó�� �Լ� ȣ��
    }
}

void EndGame()
{
    // ���� ���� ó���� ���� �߰����� ������ �ʿ��ϴٸ� ���⿡ ����
    Debug.Log("The game has officially ended."); // ���� ���� Ȯ���� ���� ����� �޽���
}


    void UpdateEnergyUI()
    {
        Debug.Log("Updating Energy UI: " + energyCount); // ����� �α� �߰�
        for (int i = 0; i < energyUIObjects.Length; i++)
        {
            if (i < energyCount)
            {
                energyUIObjects[i].SetActive(true);
            }
            else
            {
                energyUIObjects[i].SetActive(false);
            }
        }
    }
}
