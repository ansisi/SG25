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
    public TMP_Text textDisPlay;
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
        int numSentenvesToSelect = Mathf.Min(allSentences.Length, 5);
        List<string> selectedSentences = new List<string>();
        List<string> remainingSentences = new List<string>(allSentences);

        for (int i = 0; i < numSentenvesToSelect; i++)
        {
            int randomIndex = Random.Range(0, remainingSentences.Count);
            selectedSentences.Add(remainingSentences[randomIndex]);
            remainingSentences.RemoveAt(randomIndex);
        }

        sentenceData.sentences = selectedSentences.ToArray();
    }

    void DisplayNextSentence()
    {
        if (currentSentenceIndex < sentenceData.sentences.Length)
        {
            textDisPlay.text = sentenceData.sentences[currentSentenceIndex];
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
                NextSentence();
            }
            else if (correctAnswerCount >= 5)
            {
                EndGame();
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