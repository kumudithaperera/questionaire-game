using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    Questions[] questions = null;
    public Questions[] Questions
    {
        get
        {
            return questions;
        }
    }

    [SerializeField] GameEvents events = null;

    private List<AnswerData> PickedAnswers = new List<AnswerData>();

    private List<int> Finishedquestions = new List<int>();
    private int currentQuestion = 0;

    void Start()
    {
        LoadQuestions();

        foreach (var questions in Questions)
        {
            Debug.Log(questions.Info);
        }

        Display();
    }

    public void ErasedAnswers()
    {
        PickedAnswers = new List<AnswerData>();
    }

    void Display()
    {
        ErasedAnswers();
        var question = GetRandomQuestion();

        if(events.updateQuestionUI != null)
        {
            events.updateQuestionUI(question);
        }
        else
        {
            Debug.LogWarning("somethings Up");
        }
    }

    Questions GetRandomQuestion()
    {
        var randomIndex = GetRandomQuestionIndex();
        currentQuestion = randomIndex;

        return Questions[currentQuestion];
    }

    int GetRandomQuestionIndex()
    {
        var random = 0;
        if (Finishedquestions.Count < Questions.Length)
        {
            do
            {
                random = UnityEngine.Random.Range(0, Questions.Length);
            } while (Finishedquestions.Contains(random) || random == currentQuestion);
        }
        return random;
    }   

    void LoadQuestions()
    {
        Object[] objs = Resources.LoadAll("Questions");
        questions = new Questions[objs.Length];
        for(int i = 0; i< objs.Length; i++)
        {
            questions[i] = (Questions)objs[i];
        }
    }
}
