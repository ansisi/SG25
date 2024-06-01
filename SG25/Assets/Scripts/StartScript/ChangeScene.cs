using UnityEngine;
using UnityEngine.SceneManagement;

public class Example : MonoBehaviour
{
    public void NextScene()
    {
        SceneManager.LoadScene("AiTestScene"); // "AiTestScene" 씬으로 전환
    }
}
