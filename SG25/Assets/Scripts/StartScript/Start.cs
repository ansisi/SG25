using UnityEngine;
using UnityEngine.SceneManagement;

public class start : MonoBehaviour
{
    void OnPointerEnter()
    {
        SceneManager.LoadScene("AiTestScene"); // AiTestScene 이름 변경
    }
}
