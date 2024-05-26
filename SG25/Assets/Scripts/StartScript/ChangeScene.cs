using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void OnSceneChange()
    {
        // StartButton클릭 시 AiTestScene 전화
        SceneManager.LoadScene("AiTestScene"); // 예시: 게임 씬 이름
    }
}
