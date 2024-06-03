using UnityEngine;
using TMPro;


public class Experience : MonoBehaviour
{
    public GameManager GameManagerInstance; // GameManager �ν��Ͻ� ����

    public int currentExperience; // ���� ����ġ ����
    public int level; // ���� ����

    public TextMeshProUGUI textExperience; // ����ġ ǥ�ÿ� �ؽ�Ʈ UI

    private int levelUpThreshold = 5; // ���� �� �ѵ�
    private int maxExperience = 200; // �ִ� ����ġ (������ �̳�)
    private int maxExperienceHighLevel = 500; // ���� �ִ� ����ġ

    private void Start()
    {
        // Find the GameManager GameObject in the scene
        GameObject gameManagerObject = GameObject.Find("GameManager");
        //GameObiect.Find("GameManager") �� �Ҵ��� ���� �̸��� ���ƾ��մϴ�.


        if (gameManagerObject != null)
        {
            GameManagerInstance = gameManagerObject.GetComponent<GameManager>();
        }
        else
        {
            Debug.Log("GameManager �ν��Ͻ��� ã�� �� �����ϴ�!");
        }


        if (GameManagerInstance != null)
        {
            currentExperience = GameManagerInstance.currentExperience;
            level = GameManagerInstance.level;
            UpdateExperienceDisplay();
        }
    }


    public void GainExperience(int amount)
    {
        // ������ �̳����� Ȯ��
        if (GameManagerInstance.IsWithinFirstWeek()) // 'public' �Ǵ� 'protected'�� ����� IsWithinFirstWeek() �޼��� ȣ��
        {
            // �ִ� ����ġ�� �ʰ����� �ʵ��� ����
            currentExperience = Mathf.Min(currentExperience + amount, maxExperience);
        }
        else
        {
            // ���� ������ n���� �̻����� Ȯ��
            if (level >= levelUpThreshold)
            {
                // ���� �ִ� ����ġ�� �ʰ����� �ʵ��� ����
                currentExperience = Mathf.Min(currentExperience + amount, maxExperienceHighLevel);
            }
            else
            {
                // �Ϲ� �ִ� ����ġ�� �ʰ����� �ʵ��� ����
                currentExperience = Mathf.Min(currentExperience + amount, maxExperience);
            }
        }

        UpdateExperienceDisplay();
        GameManagerInstance.currentExperience = currentExperience; // GameManager ��ũ��Ʈ�� ����ġ ������Ʈ
        GameManagerInstance.CheckForLevelUp(); // ���� �� Ȯ��
    }

    public void LoseExperience(int amount)
    {
        currentExperience -= amount;
        UpdateExperienceDisplay();
        GameManagerInstance.currentExperience = currentExperience; // GameManager ��ũ��Ʈ�� ����ġ ������Ʈ
    }

    private void UpdateExperienceDisplay()
    {
        if (textExperience != null)
        {
            textExperience.text = "����ġ: " + Mathf.Min(currentExperience, (level >= levelUpThreshold ? maxExperienceHighLevel : maxExperience)) + "/" + (level >= levelUpThreshold ? maxExperienceHighLevel : maxExperience);

        }
    }
}
