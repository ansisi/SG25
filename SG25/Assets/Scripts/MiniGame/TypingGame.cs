using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "New SentenceData", menuName = "Sentence Data")]
public class SentenceData : ScriptableObject
{
    public string[] sentences;
}

public class TypingGame : MonoBehaviour
{
    public TMP_Text textDisplay;
    public TMP_Text timerText;
    public TMP_InputField inputField;

    public GameObject typingGameUI;
    public SentenceData sentenceData;

    public float timeLimit = 5f;
    private int currentSentenceIndex = 0;
    private float currentTime = 0f;
    private bool isTyping = false;
    private bool isGameOver = false;
    private int correctAnswerCount = 0;

    bool isPanelActive = false;

    private PlayerCtrl playerCtrl;
    private TimeManager timeManager;

    void Start()
    {
        playerCtrl = FindObjectOfType<PlayerCtrl>();
        timeManager = FindObjectOfType<TimeManager>();

        typingGameUI.SetActive(false);

        SelectRandomSentences();
        DisplayNextSentence();

        inputField.onEndEdit.AddListener(delegate { OnEndEdit(inputField.text); });
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            typingGameUI.SetActive(true);
            inputField.ActivateInputField();

            isPanelActive = !isPanelActive;

            if (timeManager != null)
            {
                timeManager.TimeStop(isPanelActive);
            }
        }

        if (!isGameOver && isTyping)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= timeLimit)
            {
                typingGameUI.SetActive(false);
                UpdateTimerText();
            }
            else
            {
                UpdateTimerText();
            }
        }
    }

    void UpdateTimerText()
    {
        float timeLeft = Mathf.Max(0f, timeLimit - currentTime);
        timerText.text = timeLeft.ToString("F1");
    }

    void SelectRandomSentences()
    {
        string[] allSentences = sentenceData.sentences;
        List<string> selectedSentences = new List<string>();

        // 문장을 랜덤으로 선택
        for (int i = 0; i < allSentences.Length; i++)
        {
            int randomIndex = Random.Range(i, allSentences.Length);
            string temp = allSentences[randomIndex];
            allSentences[randomIndex] = allSentences[i];
            allSentences[i] = temp;
        }

        // 선택된 문장을 다시 저장
        sentenceData.sentences = allSentences;
    }

    void DisplayNextSentence()
    {
        if (currentSentenceIndex < sentenceData.sentences.Length)
        {
            textDisplay.text = sentenceData.sentences[currentSentenceIndex];
            inputField.text = "";
            currentTime = 0f;
            isTyping = true;
            inputField.ActivateInputField();
        }
        else
        {
            EndGame();
        }
    }

    public void CheckInput(string userInput)
    {
        if (!isGameOver)
        {
            if (userInput == sentenceData.sentences[currentSentenceIndex])
            {
                correctAnswerCount++;
                if (correctAnswerCount >= 5)
                {
                    EndGame();
                }
                else
                {
                    NextSentence();
                }
            }
            else
            {
                EndGame();
            }
        }

    }

    void NextSentence()
    {
        currentSentenceIndex++;
        DisplayNextSentence();
    }

    void EndGame()
    {
        isGameOver = true;
        typingGameUI.SetActive(false);
    }

    void OnEndEdit(string userInput)
    {
        if (Input.GetKey(KeyCode.Return))
        {
            CheckInput(userInput);
        }
    }
}
