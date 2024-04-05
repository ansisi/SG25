using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TypingGame : MonoBehaviour
{
    public TMP_Text textDisplay;
    public TMP_Text timerText;
    public TMP_InputField inputField;

    public SentenceData sentenceData;

    public float timeLimit = 5f;
    private int currentSentenceIndex = 0;
    private float currentTime = 0f;
    private bool isTyping = false;
    private bool isGameOver = false;
    private int correctAnswerCount = 0;

    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameManager.Instance;
    }

    void Start()
    {
        SelectRandomSentences();
        DisplayNextSentence();

        inputField.onEndEdit.AddListener(delegate { OnEndEdit(inputField.text); });
    }

    void Update()
    {
        if (!isGameOver && isTyping)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= timeLimit)
            {
                GameFail();
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

        for (int i = 0; i < allSentences.Length; i++)
        {
            int randomIndex = Random.Range(i, allSentences.Length);
            string temp = allSentences[randomIndex];
            allSentences[randomIndex] = allSentences[i];
            allSentences[i] = temp;
        }

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
                    Gamesuccess();
                }
                else
                {
                    NextSentence();
                }
            }
            else
            {
                GameFail();
            }
        }

    }

    void NextSentence()
    {
        currentSentenceIndex++;
        DisplayNextSentence();
    }

    void Gamesuccess()
    {
        gameManager.EnergyIncrease(10);
        gameManager.MoneyIncrease(10000);

        EndGame();
    }

    void GameFail()
    {
        gameManager.EnergyDecrease(10);
        gameManager.MoneyDecrease(10000);

        EndGame();
    }

    void EndGame()
    {
        isGameOver = true;
        SceneManager.LoadScene(0);
    }

    void OnEndEdit(string userInput)
    {
        if (Input.GetKey(KeyCode.Return))
        {
            CheckInput(userInput);
        }
    }
}
