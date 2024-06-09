using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SatisfactionManager : MonoBehaviour
{
    [SerializeField] private Slider satisfactionSlider; // 만족도 슬라이더
    [SerializeField] private GameObject angryUI; // 0-25% UI
    [SerializeField] private GameObject upsetUI; // 26-50% UI
    [SerializeField] private GameObject neutralUI; // 51-75% UI
    [SerializeField] private GameObject happyUI; // 76-100% UI
    [SerializeField] private TextMeshProUGUI satisfactionText; // 만족도를 표시할 텍스트 UI 요소

    private void Start()
    {
        // 만족도를 100으로 초기화
        satisfactionSlider.minValue = 0; // 최소값 설정
        satisfactionSlider.maxValue = 100; // 최대값 설정
        satisfactionSlider.value = 100;

        // 슬라이더의 OnValueChanged 이벤트에 UpdateSatisfactionUI 메서드를 등록
        satisfactionSlider.onValueChanged.AddListener(delegate { UpdateSatisfactionUI(); });

        // 초기 만족도 UI 설정
        UpdateSatisfactionUI();
    }

    private void Update()
    {
        // 초기화가 되었는지 확인
        if (!isInitialized)
        {
            isInitialized = true;
            UpdateSatisfactionUI();
        }

        // 텍스트 업데이트
        if (satisfactionText != null && satisfactionSlider != null)
        {
            satisfactionText.text = ((int)satisfactionSlider.value).ToString(); // 만족도 값을 텍스트로 변환하여 할당
        }
    }

    private void UpdateSatisfactionUI()
    {
        float satisfactionValue = satisfactionSlider.value;

        // 만족도에 따라 해당 UI 활성화
        if (satisfactionValue <= 25)
        {
            angryUI.SetActive(true);
            upsetUI.SetActive(false);
            neutralUI.SetActive(false);
            happyUI.SetActive(false);
        }
        else if (satisfactionValue <= 50)
        {
            angryUI.SetActive(false);
            upsetUI.SetActive(true);
            neutralUI.SetActive(false);
            happyUI.SetActive(false);
        }
        else if (satisfactionValue <= 75)
        {
            angryUI.SetActive(false);
            upsetUI.SetActive(false);
            neutralUI.SetActive(true);
            happyUI.SetActive(false);
        }
        else
        {
            angryUI.SetActive(false);
            upsetUI.SetActive(false);
            neutralUI.SetActive(false);
            happyUI.SetActive(true);
        }

        // 디버그 로그 출력
        Debug.Log("현재 만족도: " + satisfactionValue);
    }

    // 추가된 변수
    private bool isInitialized = false;

    public void DecreaseSatisfaction()
    {
        satisfactionSlider.value -= 1; // 만족도 감소
        Debug.Log("만족도 감소: " + satisfactionSlider.value);

        // 최소값 제한
        if (satisfactionSlider.value < 0)
        {
            satisfactionSlider.value = 0;
        }

        // 만족도 감소 후 UI 업데이트
        UpdateSatisfactionUI();
    }
}
