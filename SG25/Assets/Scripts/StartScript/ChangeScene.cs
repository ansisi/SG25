using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void OnSceneChange()
    {
        // StartButtonŬ�� �� AiTestScene ��ȭ
        SceneManager.LoadScene("AiTestScene"); // ����: ���� �� �̸�
    }
}
