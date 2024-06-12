using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GuideManager : MonoBehaviour
{
    public GameObject[] guidePanels;

    public void NextPanel()
    {
        SwitchPanel(1);
    }
    public void PreviousPanel()
    {
        SwitchPanel(-1);
    }

    void SwitchPanel(int direction)
    {
        for (int i = 0; i < guidePanels.Length; i++)
        {
            if (guidePanels[i].activeSelf)
            {
                guidePanels[i].SetActive(false);
                int nextIndex = (i + direction + guidePanels.Length) % guidePanels.Length;
                guidePanels[nextIndex].SetActive(true);
                break;
            }
        }
    }

    public void GoGameScene()
    {
        SceneManager.LoadScene("UnderDeveloping");
    }
}
