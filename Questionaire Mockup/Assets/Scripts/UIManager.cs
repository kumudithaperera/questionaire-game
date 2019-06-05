using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;


[Serializable()]

public struct UIManagerParameters
{
    [Header("Answer Options")]
    [SerializeField] float margins;
    public float Margins
    {
        get
        {
            return margins;
        }
    }

    [Header("Resolution Screen Options")]
    [SerializeField] Color correctBGColor;
    public Color CorrectBGColor
    {
        get
        {
            return correctBGColor;
        }
    }

    [SerializeField] Color incorrectBGColor;
    public Color IncorrectBGColor
    {
        get
        {
            return incorrectBGColor;
        }
    }

    [SerializeField] Color finalBGColor;
    public Color FinalBGColor
    {
        get
        {
            return finalBGColor;
        }
    }
}



[Serializable()]
public struct UIElements 
{
    [SerializeField] RectTransform answerContentArea;
    public RectTransform AnswerContentArea
    {
        get
        {
            return answerContentArea;
        }
    }

    [SerializeField] Text questionInfoTextObject;
    public Text QuestionInfoTextObject
    {
        get
        {
            return questionInfoTextObject;
        }
    }

    [SerializeField] Text scoreText;
    public Text ScoreText
    {
        get
        {
            return scoreText;
        }
    }

    [Space]

    [SerializeField] Animator resolutionScreenAnimator;
    public Animator ResolutionScreenAnimator
    {
        get
        {
            return resolutionScreenAnimator;
        }
    }

    [SerializeField] Image resolutonBackground;
    public Image ResolutonBackground
    {
        get
        {
            return resolutonBackground;
        }
    }

    [SerializeField] Text resolutonSateInfoText;
    public Text ResolutonSateInfoText
    {
        get
        {
            return resolutonSateInfoText;
        }
    }

    [SerializeField] Text resolutonScoreText;
    public Text ResolutonScoreText
    {
        get
        {
            return resolutonScoreText;
        }
    }
    [Space]

    [SerializeField] Text highScoreText;
    public Text HighScoreText
    {
        get
        {
            return highScoreText;
        }
    }

    [SerializeField] CanvasGroup mainCanvasGroup;
    public CanvasGroup MainCanvasGroup
    {
        get
        {
            return mainCanvasGroup;
        }
    }

    [SerializeField] RectTransform finishedUIElements;
    public RectTransform FinishedUIElements
    {
        get
        {
            return finishedUIElements;
        }
    }
}


public class UIManager : MonoBehaviour
{
   public enum ResolutionScreenType
    {
        Correct, Incorrect, Finish
    }

    [Header("References")]
    [SerializeField] GameEvents events;

    [Header("UI Elements (prefabs)")]
    [SerializeField] AnswerData answerPrefab;

    [SerializeField] UIElements uIElements;

    [Space]
    [SerializeField] UIManagerParameters parameters;

    List<AnswerData> currentAnswers = new List<AnswerData>();

    private int resStateParaHash = 0;

    private IEnumerator IE_DisplayTimedResolution;

    void OnEnable() {

        events.updateQuestionUI += UpdateQuestionUI;
        events.ScoreUpdated += UpdateScoreUI;
    }

    void onDisable()
    {
        events.updateQuestionUI -= UpdateQuestionUI;
        events.ScoreUpdated -= UpdateScoreUI;


    }

    void start()
    {
        resStateParaHash = Animator.StringToHash("ScreenState");
    }

    void UpdateQuestionUI(Questions questions) {

        uIElements.QuestionInfoTextObject.text = questions.Info;
        CreateAnswers(questions);
    }


    IEnumerator DispalyTimeResolution()
    {
        yield return new WaitForSeconds(GameUtility.ResolutionDelayTime);
        
        uIElements.MainCanvasGroup.blocksRaycasts = true;
    }

    IEnumerator CalculateScore()
    {
        var scoreValue = 0;
        while (scoreValue < events.CurrentFinalScore)
        {
            scoreValue++;
            uIElements.ResolutonScoreText.text = scoreValue.ToString();

            yield return null;
        }
    }

    void CreateAnswers(Questions questions)
    {
        EraseAnswers();

        float offset = 0 - parameters.Margins;

        for(int i = 0; i < questions.Answer.Length; i++)
        {
            AnswerData newAnswer = (AnswerData)Instantiate(answerPrefab,uIElements.AnswerContentArea);
            newAnswer.UpdateData(questions.Answer[i].Info, i);

            newAnswer.Rect.anchoredPosition = new Vector2(0, offset);

            offset -= (newAnswer.Rect.sizeDelta.y + parameters.Margins);
            uIElements.AnswerContentArea.sizeDelta = new Vector2(uIElements.AnswerContentArea.sizeDelta.x, offset * -1);

            currentAnswers.Add(newAnswer);
        }
    }

    void EraseAnswers()
    {
        foreach (var answer in currentAnswers)
        {
            Destroy(answer.gameObject);
        }
        currentAnswers.Clear();
    }

    void UpdateScoreUI()
    {
        uIElements.ScoreText.text = "Score: " + events.CurrentFinalScore;
    }

}
