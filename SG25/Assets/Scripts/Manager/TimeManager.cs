using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public GameObject resultUI;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI dateText;
    public TextMeshProUGUI energyText;

    public float startHour = 9f;
    public int startDay = 1;
    public float gameTime = 60f;    //60f =  현실 시간 1초 -> 게임시간 60초 (현실 시간 1분 = 게임 시간 1시간)
    public float endHour = 26f;

    private float gameHour;
    private int gameDay;
    public bool isTimeStopped = false;

    private ObjectPicker objectPicker;
    public PlayerCtrl playerCtrl;

    void Start()
    {
        if (GameManager.Instance != null)
        {
            energyText.text = GameManager.Instance.currentEnergy.ToString();
        }
        else
        {
            Debug.LogError("GameManager 인스턴스를 찾을 수 없습니다!");
        }

        objectPicker = FindObjectOfType<ObjectPicker>();
        playerCtrl = FindObjectOfType<PlayerCtrl>();

        resultUI.SetActive(false);

        gameHour = startHour;
        gameDay = startDay;

        UpdateTimeText();
    }

    void Update()
    {
        if (isTimeStopped)
        {
            return;
        }

        gameHour += Time.deltaTime / gameTime;

        if (gameHour >= endHour)
        {
            resultUI.SetActive(true);
            isTimeStopped = true;

            if (gameDay < 31)
            {
                gameDay += 1;
            }
            else
            {
                gameDay = 1;
            }

            gameHour = 9f;
        }
        else
        {
            UpdateTimeText();
        }
    }


    private void UpdateTimeText()
    {
        int hours = Mathf.FloorToInt(gameHour) % 24;
        int minutes = Mathf.FloorToInt((gameHour - Mathf.Floor(gameHour)) * 60);

        timeText.text = string.Format("{0:D2}:{1:D2}", hours, minutes);

        dateText.text = string.Format("Day {0}", gameDay);
    }

    public void OnContinueButtonClick()
    {
        if (resultUI.activeSelf)
        {
            resultUI.SetActive(false);

            isTimeStopped = false; 
        }
    }

    public void TimeStop(bool isActive)
    {
        isTimeStopped = isActive;

        if (isActive)
        {
            playerCtrl.canLook = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            playerCtrl.canLook = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }    
    }
}
