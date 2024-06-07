using UnityEngine;
using UnityEngine.UI;

public class SatisfactionManager : MonoBehaviour
{
    [SerializeField] private Slider satisfactionSlider; // 만족도 슬라이더
    [SerializeField] private GameObject angryUI; // 0-25% UI
    [SerializeField] private GameObject upsetUI; // 26-50% UI
    [SerializeField] private GameObject neutralUI; // 51-75% UI
    [SerializeField] private GameObject happyUI; // 76-100% UI

    private void Start()
    {
        // 만족도를 100으로 초기화
        satisfactionSlider.value = 100f;

        // 초기 만족도 UI 설정
        UpdateSatisfactionUI();

        // 슬라이더의 OnValueChanged 이벤트에 UpdateSatisfactionUI 메서드를 등록
        satisfactionSlider.onValueChanged.AddListener(delegate { UpdateSatisfactionUI(); });
    }

    private void UpdateSatisfactionUI()
    {
        float satisfactionValue = satisfactionSlider.value;

        // 모든 UI 비활성화
        angryUI.SetActive(false);
        upsetUI.SetActive(false);
        neutralUI.SetActive(false);
        happyUI.SetActive(false);

        // 만족도에 따라 해당 UI 활성화
        if (satisfactionValue <= 25)
        {
            angryUI.SetActive(true);
        }
        else if (satisfactionValue <= 50)
        {
            upsetUI.SetActive(true);
        }
        else if (satisfactionValue <= 75)
        {
            neutralUI.SetActive(true);
        }
        else
        {
            happyUI.SetActive(true);
        }

        // 디버그 로그 출력
        Debug.Log("현재 만족도: " + satisfactionValue);
    }
}
