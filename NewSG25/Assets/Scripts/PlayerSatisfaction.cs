using UnityEngine;
using UnityEngine.UI;

public class PlayerSatisfaction : MonoBehaviour
{
    protected float curSatisfaction; // ���� ������
    public float maxSatisfaction; // �ִ� ������
    public Slider satisfactionBarSlider; // �������� ǥ���ϴ� UI �����̴�

    public void SetSatisfaction(float amount) // ������ ����
    {
        maxSatisfaction = amount;
        curSatisfaction = maxSatisfaction;
        UpdateSatisfactionUI(); // UI ������Ʈ
    }

    public void CheckSatisfaction() // ������ ����
    {
        if (satisfactionBarSlider != null)
            satisfactionBarSlider.value = curSatisfaction / maxSatisfaction;

        if (curSatisfaction <= 0)
        {
            // �������� 0 �����̸� ��� ���� ȣ��
            BadEnding();
        }
        else if (curSatisfaction <= maxSatisfaction / 2)
        {
            // �������� �ִ� �������� ���� �����̸� ���� ���� ȣ��
            HappyEnding();
        }
    }

    public void DamageSatisfaction(float damage) // ������ ����
    {
        if (maxSatisfaction == 0 || curSatisfaction <= 0) // �̹� �������� 0 �����̸� �н�
            return;

        curSatisfaction -= damage;
        CheckSatisfaction(); // ������ üũ
    }

    void BadEnding() // ��� ����
    {
        Debug.Log("Bad Ending - Player satisfaction is too low.");
        // ��� ������ ���õ� ó���� ���⿡ ����
    }

    void HappyEnding() // ���� ����
    {
        Debug.Log("Happy Ending - Player satisfaction is high enough.");
        // ���� ������ ���õ� ó���� ���⿡ ����
    }

    void UpdateSatisfactionUI() // ������ UI ������Ʈ
    {
        if (satisfactionBarSlider != null)
            satisfactionBarSlider.value = curSatisfaction / maxSatisfaction;
    }
}
