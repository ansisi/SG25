using UnityEngine;

public class Customer : MonoBehaviour
{
    // ������ ���� �˸� �Լ�
    public void NotifyTrashCreated()
    {
        SayRandomDialogue();
    }

    // �մ��� �� �� �ִ� ����
    public string[] customerDialogues = { "��������", "���� ġ���ּ���", "�������ݾƿ�!" };

    // ��� ��� ����
    public float dialogueInterval = 10f;
    private float lastDialogueTime;

    // ��� ��� �Լ�
    public void SayRandomDialogue()
    {
        // ������ ��� ����
        string randomDialogue = customerDialogues[Random.Range(0, customerDialogues.Length)];

        // ��� ���
        Debug.Log("�մ� ���: " + randomDialogue);

        // ������ ��� ��� �ð� ����
        lastDialogueTime = Time.time;
    }

    void Start()
    {
        lastDialogueTime = Time.time;
    }

    void Update()
    {
        // ���� ���ݸ��� ��� ���
        if (Time.time - lastDialogueTime >= dialogueInterval)
        {
            SayRandomDialogue();
        }
    }
}
