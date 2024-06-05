using UnityEngine;
using UnityEngine.UI;

public class PlayerSatisfaction : MonoBehaviour
{
    protected float curSatisfaction; // 현재 만족도
    public float maxSatisfaction; // 최대 만족도
    public Slider satisfactionBarSlider; // 만족도를 표시하는 UI 슬라이더

    public void SetSatisfaction(float amount) // 만족도 설정
    {
        maxSatisfaction = amount;
        curSatisfaction = maxSatisfaction;
        UpdateSatisfactionUI(); // UI 업데이트
    }

    public void CheckSatisfaction() // 만족도 갱신
    {
        if (satisfactionBarSlider != null)
            satisfactionBarSlider.value = curSatisfaction / maxSatisfaction;

        if (curSatisfaction <= 0)
        {
            // 만족도가 0 이하이면 배드 엔딩 호출
            BadEnding();
        }
        else if (curSatisfaction <= maxSatisfaction / 2)
        {
            // 만족도가 최대 만족도의 절반 이하이면 해피 엔딩 호출
            HappyEnding();
        }
    }

    public void DamageSatisfaction(float damage) // 만족도 감소
    {
        if (maxSatisfaction == 0 || curSatisfaction <= 0) // 이미 만족도가 0 이하이면 패스
            return;

        curSatisfaction -= damage;
        CheckSatisfaction(); // 만족도 체크
    }

    void BadEnding() // 배드 엔딩
    {
        Debug.Log("Bad Ending - Player satisfaction is too low.");
        // 배드 엔딩에 관련된 처리를 여기에 구현
    }

    void HappyEnding() // 해피 엔딩
    {
        Debug.Log("Happy Ending - Player satisfaction is high enough.");
        // 해피 엔딩에 관련된 처리를 여기에 구현
    }

    void UpdateSatisfactionUI() // 만족도 UI 업데이트
    {
        if (satisfactionBarSlider != null)
            satisfactionBarSlider.value = curSatisfaction / maxSatisfaction;
    }
}
