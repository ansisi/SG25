using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SatisfactionManager : MonoBehaviour
{
    [SerializeField] private Slider satisfactionSlider; // ������ �����̴�
    [SerializeField] private GameObject angryUI; // 0-25% UI
    [SerializeField] private GameObject upsetUI; // 26-50% UI
    [SerializeField] private GameObject neutralUI; // 51-75% UI
    [SerializeField] private GameObject happyUI; // 76-100% UI
    [SerializeField] private TextMeshProUGUI satisfationText;

    private void Start()
    {
        // �������� 100���� �ʱ�ȭ
        satisfactionSlider.value = 100;

        // �����̴��� OnValueChanged �̺�Ʈ�� UpdateSatisfactionUI �޼��带 ���
        satisfactionSlider.onValueChanged.AddListener(delegate { UpdateSatisfactionUI(); });

        // �ʱ� ������ UI ����
        UpdateSatisfactionUI();
    }

    private void Update()
    {
        if (!isInitialized)
        {
            isInitialized = true;
            UpdateSatisfactionUI();
        }

        satisfationText.text = satisfactionSlider.value.ToString();
    }

    private void UpdateSatisfactionUI()
    {
        float satisfactionValue = satisfactionSlider.value;

        // �������� ���� �ش� UI Ȱ��ȭ
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

        // ����� �α� ���
        Debug.Log("���� ������: " + satisfactionValue);
    }

    // �߰��� ����
    private bool isInitialized = false;

    public void DecreaseSatisfaction()
    {
        satisfactionSlider.value -= 10; // ������ ����
        Debug.Log("������ ����: " + satisfactionSlider.value);

        // �ּҰ� ����
        if (satisfactionSlider.value < 0)
        {
            satisfactionSlider.value = 0;
        }

        // ������ ���� �� UI ������Ʈ
        UpdateSatisfactionUI();
    }
}
