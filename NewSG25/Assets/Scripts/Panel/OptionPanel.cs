using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionPanel : MonoBehaviour
{
    private FirstPersonController playerCtrl;

    void Start()
    {
        playerCtrl = FindObjectOfType<FirstPersonController>();
    }

    private void Update()
    {
        playerCtrl.PanelOn();
    }

    public void CancelPanel()
    {
        playerCtrl.PanelOff();
        gameObject.SetActive(false);
    }

    public void GoStartScene()
    {
        SceneManager.LoadScene("StartScene");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
