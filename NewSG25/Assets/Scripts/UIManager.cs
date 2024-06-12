using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI currentMoneyText;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        currentMoneyText.text = GameManager.Instance.currentMoney.ToString("N0");
    }

    public void GameStartButton()
    {
        SceneManager.LoadScene("UnderDeveloping");
    }

    public void PanelClose()
    {
        gameObject.SetActive(false);
    }
}
