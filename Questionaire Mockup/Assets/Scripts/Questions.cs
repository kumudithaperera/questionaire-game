using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public struct Answer {
    [SerializeField] private string info;
    public string Info
    {
        get
        {
            return info;
        }
    }

    [SerializeField] private bool iscorrect;
    public bool IsCorrect
    {
        get
        {
            return iscorrect;
        }
    }
}
[CreateAssetMenu(fileName = "New question", menuName = "Quiz/new Question")]

public class Questions : ScriptableObject
{
    //for the type of answer user needed
    public enum AnswerType {Single}
   

    [SerializeField] private string info = string.Empty;
    public string Info {
        get
        {
            return info;
          
        }
    }

    [SerializeField] Answer[] answers = null;
    public Answer[] Answer
    {
        get
        {
            return answers;
        }
    }

    //parameters

    [SerializeField] private bool userTimer = false;
    public bool UserTimer
    {
        get
        {
            return userTimer;
        }
    }

    [SerializeField] private int timer = 0;
    public int Timer { get
        {
            return timer = 0;
        }
     }

    [SerializeField] private AnswerType answerType = AnswerType.Single;
    public AnswerType GetAnswerType
    {
        get
        {
            return answerType;
        }
    }

    [SerializeField] private int addScore = 10;
    public int AddScore {
        get {
            return addScore;
        }
    }

    public List<int> GetCorrectAnswer()
    {
        List<int> CorrectAnswers = new List<int>();
        for(int i = 0; i< Answer.Length; i++)
        {
            if (Answer[i].IsCorrect)
            {
                CorrectAnswers.Add(i);
            }
        }
        return CorrectAnswers;
    }
}
