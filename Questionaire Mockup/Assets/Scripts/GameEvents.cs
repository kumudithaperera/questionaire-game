using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "GameEvents", menuName = "Quiz/new GameEvents")]
public class GameEvents : ScriptableObject
{
    public delegate void updateQuestionUICallBack(Questions questions);
    public updateQuestionUICallBack updateQuestionUI;

    public delegate void updateQuestionAnswerCallBack(AnswerData pickedAnswer);
    public updateQuestionAnswerCallBack updateQuestionAnswer;

    public delegate void DispalyResolutonCallBack(UIManager.ResolutionScreenType type, int score);
    public DispalyResolutonCallBack DispalyResolutonScreen;

    public delegate void ScoreUpdatedCallBack();
    public ScoreUpdatedCallBack ScoreUpdated;

    [HideInInspector]
    public int CurrentFinalScore;
    [HideInInspector]
    public int StartUpHighscore;

}
