using UnityEngine;
using UnityEngine.SceneManagement;

public class Example : MonoBehaviour
{
    public void NextScene()
    {
        SceneManager.LoadScene("AiTestScene"); // "AiTestScene" ������ ��ȯ
    }
}
