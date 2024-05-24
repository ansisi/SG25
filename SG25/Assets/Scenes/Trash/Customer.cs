using UnityEngine;

public class Customer : MonoBehaviour
{
    public float patienceTime = 10f; // 손님의 최대 인내 시간 (초)
    public float angerRate = 0.5f; // 화가 나는 속도 (초당 증가량)

    private float anger; // 현재 화가 나는 정도
    private bool isAngry; // 화가 났는지 여부
    private float lastAngryTime; // 마지막 화가 난 시간

    string[] angryLines = { "더러워!", "냄새나!", "빨리 치워!", "지저분해!" };

    void Start()
    {
        anger = 0f;
        isAngry = false;
        lastAngryTime = Time.time;
    }

    void Update()
    {
        // 일정 시간이 지나면 화가 난다
        if (!isAngry)
        {
            anger += Time.deltaTime * angerRate;
            if (anger >= patienceTime)
            {
                isAngry = true;
                Debug.Log("[Customer] Update: 손님이 화났습니다!");
            }
        }

        // 화가 났으면 대사를 출력한다
        if (isAngry)
        {
            // 20초 간격으로 대사를 출력한다
            if (Time.time - lastAngryTime >= 20f)
            {
                // 랜덤 대사 출력
                int index = Random.Range(0, angryLines.Length);
                Debug.Log("[Customer] Update: " + angryLines[index]);

                lastAngryTime = Time.time;
            }
        }
    }

    public void Angry()
    {
        // 쓰레기가 3개 이상 쌓였을 때 호출
        isAngry = true;
        Debug.Log("[Customer] Update: 손님이 화났습니다!");
    }

    public void ResetAnger()
    {
        // 쓰레기를 치웠을 때 호출
        isAngry = false;
        anger = 0f;
    }
}