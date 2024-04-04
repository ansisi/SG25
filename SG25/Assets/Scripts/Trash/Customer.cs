using UnityEngine;

public class Customer : MonoBehaviour
{
    // 쓰레기 생성 알림 함수
    public void NotifyTrashCreated()
    {
        SayRandomDialogue();
    }

    // 손님이 할 수 있는 대사들
    public string[] customerDialogues = { "더러워요", "빨리 치워주세요", "냄새나잖아요!" };

    // 대사 출력 간격
    public float dialogueInterval = 10f;
    private float lastDialogueTime;

    // 대사 출력 함수
    public void SayRandomDialogue()
    {
        // 랜덤한 대사 선택
        string randomDialogue = customerDialogues[Random.Range(0, customerDialogues.Length)];

        // 대사 출력
        Debug.Log("손님 대사: " + randomDialogue);

        // 마지막 대사 출력 시간 갱신
        lastDialogueTime = Time.time;
    }

    void Start()
    {
        lastDialogueTime = Time.time;
    }

    void Update()
    {
        // 일정 간격마다 대사 출력
        if (Time.time - lastDialogueTime >= dialogueInterval)
        {
            SayRandomDialogue();
        }
    }
}
