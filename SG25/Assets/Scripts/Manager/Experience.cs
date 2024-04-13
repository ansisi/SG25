using UnityEngine;
using TMPro;

public class Experience : MonoBehaviour
{
    public GameManager GameManagerInstance; // GameManager 인스턴스 참조

    public int currentExperience; // 현재 경험치 변수
    public int level; // 레벨 변수

    public TextMeshProUGUI textExperience; // 경험치 표시용 텍스트 UI

    private int levelUpThreshold = 5; // 레벨 업 한도
    private int maxExperience = 200; // 최대 경험치 (일주일 이내)
    private int maxExperienceHighLevel = 500; // 고레벨 최대 경험치

    private void Start()
    {
        // Find the GameManager GameObject in the scene
        GameObject gameManagerObject = GameObject.Find("GameManager");
        //GameObiect.Find("GameManager") 에 할당한 실제 이름과 같아야합니다.

        
        if (gameManagerObject != null)
        {
            GameManagerInstance = gameManagerObject.GetComponent<GameManager>();
        }
        else
        {
            Debug.Log("GameManager 인스턴스를 찾을 수 없습니다!");
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
        // 일주일 이내인지 확인
        if (GameManagerInstance.IsWithinFirstWeek()) // 'public' 또는 'protected'로 변경된 IsWithinFirstWeek() 메서드 호출
        {
            // 최대 경험치를 초과하지 않도록 제한
            currentExperience = Mathf.Min(currentExperience + amount, maxExperience);
        }
        else
        {
            // 현재 레벨이 n레벨 이상인지 확인
            if (level >= levelUpThreshold)
            {
                // 고레벨 최대 경험치를 초과하지 않도록 제한
                currentExperience = Mathf.Min(currentExperience + amount, maxExperienceHighLevel);
            }
            else
            {
                // 일반 최대 경험치를 초과하지 않도록 제한
                currentExperience = Mathf.Min(currentExperience + amount, maxExperience);
            }
        }

        UpdateExperienceDisplay();
        GameManagerInstance.currentExperience = currentExperience; // GameManager 스크립트의 경험치 업데이트
        GameManagerInstance.CheckForLevelUp(); // 레벨 업 확인
    }

    public void LoseExperience(int amount)
    {
        currentExperience -= amount;
        UpdateExperienceDisplay();
        GameManagerInstance.currentExperience = currentExperience; // GameManager 스크립트의 경험치 업데이트
    }

    private void UpdateExperienceDisplay()
    {
        if (textExperience != null)
        {
            textExperience.text = "경험치: " + Mathf.Min(currentExperience, (level >= levelUpThreshold ? maxExperienceHighLevel : maxExperience)) + "/" + (level >= levelUpThreshold ? maxExperienceHighLevel : maxExperience);

        }
    }
}
