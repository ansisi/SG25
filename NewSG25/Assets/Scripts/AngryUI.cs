using UnityEngine;
using UnityEngine.UI;

public class AngryUI : MonoBehaviour
{
    [SerializeField] private GameObject angryUIGauge; // ������ UI ������Ʈ �迭

    private int remainingAngryCount; // ���� ������ ����
    private bool isGameOver = false; // ���� ���� ���� Ȯ��

    void Start()
    {
        //remainingAngryCount = energyUIObjects.Length; // ���� ������ ������ �ʱ�ȭ
        UpdateEnergyUI(); // �ʱ� ������ UI ����

        
    }

    void OnDestroy()
    {
        
    }

    void HandleTrashDespawned(GameObject trashObject)
    {
        if (isGameOver) return; // ���� ���� �����̸� ������Ʈ ����

        remainingAngryCount--; // ���� ������ ���� ����
        UpdateEnergyUI(); // UI ������Ʈ

        if (remainingAngryCount <= 0 && !isGameOver)
        {
            Debug.Log("Game Over - The game has ended."); // ���� ���� �α� ���
            // gameOverPanel.SetActive(true); // ���� ���� �г� Ȱ��ȭ (�ּ� ó�� �Ǵ� ����)
            isGameOver = true; // ���� ���� ���� ����
            EndGame(); // ���� ���� ó�� �Լ� ȣ��
        }
    }

    void EndGame()
    {
        // ���� ���� ó���� ���� �߰����� ������ �ʿ��ϴٸ� ���⿡ ����
        Debug.Log("The game has officially ended."); // ���� ���� Ȯ�� �α� ���
    }

    void UpdateEnergyUI()
    {
        Debug.Log("Updating Energy UI: " + remainingAngryCount); // ������ UI ������Ʈ �α� ���

            angryUIGauge.SetActive(false); // ���� ������ ������ ���� UI Ȱ��ȭ ����
            }
}
