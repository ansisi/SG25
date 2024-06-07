using UnityEngine;
using UnityEngine.UI;

public class SatisfactionManager : MonoBehaviour
{
    [SerializeField] private Slider satisfactionSlider; // ������ �����̴�
    [SerializeField] private GameObject angryUI; // 0-25% UI
    [SerializeField] private GameObject upsetUI; // 26-50% UI
    [SerializeField] private GameObject neutralUI; // 51-75% UI
    [SerializeField] private GameObject happyUI; // 76-100% UI

    private void Start()
    {
        // �������� 100���� �ʱ�ȭ
        satisfactionSlider.value = 100f;

        // �ʱ� ������ UI ����
        UpdateSatisfactionUI();

        // �����̴��� OnValueChanged �̺�Ʈ�� UpdateSatisfactionUI �޼��带 ���
        satisfactionSlider.onValueChanged.AddListener(delegate { UpdateSatisfactionUI(); });
    }

    private void UpdateSatisfactionUI()
    {
        float satisfactionValue = satisfactionSlider.value;

        // ��� UI ��Ȱ��ȭ
        angryUI.SetActive(false);
        upsetUI.SetActive(false);
        neutralUI.SetActive(false);
        happyUI.SetActive(false);

        // �������� ���� �ش� UI Ȱ��ȭ
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

        // ����� �α� ���
        Debug.Log("���� ������: " + satisfactionValue);
    }
}
