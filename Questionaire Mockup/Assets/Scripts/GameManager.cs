using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


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

    private IEnumerator IEwaitTillNExtRound = null;

    private bool IsFinished
    {
        get
        {
            return (Finishedquestions.Count < Questions.Length) ? false : true;
        }
    }

    void Start()
    {
        LoadQuestions();

        Display();
    }
    
   public void UpdateAnswer(AnswerData newAnswer)
    {
        
        if(Questions[currentQuestion].GetAnswerType == global::Questions.AnswerType.Single)
        {
            foreach (var answer in PickedAnswers)
            {
                if(answer != newAnswer)
                {
                    answer.Reset();
                }
                PickedAnswers.Clear();
                PickedAnswers.Add(newAnswer);
            }
        }
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

    public void Accept()
    {
        bool isCorrect = CheckAnswers();
        Finishedquestions.Add(currentQuestion);

        UpdateScore((isCorrect) ? Questions[currentQuestion].AddScore : -Questions[currentQuestion].AddScore);

        var type = (IsFinished) ? UIManager.ResolutionScreenType.Finish : (isCorrect) ? UIManager.ResolutionScreenType.Correct : UIManager.ResolutionScreenType.Incorrect;

        if (events.DispalyResolutonScreen != null)
        {
            events.DispalyResolutonScreen(type, Questions[currentQuestion].AddScore);
        }

        if(IEwaitTillNExtRound != null)
        {
            StopCoroutine(IEwaitTillNExtRound);
        }
        IEwaitTillNExtRound = WaitTillNextRound();
        StartCoroutine(IEwaitTillNExtRound);

    }

    IEnumerator WaitTillNextRound()
    {
        yield return new WaitForSeconds(GameUtility.ResolutionDelayTime);
        Display();
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

    bool CheckAnswers()
    {
        if (!CompareAnswers())
        {
            return false;
        }
        return true;
    }

    bool CompareAnswers()
    {
        if (PickedAnswers.Count > 0)
        {
            List<int> c = Questions[currentQuestion].GetCorrectAnswer();
            List<int> p = PickedAnswers.Select(x => x.AnswerIndex).ToList();

            var f = c.Except(p).ToList();
            var s = p.Except(c).ToList();

            return !f.Any() && !s.Any();
        }
        return false;
    }

    private void UpdateScore(int add)
    {
        events.CurrentFinalScore += add;

        if(events.ScoreUpdated != null)
        {
            events.ScoreUpdated();
        }
    }
}
