using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    public GameObject resultUI;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI dateText;

    public float startHour = 9f;
    public int startDay = 1;
    public float gameTime = 60f;    //60f = ���� �ð� 1�� -> ���ӽð� 60�� (���� �ð� 1�� = ���� �ð� 1�ð�)
    public float endHour = 26f;

    private float gameHour;
    private int gameDay;
    public bool isTimeStopped = false;

    public FirstPersonController playerCtrl;
    [SerializeField] private Transform checkoutTransform; // ���� ��ġ

    private void Start()
    {
        playerCtrl = FindObjectOfType<FirstPersonController>();
        resultUI.SetActive(false);
        gameHour = startHour;
        gameDay = startDay;
        UpdateTimeText();
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (isTimeStopped)
        {
            return;
        }

        gameHour += Time.deltaTime / gameTime;

        if (gameHour >= endHour)
        {
            if (AreCustomersAtCheckout())
            {
                Debug.Log("�մ��� ���뿡 �ֽ��ϴ�. �ð��� �Ѿ�� �ʽ��ϴ�.");
                return;
            }

            if (gameDay < 25)
            {
                resultUI.SetActive(true);
                isTimeStopped = true;
                gameDay += 1;
                gameHour = 9f;
                playerCtrl.PanelOn();

                
            }
            else
            {
                SceneManager.LoadScene("HappyEndingScene");
            }
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
            playerCtrl.PanelOff();
        }
    }

    public void TimeStop(bool isActive)
    {
        isTimeStopped = isActive;
    }

    internal void NextDayLogic()
    {
        resultUI.SetActive(false);
        isTimeStopped = false;
        gameDay += 1;
        gameHour = startHour;
        playerCtrl.PanelOff();
        UpdateTimeText();
    }

    private bool AreCustomersAtCheckout()
    {
        GameObject[] customers = GameObject.FindGameObjectsWithTag("Customer");

        foreach (GameObject customer in customers)
        {
            float distanceToCheckout = Vector3.Distance(customer.transform.position, checkoutTransform.position);
            if (distanceToCheckout <= 2.0f) // �Ÿ� ���� ����
            {
                // 10�� �Ŀ� ���� ���� �̵�
                StartCoroutine(MoveToNextDayAfterDelay(10.0f));
                return true;

            }
        }
        return false;
    }

    IEnumerator MoveToNextDayAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        NextDayLogic();
    }
}
