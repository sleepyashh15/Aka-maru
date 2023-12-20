using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class QuizManager : MonoBehaviour
{
    [System.Serializable]
    public class Question
    {
        public string question;
        public List<string> answers;
        public string correctAnswer;
    }

    public List<Question> questions;

    public Text questionText;
    public List<Button> answerButtons;

    private Question currentQuestion;

    void Start()
    {
        SetRandomQuestion();
    }

    void SetRandomQuestion()
    {
        if (questions.Count == 0)
        {
            Debug.Log("Quiz completed!");
            return;
        }

        int randomIndex = Random.Range(0, questions.Count);
        currentQuestion = questions[randomIndex];

        questionText.text = currentQuestion.question;

        List<string> answerList = new List<string>(currentQuestion.answers);
        answerList.Shuffle();

        for (int i = 0; i < answerButtons.Count; i++)
        {
            answerButtons[i].GetComponentInChildren<Text>().text = answerList[i];
        }

        questions.RemoveAt(randomIndex);
    }

    public void CheckAnswer(string selectedAnswer)
    {
        if (selectedAnswer == currentQuestion.correctAnswer)
        {
            Debug.Log("Correct!");
        }
        else
        {
            Debug.Log("Wrong! The correct answer is: " + currentQuestion.correctAnswer);
        }

        SetRandomQuestion();
    }
}

public static class ListExtensions
{
    // Fisher-Yates shuffle algorithm
    public static void Shuffle<T>(this List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}