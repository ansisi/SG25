using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SatisfactionManager : MonoBehaviour
{
    [SerializeField] private Slider satisfactionSlider; // ������ �����̴�
    [SerializeField] private GameObject angryUI; // 0-25% UI
    [SerializeField] private GameObject upsetUI; // 26-50% UI
    [SerializeField] private GameObject neutralUI; // 51-75% UI
    [SerializeField] private GameObject happyUI; // 76-100% UI
    [SerializeField] private TextMeshProUGUI satisfactionText; // �������� ǥ���� �ؽ�Ʈ UI ���

    private void Start()
    {
        // �������� 100���� �ʱ�ȭ
        satisfactionSlider.minValue = 0; // �ּҰ� ����
        satisfactionSlider.maxValue = 100; // �ִ밪 ����
        satisfactionSlider.value = 100;

        // �����̴��� OnValueChanged �̺�Ʈ�� UpdateSatisfactionUI �޼��带 ���
        satisfactionSlider.onValueChanged.AddListener(delegate { UpdateSatisfactionUI(); });

        // �ʱ� ������ UI ����
        UpdateSatisfactionUI();
    }

    private void Update()
    {
        // �ʱ�ȭ�� �Ǿ����� Ȯ��
        if (!isInitialized)
        {
            isInitialized = true;
            UpdateSatisfactionUI();
        }

        // �ؽ�Ʈ ������Ʈ
        if (satisfactionText != null && satisfactionSlider != null)
        {
            satisfactionText.text = ((int)satisfactionSlider.value).ToString(); // ������ ���� �ؽ�Ʈ�� ��ȯ�Ͽ� �Ҵ�
        }
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
        satisfactionSlider.value -= 1; // ������ ����
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
