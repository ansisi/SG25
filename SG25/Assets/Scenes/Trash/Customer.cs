using UnityEngine;

public class Customer : MonoBehaviour
{
    public float patienceTime = 10f; // �մ��� �ִ� �γ� �ð� (��)
    public float angerRate = 0.5f; // ȭ�� ���� �ӵ� (�ʴ� ������)

    private float anger; // ���� ȭ�� ���� ����
    private bool isAngry; // ȭ�� ������ ����
    private float lastAngryTime; // ������ ȭ�� �� �ð�

    string[] angryLines = { "������!", "������!", "���� ġ��!", "��������!" };

    void Start()
    {
        anger = 0f;
        isAngry = false;
        lastAngryTime = Time.time;
    }

    void Update()
    {
        // ���� �ð��� ������ ȭ�� ����
        if (!isAngry)
        {
            anger += Time.deltaTime * angerRate;
            if (anger >= patienceTime)
            {
                isAngry = true;
                Debug.Log("[Customer] Update: �մ��� ȭ�����ϴ�!");
            }
        }

        // ȭ�� ������ ��縦 ����Ѵ�
        if (isAngry)
        {
            // 20�� �������� ��縦 ����Ѵ�
            if (Time.time - lastAngryTime >= 20f)
            {
                // ���� ��� ���
                int index = Random.Range(0, angryLines.Length);
                Debug.Log("[Customer] Update: " + angryLines[index]);

                lastAngryTime = Time.time;
            }
        }
    }

    public void Angry()
    {
        // �����Ⱑ 3�� �̻� �׿��� �� ȣ��
        isAngry = true;
        Debug.Log("[Customer] Update: �մ��� ȭ�����ϴ�!");
    }

    public void ResetAnger()
    {
        // �����⸦ ġ���� �� ȣ��
        isAngry = false;
        anger = 0f;
    }
}